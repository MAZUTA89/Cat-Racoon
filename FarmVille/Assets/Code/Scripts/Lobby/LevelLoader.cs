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

namespace Assets.Code.Scripts.Lobby
{
    public class LevelLoader : MonoBehaviour
    {
        public TextMeshProUGUI ProcessText;
        public static Action onLevelLoadedEvent;
        const string c_LoadLevelText = "Loading level ...";
        LobbyConnection _lobbyConnection;
        SceneName _sceneName;
        private void Start()
        {
            _lobbyConnection = GetComponent<LobbyConnection>();
            _sceneName = SceneName.Farm;
            _lobbyConnection.onClientConnectionCreatedEvent 
                += OnClientConnecitonCreated;
            _lobbyConnection.onServerConnectionCreatedEvent
                += OnServerConnectionCreated;
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
            await LoadLevel(_sceneName);
            onLevelLoadedEvent?.Invoke();
        }

        public async void OnClientConnecitonCreated(Client client)
        {
            User.Instance.InitializeUserBase(client);
            await LoadLevel(_sceneName);
            onLevelLoadedEvent?.Invoke();
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

            while(asyncLoad.isDone == false)
            {
                Debug.Log("Загрузка уровня!");
                await Task.Delay(1);
            }

            await tcs.Task;
        }
    }
}
