using UnityEngine;
using Unity.Netcode;

public class PongGameManager : NetworkBehaviour
{
    private int player1Points = 0;
    private int player2Points = 0;

    // Fungsi untuk menambah poin Player 1
    public void AddPointToPlayer1()
    {
        player1Points++;
        UpdateScore();
    }

    // Fungsi untuk menambah poin Player 2
    public void AddPointToPlayer2()
    {
        player2Points++;
        UpdateScore();
    }

    // Fungsi untuk memperbarui skor (bisa dihubungkan ke UI di tempat lain jika diperlukan)
    private void UpdateScore()
    {
        Debug.Log($"Player 1: {player1Points} - Player 2: {player2Points}");
        // Anda bisa menghubungkan pembaruan skor ke UI di tempat lain atau hanya menampilkan di console
    }
}
