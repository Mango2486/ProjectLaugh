using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private PlayerInputAction playerInputActions;

    public EventHandler Player1_Interact;
    public EventHandler Player2_Interact;

    private void Awake() {
        playerInputActions = new PlayerInputAction();
        playerInputActions.Players.Enable();

        //»¥¶¯¼ü¼àÌý
        playerInputActions.Players.Player_1Interact.performed += On_Player1_Interact;
        playerInputActions.Players.Player_2Interact.performed += On_Player2_Interact;
    }

    private void On_Player2_Interact(InputAction.CallbackContext context) {
        Player2_Interact?.Invoke(this, EventArgs.Empty);
    }

    private void On_Player1_Interact(InputAction.CallbackContext context) {
        Player1_Interact?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 Get_Player1_InputVetor() {
        var inputVector_1 = playerInputActions.Players.Player_1Move.ReadValue<Vector2>();

        return inputVector_1;
    }
    
    public Vector2 Get_Player2_InputVector() {
        var inputVector_2 = playerInputActions.Players.Player_2Move.ReadValue<Vector2>();

        return inputVector_2;
    }
}
