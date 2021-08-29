using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float waitTime;
    Komaru player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Komaru>();
        effector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") == 0)
        {
            waitTime = 0.5f;
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            effector.rotationalOffset = 180f;
            if (waitTime <= 0)
            {
                waitTime = 0.5f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            effector.rotationalOffset = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetAxis("Vertical") < 0)
            {
                player.onLadder = true;
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), other, false);
                Debug.Log("Climb");
            }
        }
        
    }


}
