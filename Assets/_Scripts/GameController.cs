using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text[] buttonList;

    public string playerSide;
    public bool isYourTurn;

    GameObject networkedClient;


    private void Awake()
    {
        SetGameControllerReferencesOnButtons();

        networkedClient = GameObject.Find("Client");
        if (playerSide == "X")
        {
            isYourTurn = true;
        }
        else
        {
            isYourTurn = false;
        }
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

        CheckForVictory();

        ChangeSides();


    }

    void CheckForVictory()
    {
        if ( // top straight victory
           buttonList[0].text == "X" && buttonList[1].text == "X" && buttonList[2].text == "X"
           ||
           buttonList[0].text == "0" && buttonList[1].text == "0" && buttonList[2].text == "0"
           || // middle straight victory
           buttonList[3].text == "X" && buttonList[4].text == "X" && buttonList[5].text == "X"
           ||
           buttonList[3].text == "0" && buttonList[4].text == "0" && buttonList[5].text == "0"
           || // bottom straight victory
           buttonList[6].text == "X" && buttonList[7].text == "X" && buttonList[8].text == "X"
           ||
           buttonList[6].text == "0" && buttonList[7].text == "0" && buttonList[8].text == "0"
           || // left straight victory
           buttonList[0].text == "X" && buttonList[3].text == "X" && buttonList[6].text == "X"
           ||
           buttonList[0].text == "0" && buttonList[3].text == "0" && buttonList[6].text == "0"
           || // center straight victory
           buttonList[1].text == "X" && buttonList[4].text == "X" && buttonList[7].text == "X"
           ||
           buttonList[1].text == "0" && buttonList[4].text == "0" && buttonList[7].text == "0"
           || // right straight victory
           buttonList[2].text == "X" && buttonList[5].text == "X" && buttonList[8].text == "X"
           ||
           buttonList[2].text == "0" && buttonList[5].text == "0" && buttonList[8].text == "0"
           || // up diagonal victory
           buttonList[0].text == "X" && buttonList[4].text == "X" && buttonList[8].text == "X"
           ||
           buttonList[0].text == "0" && buttonList[4].text == "0" && buttonList[8].text == "0"
           || // down diagonal victory
           buttonList[2].text == "X" && buttonList[4].text == "X" && buttonList[6].text == "X"
           ||
           buttonList[2].text == "0" && buttonList[4].text == "0" && buttonList[6].text == "0"
           )
        {
            GameOver();
        }
    }
    void ChangeSides() // TODO: This does nothing. 
    {
        isYourTurn = !isYourTurn;
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

    public void ReplayMarkButton(int row, int column, string letter)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            if (buttonList[i].GetComponentInParent<GridButton>().row == row && buttonList[i].GetComponentInParent<GridButton>().column == column)
            {
                buttonList[i].text = letter;
                buttonList[i].GetComponentInParent<Button>().interactable = false;
            }
        }
        EndTurn();
    }
}
