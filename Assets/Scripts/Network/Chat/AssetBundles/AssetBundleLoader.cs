using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleLoader : MonoBehaviour
{
    public void LoadAssetBundle(string resourceUrl, System.Action<GameObject> Callback)
    {
        StartCoroutine(LoadAssetBundleCoroutine(resourceUrl, Callback));
    }

    IEnumerator LoadAssetBundleCoroutine(string resourceUrl, System.Action<GameObject> Callback)
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(resourceUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

            GameObject[] bundleObjects = bundle.LoadAllAssets<GameObject>();

            if (bundleObjects.Length != 1)
            {
                Debug.Log("Bundle pack can't be loaded");
            }
            else
            {
                Callback(Instantiate(bundleObjects[0]));
            }

            bundle.Unload(false);
        }

        Destroy(gameObject);
    }
}
