using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ARW.Room;

namespace ARW.Room.Manager{
	public class ARWRoomManager{

		private List<ARWRoom> _allRooms;
		public List<ARWRoom> allRooms{get{	return _allRooms;	}}

		public static ARWRoomManager instance;

		public ARWRoomManager(){
			Init();
		}

		private void Init(){
			instance = this;
			_allRooms = new List<ARWRoom>();
		}

		public ARWRoom CreateRoom(string roomData){
			ARWRoom newRoom = new ARWRoom(roomData);
			if(this._allRooms.Where(a=>a.id == newRoom.id).FirstOrDefault() == null){
				this._allRooms.Add(newRoom);
				return newRoom;
			}
			return null;
		}
	}
}