using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public bool isMe{
		get{
			return this.senderPlayerId == ChatPanelManager.instance.user.playerId ? true : false;
		}
	}
	
#endregion

	public Message(JSONObject messageData){
		this._messageId = int.Parse(messageData.GetString("message_id"));
		this._body = messageData.GetString("body");
		this._sendDate = System.DateTime.Parse(messageData.GetString("send_date"));
		this._senderPlayerId = int.Parse(messageData.GetString("sender_id"));
		this._talkId = int.Parse(messageData.GetString("talk_id"));
	}

	public void InitMessage(MessagePrefab messageObj, int index){
		messageObj.isUsing = true;
		messageObj.msjText.text = this.body;
		messageObj.transform.SetParent(ChatPanelManager.instance.messagesParent);
		messageObj.transform.localPosition = new Vector3(0, 0 - 100 * index, 0);
		messageObj.transform.eulerAngles = Vector3.zero;
	}
}