using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingPlatform : MonoBehaviour
{

    Rigidbody2D body2D;
    Animator ani;
    
    void Start()
    {
        body2D = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (Input.GetAxis("Vertical") != 0)
            {
                body2D.velocity = new Vector2(0, Input.GetAxis("Vertical") * 100);
                ani.SetInteger("Rising", 1);
            }
            else
            {
                ani.SetInteger("Rising", 0);
            }
        }
    }


}
