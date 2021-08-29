using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public bool snapped;

    [SerializeField] Transform LadderMask;
    [SerializeField] Transform player;
    

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetButton("Vertical"))
            {
                Debug.Log("Event triggered");
                snapped = true;
                player.position = new Vector2(LadderMask.localPosition.x, player.position.y);
            }
            else
            {
                snapped = false;
            }
            
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            snapped = false;
        }
    }
}
