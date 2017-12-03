using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Player{

#region Private_Variables
	private int _playerId;
	private string _playerName;
	private string _language;
	private System.DateTime _createdDate;
	private List<Talk> _playerTalks;
#endregion

#region Public_Variables
	public int playerId{
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

#region Public_Methods

#endregion
}