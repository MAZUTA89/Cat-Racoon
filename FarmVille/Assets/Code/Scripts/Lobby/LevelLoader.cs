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
using UnityEngine.Playables;

namespace Assets.Code.Scripts.Lobby
{
    public class LevelLoader : MonoBehaviour
    {
        public event Action OnCanceledOrFailedLoadLevelSignalEvent;
        public event Action OnStartCheckLoadLevelSignalEvent;
        public event Action OnStopCheckLoadLevelSignalEvent;
        [SerializeField] List<GameObject> BackButtons;
        public static Action onLevelLoadedEvent { get; private set; }
        const string c_LoadLevelText = "Loading level ...";
        const string c_CheckText = "Check signal ...";
        const string c_CheckSignalFailedText = "Check signal failed!";
        static int i = 0;
        TextMeshProUGUI _processText;
        GameObject _loadImage;
        LobbyConnection _lobbyConnection;
        SceneName _sceneName;
        bool _signalResult;

        LevelSignalChecker _signalChecker;

        CancellationTokenSource _cancellationTokenSource;
        Server _server;
        Client _client;

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
            i++;
            _cancellationTokenSource = new CancellationTokenSource();
            _server = server;
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
            else
            {
                _server?.Stop();
                OnCanceledOrFailedLoadLevelSignalEvent?.Invoke();
                //_processText.gameObject.SetActive(true);
                //_processText.text = c_CheckSignalFailedText;
                ActivateBackButtons();
            }
        }

        public async void OnClientConnecitonCreated(Client client)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _client = client;
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
            else
            {
                _client?.Stop();
                OnCanceledOrFailedLoadLevelSignalEvent?.Invoke();
                //_processText.gameObject.SetActive(true);
                //_processText.text = c_CheckSignalFailedText;
                ActivateBackButtons();
            }
        }

        async Task CheckSignal(Task checkSignalLoadingTask)
        {
            OnStartCheckLoadLevelSignalEvent?.Invoke();
            /*_processText.text = c_CheckText*/;
            await Task.Delay(1000);
            await checkSignalLoadingTask.ContinueWith((compliteTask) =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    DeactivateBackButtons();
                });
            });
            OnStopCheckLoadLevelSignalEvent?.Invoke();
            //_processText.text = c_LoadLevelText;
            //await Task.Delay(1000);
            //_processText.gameObject.SetActive(false);
            //_loadImage.SetActive(false);

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
            finally
            {
                _server?.Stop();
                _client?.Stop();
            }
        }
        void DeactivateBackButtons()
        {
            foreach (var button in BackButtons)
            {
                button.SetActive(false);
            }
        }
        void ActivateBackButtons()
        {
            foreach (var button in BackButtons)
            {
                button.SetActive(true);
            }
        }

    }
}
