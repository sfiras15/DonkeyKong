using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public Vector2 direction;
    public float minTime = 2f;
    public float maxTime = 4f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Spawn),0f,Random.Range(minTime,maxTime));
    }

    private void Spawn()
    {
        Instantiate(prefab,transform.position,Quaternion.identity);
    }
}
