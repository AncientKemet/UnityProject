using UnityEngine;
using System.Collections;
using OldBlood.Code.Core.Server;

public class ServerBH : MonoBehaviour {

    void OnEnable()
    {
        Server.Instance.StartServer();
    }

    void OnDisable()
    {
        Server.Instance.Stop();
    }
	
	void FixedUpdate () {
        Server.Instance.ServerUpdate();
	}
}
