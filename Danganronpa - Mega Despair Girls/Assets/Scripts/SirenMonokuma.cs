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
        setContactDamage(1);
        setMaxHealth(1);
        currentHealth = maxHealth;
        setScorePoints(250);
        Komaru = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        body2D.velocity = new Vector2(speed, 0);
        Invoke("changeDirection", 0.5f);
        despawn();
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
            if (speed > 0)
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
