using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inv3;


/*
 * Copyright SentientDragon5 2022
 * Please Attribute
 * 
 * This is a simple class that allows players to interact with nearby objects and add objects to inventory.
 */

public class Interactor : MonoBehaviour
{
    public float radius = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UIInv.UIInventory.PAUSED)
            return;
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, radius);
            foreach(Collider c in cols)
            {
                if(c.TryGetComponent(out Pickup3 p))
                {
                    p.Interact(transform);
                }
                //if(c.TryGetComponent<Pickup>(out Pickup p))
                //{
                    //GetComponent<Inventory>().
                //}
            }
        }
    }
}
