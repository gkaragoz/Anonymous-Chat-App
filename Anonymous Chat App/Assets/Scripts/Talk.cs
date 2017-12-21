using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MaterialUI;

using ARW;
using ARW.Com;
using ARW.Requests;

public class Talk{

#region Private_Variables
	private int _talkId;
	private string _recieverPlayerId;
	private string _senderPlayerId;
	private string _receiverName;
	private List<Message> _talkMessages;
	private float tempDelta;

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

	public bool canISendMsg = true;
#endregion

	public Talk(JSONObject talkData){
		this.tempDelta = 0;
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

	public void AddMessage(Message newMsg){
		this._talkMessages.Add(newMsg);

		if(newMsg.senderPlayerId != this.senderPlayerId){
			this.canISendMsg = true;
		}

		if(AppManager.instance.currentTalk == null || this.talkId != AppManager.instance.currentTalk.talkId)		return;

		float delta = newMsg.InitMessage(this, this.talkMessages.Length - 1, tempDelta, 640);
		if(delta > 30)	this.tempDelta += delta;
	}

	public void EnterTalk(){
		ChatPanelManager.instance.currentTalkText.GetComponent<Text>().text = this.receiverName;
		AppManager.instance.appStatus = AppManager.AppStatus.CONVERSATION;
		AppManager.instance.currentTalk = this;

		ChatPanelManager.instance.talksScreen.gameObject.SetActive(false);

		if(this.talkMessages.Length == 1 && this.talkMessages[0].senderPlayerId == ChatPanelManager.instance.user.playerId){
			this.canISendMsg = false;
		}
		for(int ii = this.talkMessages.Length -1 ; ii >= 0; ii--){

			Message currentMessage = this.talkMessages[ii];

			float x = currentMessage.InitMessage(this, this.talkMessages.Length - ii - 1, tempDelta);
			if( x>30)	tempDelta+= x;
		}
		ChatPanelManager.instance.conversationScreen.gameObject.SetActive(true);

		ChatPanelManager.instance.sendMessageInputField.text = String.Empty;
		ChatPanelManager.instance.sendMessageButton.onClick.RemoveAllListeners();
		ChatPanelManager.instance.sendMessageButton.onClick.AddListener(delegate(){
			if(ChatPanelManager.instance.sendMessageInputField.text == "")		return;

			if(!this.canISendMsg){
				DialogManager.ShowAlert("You can not send more messages. Please wait response.", "Spam Alert!", MaterialIconHelper.GetIcon(MaterialIconEnum.ADD_ALERT));
				return;
			}

			ARWObject obj = new IARWObject();
			obj.PutString("sender_id", this.senderPlayerId);
			obj.PutString("body", ChatPanelManager.instance.sendMessageInputField.text);
			obj.PutString("send_date", System.DateTime.Now.ToString());
			obj.PutInt("talk_id", this.talkId);

			Debug.Log("Sending msj : " + this.talkId + " : " + this.senderPlayerId);
			ARWServer.instance.SendExtensionRequest("SendMessage", obj, false);
			ChatPanelManager.instance.sendMessageInputField.text = "";

			if(this.talkMessages.Length == 0 && this.senderPlayerId == ChatPanelManager.instance.user.playerId){
				this.canISendMsg = false;
			}
		});

		ChatPanelManager.instance.conversationScreen.GetChild(0).GetComponent<ScrollRect>().normalizedPosition = Vector2.zero;
	}

	public void CloseTalk(){
		AppManager.instance.appStatus = AppManager.AppStatus.TALK_SCREEN;

		AppManager.instance.currentTalk = null;
	}
}