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

    public bool IsSpawn()
    {
        return _playerControls.PlayerActions.Spawn.WasPerformedThisFrame();
    }

    public bool IsCell_1()
    {
        return _playerControls.PlayerActions.Cell_1.WasPerformedThisFrame();
    }
     public bool IsCell_2()
    {
        return _playerControls.PlayerActions.Cell_2.WasPerformedThisFrame();
    }
    public bool IsCell_3()
    {
        return _playerControls.PlayerActions.Cell_3.WasPerformedThisFrame();
    }
    public bool IsCell_4()
    {
        return _playerControls.PlayerActions.Cell_4.WasPerformedThisFrame();
    }
    public bool IsCell_5()
    {
        return _playerControls.PlayerActions.Cell_5.WasPerformedThisFrame();
    }
    public bool IsCell_6()
    {
        return _playerControls.PlayerActions.Cell_6.WasPerformedThisFrame();
    }
    public bool IsCell_7()
    {
        return _playerControls.PlayerActions.Cell_7.WasPerformedThisFrame();
    }
    public void Dispose()
    {
        _playerControls.Disable();
        _playerControls.Dispose();
    }
}
