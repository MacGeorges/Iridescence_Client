using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public List<RegionElement> pendingElements = new List<RegionElement>();

    public static EnvironmentManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        foreach(RegionElement regionRegionElement in pendingElements)
        {
            GameObject regionElement = new GameObject();
            regionElement.name = "regionElement - " + regionRegionElement.elementID;
            EnvironmentPart environmentPart = regionElement.AddComponent<EnvironmentPart>();
            environmentPart.Init(regionRegionElement);
        }

        pendingElements = new List<RegionElement>();
    }
}
