using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

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

	private void Start(){
		instance = this;

		new PlayServicesManager();

		PlayServicesManager.instance.SignIn();

		this.googlePlayAccountId = PlayServicesManager.instance.GetId();
		if(this.googlePlayAccountId == "-1"){
			Application.Quit();
			return;
		}

		ServerManager.instance.Init();
		// TextAsset playerData = Resources.Load<TextAsset>("ExamplePlayer");
	}

	public void InitPlayer(string playerData){
		JSONObject playerJson = new JSONObject(playerData);
		Player me = new Player(playerJson);
		Debug.Log(me.playerName + " : " + me.playerId + " : " + me.playerTalks.Length);

		new MessagesPoolSystem(poolSystemParent);
		new ChatPanelManager(me);
	}

	private void FixedUpdate(){
		if(this.appStatus == AppStatus.CONVERSATION){
			if(Input.GetKeyDown(KeyCode.Escape) && this.currentTalk != null){
				ChatPanelManager.instance.OpenTalksScreen();
			}
		}
	}
}