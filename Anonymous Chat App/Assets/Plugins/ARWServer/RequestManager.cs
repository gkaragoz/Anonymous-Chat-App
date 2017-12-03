using System;
using System.Collections;
using System.Collections.Generic;
using ARW.Requests;

namespace ARW.Requests.Manager{

	public class RequestManager{

		private string wrondData = "";
		private string requestData;

		private bool beginReq;

		public RequestManager(){
			wrondData = String.Empty;
			requestData = String.Empty;
			beginReq = false;
		}

		public void StartOrStopRequest(){
			if(!beginReq){
				beginReq = true;
				this.wrondData = String.Empty;
				return;
			}

			// UnityEngine.Debug.Log("==========> " + this.wrondData);
			Request inComingReques = Request.Extract(wrondData);
			DoRequest(inComingReques);
			this.beginReq = false;
		}

		public void AddChar(char reqChar){
			this.wrondData += reqChar;
		}

		public void DoRequest(Request request){
			request.DoRequest();
		}
	}
}