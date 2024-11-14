using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIGame : MonoBehaviour
{
    public PongGameManager gameManager;

    [SerializeField] private ControlPaddle paddle1;
    [SerializeField] private ControlPaddle paddle2;

    [SerializeField] private TMP_Text healthP1Text;
    [SerializeField] private TMP_Text healthP2Text;
    [SerializeField] private TMP_Text winNotificationText;
    [SerializeField] private Button restartButton;

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<PongGameManager>();
        }

        if (paddle1 != null)
        {
            paddle1.health.OnValueChanged += (oldHealth, newHealth) => UpdateHealthUI();
        }

        if (paddle2 != null)
        {
            paddle2.health.OnValueChanged += (oldHealth, newHealth) => UpdateHealthUI();
        }

        if (winNotificationText != null)
        {
            winNotificationText.gameObject.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);
            restartButton.onClick.AddListener(OnRestartButtonClicked);
        }
    }

    private void UpdateHealthUI()
    {
        if (paddle1 != null)
        {
            healthP1Text.text = $"HP P1: {paddle1.health.Value}";
        }

        if (paddle2 != null)
        {
            healthP2Text.text = $"HP P2: {paddle2.health.Value}";
        }
    }

    public void DisplayWinNotification(string message)
    {
        if (winNotificationText != null)
        {
            winNotificationText.text = message;
            winNotificationText.gameObject.SetActive(true);
        }
    }

    public void ShowRestartButton()
    {
        if (restartButton != null && !restartButton.gameObject.activeInHierarchy)
        {
            restartButton.gameObject.SetActive(true);
        }
    }

    private void OnRestartButtonClicked()
    {
        winNotificationText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        if (NetworkManager.Singleton.IsServer)
        {
            StartCoroutine(RestartGame());
        }
    }

    private IEnumerator RestartGame()
    {
        if (paddle1 != null) paddle1.ResetHealth();
        if (paddle2 != null) paddle2.ResetHealth();

        yield return new WaitForSeconds(1);

        NetworkManager.Singleton.SceneManager.LoadScene("InGame", LoadSceneMode.Single);
    }
}
