using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTurnButton : MonoBehaviour
{
    public NetworkedClient networkedClient;
    public GameController gameController;
    int turnCount = 0;

    private void Start()
    {
        networkedClient = GameObject.Find("Client").GetComponent<NetworkedClient>();
        gameController = GameObject.Find("ReplayGameController").GetComponent<GameController>();
    }
    public void NextButtonPressed()
    {
        string[] csv = networkedClient.turnList.Split(',');
        //for (int i = 0; i < csv.Length; i+=2)
        //{
        //    gameController.MarkButton(int.Parse(csv[i]), int.Parse(csv[i+1]));
        //}
        gameController.ReplayMarkButton(int.Parse(csv[turnCount]), int.Parse(csv[turnCount+1]), turnCount%4 == 0 ? "X" : "0");
        turnCount += 2;
    }
}
