using Assets.Code.Scripts;
using Assets.Code.Scripts.Boot.Communication;
using System;
using UnityEngine;

public class InputService : IDisposable
{
    PlayerControls _playerControls;
    public InputService()
    {
        _playerControls = new PlayerControls();
        _playerControls.Enable();
        CommunicationEvents.OnStartCommunicateEvent += OnStartCommunicate;
        CommunicationEvents.OnWaitForCommunicateEvent += OnWaitForCommunicate;
        GameEvents.OnGameOverEvent += OnGameOver;
    }

    public Vector2 GetMovement()
    {
        return _playerControls.PlayerActions.Movement.ReadValue<Vector2>();
    }

    public bool IsSpawn()
    {
        return _playerControls.PlayerActions.Spawn.WasPerformedThisFrame();
    }

    public bool IsMouseLeftButton()
    {
        return _playerControls.PlayerActions.Click.WasPerformedThisFrame();
    }
    public Vector2 GetMousePosition()
    {
        return _playerControls.PlayerActions.MousePosition.ReadValue<Vector2>();
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
    public bool IsStatsCommand()
    {
        return _playerControls.SystemActions.CommunicationStats.WasPerformedThisFrame();
    }
    public void Dispose()
    {
        _playerControls.Disable();
        CommunicationEvents.OnStartCommunicateEvent -= OnStartCommunicate;
        CommunicationEvents.OnWaitForCommunicateEvent -= OnWaitForCommunicate;
        GameEvents.OnGameOverEvent -= OnGameOver;
        _playerControls.Dispose();
    }
    void OnWaitForCommunicate()
    {
        _playerControls.Disable();
    }
    void OnStartCommunicate()
    {
        _playerControls.Enable();
    }
    public void OnGameOver()
    {
        _playerControls.Disable();
    }
}
