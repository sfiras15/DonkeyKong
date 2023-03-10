using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private Rigidbody2D rigidB;
    public float speed = 3f;
    // Start is called before the first frame update
    void Awake()
    {
        rigidB = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.layer == 7){
            // Speed up the barrels once they hit a platform
            rigidB.AddForce(other.transform.right * speed,ForceMode2D.Impulse);
        }
    }
}
