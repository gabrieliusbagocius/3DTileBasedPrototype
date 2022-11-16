using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameMode
{
    SinglePlayer,
    LocalMultiplayer
}

public class GameManager : Singleton<GameManager>
{
    public GameMode currentGameMode;

    public SinglePlayerCameraMode singlePlayerCameraMode;

    public int numberOfPlayers;

    private List<HeroController> activePlayerControllers;
    private bool isPaused;
    private HeroController focusedPlayerController = null;

    private void Start()
    {
        isPaused = false;
        GridManager.Instance.GenerateGrid();
        SetupPlayerBasedOnGameState();
        SpawnEnemies();
    }

    private void SetupPlayerBasedOnGameState()
    {
        switch (currentGameMode)
        {
            case GameMode.SinglePlayer:
                SetupSinglePlayer();
                break;

            case GameMode.LocalMultiplayer:
                SetupLocalMultiplayer();
                break;
        }
    }

    private void SetupSinglePlayer()
    {
        activePlayerControllers = new List<HeroController>();
        SpawnPlayer();
        SetupActivePlayers();
        SetupSinglePlayerCamera();
    }

    private void SetupLocalMultiplayer()
    {
        SpawnPlayers();
        SetupActivePlayers();
    }

    private void SpawnPlayer()
    {
        GameObject spawnedPlayer = UnitManager.Instance.SpawnHeroes();
        AddPlayerToActivePlayerList(spawnedPlayer.GetComponent<HeroController>());
    }

    private void SpawnPlayers()
    {
        activePlayerControllers = new List<HeroController>();

        for (int i = 0; i < numberOfPlayers; i++)
        {
            GameObject spawnedPlayer = UnitManager.Instance.SpawnHeroes();
            AddPlayerToActivePlayerList(spawnedPlayer.GetComponent<HeroController>());
        }
    }

    private void SpawnEnemies()
    {
        UnitManager.Instance.SpawnEnemies();
    }

    private void AddPlayerToActivePlayerList(HeroController newPlayer)
    {
        activePlayerControllers.Add(newPlayer);
    }

    private void SetupActivePlayers()
    {
        for (int i = 0; i < activePlayerControllers.Count; i++)
        {
            activePlayerControllers[i].SetupPlayer(i);
        }
    }

    private void SetupSinglePlayerCamera()
    {
        CameraManager.Instance.SetupSinglePlayerCamera(singlePlayerCameraMode);
    }

    //Get Data ----

    public List<HeroController> GetActivePlayerControllers()
    {
        return activePlayerControllers;
    }

    public HeroController GetFocusedPlayerController()
    {
        return focusedPlayerController;
    }

    public int NumberOfConnectedDevices()
    {
        return InputSystem.devices.Count;
    }

    //Pause Utilities ----

    private void ToggleTimeScale()
    {
        float newTimeScale = 0f;

        switch (isPaused)
        {
            case true:
                newTimeScale = 0f;
                break;

            case false:
                newTimeScale = 1f;
                break;
        }

        Time.timeScale = newTimeScale;
    }
}