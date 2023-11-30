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
using System.Threading;
using PimDeWitte.UnityMainThreadDispatcher;

namespace Assets.Code.Scripts.Lobby
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] List<GameObject> BackButtons;

        public static Action onLevelLoadedEvent { get; private set; }
        const string c_LoadLevelText = "Loading level ...";
        const string c_CheckText = "Check signal ...";
        TextMeshProUGUI _processText;
        GameObject _loadImage;
        LobbyConnection _lobbyConnection;
        SceneName _sceneName;
        bool _signalResult;

        LevelSignalChecker _signalChecker;

        CancellationTokenSource _cancellationTokenSource;

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
            _signalResult = false;
            _lobbyConnection = GetComponent<LobbyConnection>();
            _sceneName = SceneName.Farm;
            _lobbyConnection.onClientConnectionCreatedEvent
                += OnClientConnecitonCreated;
            _lobbyConnection.onServerConnectionCreatedEvent
                += OnServerConnectionCreated;
            _lobbyConnection.OnCancelClientConnectionEvent
                += OnClientCanceled;
            _lobbyConnection.OnCancelServerConnectionEvent
                += OnServerCanceled;
            _signalChecker = new LevelSignalChecker();
        }
        private void OnDisable()
        {
            _lobbyConnection.onClientConnectionCreatedEvent
                -= OnClientConnecitonCreated;
            _lobbyConnection.onServerConnectionCreatedEvent
                -= OnServerConnectionCreated;
            _lobbyConnection.OnCancelClientConnectionEvent
                -= OnClientCanceled;
            _lobbyConnection.OnCancelServerConnectionEvent
                -= OnServerCanceled;
        }

        public async void OnServerConnectionCreated(Server server)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            User.Instance.InitializeUserBase(server);
            Task checkLevelLoadingTask =
                Task.Run(async () =>
                {
                    _signalResult = await _signalChecker.CheckServerLevelSignal(server,
                        _cancellationTokenSource.Token);
                }, _cancellationTokenSource.Token);

            await CheckSignal(checkLevelLoadingTask);
            if (!_cancellationTokenSource.Token.IsCancellationRequested && _signalResult)
            {
                await LoadLevel(_sceneName);
                onLevelLoadedEvent?.Invoke();
            }
        }

        public async void OnClientConnecitonCreated(Client client)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            User.Instance.InitializeUserBase(client);
            Task checkLevelLoadingTask =
                Task.Run(async () =>
                {
                    _signalResult = await _signalChecker.CheckClientLevelSignal(client,
                        _cancellationTokenSource.Token);
                }, _cancellationTokenSource.Token);

            await CheckSignal(checkLevelLoadingTask);
            if (!_cancellationTokenSource.Token.IsCancellationRequested && _signalResult)
            {
                await LoadLevel(_sceneName);
                onLevelLoadedEvent?.Invoke();
            }
        }

        async Task CheckSignal(Task checkSignalLoadingTask)
        {
            _processText.text = c_CheckText;
            await Task.Delay(1000);
            await checkSignalLoadingTask.ContinueWith((compliteTask) =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    DeactivateBackButtons();
                });
            });

            _processText.text = c_LoadLevelText;
            await Task.Delay(1000);
            _processText.gameObject.SetActive(false);
            _loadImage.SetActive(false);

        }
        async Task LoadLevel(SceneName sceneName)
        {
            var tcs = new TaskCompletionSource<bool>();

            AsyncOperation asyncLoad =
                SceneManager.LoadSceneAsync(sceneName.ToString());

            asyncLoad.completed += (operation) =>
            {
                tcs.SetResult(true);
            };

            while (asyncLoad.isDone == false)
            {
                Debug.Log("Загрузка уровня!");
                await Task.Delay(1);
            }

            await tcs.Task;
        }

        public void OnServerCanceled()
        {
            Cancel();
        }

        public void OnClientCanceled()
        {
            Cancel();
        }

        void Cancel()
        {
            try
            {
                _cancellationTokenSource?.Cancel();
            }
            catch (AggregateException ex)
            {
                Debug.Log(ex.Message);
            }
        }
        void DeactivateBackButtons()
        {
            foreach (var button in BackButtons)
            {
                button.SetActive(false);
            }
        }

    }
}
