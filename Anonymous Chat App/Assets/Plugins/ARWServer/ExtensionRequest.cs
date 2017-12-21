using System;
using System.Collections;
using System.Collections.Generic;

using ARW.Events;

namespace ARW.Requests.Extensions{
	public class ExtensionRequest{
		private string _cmd;
		private ARWEventHandler _handler;

		public string cmd{
			get{ 	return _cmd;	}
		}
		public ARWEventHandler handler{
			get{ 	return _handler;	}
		}

		public ExtensionRequest(string cmd, ARWEventHandler handler){
			this._cmd = cmd;
			this._handler = handler;
		}
	}
}