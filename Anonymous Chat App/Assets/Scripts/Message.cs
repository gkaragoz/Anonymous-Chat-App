using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message{

#region Private_Variables
	private int _messageId;
	private string _body;
	private string _sendDate;
	private string _senderPlayerId;
	private int _talkId;
#endregion

#region Public_Variables
	public int messageId{
		get{	return _messageId;	}
	}

	public string body{
		get{	return _body;		}
	}

	public string sendDate{
		get{	return _sendDate;	}
	}

	public string senderPlayerId{
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
		this._sendDate = messageData.GetString("send_date");
		this._senderPlayerId = messageData.GetString("sender_id");
		this._talkId = int.Parse(messageData.GetString("talk_id"));
	}

	public float InitMessage(Talk talk,int index, float tempDelta, int offset = 0){
		
		string prefabPath = this.isMe == true ? "pnlSenderMessage" : "pnlReceiverMessage";
		Transform messageObj = (Transform)MonoBehaviour.Instantiate(Resources.Load<Transform>("Talks/" + prefabPath), Vector3.zero, Quaternion.identity);

		Text msjText = messageObj.Find("PanelLayer/messageContent").GetComponent<Text>();
		msjText.text = this.body;

		float delta = 35 * msjText.preferredHeight / 25;
		if(delta >= 30f)
			messageObj.GetComponent<RectTransform>().sizeDelta += new Vector2(0, delta);

		messageObj.SetParent(AppManager.instance.messageObjectParent);
		messageObj.localEulerAngles = Vector2.zero;
		messageObj.localScale = Vector3.one;

		messageObj.localPosition = new Vector3(-600 + offset, -tempDelta -90 - 160 * index, 0);

		talk.msgObjs.Add(messageObj);
		return delta;
	}
}