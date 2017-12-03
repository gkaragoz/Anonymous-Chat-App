using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ARW.Com;
using ARW.SRP;
using ARW.Events;

namespace ARW.Requests{

	public class Request{

		private string eventName;
		private ARWObject arwObject;
		private SpecialRequestParam specialRequestParam;

		public Request(string eventName, ARWObject arwObject, SpecialRequestParam sP = null){
			this.eventName = eventName;
			this.arwObject = arwObject;
			this.specialRequestParam = sP;
		}


		public byte[] Compress(){
			string reqData = "";
			if(this.arwObject == null)
				this.arwObject = new IARWObject();
			if(this.specialRequestParam == null)
				this.specialRequestParam = new SpecialRequestParam();

			string arwObjData = System.Text.Encoding.UTF8.GetString (this.arwObject.Compress()).Replace("\0", null).Replace("\"",null);
			
			reqData += this.eventName + "^^";
			reqData += arwObjData + "^^";
			reqData += this.specialRequestParam.Compress();

			return System.Text.Encoding.UTF8.GetBytes(reqData);
		}

		public static Request Extract(byte[] reqBytes){
			string reqData = System.Text.Encoding.UTF8.GetString(reqBytes).Replace("\0", "").Replace("\"", "");

			return Extract(reqData);
		}

		public static Request Extract(string reqData){
			string[] reqParts = reqData.Split(new string[]{"^^"}, StringSplitOptions.None);
			if(reqParts.Length != 3)	return null;

			string eventName = reqParts[0];
			ARWObject obj = IARWObject.Extract(reqParts[1]);
			SpecialRequestParam specialParam = SpecialRequestParam.Extract(reqParts[2]);

			Request newReq = new Request(eventName, obj, specialParam);
			return newReq;
		}

		public void DoRequest(){
			if(ARWServer.instance == null)		return;

			ARWEvent currentEvent = ARWEvents.allEvents.Where(a=>a.eventName == this.eventName).FirstOrDefault();
			if(currentEvent != null){
				currentEvent.p_handler(this.arwObject, this.specialRequestParam);
			}
		}

		public static bool CanBeRequest(string reqData){
			string[] reqParts = reqData.Split(new string[]{"^^"}, StringSplitOptions.None);

			bool canBe = reqParts.Length == 3 ? true : false;
			return canBe;
		}
	}
}