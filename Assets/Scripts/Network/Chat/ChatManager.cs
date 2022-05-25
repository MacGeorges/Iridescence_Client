using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public ChatWindow chatWindowPrefab;
    public Transform chatDisplayRoot;

    public List<ChatWindow> chats = new List<ChatWindow>();

    public static ChatManager instance;

    void Start()
    {
        instance = this;
    }

    public void UpdateChat()
    {
        Debug.Log("UpdateChat");

        chats = new List<ChatWindow>();

        foreach (Transform tmpTrans in chatDisplayRoot)
        {
            Destroy(tmpTrans.gameObject);
        }

        Debug.Log("Chat cleaned");

        foreach (ServerHandler client in ServersManager.instance.connectedServers)
        {
            ChatWindow chatWindow = Instantiate(chatWindowPrefab, chatDisplayRoot);
            chatWindow.userRef = client;

            chats.Add(chatWindow);
        }

        Debug.Log("Chat populated");
    }

    public void ChatRecieved(NetworkRequest message)
    {
        foreach (ChatWindow chat in chats)
        {
            if(chat.userRef.user == message.sender)
            {
                chat.pendingMessages.Add(message);
            }
        }
    }
}
