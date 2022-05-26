using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public List<RegionElement> pendingElementsAdd = new List<RegionElement>();
    public List<RegionElement> pendingElementsRemove = new List<RegionElement>();
    public List<RegionElement> pendingElementsUpdate = new List<RegionElement>();

    public List<EnvironmentPart> elementsLibrary = new List<EnvironmentPart>();
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
            EnvironmentPart libraryPart = elementsLibrary.Find(e => e.partData.modelID == regionAddElement.modelID);

            if (libraryPart)
            {
                if (libraryPart.objectRef)
                {
                    GameObject regionElement = new GameObject();
                    regionElement.name = "regionElement - " + regionAddElement.elementID;

                    EnvironmentPart environmentPart = regionElement.AddComponent<EnvironmentPart>();
                    environmentPart.objectRef = Instantiate(libraryPart.objectRef);
                    environmentPart.Init(regionAddElement);

                    currentElements.Add(environmentPart);
                }
                else
                {
                    //Not Loaded yet. Let's add it for the next check
                    pendingElementsAdd.Add(regionAddElement);
                }
            }
            else
            {
                GameObject regionElement = new GameObject();
                regionElement.name = "regionElement - " + regionAddElement.elementID;
                EnvironmentPart environmentPart = regionElement.AddComponent<EnvironmentPart>();
                environmentPart.Init(regionAddElement);

                environmentPart.gameObject.SetActive(false);

                elementsLibrary.Add(environmentPart);

                //Model requested. Let's add the current spawn request it for the next check
                pendingElementsAdd.Add(regionAddElement);
            }
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
        switch (ObjectRequest.requestType)
        {
            case ObjectRequestType.add:
                pendingElementsAdd.Add(ObjectRequest.element);
                break;
            case ObjectRequestType.remove:
                pendingElementsRemove.Add(ObjectRequest.element);
                break;
            case ObjectRequestType.update:
                pendingElementsUpdate.Add(ObjectRequest.element);
                break;
        }
    }
}
