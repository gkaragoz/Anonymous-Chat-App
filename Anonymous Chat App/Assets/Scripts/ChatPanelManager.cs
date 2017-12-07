using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class ChatPanelManager{

	public static ChatPanelManager instance;

	private Player _user;
	public Player user{
		get{	return _user;	}
	}

	private Transform canvas;
	private Transform talksScreen;
	private Transform talksParent;
	private Transform conversationScreen;
	public Transform messagesParent;

	public ChatPanelManager(Player user){
		this._user = user;
		instance = this;

		this.canvas = GameObject.Find("Canvas").transform;
		
		this.talksScreen = this.canvas.Find("TalksScreen");
		this.talksParent = this.talksScreen.Find("TalksListSection").GetChild(0).GetChild(0);

		this.conversationScreen = this.canvas.Find("ConversationScreen");
		this.messagesParent = this.conversationScreen.Find("MessagesParent");

		Debug.Log(this.canvas +  " : " + this.talksParent);
		InitializeTalksScreen();
	}
	
	public void InitializeTalksScreen(){
		if(this.user == null)		return;

		AppManager.instance.appStatus = AppManager.AppStatus.TALK_SCREEN;

		for(int ii = 0; ii < this.user.playerTalks.Length; ii++){
			Talk currentTalk = this.user.playerTalks[ii];
			Transform newConversation = (Transform)MonoBehaviour.Instantiate(Resources.Load<Transform>("Talks/Conversation"));
			newConversation.Find("talk_button").GetComponent<Button>().onClick.AddListener(currentTalk.EnterTalk);
			newConversation.Find("user_name").GetComponent<Text>().text = currentTalk.recieverName;
			newConversation.SetParent(this.talksParent);
			newConversation.localPosition = Vector3.zero + new Vector3(532,-175 * ii,0);
			newConversation.eulerAngles = Vector3.zero;
		}
	}

	public void OpenConversation(Talk conversation){
		AppManager.instance.appStatus = AppManager.AppStatus.CONVERSATION;
		this.talksScreen.gameObject.SetActive(false);
		for(int ii = 0; ii < conversation.talkMessages.Length; ii++){
			Message currentMessage = conversation.talkMessages[ii];
			if(currentMessage.isMe){
				currentMessage.InitMessage(MessagesPoolSystem.instance.GetAvaibleMyMessage(), conversation.talkMessages.Length - ii - 1);
			}else{
				currentMessage.InitMessage(MessagesPoolSystem.instance.GetAvaibleotherUsersMessage(), conversation.talkMessages.Length - ii - 1);
			}	
		}
		this.conversationScreen.gameObject.SetActive(true);
	}
}

public class MessagesPoolSystem{

	public static MessagesPoolSystem instance;

	private List<MessagePrefab> messages;

	public MessagesPoolSystem(Transform poolSystem){
		this.messages = new List<MessagePrefab>();

		for(int ii = 0; ii < poolSystem.GetChildCount(); ii++){
			Transform currentChild = poolSystem.GetChild(ii);
			if(currentChild.GetComponent<MessagePrefab>()){
				this.messages.Add(currentChild.GetComponent<MessagePrefab>());
			}
		}

		instance = this;
	}

	public MessagePrefab GetAvaibleMyMessage(){
		return this.messages.Where(a=>a.isUsing == false && a.myMessage == true).FirstOrDefault();
	}

	public MessagePrefab GetAvaibleotherUsersMessage(){
		return this.messages.Where(a=>a.isUsing == false && a.myMessage == false).FirstOrDefault();
	}
}