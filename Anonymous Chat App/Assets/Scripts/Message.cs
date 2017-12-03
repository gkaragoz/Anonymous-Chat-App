using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Message{

#region Private_Variables
	private int _messageId;
	private string _body;
	private System.DateTime _sendDate;
	private Player _senderPlayer;
	private Talk _talk;
#endregion

#region Public_Variables
	public int messageId{
		get{	return _messageId;	}
	}

	public string body{
		get{	return _body;		}
	}

	public System.DateTime sendDate{
		get{	return _sendDate;	}
	}

	public Player senderPlayer{
		get{	return _senderPlayer;	}
	}

	public Talk talk{
		get{	return _talk;		}
	}
#endregion
}