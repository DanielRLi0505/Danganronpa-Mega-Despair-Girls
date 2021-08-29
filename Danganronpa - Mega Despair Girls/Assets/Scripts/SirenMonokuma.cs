using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirenMonokuma : Enemy
{
    GameObject Komaru;
    public int walkingSpeed = 150;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        currentHealth = maxHealth;
        setContactDamage(1);
        setMaxHealth(1);
        currentHealth = maxHealth;
        setScorePoints(250);
        Komaru = GameObject.FindGameObjectWithTag("Player");
        despawn();
    }

    // Update is called once per frame
    void Update()
    {
        body2D.velocity = new Vector2(walkingSpeed, 0);
        changeDirection();
        
    }

    void changeDirection()
    {
        if (Komaru != null)
        {
            if (Komaru.transform.position.x - transform.position.x > 0)
            {
                setSpeed(walkingSpeed);
            }
            else
            {
                setSpeed(-walkingSpeed);
            }
            Debug.Log(Komaru.transform.position.x - transform.position.x);
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
