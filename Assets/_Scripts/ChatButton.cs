using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatButton : MonoBehaviour
{
    public Text chatText;
    public Text opponentChatText;

    private NetworkedClient networkedClient;

    private void Start()
    {
        networkedClient = GameObject.Find("Client").GetComponent<NetworkedClient>();
    }

    public void ChatButtonPressed(string message)
    {
        chatText.text = message;
        chatText.gameObject.SetActive(true);
        chatText.gameObject.GetComponent<ChatText>().Invoke("DisactivateSelf", 1);

        networkedClient.SendMessageToHost(ClientToServerSignifiers.ChatSentToServer + "," + message);

    }

    public void OpponentChatted(string message)
    {
        opponentChatText.text = message;
        opponentChatText.gameObject.SetActive(true);
        opponentChatText.GetComponent<ChatText>().Invoke("DisactivateSelf", 1);
    }
}
