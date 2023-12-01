using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Assets.Code.Scripts.Lobby
{
    public class ProgressHandler : MonoBehaviour
    {
        const string c_MenuSceneName = "MenuScene";
        const string c_ConnectionText = "Waiting for connection...";
        const string c_ConnectText = "Connect...";
        const string c_SuccsessConnectionText = "Connection is established";
        const string c_ConnectionFailedText = "Connection failed";
        const string c_CanceledText = "Canceled...";
        const string c_LoadLevelText = "Loading level ...";
        const string c_CheckText = "Check signal ...";
        const string c_CheckSignalFailedText = "Check signal failed!";

        LobbyConnection _lobbyConnection;
        LevelLoader _levelLoader;
        TextMeshProUGUI _processText;
        [SerializeField] TextMeshProUGUI _endPointText;
        GameObject _loadImage;
        [Inject]
        public void Constructor(
            [Inject(Id = "ConnectionProgressText")] TextMeshProUGUI progressText,
            [Inject(Id = "ConnectionLoadImage")] GameObject loadImage)
        {
            _processText = progressText;
            _loadImage = loadImage;
        }

        private void Start()
        {
            _levelLoader = GetComponent<LevelLoader>();
            _lobbyConnection = GetComponent<LobbyConnection>();
            _lobbyConnection.OnStartCreateConnectionEvent 
                += OnStartCreateConnection;
            _lobbyConnection.OnCancelClientConnectionEvent 
                += OnCancelClientConnection;
            _lobbyConnection.OnCancelServerConnectionEvent 
                += OnCancelServerConnection;
            _lobbyConnection.OnCreateServerEndPointEvent +=
                OnCreateServerEndPoint;
            _levelLoader.OnCanceledOrFailedLoadLevelSignalEvent 
                += OnCanceledOrFailedLoadLevelSignal;
            _levelLoader.OnStartCheckLoadLevelSignalEvent 
                += OnStartCheckLoadLevelSignal;
            _levelLoader.OnStopCheckLoadLevelSignalEvent 
                += OnStopCheckLoadLevelSignal;
            _lobbyConnection.OnCreateConnectionSeccessEvent
                += OnCreateConnectionSeccess;
            _lobbyConnection.OnCreateConnectionFailedEvent
                += OnCreateConnectionFailed;
        }

        

        private void OnDisable()
        {
            _lobbyConnection.OnStartCreateConnectionEvent
                -= OnStartCreateConnection;
            _lobbyConnection.OnCancelClientConnectionEvent 
                -= OnCancelClientConnection;
            _lobbyConnection.OnCancelServerConnectionEvent
                -= OnCancelServerConnection;
            _lobbyConnection.OnCreateServerEndPointEvent -=
                OnCreateServerEndPoint;
            _levelLoader.OnCanceledOrFailedLoadLevelSignalEvent
                -= OnCanceledOrFailedLoadLevelSignal;
            _levelLoader.OnStartCheckLoadLevelSignalEvent
                -= OnStartCheckLoadLevelSignal;
            _levelLoader.OnStopCheckLoadLevelSignalEvent
                -= OnStopCheckLoadLevelSignal;
            _lobbyConnection.OnCreateConnectionSeccessEvent
                -= OnCreateConnectionSeccess;
            _lobbyConnection.OnCreateConnectionFailedEvent
                -= OnCreateConnectionFailed;
        }
        public void OnCanceledOrFailedLoadLevelSignal()
        {
            _processText.gameObject.SetActive(true);
            _processText.text = c_CheckSignalFailedText;
        }
        public void OnStartCheckLoadLevelSignal()
        {
            _endPointText.gameObject.SetActive(false);
            _processText.text = c_CheckText;
        }
        public void OnStopCheckLoadLevelSignal()
        {
            _processText.text = c_LoadLevelText;
            //await Task.Delay(1000);
            _processText.gameObject.SetActive(false);
            _loadImage.SetActive(false);
        }
        public void OnCancelServerConnection()
        {
            _processText.text = c_CanceledText;
        }
        public void OnCancelClientConnection()
        {
            _processText.text = c_CanceledText;
        }
        public void OnStartCreateConnection()
        {
            _processText.text = c_ConnectionText;
        }
        public void OnCreateConnectionSeccess()
        {
            _processText.text = c_SuccsessConnectionText;
        }
        public void OnCreateConnectionFailed()
        {
            _processText.text = c_ConnectionFailedText;
        }
        public void OnCreateServerEndPoint(string endPoint)
        {
            _endPointText.gameObject.SetActive(true);
            _endPointText.text = endPoint;
        }

    }
}
