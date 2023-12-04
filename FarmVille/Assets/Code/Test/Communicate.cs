using Assets.Code.Scripts.Boot.Data;
using Assets.Code.Scripts.Communication;
using ClientServer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Communicate : MonoBehaviour
{
    [SerializeField] GameObject PlayerObject;
    [SerializeField] GameObject ConnectedObject;
    [SerializeField] GameObject Create;
    [SerializeField] GameObject Connect;
    Communicator _communicator;
    PlayerData _sendData;
    PlayerData _recvData;
    Vector2 pos;
    public Vector2 Recv;
    // Start is called before the first frame update
    void Start()
    {
        _sendData = new PlayerData();
        _recvData = new PlayerData();
    }
    private void OnEnable()
    {
        Connector.OnConnectionCreated += OnConnectionCreated;
    }
    private void OnDisable()
    {
        Connector.OnConnectionCreated -= OnConnectionCreated;
        _communicator?.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        pos = new Vector2 (Random.value * 2, Random.value * 2);
        _sendData.UpdatePosition(pos);
        Recv = _recvData.GetPosition();
        PlayerObject.transform.position = _sendData.GetPosition();
        ConnectedObject.transform.position = _communicator.GetData().GetPosition();
    }

    public void OnConnectionCreated(TCPBase user)
    {
        Connect.SetActive(false);
        Create.SetActive(false);
        Debug.Log($"{DateTime.Now}");

        _communicator = new Communicator(user, _sendData, _recvData, 1000);

        _communicator.Start();
    }
}
