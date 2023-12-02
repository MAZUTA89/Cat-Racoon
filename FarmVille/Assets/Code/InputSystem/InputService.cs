using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputService : IDisposable
{
    PlayerControls _playerControls;
    public InputService()
    {
        _playerControls = new PlayerControls();
        _playerControls.Enable();
    }

    public Vector2 GetMovement()
    {
        return _playerControls.PlayerActions.Movement.ReadValue<Vector2>();
    }

    public void Dispose()
    {
        _playerControls.Disable();
        _playerControls.Dispose();
    }
}
