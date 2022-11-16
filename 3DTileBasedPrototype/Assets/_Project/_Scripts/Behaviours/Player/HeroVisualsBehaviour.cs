using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroVisualsBehaviour : MonoBehaviour
{

    private int playerID;
    private PlayerInput playerInput;

    [Header("Device Display Settings")]
    public DeviceDisplayConfigurator deviceDisplaySettings;

    [Header("Player Material")]
    public MeshRenderer playerMeshRenderer;

    public void SetupBehaviour(int newPlayerID, PlayerInput newPlayerInput)
    {
        playerID = newPlayerID;
        playerInput = newPlayerInput;

    }


}
