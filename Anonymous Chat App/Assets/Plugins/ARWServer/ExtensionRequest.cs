using System;
using System.Collections;
using System.Collections.Generic;

using ARW.Events;

namespace ARW.Requests.Extensions{
	public class ExtensionRequest{
		private string _cmd;
		private EventHandler _handler;

		public string cmd{
			get{ 	return _cmd;	}
		}
		public EventHandler handler{
			get{ 	return _handler;	}
		}

		public ExtensionRequest(string cmd, EventHandler handler){
			this._cmd = cmd;
			this._handler = handler;
		}
	}
}