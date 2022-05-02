using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using static Saving.SaveUtil;

/*
 * Copyright SentientDragon5 2022
 * Please Attribute
 * 
 * The inventory is the component where it actually stores which items the player is holding. It can be saved and loaded through the charsave
 * The important thing here is that the inventory tells the UI when to refresh. It also has methods to convert to and from jagged arrays for saving
 * 
 */


namespace Inv3
{
    public class Inventory3 : MonoBehaviour
    {
        public Transform pickupParent;
        public Slot[] slots;
        public int maxSlots = 5;
        private void Start()
        {
            slots = new Slot[maxSlots];
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i] = new Slot(-1, 0);
            }
        }

        public bool recreate = true;
        public UnityEvent onChange;
        /// <summary>
        /// Will return true if successfully Added, and False if no open slots.
        /// </summary>
        /// <param name="ki"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool Add(int ki, int a)
        {
            int slot = NextPartialOrEmpty(ki);
            if (slot == -1)
                return false;
            int maxStack = slots[slot].Item.stackAmount;
            int room = maxStack - slots[slot].amount;
            if(a > room)
            {
                slots[slot].amount = maxStack;
                AddOrDrop(ki, a - room);
                recreate = true;
            }
            else
            {
                slots[slot].amount += a;
            }
            onChange.Invoke();
            return true;
        }
        public void AddOrDrop(int ki, int a)
        {
            if(!Add(ki, a))
            {
                recreate = true;
                //Drop Item
                Pickup3.Drop(pickupParent, ItemKey3.Key.GetItem(ki), transform.position - transform.forward);
            }
        }
        public void Drop(int si)
        {
            Pickup3.Drop(pickupParent, ItemKey3.Key.GetItem(slots[si].index), transform.position - transform.forward);
            Remove(si, 1);
        }
        public bool Add(ItemAsset3 item, int a)
        {
            int slot = NextPartialOrEmpty(item, out bool found);
            if (slot == -1)
                return false;
            if(found)
            {
                int maxStack = item.stackAmount;
                int room = maxStack - slots[slot].amount;
                if (a > room)
                {
                    slots[slot].amount = maxStack;
                    AddOrDrop(item, a - room);
                    recreate = true;
                }
                else
                {
                    slots[slot].amount += a;
                }
            }
            else
            {
                recreate = true;
                slots[slot] = new Slot(item.index, a);
            }
            onChange.Invoke();
            return true;
        }
        public void AddOrDrop(ItemAsset3 item, int a)
        {
            if (!Add(item, a))
            {
                //Drop Item
                Pickup3.Drop(null, item, transform.position - transform.forward);
            }
        }

        /// <summary>
        /// Find the amount of that item
        /// </summary>
        public int ItemAmount(int ki)
        {
            int amount = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                if(slots[i].index == ki)
                {
                    amount += slots[i].amount;//if there are multiple slots holding
                }
            }
            return amount;
        }
        public int NextEmptySlot()
        {
            for (int i = slots.Length - 1; i >= 0; i--)
            {
                if(slots[i] == null || slots[i].index == -1)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// Will return the next partially full slot of item(ki), or if none, then the next empty slot. if not -1
        /// </summary>
        public int NextPartialOrEmpty(int ki)
        {
            int empty = -1;
            for (int i = slots.Length - 1; i >= 0; i--)
            {
                if (slots[i] == null || slots[i].index == -1)
                {
                    empty = i;
                }
                if(slots[i].index == ki && slots[i].amount < slots[i].Item.stackAmount)
                {
                    return i;
                }
            }
            return empty;
        }
        public int NextPartialOrEmpty(ItemAsset3 item, out bool found)
        {
            found = true;
            int empty = -1;
            for (int i = slots.Length-1; i >= 0; i--)
            {
                if (slots[i] == null || slots[i].index == -1 || slots[i].Item == null)
                {
                    empty = i;
                }
                else
                {
                    if (slots[i].Item.index == item.index && slots[i].amount < slots[i].Item.stackAmount)
                    {
                        return i;
                    }
                }
            }
            found = false;
            return empty;
        }
        public int NextPartialOrEmpty(ItemAsset3 item)
        {
            int empty = -1;
            for (int i = slots.Length - 1; i >= 0; i--)
            {
                if (slots[i] == null || slots[i].index == -1 || slots[i].Item == null)
                {
                    empty = i;
                }
                else
                {
                    if (slots[i].Item.index == item.index && slots[i].amount < slots[i].Item.stackAmount)
                    {
                        return i;
                    }
                }
            }
            return empty;
        }
        public void Remove(int si, int amount)
        {
            if (slots[si].amount > amount)
                slots[si].amount -= amount;
            else
            {
                slots[si].SetEmpty();
                recreate = true;
            }
            onChange.Invoke();
        }


        public void Save(out int[][] inventorySave)
        {
            inventorySave = ToJagged(slots);
        }
        public void Load(int[][] inventorySave)
        {
            slots = ToGroups(inventorySave);
            recreate = true;
            onChange.Invoke();
        }


        public static int[][] ToJagged(Slot[] slots)
        {
            int[][] o = new int[slots.Length][];
            for (int i = 0; i < slots.Length; i++)
            {
                o[i] = new int[2];
                if (i < slots.Length)
                {
                    o[i][0] = slots[i].index;
                    o[i][1] = slots[i].amount;
                }
                else
                {
                    o[i][0] = -1; // arr index 0 holds the index in the item key
                    o[i][1] = 0;  // arr index 1 holds the amount of items
                }
            }
            return o;
        }
        public static Slot[] ToGroups(int[][] arr)
        {
            Slot[] o = new Slot[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                o[i] = new Slot(arr[i][0], arr[i][1]);
            }
            return o;
        }
    }

    [System.Serializable]
    public class Slot
    {
        //[HideInInspector] public string name;

        public ItemAsset3 Item
        {
            get
            {
                if (index == -1)
                    return null;
                return ItemKey3.Key.GetItem(index);
            }
        }
        public int index;
        public int amount;

        public static bool Active(Slot s)
        {
            return !(s.Equals(null) || s.index == -1 || s.amount == 0);
        }

        public Slot(int i, int a)
        {
            index = i;
            amount = a;
        }
        public void Add(int a)
        {
            amount += a;
        }
        public void SetEmpty()
        {
            index = -1;
            amount = 0;
        }
    }
}

