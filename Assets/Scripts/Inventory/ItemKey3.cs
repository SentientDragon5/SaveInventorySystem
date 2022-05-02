using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Copyright SentientDragon5 2022
 * Please Attribute
 * 
 * This is the key that we use to have indexes of each itemAsset that now we can save just the Indexes to the binary file.
 */

namespace Inv3
{
    [CreateAssetMenu(fileName = "_Key", menuName = "Inventory/Item Key")]
    public class ItemKey3 : ScriptableObject
    {
        public ItemAsset3[] items;

        [ContextMenu("Give Indexes")]
        public void GiveIndexes()
        {
            for(int i=0; i<items.Length; i++)
            {
                items[i].index = i;
            }
        }
        public ItemAsset3 GetItem(int index)
        {
            if (index == -1)
                return null;
            return items[index];
        }
        public void GetIndexes(ItemAsset3 item, out int index)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (item.Equals(GetItem(i)))
                {
                    index = i;
                    return;
                }
            }
            index = -1;
        }


        #region Singleton
        public static ItemKey3 Key
        {
            get
            {
                if(key == null)
                {
                    ItemKey3[] keys = Object.FindObjectsOfType<ItemKey3>();
                    if (keys.Length > 0)
                        key = keys[0];
                }
                return key;
            }
        }
        public static ItemKey3 key;
        [ContextMenu("MAKE STATIC")]
        public void SetStatic()
        {
            key = this;
        }
        private void OnValidate()
        {
            GiveIndexes();
            if (key == null)
                key = this;
        }
        private void OnDestroy()
        {
            if (key == this)
                key = null;
        }
        #endregion
    }
}