using UnityEngine;
using TMPro;

public class UIGame : MonoBehaviour
{
    public PongGameManager gameManager; // Referensi ke PongGameManager

    // UI untuk Health Paddle 1 dan Paddle 2
    public TMP_Text healthP1Text;
    public TMP_Text healthP2Text;

    // Notifikasi Kemenangan
    public TMP_Text winNotificationText; // Text untuk notifikasi kemenangan

    private void Start()
    {
        // Sembunyikan teks notifikasi kemenangan di awal
        if (winNotificationText != null)
        {
            winNotificationText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        UpdateHealthUI();
        CheckWinCondition();
    }

    private void UpdateHealthUI()
    {
        if (gameManager != null)
        {
            // Periksa apakah paddle1 dan paddle2 tidak null, lalu perbarui teks health
            if (gameManager.paddle1 != null && healthP1Text != null)
            {
                healthP1Text.text = $"Health Paddle 1: {gameManager.paddle1.health}";
            }

            if (gameManager.paddle2 != null && healthP2Text != null)
            {
                healthP2Text.text = $"Health Paddle 2: {gameManager.paddle2.health}";
            }
        }
    }

    private void CheckWinCondition()
    {
        if (gameManager != null)
        {
            if (gameManager.paddle1 != null && gameManager.paddle1.health <= 0)
            {
                DisplayWinNotification("Paddle 2 Menang!");
            }
            else if (gameManager.paddle2 != null && gameManager.paddle2.health <= 0)
            {
                DisplayWinNotification("Paddle 1 Menang!");
            }
        }
    }

    private void DisplayWinNotification(string message)
    {
        if (winNotificationText != null)
        {
            winNotificationText.text = message;
            winNotificationText.gameObject.SetActive(true); // Menampilkan teks notifikasi kemenangan
        }
    }
}
