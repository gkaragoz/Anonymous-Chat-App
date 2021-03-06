﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq;

using ARW.Config;
using ARW.Requests;
using ARW.Requests.Manager;
using ARW.Events;
using ARW.Com;
using ARW.Events.Cmd;
using ARW.SRP;
using ARW.Users;
using ARW.Users.Manager;
using ARW.Room;
using ARW.Room.Manager;
using ARW.Requests.Extensions;

namespace ARW{
	
	public class ARWServer{
		
		private TcpClient tcpClient;
		private NetworkStream networkStream{
			get{
				try{
					if(tcpClient != null)	return tcpClient.GetStream();
				}catch(ObjectDisposedException){
					return null;
				}catch(System.Exception){
					ARWEvents.CONNECTION_LOST.p_handler(null, null);
					this.tcpClient = null;
				}

				return null;
			}
		}

		private TcpListener tcpListener;

		private RequestManager requestManager;
		private List<ExtensionRequest> extensionRequests;

		private DateTime firstServerTime;
		private DateTime _serverTime;
		public DateTime serverTime{
			set{
				this._serverTime = value;
			}
			get{
				TimeSpan different = DateTime.Now - firstServerTime;
				return _serverTime.Add(different);
			}
		}

		public static ARWServer instance;

		public bool isConnected = false;

		public void Init(){
			if(instance != null)		return;

			instance = this;
			this.requestManager = new RequestManager();
			ARWUserManager.instance = new ARWUserManager();
			new ARWRoomManager();
			
			ARWEvents.Init();

		}

		public void Connect(ConfigData cfg){
			this.tcpClient = new TcpClient();
			try{
				this.firstServerTime = DateTime.Now;
				this.tcpClient.Connect(cfg.host, cfg.tcpPort);

				this.SendRequest(new Request(ARWServer_CMD.Connection_Success, null));
			}catch(SocketException e){
			}
		}

		public void ProcessEvents(){
			if(this.tcpClient == null || this.networkStream == null)		return;

			try{
				if(this.tcpClient.Client.Poll(1, SelectMode.SelectRead) && !this.networkStream.DataAvailable && this.isConnected){
					ARWEvents.CONNECTION_LOST.p_handler(null, null);
					return;
				}
			
				byte[] readBytes = new byte[4096];
				if(this.networkStream.DataAvailable){
					this.networkStream.Read(readBytes, 0, readBytes.Length);
				}else
					return;

				ParseRequestData(readBytes);
			}
			catch(System.ObjectDisposedException){}
			catch(System.IO.IOException){}
			catch(System.OutOfMemoryException){}
		}

		public void SendExtensionRequest(string cmd, ARWObject obj, bool roomRequest){
			SpecialRequestParam reqSpParam = new SpecialRequestParam();
			reqSpParam.PutVariable("cmd", cmd);
			reqSpParam.PutVariable("isRoom", roomRequest);
			if(roomRequest){
				reqSpParam.PutVariable("room_id", ARWUserManager.instance.me.lastJoinedRoom);
			}

			Request newRequest = new Request(ARWServer_CMD.Extension_Request, obj, reqSpParam);
			this.SendRequest(newRequest);
		}

		public void SendLoginRequest(string userName, ARWObject arwObject){
			SpecialRequestParam loginParam = new SpecialRequestParam();
			loginParam.PutVariable("user_name", userName);

			Request loginRequest = new Request(ARWServer_CMD.Login, arwObject, loginParam);
			this.SendRequest(loginRequest);
		}

		public void AddEventHandlers(ARWEvent arwEvent, ARWEventHandler eventHandler){
			arwEvent.handler += eventHandler;
		}

		public void AddExtensionRequest(string cmd, ARWEventHandler handler){
			if(this.extensionRequests == null)		this.extensionRequests = new List<ExtensionRequest>();

			ExtensionRequest ex = this.extensionRequests.Where(a=>a.cmd == cmd).FirstOrDefault();
			if(ex == null){
				this.extensionRequests.Add(new ExtensionRequest(cmd, handler));
			}
		}

		public ExtensionRequest GetExtensionRequest(string cmd){
			ExtensionRequest ex = this.extensionRequests.Where(a=>a.cmd == cmd).FirstOrDefault();
			return ex;
		}
		
		public void Disconnection(){
			if(!this.isConnected || this.tcpClient == null )	return;

			Request disConnectionReq = new Request(ARWServer_CMD.Disconnection, null);
			this.SendRequest(disConnectionReq);
			this.tcpClient.Close();
			ARWEvents.DISCONNECTION.p_handler(null, null);
		}
//==================================================================================================
//===================================     Private Methods    =======================================
//==================================================================================================

		private void ParseRequestData(byte[] readBytes){
			var message = System.Text.Encoding.UTF8.GetString(readBytes);
			for(int ii = 0; ii < message.Length; ii++){
				char reqChar = message[ii];
				if(reqChar == '|'){
					this.requestManager.StartOrStopRequest();
				}else{
					this.requestManager.AddChar(reqChar);
				}
			}
		}

		private void HandleRequest(string reqData){
			Request inComingRequest = Request.Extract(reqData);
			inComingRequest.DoRequest();
		}

		private void SendRequest(Request request){
			if(this.tcpClient == null)		return;
			
			byte[] reqBytes = request.Compress();
			string data = "|";
			data += System.Text.Encoding.UTF8.GetString(reqBytes) + "|";

			reqBytes = System.Text.Encoding.UTF8.GetBytes(data);
			try{
				this.tcpClient.Client.Send(reqBytes);
			}
			catch(SocketException){}
			catch(ObjectDisposedException){}
		}

		public void SetServerTime(DateTime firstServerTime){
			TimeSpan requestDelay = DateTime.Now - this.firstServerTime;
			firstServerTime.Add(requestDelay);
			this.serverTime = firstServerTime;
		}
	}
}
