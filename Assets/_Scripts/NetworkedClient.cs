using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedClient : MonoBehaviour
{

    int connectionID;
    int maxConnections = 1000;
    int reliableChannelID;
    int unreliableChannelID;
    int hostID;
    int socketPort = 5491;
    byte error;
    bool isConnected = false;
    int ourClientID;

    public string turnList;

    GameObject uiManager; 

    void Start()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects)
        {
            if (go.name == "UIManager")
                uiManager = go;
        }

            Connect();

    }

    void Update()
    {


        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    SendMessageToHost(ClientToServerSignifiers.CreateAccountAttempt + "," + "AccountTest" + "," + "PasswordTest");
        //}  else if (Input.GetKeyDown(KeyCode.L))
        //{
        //    SendMessageToHost(ClientToServerSignifiers.LoginAttempt + "," + "AccountTest" + "," + "PasswordTest");
        //}
        UpdateNetworkConnection();
    }

    private void UpdateNetworkConnection()
    {
        if (isConnected)
        {
            int recHostID;
            int recConnectionID;
            int recChannelID;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out dataSize, out error);

            switch (recNetworkEvent)
            {
                case NetworkEventType.ConnectEvent:
                    Debug.Log("connected.  " + recConnectionID);
                    ourClientID = recConnectionID;
                    break;
                case NetworkEventType.DataEvent:
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    ProcessRecievedMsg(msg, recConnectionID);
                    //Debug.Log("got msg = " + msg);
                    break;
                case NetworkEventType.DisconnectEvent:
                    isConnected = false;
                    Debug.Log("disconnected.  " + recConnectionID);
                    break;
            }
        }
    }

    private void Connect()
    {

        if (!isConnected)
        {
            Debug.Log("Attempting to create connection");

            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            reliableChannelID = config.AddChannel(QosType.Reliable);
            unreliableChannelID = config.AddChannel(QosType.Unreliable);
            HostTopology topology = new HostTopology(config, maxConnections);
            hostID = NetworkTransport.AddHost(topology, 0);
            Debug.Log("Socket open.  Host ID = " + hostID);

            connectionID = NetworkTransport.Connect(hostID, "192.168.0.93", socketPort, 0, out error); // server is local on network

            if (error == 0)
            {
                isConnected = true;

                Debug.Log("Connected, id = " + connectionID);

            }
        }
    }

    public void Disconnect()
    {
        NetworkTransport.Disconnect(hostID, connectionID, out error);
    }

    public void SendMessageToHost(string msg)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(msg);
        NetworkTransport.Send(hostID, connectionID, reliableChannelID, buffer, msg.Length * sizeof(char), out error);
    }

    private void ProcessRecievedMsg(string msg, int id)
    {
        Debug.Log("msg recieved = " + msg + ".  connection id = " + id);

        string[] csv = msg.Split(',');

        int signifier = int.Parse(csv[0]);

        if (signifier == ServerToClientSignifiers.LoginSuccess)
        {
            uiManager.GetComponent<UIManager>().ChangeGameState(GameStates.MainMenu);
        }
        else if (signifier == ServerToClientSignifiers.GameRoomStarted)
        {
            uiManager.GetComponent<UIManager>().ChangeGameState(GameStates.PlayingTicTacToe);
            GameObject.Find("GameController").GetComponent<GameController>().playerSide = csv[1];
            if (csv[1] == "X")
            {
                GameObject.Find("GameController").GetComponent<GameController>().isYourTurn = true;
            }
        }
        else if (signifier == ServerToClientSignifiers.OpponentPlayed)
        {
            Debug.Log("OPPONENT SENT A PLAY");
            GameObject.Find("GameController").GetComponent<GameController>().MarkButton(int.Parse(csv[1]), int.Parse(csv[2]));
        }
        else if (signifier == ServerToClientSignifiers.ChatSentToClient)
        {
            Debug.Log("RECEIVED A MESSAGE");
            GameObject.Find("ChatManager").GetComponent<ChatButton>().OpponentChatted(csv[1]);
        }
        else if (signifier == ServerToClientSignifiers.ReplaySent)
        {
            for (int i = 2; i < csv.Length; i += 3)
            {
                turnList += csv[i] + "," + csv[i + 1] + ",";
            }
            Debug.Log("TURNLIST SAYS: " + turnList);
            uiManager.GetComponent<UIManager>().ChangeGameState(GameStates.Replay);

        }
    }

    public bool IsConnected()
    {
        return isConnected;
    }


}

public static class ClientToServerSignifiers
{
    public const int CreateAccountAttempt = 1;
    public const int LoginAttempt = 2;
    public const int AddToGameRoomQueue = 3;
    public const int TicTacToePlay = 4;
    public const int ChatSentToServer = 5;
    public const int RequestReplay = 6;


}

public static class ServerToClientSignifiers
{
    public const int CreateAccountSuccess = 1;
    public const int LoginSuccess = 2;

    public const int CreateAccountFailure = 3;
    public const int LoginFailure = 4;

    public const int GameRoomStarted = 5;
    public const int OpponentPlayed = 6;
    public const int ChatSentToClient = 7;
    public const int ReplaySent = 8;

}



