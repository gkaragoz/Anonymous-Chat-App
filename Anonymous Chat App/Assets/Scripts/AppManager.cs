using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using ARW;
using ARW.Com;
using ARW.Requests;
using ARW.Events;

using MaterialUI;

public class AppManager : MonoBehaviour{

	public static AppManager instance;

	public string googlePlayAccountId;
	public Transform poolSystemParent;
	
	public Talk currentTalk;

	public AppStatus appStatus;
	public enum AppStatus{
		None,
		Connection,
		LOGIN,
		TALK_SCREEN,
		CONVERSATION
	}

    public Transform[] conversationPrefabs;
	private Button loginButton;
    private Button signupButton;
    private Button registerButton;
    public InputField inputPasswordOnSignup;
    public InputField inputEmailOnSignup;
    public InputField inputEmailOnLogin;
    public InputField inputPasswordOnLogin;
    public InputField inputNickname;
    public ScreenView screenView;

    public Transform messageObjectParent;

	private void Start(){
		instance = this;

        if (Application.internetReachability == NetworkReachability.NotReachable)
            DialogManager.ShowAlert("Please check your internet connection.", "Alert!", MaterialIconHelper.GetIcon(MaterialIconEnum.ADD_ALERT));

        GameObject canvas = GameObject.Find("Canvas");

		this.screenView = canvas.transform.Find("Screen View").GetComponent<ScreenView>();
        this.loginButton = canvas.transform.Find("Screen View/WelcomeScreen/pnlWelcome/PanelLayer/btnStart").GetComponent<Button>();
        this.signupButton = canvas.transform.Find("Screen View/WelcomeScreen/pnlWelcome/PanelLayer/btnSignup").GetComponent<Button>();
        this.registerButton = canvas.transform.Find("Screen View/RegisterScreen/pnlRegister/PanelLayer/btnStart").GetComponent<Button>();
        this.inputPasswordOnSignup = canvas.transform.Find("Screen View/RegisterScreen/pnlRegister/PanelLayer/inputPassword").GetComponent<InputField>();
        this.inputEmailOnSignup = canvas.transform.Find("Screen View/RegisterScreen/pnlRegister/PanelLayer/inputEmail").GetComponent<InputField>();
        this.inputEmailOnLogin = canvas.transform.Find("Screen View/WelcomeScreen/pnlWelcome/PanelLayer/inputEmail").GetComponent<InputField>();
        this.inputPasswordOnLogin = canvas.transform.Find("Screen View/WelcomeScreen/pnlWelcome/PanelLayer/inputPassword").GetComponent<InputField>();
        this.inputNickname = canvas.transform.Find("Screen View/RegisterScreen/pnlRegister/PanelLayer/inputNickname").GetComponent<InputField>();

        this.messageObjectParent = canvas.transform.Find("Screen View/ConversationScreen/Scroll View/Viewport/Content");

        new ChatPanelManager(canvas.transform.Find("Screen View/WelcomeScreen"),
            canvas.transform.Find("Screen View/RegisterScreen"),
			canvas.transform.Find("Screen View/TalksScreen"), 
			canvas.transform.Find("Screen View/ConversationScreen"));

		this.loginButton.onClick.AddListener(delegate() {
            string email = inputEmailOnLogin.text;
            string password = inputPasswordOnLogin.text;

            if (email.Length <= 0 || password.Length <= 0)
            {
                DialogManager.ShowAlert("Please enter your email and password.", "Alert!", MaterialIconHelper.GetIcon(MaterialIconEnum.ADD_ALERT));
                return;
            }

            if (!ServerManager.instance.canLogin)
            {
                DialogManager.ShowAlert("Server connection error.", "Alert!", MaterialIconHelper.GetIcon(MaterialIconEnum.ADD_ALERT));
                return;
            }

			ARWObject obj = new IARWObject();
			obj.PutString("player_id", email);
			obj.PutString("player_password", password);
            Debug.Log("+++++");
			PlayerPrefs.SetString("player_id", email);
            PlayerPrefs.SetString("player_pass", password);
			ARWServer.instance.SendExtensionRequest("Login", obj, false);
		});

        this.registerButton.onClick.AddListener(delegate ()
        {
            string nickname = inputNickname.text;
            string password = inputPasswordOnSignup.text;
            string email = inputEmailOnSignup.text;
            string language = Application.systemLanguage.ToString();

            if (nickname.Length <= 0 || email.Length <= 0 || password.Length <= 0)
            {
                DialogManager.ShowAlert("Please enter your nickname, email and password.", "Alert!", MaterialIconHelper.GetIcon(MaterialIconEnum.ADD_ALERT));
                return;
            }

            if (!ServerManager.instance.canLogin)
            {
                DialogManager.ShowAlert("Server connection error.", "Alert!", MaterialIconHelper.GetIcon(MaterialIconEnum.ADD_ALERT));
                return;
            }

            ARWObject obj = new IARWObject();
            obj.PutString("player_id", email);
            obj.PutString("player_password", password);
            obj.PutString("language", language);
            obj.PutString("player_nickname", nickname);

            PlayerPrefs.SetString("player_id", email);
            PlayerPrefs.SetString("player_pass", password);

            ARWServer.instance.SendExtensionRequest("Register", obj, false);            
        });

		ServerManager.instance.Init();
		ServerManager.instance.arwServer.SendLoginRequest("GUEST", null);
		// TextAsset playerData = Resources.Load<TextAsset>("ExamplePlayer");
	}

	public void InitPlayer(string playerData){
		JSONObject playerJson = new JSONObject(playerData);
		
		Player me = new Player(playerJson);
		Debug.Log(me.playerName + " : " + me.playerId + " : " + me.playerTalks.Length);

        try{
            if(me.playerId.Length == 0){
                Debug.Log("xxxxxxxxx");
            }
        }catch(System.NullReferenceException){
            Debug.Log("Wrong Player Data " + PlayerPrefs.GetString("player_id"));
            ARWObject obj = new IARWObject();
            obj.PutString("player_id", PlayerPrefs.GetString("player_id"));
            ARWServer.instance.SendExtensionRequest("Relogin", null, false);
            return;
        }

        foreach(Talk t in me.playerTalks){
            Debug.Log(t.receiverName + " : " + t.talkId);
        }

		ChatPanelManager.instance.InitPanel(me);
	}

	private void FixedUpdate(){
		if(this.appStatus == AppStatus.CONVERSATION){
			if(Input.GetKeyDown(KeyCode.Escape) && this.currentTalk != null){
                if(this.currentTalk != null){
                    for(int ii = 0; ii< this.messageObjectParent.GetChildCount(); ii++){
                        Destroy(this.messageObjectParent.GetChild(ii).gameObject);
                    }
                }
                ChatPanelManager.instance.OpenTalksScreen();
			}
		}
	}
}