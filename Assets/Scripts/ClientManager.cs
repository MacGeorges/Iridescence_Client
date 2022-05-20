using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    void Start()
    {
        Thread listenerThread = new Thread(AsynchronousClient.StartClient);
        listenerThread.Start();
    }
}
