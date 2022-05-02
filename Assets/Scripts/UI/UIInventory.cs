using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inv3;
using UnityEngine.Events;

/*
 * Copyright SentientDragon5 2022
 * Please Attribute
 * 
 * The UI Inventory is made separate from the inventory system so that they can be implemented together or apart. they are codependent on each other here but the UI selectable
 * system can apply to any inventory.
 * The UI Inventory is told not to refresh every frame, but only when the Inventory says so. there is a UnityEvent that I use to call back.
 * 
 * if you do not already understand the concept of callbacks, look them up. It will allow your system to be more compact and run more efficiently
 * than every frame.
 */


namespace UIInv
{
    public class UIInventory : MonoBehaviour
    {
        //Singleton
        public static UIInventory instance;
        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public Inventory3 inv;
        public GameObject slotPrefab;
        public Transform group;
        public UISlot[] slots;

        public Transform hideShow;
        bool hide = true;
        public static bool PAUSED = false;
        public UnityEvent OnRecreate;
        //I integrated time into this menu. I often do this in a separate script, and I would recommend doing so.

        private void Start()
        {
            CreateUI();
            hideShow.gameObject.SetActive(!hide);
            inv.onChange.AddListener(RefreshUI);
        }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                PAUSED = !PAUSED;
                Time.timeScale = hide ? 0 : 1;
                hide = !hide;
                hideShow.gameObject.SetActive(!hide);
                RefreshUI();
            }
        }
        public void RefreshUI()
        {
            if (inv.recreate)
            {
                CreateUI();
                inv.recreate = false;
            }
            else
                UpdateUI();
        }

        [ContextMenu("Create")]
        public void CreateUI()
        {
            DestroyAllChildren(group);
            slots = new UISlot[inv.slots.Length];
            for (int i = 0; i <slots.Length; i++)
            {
                GameObject g = Instantiate(slotPrefab, group);
                if (Slot.Active(inv.slots[i]))
                    g.name = "Slot " + i + " (" + inv.slots[i].Item.name + ") ";
                else
                    g.name = "EMPTY Slot";

                slots[i] = g.GetComponent<UISlot>();
                slots[i].Set(inv.slots[i]);
            }

            OnRecreate.Invoke();
        }
        [ContextMenu("Update")]
        public void UpdateUI()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].Refresh(inv.slots[i].amount);
            }
        }

        void DestroyAllChildren(Transform t)
        {
            for (int i = 0; i < t.childCount; i++)
            {
                Destroy(t.GetChild(i).gameObject);
            }
        }
    }
}