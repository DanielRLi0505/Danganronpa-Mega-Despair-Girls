using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chargedBullet : Bullet
{
    // Start is called before the first frame update
    void Start()
    {
        damage = 4;
        Invoke("DestroySelf", .5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
