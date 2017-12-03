using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using ARW.Users;
using ARW.Users.Manager;
using ARW.SRP;

namespace ARW.Room{

	public class ARWRoom{

		private string _name;
		public string name{
			get{	return _name; }
		}
		
		private string _tag;
		public string tag{
			get{	return _tag; }
		}

		private int _id;
		public int id{
			get{	return _id;	}
		}

		private List<ARWUser> _userList;
		public ARWUser[] userList{
			get{	return _userList.ToArray();	}
		}

		public ARWRoom(string roomData){
			JSONObject obj = new JSONObject(roomData);

			this._userList = new List<ARWUser>();

			this._name = obj.GetString("name");
			this._id = int.Parse(obj.GetString("id"));
			this._tag = obj.GetString("tag");

			for(int ii = 0; ii< obj.GetField("users").list.Count; ii++){
				JSONObject userJson = obj.GetField("users").list[ii];
				if(this._userList.Where(a=>a.userId == int.Parse(userJson.GetString("user_id"))).FirstOrDefault() == null){
					ARWUser newUser = ARWUserManager.instance.CreateUser(userJson);
					this._userList.Add(newUser);
				}
			}
		}

		public void AddUserToRoom(ARWUser user){
			if(!this._userList.Contains(user)){
				this._userList.Add(user);
			}
		}

		public void RemoveUser(ARWUser user){
			this._userList.Remove(user);
		}
	}
}