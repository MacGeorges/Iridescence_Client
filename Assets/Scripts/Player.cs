using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 lastPosition;
    public Quaternion lastRotation;

    void Start()
    {
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    void Update()
    {
        if ((transform.position != lastPosition) || (transform.rotation != lastRotation))
        {
            NetworkRequest request = new NetworkRequest();
            request.sender = ClientManager.instance.user;
            request.requestType = RequestType.playerAction;

            PlayerActionRequest newPlayerActionRequest = new PlayerActionRequest();
            newPlayerActionRequest.spatialData = new SerializableTransform();
            newPlayerActionRequest.spatialData.position = new SerializableVector3(transform.position);
            newPlayerActionRequest.spatialData.rotation = new SerializableQuaternion(transform.rotation);
            newPlayerActionRequest.spatialData.scale = new SerializableVector3(transform.localScale);

            request.serializedRequest = JsonUtility.ToJson(newPlayerActionRequest);

            foreach (ServerHandler connectedServer in ServersManager.instance.connectedServers)
            {
                connectedServer.Send(request);
            }
        }

        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }
}
