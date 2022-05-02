using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * Copyright SentientDragon5 2022
 * Please Attribute
 * 
 * To interact with items
 */

namespace UIInv
{
    public class UISelectEvent : UISelectable
    {
        public UnityEvent OnInteract;
        public override void Interact(int si)
        {
            OnInteract.Invoke();
        }
    }
}