using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{
    /*(Komaru player;
    public LayerMask whatIsGround;
    public LayerMask whatIsTop;
    bool isGrounded;

    //Rigidbody2D body2D;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Komaru>();
      //  body2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //body2D.velocity = player.velocity;
        isGrounded = getGround();
    }

    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D collider)
    {
        isGrounded = collider != null && (((1 << collider.gameObject.layer) & whatIsGround) != 0)
            || collider != null && (((1 << collider.gameObject.layer) & whatIsTop) != 0);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("LadderTop"))
        {
            isGrounded = false;
            Debug.Log("In air");
        }
    }

    public bool getGround()
    {
        return isGrounded;
    }*/
}
