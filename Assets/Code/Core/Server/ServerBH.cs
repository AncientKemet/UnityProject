using UnityEngine;
using System.Collections;
using OldBlood.Code.Core.Server;

public class ServerBH : MonoBehaviour
{

    private Server server;

    void OnEnable()
    {
        server = new Server();;
        server.StartServer();
    }

    void OnDisable()
    {
        server.Stop();
    }
	
	void FixedUpdate () {
        server.ServerUpdate();
	}
}
