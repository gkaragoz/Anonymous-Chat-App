using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using ARW;
using ARW.Com;
using ARW.Config;
using ARW.Users;
using ARW.Events;
using ARW.Requests.Extensions;

public class ServerManager : MonoBehaviour{

	public static ServerManager instance;

	public ARWServer arwServer;

	private string GETUSERDATA = "GetUserData";
	private string SENDMESSAGE = "SendMessage";
	private string FINDCONVERSATION = "FindConversation";
	private string FINDEDCONVERSATION = "FindedConversation";
	private string CANNOTFINDACTIVEUSER = "CannotFindActiveUser";

	private void Awake(){
		arwServer = new ARWServer();
		arwServer.Init();

		ConfigData cfg = new ConfigData(8081, 9933, "localhost");

		arwServer.AddEventHandlers(ARWEvents.CONNECTION, OnConnectionSuccess);
		arwServer.AddEventHandlers(ARWEvents.LOGIN, OnLoginSuccess);
		arwServer.AddExtensionRequest(GETUSERDATA, GetUserData);

		instance = this;
		arwServer.Connect(cfg);
	}

	private void Update(){
		arwServer.ProcessEvents();
	}

	private void OnConnectionSuccess(ARWObject obj, object value){
		Debug.Log("Connection Success");
		AppManager.instance.appStatus = AppManager.AppStatus.Connection;

		arwServer.SendLoginRequest("123123123", null);
	}

	private void OnLoginSuccess(ARWObject obj, object value){
		Debug.Log("Login Success");
		Debug.Log("Sending GetUserData Request");

		obj = new IARWObject();
		obj.PutString("player_id", "123123123");
		obj.PutString("player_nickname", "powerLED");
		obj.PutString("language", Application.systemLanguage.ToString());

		arwServer.SendExtensionRequest("GetUserData", obj, false);
	}

	private void GetUserData(ARWObject obj, object value){
		if(obj.GetString("error") == ""){
			AppManager.instance.InitPlayer(obj.GetString("player_data"));
		}else{
			Debug.Log("GetUserData Error : " + obj.GetString("error"));
		}
	}
}