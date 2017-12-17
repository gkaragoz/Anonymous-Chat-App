using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class PlayServicesManager {
	
    public static PlayServicesManager instance;

    public PlayServicesManager(){
        instance = this;
    }

	public void SignIn(){
        // authenticate user:
        Social.localUser.Authenticate((bool success) => {
            // handle success or failure
            Debug.Log("Google Play LOGIN: Success");
        });
    }

    public string GetId(){
        if (Social.localUser.authenticated)
            return Social.localUser.id;
        else
            return "-1";
    }
}
