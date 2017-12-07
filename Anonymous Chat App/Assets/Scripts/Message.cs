using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Message{

#region Private_Variables
	private int _messageId;
	private string _body;
	private System.DateTime _sendDate;
	private int _senderPlayerId;
	private int _talkId;
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

	public int senderPlayerId{
		get{	return _senderPlayerId;	}
	}

	public int talkId{
		get{	return _talkId;		}
	}
#endregion

	public Message(JSONObject messageData){
		this._messageId = int.Parse(messageData.GetString("message_id"));
		this._body = messageData.GetString("body");
		this._sendDate = System.DateTime.Parse(messageData.GetString("send_date"));
		this._senderPlayerId = int.Parse(messageData.GetString("sender_id"));
		this._talkId = int.Parse(messageData.GetString("talk_id"));
	}
}