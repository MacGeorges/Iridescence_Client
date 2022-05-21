using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkUtility
{
    public static string SerializeRequest(NetworkRequest request)
    {
        return JsonUtility.ToJson(request);
    }
}
