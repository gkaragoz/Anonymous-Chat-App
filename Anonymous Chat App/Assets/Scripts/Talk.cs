using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Talk{

#region Private_Variables
	private int _talkId;
	private string _recieverPlayerId;
	private string _senderPlayerId;
	private string _receiverName;
	private List<Message> _talkMessages;
#endregion

#region Public_Variables
	public int talkId{
		get{	return _talkId;				}
	}

	public string recieverPlayerId{
		get{	return _recieverPlayerId;		}
	}

	public string senderPlayerId{
		get{	return _senderPlayerId;		}
	}

	public string receiverName{
		get{	return _receiverName;		}
	}

	public Message[] talkMessages{
		get{	return _talkMessages.OrderByDescending(a=>a.sendDate).ToArray();	}
	}
#endregion

	public Talk(JSONObject talkData){
		this._talkMessages = new List<Message>();

		this._talkId = int.Parse(talkData.GetString("talk_id"));
		this._recieverPlayerId = talkData.GetString("receiver_id");
		this._senderPlayerId = talkData.GetString("sender_id");
		this._receiverName = talkData.GetString("receiver_name");
		
		for(int ii = 0; ii< talkData.GetField("talk_messages").list.Count; ii++){
			JSONObject currentMessage = talkData.GetField("talk_messages").list[ii];
			Message newMessage = new Message(currentMessage);

			this._talkMessages.Add(newMessage);
		}
	}

	public void EnterTalk(){
		
		AppManager.instance.appStatus = AppManager.AppStatus.CONVERSATION;
		AppManager.instance.currentTalk = this;

		ChatPanelManager.instance.talksScreen.gameObject.SetActive(false);

		float tempDelta = 0;
		for(int ii = this.talkMessages.Length -1 ; ii >= 0; ii--){

			Message currentMessage = this.talkMessages[ii];

			float x = currentMessage.InitMessage(this.talkMessages.Length - ii - 1, tempDelta);
			if( x>30)	tempDelta+= x;
		}
		ChatPanelManager.instance.conversationScreen.gameObject.SetActive(true);
	}

	public void CloseTalk(){
		
	}
}