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

    public Transform registerScreen;
	public Transform talksScreen;
	public Transform welcomeScreen;
	public Transform conversationScreen;

	private Transform talksContentParent;
	private Button newConversationButton;

	public InputField sendMessageInputField;
	public Button sendMessageButton;
	public Transform messagesParent;

	public ChatPanelManager(Transform welcomeScreen, Transform registerScreen, Transform talksScreen, Transform conversationScreen){
		instance = this;

		this.welcomeScreen = welcomeScreen;
        this.registerScreen = registerScreen;
		this.talksScreen = talksScreen;
		this.conversationScreen = conversationScreen;

		this.talksContentParent = this.talksScreen.Find("Scroll View/Viewport/Content");

		this.sendMessageButton = this.conversationScreen.Find("pnlSendMessage/PanelLayer/btnSendMessage").GetComponent<Button>();
		this.sendMessageInputField = this.conversationScreen.Find("pnlSendMessage/PanelLayer/inputMessage").GetComponent<InputField>();

		this.newConversationButton = this.talksScreen.Find("New Conversation").GetComponent<Button>();
        this.newConversationButton.onClick.AddListener(delegate(){
			ARWObject obj = new IARWObject();
			ServerManager.instance.arwServer.SendExtensionRequest("FindConversation",obj, false);
		});

		talksScreen.gameObject.SetActive(false);
		conversationScreen.gameObject.SetActive(false);
        registerScreen.gameObject.SetActive(false);
	}

	public void InitPanel(Player user){
		this.welcomeScreen.gameObject.SetActive(false);
		this.talksScreen.gameObject.SetActive(true);
		this._user = user;

		InitializeTalksScreen();
	}
	
	public void InitializeTalksScreen(int xOffset = 0){
		if(this.user == null)		return;
		this.welcomeScreen.gameObject.SetActive(false);
		this.talksScreen.gameObject.SetActive(true);

		AppManager.instance.appStatus = AppManager.AppStatus.TALK_SCREEN;

		for(int ii = 0; ii < this.user.playerTalks.Length; ii++){
			Talk currentTalk = this.user.playerTalks[ii];
			Transform newConversation = (Transform)MonoBehaviour.Instantiate(Resources.Load<Transform>("Talks/Conversation"));
			newConversation.Find("PanelLayer/btnStartTalk").GetComponent<Button>().onClick.AddListener(currentTalk.EnterTalk);
			newConversation.Find("PanelLayer/txtNickname").GetComponent<Text>().text = currentTalk.receiverName;
			newConversation.SetParent(this.talksContentParent);
			newConversation.localPosition = Vector3.zero + new Vector3(0,-200 * ii, 0);

			newConversation.localPosition += new Vector3(xOffset, -150,0);

			newConversation.localScale = Vector3.one;
			newConversation.eulerAngles = Vector3.zero;
		}
	}

	public void InitNewTalk(Talk newTalk){
		Transform newConversation = (Transform)MonoBehaviour.Instantiate(Resources.Load<Transform>("Talks/Conversation"));
		newConversation.Find("PanelLayer/btnStartTalk").GetComponent<Button>().onClick.AddListener(newTalk.EnterTalk);
		newConversation.Find("PanelLayer/txtNickname").GetComponent<Text>().text = newTalk.receiverName;
		newConversation.SetParent(this.talksContentParent);
		newConversation.localPosition = Vector3.zero + new Vector3(0,-200 * (user.playerTalks.Length-1), 0);

		newConversation.localPosition += new Vector3(640, -150,0);

		newConversation.localScale = Vector3.one;
		newConversation.eulerAngles = Vector3.zero;
	}

	public void OpenTalksScreen(){
		AppManager.instance.currentTalk.CloseTalk();
		AppManager.instance.currentTalk = null;
		this.conversationScreen.gameObject.SetActive(false);
		this.talksScreen.gameObject.SetActive(true);
		AppManager.instance.appStatus = AppManager.AppStatus.TALK_SCREEN;
	}
}