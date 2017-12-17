using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using ARW;
using ARW.Com;
using ARW.Requests;

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
	private Button newConversationButton;
	public Transform messagesParent;

	public ChatPanelManager(Player user){
		this._user = user;
		instance = this;

		this.canvas = GameObject.Find("Canvas").transform;
		
		this.talksScreen = this.canvas.Find("TalksScreen");
		this.talksParent = this.talksScreen.Find("TalksListSection").GetChild(0).GetChild(0);

        this.newConversationButton = this.talksScreen.Find("TalksListSection").Find("New Conversation").GetComponent<Button>();
        this.newConversationButton.onClick.AddListener(delegate(){
			ARWObject obj = new IARWObject();
			ServerManager.instance.arwServer.SendExtensionRequest("FindConversation",obj, false);
		});

		this.conversationScreen = this.canvas.Find("ConversationScreen");
		this.messagesParent = this.conversationScreen.Find("MessagesParent");

		InitializeTalksScreen();
	}
	
	public void InitializeTalksScreen(){
		if(this.user == null)		return;

		AppManager.instance.appStatus = AppManager.AppStatus.TALK_SCREEN;

		for(int ii = 0; ii < this.user.playerTalks.Length; ii++){
			Talk currentTalk = this.user.playerTalks[ii];
			Transform newConversation = (Transform)MonoBehaviour.Instantiate(Resources.Load<Transform>("Talks/Conversation"));
			newConversation.Find("talk_button").GetComponent<Button>().onClick.AddListener(currentTalk.EnterTalk);
			newConversation.Find("user_name").GetComponent<Text>().text = currentTalk.receiverName;
			newConversation.SetParent(this.talksParent);
			newConversation.localPosition = Vector3.zero + new Vector3(532,-175 * ii,0);
			newConversation.eulerAngles = Vector3.zero;
		}
	}

	public void OpenConversation(Talk conversation){
		AppManager.instance.appStatus = AppManager.AppStatus.CONVERSATION;
		AppManager.instance.currentTalk = conversation;

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

	public void OpenTalksScreen(){
		AppManager.instance.currentTalk.CloseTalk();
		AppManager.instance.currentTalk = null;
		this.conversationScreen.gameObject.SetActive(false);
		this.talksScreen.gameObject.SetActive(true);
		AppManager.instance.appStatus = AppManager.AppStatus.TALK_SCREEN;
	}
}

public class MessagesPoolSystem{

	public static MessagesPoolSystem instance;

	private List<MessagePrefab> messages;
	private Transform poolSystemParent;

	public MessagesPoolSystem(Transform poolSystem){
		this.poolSystemParent = poolSystem;
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

	public void Reset(){
		MessagePrefab[] usingPrefabs = this.messages.Where(a=>a.isUsing == true).ToArray();

		foreach(MessagePrefab p in usingPrefabs){
			p.isUsing = false;
			p.transform.SetParent(this.poolSystemParent);
		}
	}
}