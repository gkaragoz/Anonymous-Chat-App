using System;
using System.Linq;
using ARW;
using ARW.Com;
using ARW.Events;
using ARW.Requests;
using ARW.Requests.Extensions;
using ARW.SRP;
using ARW.Users;
using ARW.Users.Manager;
using ARW.Room;
using ARW.Room.Manager;
namespace ARW.PrivateHandlers
{
	public class PrivateEventHandlers
	{
		public void P_Connection(ARWObject arwObject, SpecialRequestParam specialReqParam){
			if(ARWServer.instance == null)		return;

			// DateTime t = DateTime.Parse(specialReqParam.GetString("server_time"));
			// ARWServer.instance.SetServerTime(t);
			
			if (specialReqParam.GetString ("error") == "")
				ARWServer.instance.isConnected = true;

			if (ARWEvents.CONNECTION.handler != null)
				ARWEvents.CONNECTION.handler (arwObject);
		}

		public void P_Disconnection(ARWObject arwObject, SpecialRequestParam specialReqParam){
			if(ARWServer.instance == null)		return;

			ARWServer.instance.isConnected = false;
			if(ARWEvents.DISCONNECTION.handler == null)		return;

			ARWObject obj = new IARWObject();
			ARWEvents.DISCONNECTION.handler(obj);
		}

		public void P_Connection_Lost(ARWObject arwObject, SpecialRequestParam specialReqParam){

			if(ARWServer.instance == null)		return;

			ARWServer.instance.isConnected = false;
			if(ARWEvents.CONNECTION_LOST.handler == null)		return;

			ARWObject obj = new IARWObject();
			ARWEvents.CONNECTION_LOST.handler(obj);
		}

		public void P_Login(ARWObject obj, SpecialRequestParam specialReqParam){
			if(ARWServer.instance == null)		return;

			ARWUser newUser = ARWUserManager.instance.CreateUser(new JSONObject(specialReqParam.GetString("user_properties")));

			if(ARWEvents.LOGIN.handler == null)	return;

			ARWEvents.LOGIN.handler(obj, newUser);
		}

		public void P_Join_Room(ARWObject obj, SpecialRequestParam specialReqParam){
			if(ARWServer.instance == null)		return;

			string roomData = specialReqParam.GetString("room_properties");
			JSONObject roomDataJson = new JSONObject(roomData);
			ARWRoom newRoom = ARWRoomManager.instance.CreateRoom(roomData);
			ARWUserManager.instance.me.lastJoinedRoom = newRoom;

			if(ARWEvents.ROOM_JOIN.handler == null)		return;

			ARWEvents.ROOM_JOIN.handler(obj, newRoom);
		}

		public void P_User_Enter_Room(ARWObject obj, SpecialRequestParam specialReqParam){
			if(ARWServer.instance == null)		return;

			string userProperties = specialReqParam.GetString("user_properties");
			ARWUser newUser = ARWUserManager.instance.CreateUser(new JSONObject(userProperties));
			
			ARWRoom currentRoom = ARWRoomManager.instance.allRooms.Where(a=>a.id == specialReqParam.GetInt("room_id")).FirstOrDefault();
			if(currentRoom == null)		return;
			currentRoom.AddUserToRoom(newUser);

			if(ARWEvents.USER_ENTER_ROOM.handler == null)		return;

			ARWEvents.USER_ENTER_ROOM.handler(obj, newUser);
		}

		public void P_User_Exit_Room(ARWObject obj, SpecialRequestParam specialReqParam){
			if(ARWServer.instance == null)		return;

			ARWRoom currentRoom = ARWRoomManager.instance.allRooms.Where(a=>a.id == specialReqParam.GetInt("room_id")).FirstOrDefault();
			if(currentRoom == null)		return;

			int userId = specialReqParam.GetInt("user_id");
			ARWUser userLeaved = currentRoom.userList.Where(a=>a.userId == userId).FirstOrDefault();
			if(userLeaved == null)		return;

			currentRoom.RemoveUser(userLeaved);

			if(ARWEvents.USER_EXIT_ROOM.handler == null)		return;

			ARWEvents.USER_EXIT_ROOM.handler(obj, userLeaved);
		}

		public void P_Extension_Response(ARWObject obj, SpecialRequestParam specialReqParam){
			ExtensionRequest currentExtension = ARWServer.instance.GetExtensionRequest(specialReqParam.GetString("cmd"));
			if(currentExtension == null)		return;

			currentExtension.handler(obj);
		}
	}
}