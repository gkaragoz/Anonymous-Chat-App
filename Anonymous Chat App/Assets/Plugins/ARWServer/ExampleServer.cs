using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARW;
using ARW.Users;
using ARW.Com;
using ARW.Config;
using ARW.Events;
using ARW.Room;

public class ExampleServer : MonoBehaviour {

	ARWServer arwServer;

	void Start () {
		arwServer = new ARWServer();

		arwServer.Init();
		ConfigData cfg = new ConfigData(8081, 8081, "localhost");

		arwServer.AddEventHandlers(ARWEvents.CONNECTION, ConnectionEventHandler);
		arwServer.AddEventHandlers(ARWEvents.CONNECTION_LOST, ConnectionLostEventHandler);
		arwServer.AddEventHandlers(ARWEvents.DISCONNECTION, DisConnectionEventHandler);
		arwServer.AddEventHandlers(ARWEvents.LOGIN, LoginEventHandler);
		arwServer.AddEventHandlers(ARWEvents.ROOM_JOIN, JoinRoomEventHandler);
		arwServer.AddEventHandlers(ARWEvents.USER_ENTER_ROOM, UserEnterRoomEventHandler);
		arwServer.AddEventHandlers(ARWEvents.USER_EXIT_ROOM, UserExitRoomEventHandler);

		arwServer.Connect(cfg);

	}
	
	// Update is called once per frame
	void Update () {
		arwServer.ProcessEvents();
	}

	private void ConnectionEventHandler(ARWObject obj, object eventParam){
		Debug.Log("Connection Success");
		arwServer.SendLoginRequest("cem", null);
	}

	private void ConnectionLostEventHandler(ARWObject obj, object eventParam){
		Debug.Log("Connection Lost");
	}

	private void DisConnectionEventHandler(ARWObject obj, object eventParam){
		Debug.Log("Disconnection");
	}

	private void LoginEventHandler(ARWObject obj, object eventParam){
		ARWUser loginnedUser = (ARWUser)eventParam;
		Debug.Log(loginnedUser.userName + " : " + loginnedUser.userId + " : " + loginnedUser.isMe);
	}

	private void JoinRoomEventHandler(ARWObject obj, object eventParam){
		ARWRoom joinedRoom = (ARWRoom)eventParam;
		Debug.Log(joinedRoom.name + " : " + joinedRoom.id + " : " + joinedRoom.userList.Length);
	}

	private void UserEnterRoomEventHandler(ARWObject obj, object eventParam){
		ARWUser newUser = (ARWUser)eventParam;
		Debug.Log("User Enter Room = " + newUser.userName + " : " + newUser.userId + " : " + newUser.isMe);
	}

	private void UserExitRoomEventHandler(ARWObject obj, object eventParam){
		ARWUser newUser = (ARWUser)eventParam;
		Debug.Log("User Exit Room = " + newUser.userName + " : " + newUser.userId + " : " + newUser.isMe);
	}

	private void OnApplicationQuit(){
		arwServer.Disconnection();
	}
}
