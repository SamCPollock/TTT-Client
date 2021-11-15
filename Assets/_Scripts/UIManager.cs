using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
     GameObject submitButton, userNameInput, passwordInput, createToggle, loginToggle;

     GameObject networkedClient;

    GameObject findGameRoomButton, placeholderGameButton;

    GameObject waitText;

    GameObject loginState, mainMenuState, playingState, waitingForMatchState, replayState;


    void Start()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects)
        {
            if (go.name == "UsernameInput")
                userNameInput = go;
            else if (go.name == "PasswordInput")
                passwordInput = go;
            else if (go.name == "CreateToggle")
                createToggle = go;
            else if (go.name == "LoginToggle")
                loginToggle = go;
            else if (go.name == "SubmitButton")
                submitButton = go;
            else if (go.name == "Client")
                networkedClient = go;

            else if (go.name == "FindGameRoomButton")
                findGameRoomButton = go;
            else if (go.name == "PlaceholderGameButton")
                placeholderGameButton = go;

            else if (go.name == "WaitText")
                waitText = go;

            // States
            else if (go.name == "State-Login")
                loginState = go;
            else if (go.name == "State-MainMenu")
                mainMenuState = go;
            else if (go.name == "State-Playing")
                playingState = go;
            else if (go.name == "State-WaitingForMatch")
                waitingForMatchState = go;
            else if (go.name == "State-Replay")
                replayState = go;

        }

        submitButton.GetComponent<Button>().onClick.AddListener(SubmitButtonPressed);

        loginToggle.GetComponent<Toggle>().onValueChanged.AddListener(LoginToggleChanged);
        createToggle.GetComponent<Toggle>().onValueChanged.AddListener(CreateToggleChanged);

        findGameRoomButton.GetComponent<Button>().onClick.AddListener(FindGameRoomButtonPressed);
        //placeholderGameButton.GetComponent<Button>().onClick.AddListener(PlaceholderGameButtonPressed);

        // Set initial game state
        ChangeGameState(GameStates.Login);
        //ChangeGameState(GameStates.PlayingTicTacToe);


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ChangeGameState(GameStates.MainMenu);
        }
    }

    public void SubmitButtonPressed()
    {
        string n = userNameInput.GetComponent<InputField>().text;
        string p = passwordInput.GetComponent<InputField>().text;

        string msg;
        if (createToggle.GetComponent<Toggle>().isOn)
            msg = ClientToServerSignifiers.CreateAccountAttempt + "," + n + "," + p;
        else
            msg = ClientToServerSignifiers.LoginAttempt + "," + n + "," + p;

        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
    }
    private void CreateToggleChanged(bool newValue)
    {
        loginToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(!newValue);
    }

    private void LoginToggleChanged(bool newValue)
    {
        createToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(!newValue);

    }

    private void FindGameRoomButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.AddToGameRoomQueue + "");
        ChangeGameState(GameStates.WaitingForMatch);

    }

    private void PlaceholderGameButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToePlay + "");

    }

    public void ChangeGameState(int newState)
    {
        loginState.SetActive(false);
        mainMenuState.SetActive(false);
        playingState.SetActive(false);
        waitingForMatchState.SetActive(false);
        replayState.SetActive(false);

        //submitButton.SetActive(false);
        //userNameInput.SetActive(false);
        //passwordInput.SetActive(false);
        //createToggle.SetActive(false);
        //loginToggle.SetActive(false);
        //findGameRoomButton.SetActive(false);
        //placeholderGameButton.SetActive(false);
        //waitText.SetActive(false);

        if (newState == GameStates.Login)
        {
            loginState.SetActive(true);
            //submitButton.SetActive(true);
            //userNameInput.SetActive(true);
            //passwordInput.SetActive(true);
            //createToggle.SetActive(true);
            //loginToggle.SetActive(true);
        }
        else if (newState == GameStates.MainMenu)
        {
            mainMenuState.SetActive(true);
            //findGameRoomButton.SetActive(true);
        }
        else if (newState == GameStates.WaitingForMatch)
        {
            waitingForMatchState.SetActive(true);
            //waitText.SetActive(true);
        }
        else if (newState == GameStates.PlayingTicTacToe)
        {
            playingState.SetActive(true);
            //placeholderGameButton.SetActive(true);
        }
        else if (newState == GameStates.Replay)
        {
            replayState.SetActive(true);
        }

    }


}

public static class GameStates
{
    public const int Login              = 1;
    public const int MainMenu           = 2;
    public const int WaitingForMatch    = 3;
    public const int PlayingTicTacToe   = 4;
    public const int Replay             = 5;

}