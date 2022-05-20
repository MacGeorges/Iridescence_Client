using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    void Start()
    {
        AsynchronousClient.StartClient();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ServersManager.instance.connectedServers[0].Send("Client just pressed a key!<EOF>");
        }
    }
}
