using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monokuma : Enemy
{

    // Start is called before the first frame update
    public int walkingSpeed = -150;
    public float checkRadius;
    

    [SerializeField] Transform wallCheck;
    [SerializeField] Transform wallCheck2;
    [SerializeField] LayerMask whatIsWall;

    void Start()
    {
        base.Start();
        currentHealth = maxHealth;
        setContactDamage(1);
        setMaxHealth(1);
        setScorePoints(250);
        setSpeed(walkingSpeed);
    }

    // Update is called once per frame

    void Update()
    {
        body2D.velocity = new Vector2(speed, 0);
        despawn();

    }
    void changeDirection()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Wall"))
        {
            walkingSpeed = -walkingSpeed;
            setSpeed(walkingSpeed);
            if (walkingSpeed > 0)
            {
                changeState("monowalkright");
            }
            else
            {
                changeState("monowalk");
            }
        }
    }
}
