using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class AppManager : MonoBehaviour{

	public static AppManager instance;

	public Transform poolSystemParent;
	
	public AppStatus appStatus;
	public enum AppStatus{
		LOGIN,
		TALK_SCREEN,
		CONVERSATION
	}

	private void Awake(){
		instance = this;

		TextAsset playerData = Resources.Load<TextAsset>("ExamplePlayer");
		JSONObject playerJson = new JSONObject(playerData.text);

		Player me = new Player(playerJson);
		Debug.Log(me.playerName + " : " + me.playerId + " : " + me.playerTalks.Length);

		new MessagesPoolSystem(poolSystemParent);
		new ChatPanelManager(me);
	}
}