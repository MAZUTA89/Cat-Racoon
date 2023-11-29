using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ClientServer.Client;
using ClientServer.Server;
using UnityEngine.SceneManagement;
using Assets.Code.Scripts.Boot;
using TMPro;
using Zenject;
using Assets.Code.Scripts.Lobby.LoadingLevel;

namespace Assets.Code.Scripts.Lobby
{
    public class LevelLoader : MonoBehaviour
    {
        TextMeshProUGUI _processText;
        GameObject _loadImage;
        public static Action onLevelLoadedEvent { get; private set; }
        const string c_LoadLevelText = "Loading level ...";
        const string c_CheckText = "Check signal ...";
        LobbyConnection _lobbyConnection;
        SceneName _sceneName;

        LevelSignalChecker _signalChecker;

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
            _lobbyConnection = GetComponent<LobbyConnection>();
            _sceneName = SceneName.Farm;
            _lobbyConnection.onClientConnectionCreatedEvent 
                += OnClientConnecitonCreated;
            _lobbyConnection.onServerConnectionCreatedEvent
                += OnServerConnectionCreated;
            _signalChecker = new LevelSignalChecker();
        }
        private void OnDisable()
        {
            _lobbyConnection.onClientConnectionCreatedEvent
                -= OnClientConnecitonCreated;
            _lobbyConnection.onServerConnectionCreatedEvent
                -= OnServerConnectionCreated;
        }

        public async void OnServerConnectionCreated(Server server)
        {
            User.Instance.InitializeUserBase(server);
            Task checkLevelLoadingTask =
                Task.Run(async () => {
                    await _signalChecker.CheckServerLevelSignal(server);
            });
            _processText.text = c_CheckText;
            await Task.Delay(1000);
            await checkLevelLoadingTask;
            _processText.text = c_LoadLevelText;
            await Task.Delay(1000);
            await LoadLevel(_sceneName);
        }

        public async void OnClientConnecitonCreated(Client client)
        {
            User.Instance.InitializeUserBase(client);
            Task checkLevelLoadingTask =
                Task.Run(async () => {
                    await _signalChecker.CheckClientLevelSignal(client);
                });

            _processText.text = c_CheckText;
            await Task.Delay(1000);
            await checkLevelLoadingTask;
            _processText.text = c_LoadLevelText;
            await Task.Delay(1000);
            await LoadLevel(_sceneName);
        }


        async Task LoadLevel(SceneName sceneName)
        {
            var tcs = new TaskCompletionSource<bool>();

            AsyncOperation asyncLoad = 
                SceneManager.LoadSceneAsync(sceneName.ToString());

            asyncLoad.completed += (operation) =>
            {
                _processText.gameObject.SetActive(false);
                _loadImage.gameObject.SetActive(false);
                onLevelLoadedEvent?.Invoke();
                tcs.SetResult(true);
            };

            while(asyncLoad.isDone == false)
            {
                Debug.Log("Загрузка уровня!");
                await Task.Delay(1);
            }
            await tcs.Task;
           
        }
    }
}
