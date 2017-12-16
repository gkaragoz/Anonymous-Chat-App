using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Talk{

#region Private_Variables
	private int _talkId;
	private int _recieverPlayerId;
	private int _senderPlayerId;
	private string _receiverName;
	private List<Message> _talkMessages;
#endregion

#region Public_Variables
	public int talkId{
		get{	return _talkId;				}
	}

	public int recieverPlayerId{
		get{	return _recieverPlayerId;		}
	}

	public int senderPlayerId{
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
		this._recieverPlayerId = int.Parse(talkData.GetString("reciver_id"));
		this._senderPlayerId = int.Parse(talkData.GetString("sender_id"));
		this._receiverName = talkData.GetString("receiver_name");
		
		for(int ii = 0; ii< talkData.GetField("talk_messages").list.Count; ii++){
			JSONObject currentMessage = talkData.GetField("talk_messages").list[ii];
			Message newMessage = new Message(currentMessage);

			this._talkMessages.Add(newMessage);
		}
	}

	public void EnterTalk(){
		ChatPanelManager.instance.OpenConversation(this);
	}

	public void CloseTalk(){
		MessagesPoolSystem.instance.Reset();
	}
}