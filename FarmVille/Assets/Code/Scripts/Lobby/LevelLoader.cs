using System;
using System.Threading.Tasks;
using UnityEngine;
using ClientServer.Client;
using ClientServer.Server;
using UnityEngine.SceneManagement;
using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Lobby.LoadingLevel;
using System.Threading;
using PimDeWitte.UnityMainThreadDispatcher;

namespace Assets.Code.Scripts.Lobby
{
    public class LevelLoader : MonoBehaviour
    {
        public event Action OnCanceledOrFailedLoadLevelSignalEvent;
        public event Action OnStartCheckLoadLevelSignalEvent;
        public event Action OnStopCheckLoadLevelSignalEvent;
        public static Action onLevelLoadedEvent { get; set; }
        LobbyConnection _lobbyConnection;
        SceneName _sceneName;
        bool _signalResult;

        LevelSignalChecker _signalChecker;

        CancellationTokenSource _cancellationTokenSource;
        Server _server;
        Client _client;

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
            _server = server;
            User.Instance.InitializeUserBase(server, ConnectionType.Server);
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
            }
        }

        public async void OnClientConnecitonCreated(Client client)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _client = client;
            User.Instance.InitializeUserBase(client, ConnectionType.Client);
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
            }
        }
        async Task CheckSignal(Task checkSignalLoadingTask)
        {
            OnStartCheckLoadLevelSignalEvent?.Invoke();
            await Task.Delay(1000);
            await checkSignalLoadingTask.ContinueWith((compliteTask) =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    OnStopCheckLoadLevelSignalEvent?.Invoke();
                });
            });

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
    }
}
