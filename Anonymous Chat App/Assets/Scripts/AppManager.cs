using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using ARW;
using ARW.Com;
using ARW.Requests;
using ARW.Events;

public class AppManager : MonoBehaviour{

	public static AppManager instance;

	public string googlePlayAccountId;
	public Transform poolSystemParent;
	
	public Talk currentTalk;

	public AppStatus appStatus;
	public enum AppStatus{
		None,
		Connection,
		LOGIN,
		TALK_SCREEN,
		CONVERSATION
	}

	private Button loginButton;

	private void Start(){
		instance = this;
		GameObject canvas = GameObject.Find("Canvas");

		new ChatPanelManager(canvas.transform.Find("Screen View/WelcomeScreen"),
			canvas.transform.Find("Screen View/TalksScreen"), 
			canvas.transform.Find("Screen View/ConversationScreen"));

		this.loginButton = canvas.transform.Find("Screen View/WelcomeScreen/pnlWelcome/PanelLayer/btnStart").GetComponent<Button>();
		this.loginButton.onClick.AddListener(delegate(){
			if(!ServerManager.instance.canLogin)		return;
			string nickname = GameObject.Find("Canvas/Screen View/WelcomeScreen/pnlWelcome/PanelLayer/inputNickname").GetComponent<InputField>().text;

			Debug.Log(AppManager.instance.googlePlayAccountId);
			ARWObject obj = new IARWObject();
			obj.PutString("player_id", AppManager.instance.googlePlayAccountId);
			obj.PutString("player_nickname", nickname);
			obj.PutString("language", Application.systemLanguage.ToString());
			ARWServer.instance.SendExtensionRequest("GetUserData", obj, false);
		});
		
		new PlayServicesManager();

		ServerManager.instance.Init();

		PlayServicesManager.instance.SignIn();
		this.googlePlayAccountId = PlayServicesManager.instance.GetId();
		if(this.googlePlayAccountId == "-1"){
			Application.Quit();
			return;
		}

		ServerManager.instance.arwServer.SendLoginRequest(AppManager.instance.googlePlayAccountId, null);
		// TextAsset playerData = Resources.Load<TextAsset>("ExamplePlayer");
	}

	public void InitPlayer(string playerData){
		JSONObject playerJson = new JSONObject(playerData);
		Player me = new Player(playerJson);
		Debug.Log(me.playerName + " : " + me.playerId + " : " + me.playerTalks.Length);
		PlayerPrefs.SetString("playerName", me.playerName);

		// new MessagesPoolSystem(poolSystemParent);
		ChatPanelManager.instance.InitPanel(me);
	}

	private void FixedUpdate(){
		if(this.appStatus == AppStatus.CONVERSATION){
			if(Input.GetKeyDown(KeyCode.Escape) && this.currentTalk != null){
				ChatPanelManager.instance.OpenTalksScreen();
			}
		}
	}
}