using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ARW.Users;
using ARW.SRP;

namespace ARW.Users.Manager{
	public class ARWUserManager{

		private List<ARWUser> _allUser;
		public List<ARWUser> allUser{get{return _allUser;	}}
		public ARWUser me;

		public static ARWUserManager instance;

		public ARWUserManager(){
			this._allUser = new List<ARWUser>();
		}

		public ARWUser CreateUser(JSONObject userInfo){
			ARWUser newUser = new ARWUser(userInfo);
			
			if(this._allUser.Where(a=>a.userId == newUser.userId).FirstOrDefault() == null){
				this._allUser.Add(newUser);
			}


			if(newUser.isMe){
				this.me = newUser;
			}
			return newUser;
		}

		public bool UserIsExist(int userId){
			ARWUser user = this._allUser.Where(a=>a.userId == userId).FirstOrDefault();

			return user == null ? false : true;
		}
	}
}