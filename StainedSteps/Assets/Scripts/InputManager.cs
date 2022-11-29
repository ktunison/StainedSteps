using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Instance
    {
        get
        {
            return _instance;
        }
    }
    
    private AllMovement allMovement;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        allMovement = new AllMovement();
    }

    private void OnEnable()
    {
        allMovement.Enable();
    }

    private void OnDisable()
    {
        allMovement.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return allMovement.WASD.Movement.ReadValue<Vector2>();
    }
    
    public Vector2 GetMouseDelta()
    {
        return allMovement.WASD.Look.ReadValue<Vector2>();
    }

    public bool PlayerJumpedThisFrame()
    {
        return allMovement.WASD.Jump.triggered;
    }
}
