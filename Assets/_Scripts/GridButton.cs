using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridButton : MonoBehaviour
{
    public Button button;
    public Text buttonText;

    public int row;
    public int column;

    private GameController gameController;

    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }

    public void SetSpace()
    {
        if (gameController.isYourTurn)
        {
            buttonText.text = gameController.GetPlayerSide();
            button.interactable = false;
            gameController.TrackButtonPressed(row, column);
            gameController.EndTurn();
        }
    }    
}
