using UnityEngine;

public class Experimental_ServerConnection
{
    private static IPAdress IPAdress;
    private static ConnexionType connexionType;

    public static void Init(IPAdress newIPAdress, ConnexionType newConnexionType)
    {
        IPAdress = newIPAdress;
        connexionType = newConnexionType;
    }

    /*public void StartConnection(IPAdress IPAdress, ConnexionType connexionType)
    {
        Debug.Log("Server Connection Started! " + IPAdress.ToString());
    }*/

    public static void StartConnection()
    {
        Debug.Log("Server Connection Started! " + IPAdress.ToString());
    }
}
