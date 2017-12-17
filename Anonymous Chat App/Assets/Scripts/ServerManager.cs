using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using ARW;
using ARW.Com;
using ARW.Config;
using ARW.Users;
using ARW.Events;
using ARW.Requests.Extensions;

public class ServerManager : MonoBehaviour{

	public static ServerManager instance;

	public ARWServer arwServer;

	private string GETUSERDATA = "GetUserData";
	private string SENDMESSAGE = "SendMessage";
	private string FINDCONVERSATION = "FindConversation";
	private string FINDEDCONVERSATION = "FindedConversation";
	private string CANNOTFINDACTIVEUSER = "CannotFindActiveUser";

	private void Awake(){
		instance = this;
	}

	public bool canLogin = false;
	public void Init(){
		arwServer = new ARWServer();
		arwServer.Init();

		ConfigData cfg = new ConfigData(8081, 9933, "192.168.1.105");

		arwServer.AddEventHandlers(ARWEvents.CONNECTION, OnConnectionSuccess);
		arwServer.AddEventHandlers(ARWEvents.LOGIN, OnLoginSuccess);

		arwServer.AddExtensionRequest(GETUSERDATA, GetUserData);
		arwServer.AddExtensionRequest(FINDEDCONVERSATION, FindedConversationHandler);
		arwServer.AddExtensionRequest(CANNOTFINDACTIVEUSER, CannotFindActiveUser);

		arwServer.Connect(cfg);
	}

	private void Update(){
		if(this.arwServer != null){
			arwServer.ProcessEvents();
		}
	}

	private void OnConnectionSuccess(ARWObject obj, object value){
		Debug.Log("Connection Success");
		AppManager.instance.appStatus = AppManager.AppStatus.Connection;
	}

	private void OnLoginSuccess(ARWObject arwObj, object value){
		Debug.Log("Login Success");
		this.canLogin = true;

		if(PlayerPrefs.GetString("playerName") == ""){
			ChatPanelManager.instance.welcomeScreen.gameObject.SetActive(true);
		}else{
			ARWObject obj = new IARWObject();
			obj.PutString("player_id", AppManager.instance.googlePlayAccountId);
			obj.PutString("player_nickname", PlayerPrefs.GetString("playeName"));
			obj.PutString("language", Application.systemLanguage.ToString());
			ARWServer.instance.SendExtensionRequest("GetUserData", obj, false);
		}
	}

	private void GetUserData(ARWObject obj, object value){
		if(obj.GetString("error") == ""){
			AppManager.instance.InitPlayer(obj.GetString("player_data"));
		}else{
			Debug.Log("GetUserData Error : " + obj.GetString("error"));
		}
	}

	private void FindedConversationHandler(ARWObject obj, object value){
		string newTalkData = obj.GetString("talk_data");

		JSONObject talkJson = new JSONObject(newTalkData);
		Talk newTalk = new Talk(talkJson);
		ChatPanelManager.instance.user.AddTalk(newTalk);

		Debug.Log(newTalk.talkId + " : " + newTalk.receiverName + " : " + newTalk.talkMessages.Length);
	}

	private void CannotFindActiveUser(ARWObject obj, object value){

	}

    private void OnApplicationQuit()
    {
        arwServer.Disconnection();
    }
}