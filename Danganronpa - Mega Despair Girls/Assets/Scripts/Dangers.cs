using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dangers : Enemy
{

    // Start is called before the first frame update
    void Start()
    {
        setContactDamage(28);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*void setContactDamage(int contact)
    {
        base.setContactDamage(contact);
    }
    int getContactDamage()
    {
        return contactDamage;
    }*/

    public override void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerStay2D(other);
    }
}
