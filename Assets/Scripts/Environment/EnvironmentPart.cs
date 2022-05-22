using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentPart : MonoBehaviour
{
    public RegionElement partData;

    public GameObject objectRef;

    public void Init(RegionElement newPartData)
    {
        partData = newPartData;

        CloudWrapper.GetPartData(partData.modelID, SpawnPart);
    }

    private void SpawnPart(string partData)
    {
        Part part = JsonUtility.FromJson<Part>(partData);

        GameObject BundlerLoaderHelper = new GameObject();
        AssetBundleLoader BundleLoader = BundlerLoaderHelper.AddComponent<AssetBundleLoader>();
        BundleLoader.LoadAssetBundle(part.resourceURL, SpawnedPart);
    }

    private void SpawnedPart(GameObject spawnedPart)
    {
        objectRef = spawnedPart;

        spawnedPart.transform.parent = transform;

        spawnedPart.transform.position = partData.spatialData.position.ToVector3();
        spawnedPart.transform.rotation = partData.spatialData.rotation.ToQuaternion();
        spawnedPart.transform.localScale = partData.spatialData.scale.ToVector3();
    }
}
