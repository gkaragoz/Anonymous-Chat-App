using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Talk{

#region Private_Variables
	private int _talkId;
	private Player _recieverPlayer;
	private Player _senderPlayer;
	private List<Message> _talkMessages;
#endregion

#region Public_Variables
	public int talkId{
		get{	return _talkId;				}
	}

	public Player recieverPlayer{
		get{	return _recieverPlayer;		}
	}

	public Player senderPlayer{
		get{	return _senderPlayer;		}
	}

	public Message[] talkMessages{
		get{	return _talkMessages.OrderByDescending(a=>a.sendDate).ToArray();	}
	}
#endregion
}