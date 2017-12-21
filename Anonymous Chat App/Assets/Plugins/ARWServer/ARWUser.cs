using System;
using System.Collections;
using System.Collections.Generic;
using ARW.Room;

namespace ARW.Users{
	public class ARWUser{

		private string _userName;
		private int _userId;
		private bool _isMe;

		public ARWRoom lastJoinedRoom;
		public string userName{get{	return _userName;}}
		public int userId{get{return _userId;}}
		public bool isMe{get{return _isMe;}}

		public ARWUser(JSONObject userJson){
			this._userName = userJson.GetString("user_name");
			this._userId = int.Parse(userJson.GetString("user_id"));
			this._isMe = bool.Parse(userJson.GetString("user_isMe"));
		}
	}
}