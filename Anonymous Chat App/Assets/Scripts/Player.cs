using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Player{

#region Private_Variables
	private string _playerId;
	private string _playerName;
	private string _language;
	private System.DateTime _createdDate;
	private List<Talk> _playerTalks;
#endregion

#region Public_Variables
	public string playerId{
		get{	return _playerId;	}
	}

	public string playerName{
		get{	return _playerName;	}
	}

	public string language{
		get{	return _language;	}
	}

	public Talk[] playerTalks{
		get{	return _playerTalks.ToArray();	}
	}
#endregion

	public Player(JSONObject playerData){
		this._playerTalks = new List<Talk>();

		this._playerId = playerData.GetString("player_id");
		this._playerName = playerData.GetString("player_nickname");
		this._language = playerData.GetString("language");
		// this._createdDate = System.DateTime.Parse(playerData.GetString("created_date"));

		for(int ii = 0; ii < playerData.GetField("player_talks").list.Count; ii++){
			JSONObject currentTalkData = playerData.GetField("player_talks").list[ii];
			Talk newTalk = new Talk(currentTalkData);

			this._playerTalks.Add(newTalk);
		}
	}

	public void AddTalk(Talk talk){
		this._playerTalks.Add(talk);
	}
}