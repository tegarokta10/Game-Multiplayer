using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectbintang : MonoBehaviour
{

    private void Update(){
        this.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Periksa jika objek bertabrakan dengan objek target yang diinginkan
        if (other.CompareTag("Paddle1") || other.CompareTag("Paddle2")) // Pastikan object lain memiliki tag "TargetObject"
        {
            Debug.Log("Trigger activated with target object!");
            
            // Tambahkan efek atau fungsi yang diinginkan saat bertabrakan
            // Contohnya: Destroy(gameObject);
            
        }
        
    }
}
