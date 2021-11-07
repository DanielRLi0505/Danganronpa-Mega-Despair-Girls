using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dangers : MonoBehaviour
{

    // Start is called befo0re the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Komaru>().Defeat();
        }
    }
}
