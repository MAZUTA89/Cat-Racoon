using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Assets.Code.Scripts.Gameplay
{
    public class GameOverUI : MonoBehaviour
    {
        const string c_menuSceneName = "MenuScene";
        [SerializeField] GameObject GameOverPanel;
        [SerializeField] TextMeshProUGUI LoseText;
        [SerializeField] TextMeshProUGUI WinText;
        [SerializeField] MoneyDisplayer PlayerCoin;
        [SerializeField] MoneyDisplayer ConnectedPlayerCoin;
        private void Start()
        {
            GameEvents.OnGameOverEvent += OnGameOver;
            GameOverPanel.SetActive(false);
            LoseText.gameObject.SetActive(false);
            WinText.gameObject.SetActive(false);
        }
        private void OnDisable()
        {
            GameEvents.OnGameOverEvent -= OnGameOver;
        }

        void OnGameOver()
        {
            GameOverPanel.SetActive(true);
            if (PlayerCoin.GetMoney() > ConnectedPlayerCoin.GetMoney())
            {
                WinText.gameObject.SetActive(true);
            }
            else
                LoseText.gameObject.SetActive(true);
        }

        public void OnExit()
        {
            SceneManager.LoadScene(c_menuSceneName);
        }
    }
}
