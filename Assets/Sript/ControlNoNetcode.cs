using UnityEngine;

public class ControlNoNetcode : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject ballPrefab;
    public Transform spawnPoint; // Tempat di mana bola akan di-spawn
    public KeyCode moveUpKey = KeyCode.W; // Tombol untuk gerak ke atas
    public KeyCode moveDownKey = KeyCode.S; // Tombol untuk gerak ke bawah
    public KeyCode shootKey = KeyCode.Space; // Tombol untuk menembakkan bola

    void Update()
    {
        // Mengatur pergerakan ke atas
        if (Input.GetKey(moveUpKey))
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
        
        // Mengatur pergerakan ke bawah
        if (Input.GetKey(moveDownKey))
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }

        // Menembakkan bola saat tombol shootKey ditekan
        if (Input.GetKeyDown(shootKey))
        {
            ShootBall();
        }
    }

    private void ShootBall()
    {
        // Membuat bola baru di posisi spawnPoint dan menambahkan kecepatan
        GameObject ball = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            rb.velocity = Vector2.right * 5f; // Menembakkan bola ke kanan
        }
    }
}
