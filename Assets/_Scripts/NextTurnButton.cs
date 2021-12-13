using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTurnButton : MonoBehaviour
{
    public NetworkedClient networkedClient;
    public GameController gameController;
    public int turnCount = 0;  

    private void Start()
    {
        networkedClient = GameObject.Find("Client").GetComponent<NetworkedClient>();
        gameController = GameObject.Find("ReplayGameController").GetComponent<GameController>();
    }
    public void NextButtonPressed()
    {
        string[] csv = networkedClient.turnList.Split(',');

        int rowToMark = int.Parse(csv[turnCount * 2]);
        int columnToMark = int.Parse(csv[turnCount * 2 + 1]);
        bool isX = turnCount % 2 == 0;

        gameController.ReplayMarkButton(rowToMark, columnToMark, (isX ? "X" : "0")); 
        turnCount += 1;
    }
}
