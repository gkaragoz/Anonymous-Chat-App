using System;
using ARW.Com;
using ARW.SRP;

namespace ARW.Events{

	public delegate void ARWEventHandler(ARWObject evntObj, object eventParam = null);
	public delegate void PrivateHandler(ARWObject arwObject, SpecialRequestParam specialRequestParams);

	public class ARWEvent
	{
		public ARWEventHandler handler;
		public PrivateHandler p_handler;
		public string eventName;

		public ARWEvent(){
			eventName = String.Empty;
			handler = null;
			p_handler = null;
		}

		public ARWEvent(string eventName){
			this.eventName = eventName;
			handler = null;
			p_handler = null;
		}
	}
}