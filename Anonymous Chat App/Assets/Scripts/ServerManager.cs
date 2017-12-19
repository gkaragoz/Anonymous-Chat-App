using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using ARW;
using ARW.Com;
using ARW.Config;
using ARW.Users;
using ARW.Events;
using ARW.Requests.Extensions;
using MaterialUI;

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

		ConfigData cfg = new ConfigData(8081, 9933, "165.227.135.227");

		arwServer.AddEventHandlers(ARWEvents.CONNECTION, OnConnectionSuccess);
		arwServer.AddEventHandlers(ARWEvents.CONNECTION_LOST, OnConnectionLost);
		arwServer.AddEventHandlers(ARWEvents.LOGIN, OnLoginSuccess);

		arwServer.AddExtensionRequest(GETUSERDATA, GetUserData);
		arwServer.AddExtensionRequest(FINDEDCONVERSATION, FindedConversationHandler);
		arwServer.AddExtensionRequest(CANNOTFINDACTIVEUSER, CannotFindActiveUser);
		arwServer.AddExtensionRequest("Register", RegisterHandler);
		arwServer.AddExtensionRequest("SendMessage", SendMessageHandler);

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
		AppManager.instance.screenView.Transition(0);
	}

	private void OnConnectionLost(ARWObject obj, object value){
		Debug.Log("Connection Fail");
		DialogManager.ShowAlert("Please check your internet connection.", "Alert!", MaterialIconHelper.GetIcon(MaterialIconEnum.ADD_ALERT));
		ChatPanelManager.instance.welcomeScreen.gameObject.SetActive(false);
	}

	private void OnLoginSuccess(ARWObject arwObj, object value){
		Debug.Log("Server Login Success");
		this.canLogin = true;

		string email = PlayerPrefs.GetString("player_id");
		string pass = PlayerPrefs.GetString("player_pass");

		AppManager.instance.inputEmailOnLogin.text = email;
		AppManager.instance.inputPasswordOnLogin.text = pass;
		// ARWObject obj = new IARWObject();
		// obj.PutString("player_id", email);
		// obj.PutString("player_password", pass);
		// arwServer.SendExtensionRequest("Login", obj, false);
	}

	private void GetUserData(ARWObject obj, object value){
		if(obj.GetString("error") == ""){
			// AppManager.instance.screenView.Transition(2);
			AppManager.instance.InitPlayer(obj.GetString("player_data"));
		}else{
			Debug.Log("GetUserData Error : " + obj.GetString("error"));
		}
	}

	private void RegisterHandler(ARWObject obj, object value){
		ChatPanelManager.instance.welcomeScreen.gameObject.SetActive(true);
	}

	private void FindedConversationHandler(ARWObject obj, object value){
		string newTalkData = obj.GetString("talk_data");

		JSONObject talkJson = new JSONObject(newTalkData);
		Talk newTalk = new Talk(talkJson);

        if (newTalk.receiverName == "")
        {
            DialogManager.ShowAlert("Server connection error.", "Alert!", MaterialIconHelper.GetIcon(MaterialIconEnum.ADD_ALERT));
            return;
        }

		ChatPanelManager.instance.user.AddTalk(newTalk);
		ChatPanelManager.instance.InitNewTalk(newTalk);

		Debug.Log(newTalk.talkId + " : " + newTalk.receiverName + " : " + newTalk.talkMessages.Length);
	}

	private void SendMessageHandler(ARWObject obj, object value){
		string messageData = obj.GetString("message_data");

		Message newMessage = new Message(new JSONObject(messageData));
		Talk currentTalk = ChatPanelManager.instance.user.playerTalks.Where(a=>a.talkId == newMessage.talkId).FirstOrDefault();

        if (currentTalk == null)
        {
            DialogManager.ShowAlert("Server connection error.", "Alert!", MaterialIconHelper.GetIcon(MaterialIconEnum.ADD_ALERT));
            return;
        }

		currentTalk.AddMessage(newMessage);
	}

	private void CannotFindActiveUser(ARWObject obj, object value){
		DialogManager.ShowAlert("There is no active user in server.", "Alert!", MaterialIconHelper.GetIcon(MaterialIconEnum.ADD_ALERT));
	}

    private void OnApplicationQuit(){
        arwServer.Disconnection();
    }
}