using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using ClientServer;
using ClientServer.Client;
using ClientServer.Server;

namespace Assets.Code.Scripts.Lobby
{
    public class LobbyService : MonoBehaviour
    {
        GameObject _startPanel;
        GameObject _createPanel;
        GameObject _connectPanel;
        [Inject]
        public void Constructor(
            [Inject(Id = "StartPanel")] GameObject startPanel,
            [Inject(Id = "CreatePanel")] GameObject createPanel,
            [Inject(Id = "ConnectPanel")] GameObject connectPanel
            )
        {
            _startPanel = startPanel;
            _createPanel = createPanel;
            _connectPanel = connectPanel;
        }
        public void OnConnect()
        {
            _startPanel.SetActive(false);
            _connectPanel.SetActive(true);
        }
        public void OnCreate()
        {
            _startPanel.SetActive(false);
            _createPanel.SetActive(true);
        }
        public void OnExit()
        {
            Application.Quit();
        }
        public void ConnectBack()
        {
            _connectPanel.SetActive(false);
            _startPanel.SetActive(true);
        }
        public void CreateBack()
        {
            _createPanel.SetActive(false);
            _startPanel.SetActive(true);
        }
    }
}
