using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Talk{

#region Private_Variables
	private int _talkId;
	private int _recieverPlayerId;
	private int _senderPlayerId;
	private string _recieverName;
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

	public string recieverName{
		get{	return _recieverName;		}
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
		this._recieverName = talkData.GetString("reciever_name");
		
		for(int ii = 0; ii< talkData.GetField("talk_messages").list.Count; ii++){
			JSONObject currentMessage = talkData.GetField("talk_messages").list[ii];
			Message newMessage = new Message(currentMessage);

			this._talkMessages.Add(newMessage);
		}
	}
}