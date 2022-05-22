using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CloudWrapperHelper : MonoBehaviour
{
    public void SendRequest_GET(string URL, System.Action<string> Callback)
    {
        StartCoroutine(Request_Coroutine_GET(URL, Callback));
    }

    public void SendRequest_POST(string URL, string Data, System.Action<string> Callback)
    {
        StartCoroutine(Request_Coroutine_POST(URL, Data, Callback));
    }

    IEnumerator Request_Coroutine_GET(string uri, System.Action<string> Callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("Connection Error: " + webRequest.error);

                    if (Callback != null)
                    {
                        Callback(webRequest.error);
                    }
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log("Error: " + webRequest.error);

                    if (Callback != null)
                    {
                        Callback(webRequest.error);
                    }
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log("HTTP Error: " + webRequest.error);

                    if (Callback != null)
                    {
                        Callback(webRequest.error);
                    }
                    break;
                case UnityWebRequest.Result.Success:
                    if (Callback != null)
                    {
                        Callback(webRequest.downloadHandler.text);
                    }
                    break;
            }
        }

        DestroyImmediate(gameObject);
    }

    IEnumerator Request_Coroutine_POST(string uri, string data, System.Action<string> Callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("Data", data);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("Connection Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log("HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    if (Callback != null)
                    {
                        Callback(webRequest.downloadHandler.text);
                    }
                    break;
            }
        }

        DestroyImmediate(gameObject);
    }
}
