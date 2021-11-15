using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text[] buttonList;

    private string playerSide;

    GameObject networkedClient;


    private void Awake()
    {
        SetGameControllerReferencesOnButtons();
        playerSide = "X";
        networkedClient = GameObject.Find("Client");
    }

    void SetGameControllerReferencesOnButtons()
    {

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridButton>().SetGameControllerReference(this);
            Debug.Log("Setting game controller reference of:" + i);
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void EndTurn()
    {
        // top straight victory
        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide
            ||
            buttonList[0].text == "0" && buttonList[1].text == "0" && buttonList[2].text == "0")
        {
            GameOver();
        }
        //middle straight victory
        if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide
            ||
            buttonList[3].text == "0" && buttonList[4].text == "0" && buttonList[5].text == "0")
        {
            GameOver();
        }
        // bottom straight victory
        if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide
            ||
            buttonList[6].text == "0" && buttonList[7].text == "0" && buttonList[8].text == "0")
        {
            GameOver();
        }

        // left straight victory
        if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide
            ||
            buttonList[0].text == "0" && buttonList[3].text == "0" && buttonList[6].text == "0")
        {
            GameOver();
        }
        //center straight victory
        if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide
            ||
            buttonList[1].text == "0" && buttonList[4].text == "0" && buttonList[7].text == "0")
        {
            GameOver();
        }
        // right straight victory
        if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide
            ||
            buttonList[2].text == "0" && buttonList[5].text == "0" && buttonList[8].text == "0")
        {
            GameOver();
        }

        //up diagonal victory
        if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide
            ||
            buttonList[0].text == "0" && buttonList[4].text == "0" && buttonList[8].text == "0")
        {
            GameOver();
        }
        // down diagonal victory
        if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide
            ||
            buttonList[2].text == "0" && buttonList[4].text == "0" && buttonList[6].text == "0")
        {
            GameOver();
        }



        ChangeSides();


    }

    void ChangeSides()
    {
       // playerSide = (playerSide == "X") ? "0" : "X";
    }

    void GameOver()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = false;
        }
    }

    public void TrackButtonPressed(int row, int column)
    {

        Debug.Log("BUTTON PRESSED -- Row: " + row + " Column: " + column);
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToePlay + "," + row + "," + column);   // Tell server which button was pressed

    }

    public void MarkButton(int row, int column)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            if (buttonList[i].GetComponentInParent<GridButton>().row == row && buttonList[i].GetComponentInParent<GridButton>().column == column)
            {
                if (playerSide == "X")
                {
                    buttonList[i].text = "0";
                }
                else
                {
                    buttonList[i].text = "X";
                }
                buttonList[i].GetComponentInParent<Button>().interactable = false;
            }
        }
        EndTurn();
    }
}
