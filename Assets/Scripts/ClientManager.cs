using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public NetworkUser user;

    public static ClientManager instance;

    void Start()
    {
        instance = this;
        AsynchronousClient.StartClient();
    }
}
