using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class AppManager : MonoBehaviour{

	private void Awake(){
		TextAsset playerData = Resources.Load<TextAsset>("ExamplePlayer");
		JSONObject playerJson = new JSONObject(playerData.text);

		Player me = new Player(playerJson);
		Debug.Log(me.playerName + " : " + me.playerId + " : " + me.playerTalks.Length);

		new ChatPanelManager(me);
	}
}