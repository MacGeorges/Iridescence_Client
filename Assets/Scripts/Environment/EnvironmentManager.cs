using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public List<RegionElement> pendingElementsAdd = new List<RegionElement>();
    public List<RegionElement> pendingElementsRemove = new List<RegionElement>();
    public List<RegionElement> pendingElementsUpdate = new List<RegionElement>();

    public List<EnvironmentPart> currentElements = new List<EnvironmentPart>();

    public static EnvironmentManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        List<RegionElement> pendingElementsAddWork = new List<RegionElement>(pendingElementsAdd);
        pendingElementsAdd = new List<RegionElement>();

        foreach (RegionElement regionAddElement in pendingElementsAddWork)
        {
            GameObject regionElement = new GameObject();
            regionElement.name = "regionElement - " + regionAddElement.elementID;
            EnvironmentPart environmentPart = regionElement.AddComponent<EnvironmentPart>();
            environmentPart.Init(regionAddElement);

            currentElements.Add(environmentPart);
        }

        List<RegionElement> pendingElementsRemoveWork = new List<RegionElement>(pendingElementsRemove);
        pendingElementsRemove = new List<RegionElement>();

        foreach (RegionElement regionRemoveElement in pendingElementsRemoveWork)
        {
            EnvironmentPart wantedElement = currentElements.Find(e => e.partData.elementID == regionRemoveElement.elementID);

            if (wantedElement)
            {
                Destroy(wantedElement.gameObject);

                currentElements.Remove(wantedElement);
            }
        }
    }

    public void HandleObjectRequest(ObjectRequest ObjectRequest)
    {
        Debug.Log("ObjectRequest " + ObjectRequest.requestType);
        switch (ObjectRequest.requestType)
        {
            case ObjectRequestType.add:
                pendingElementsAdd.Add(ObjectRequest.element);
                Debug.Log("pendingElementsAdded");
                break;
            case ObjectRequestType.remove:
                pendingElementsRemove.Add(ObjectRequest.element);
                Debug.Log("pendingElementsRemoved");
                break;
            case ObjectRequestType.update:
                pendingElementsUpdate.Add(ObjectRequest.element);
                Debug.Log("pendingElementsUpdated");
                break;
        }
    }
}
