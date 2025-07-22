using UnityEngine;

public class Experimental_NetworkStructsEnums{}

public enum ConnexionType
{
    UDP,
    TCP
}

public struct IPAdress
{
    public int byte1;
    public int byte2;
    public int byte3;
    public int byte4;

    public IPAdress(int byte1, int byte2, int byte3, int byte4)
    {
        this.byte1 = byte1;
        this.byte2 = byte2;
        this.byte3 = byte3;
        this.byte4 = byte4;
    }

    public override string ToString()
    {
        return byte1.ToString() + "." + byte2.ToString() + "." + byte3.ToString() + "." + byte4.ToString();
    }
}