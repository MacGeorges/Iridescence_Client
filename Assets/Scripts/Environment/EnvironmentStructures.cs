using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentStructures {}

[Serializable]
public struct Part
{
    public int partID;
    public string resourceURL;
}

[Serializable]
public struct SerializableTransform
{
    public SerializableVector3 position;
    public SerializableQuaternion rotation;
    public SerializableVector3 scale;
}

[Serializable]
public struct RegionElements
{
    public List<RegionElement> regionElements;
}

[Serializable]
public struct RegionElement
{
    public int elementID;
    public int modelID;
    public SerializableTransform spatialData;
}

[Serializable]
public struct SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(float newX, float newY, float newZ)
    {
        x = newX;
        y = newY;
        z = newZ;
    }

    public SerializableVector3(Vector3 RefVector3)
    {
        x = RefVector3.x;
        y = RefVector3.y;
        z = RefVector3.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

[Serializable]
public struct SerializableQuaternion
{
    public float x;
    public float y;
    public float z;
    public float w;

    public SerializableQuaternion(float newX, float newY, float newZ, float newW)
    {
        x = newX;
        y = newY;
        z = newZ;
        w = newW;
    }

    public SerializableQuaternion(Quaternion RefQuaternion)
    {
        x = RefQuaternion.x;
        y = RefQuaternion.y;
        z = RefQuaternion.z;
        w = RefQuaternion.w;
    }

    public Quaternion ToQuaternion()
    {
        return new Quaternion(x, y, z, w);
    }
}