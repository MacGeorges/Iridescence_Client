using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServersManager : MonoBehaviour
{
    public List<ServerHandler> connectedServers;

    public static ServersManager instance;

    private void Awake()
    {
        instance = this;
    }
}
