using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inv3;
using TMPro;
using UnityEngine.UI;

/*
 * Copyright SentientDragon5 2022
 * Please Attribute
 * 
 * UISlots are for the actual visuals in the inventory and they allow it to not just be set once, but updated. 
 * They also allow for the player to interact with the item to eat, use, drop or do such actions with the item in the inventory.
 */

namespace UIInv
{
    public class UISlot : UISelectable
    {
        public int ki = -1;//short for key index
        public bool empty;
        public Sprite icon;
        public int amount;

        public TextMeshProUGUI tmp;
        public Image iconImage;

        public override void Interact(int si)
        {
            // use an item? create a dropdown
            // drop for now
            if(ki != -1 && !empty)
                UIInventory.instance.inv.Drop(si);
        }
        private void Start()
        {
            //img = GetComponentsInChildren<Image>()[1];
            //tmp = GetComponentsInChildren<TextMeshProUGUI>()[0];
        }
        public UISlot Set(Slot s)
        {
            if (s.Equals(null) || s.index == -1 || s.amount == 0)
            {
                ki = -1;
                empty = true;
                tmp.gameObject.SetActive(false);
                iconImage.gameObject.SetActive(false);
                return this;
            }
            empty = false;
            //Debug.Log(s.index);

            ki = s.index;
            icon = s.Item.icon;
            amount = s.amount;
            iconImage.sprite = icon;//Sprite.Create(icon, new Rect(Vector2.zero, new Vector2(icon.width, icon.height)), Vector2.one / 2);
            tmp.text = "" + amount;
            return this;
        }
        public void Refresh(int amount)
        {
            if(empty)
            {
                tmp.gameObject.SetActive(false);
                return;
            }
            this.amount = amount;
            tmp.text = "" + amount;
        }

    }
}