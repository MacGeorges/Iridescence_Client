using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CloudWrapper
{
    public static string Cloud_Access_URL = "http://127.0.0.1/Iridescence/Cloud_Access/";
    //public static string Assets_Storage_URL = "http://127.0.0.1/";

    public static void GetAllRegionElements(System.Action<string> Callback)
    {
        SendRequest(RequestMethod.Get, Cloud_Access_URL + "GetAllRegionElements.php", Callback);
    }

    public static void GetPartData(int partID, System.Action<string> Callback)
    {
        SendRequest(RequestMethod.Get, Cloud_Access_URL + "GetPartData.php?PartID=" + partID, Callback);
    }

    private static CloudWrapperHelper BuildHelper()
    {
        GameObject wrapperHelper = new GameObject();
        return wrapperHelper.AddComponent<CloudWrapperHelper>();
    }

    private static void SendRequest(RequestMethod requestMethod, string URL, System.Action<string> Callback = null, string data = "")
    {
        CloudWrapperHelper wrapperHelper = BuildHelper();

        switch (requestMethod)
        {
            case RequestMethod.Get:
                wrapperHelper.SendRequest_GET(URL, Callback);
                break;
            case RequestMethod.Post:
                wrapperHelper.SendRequest_POST(URL, data, Callback);
                break;
        }
    }
}
