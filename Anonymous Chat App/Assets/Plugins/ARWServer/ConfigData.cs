using System;
using System.Collections;
using System.Collections.Generic;

namespace ARW.Config{

	public class ConfigData{

		public int tcpPort;
		public int udpPort;
		public string host;

		public ConfigData(int tcpPort, int udpPort, string host){
			this.tcpPort = tcpPort;
			this.udpPort = udpPort;
			this.host = host;
		}
	}
}