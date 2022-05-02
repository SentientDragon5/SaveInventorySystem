using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Copyright SentientDragon5 2022
 * Please Attribute
 * 
 * These are the Scriptable Objs that you use to add to the key to have an inventory system
 */

namespace Inv3
{
    [CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item Asset")]
    public class ItemAsset3 : ScriptableObject
    {
        //public new string name = "Item";
        public Sprite icon;
        public int stackAmount = 64;

        public GameObject prefab;

        public int index;
    }
}
