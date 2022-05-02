using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/*
 * Copyright SentientDragon5 2022
 * Please Attribute
 * 
 * To show what items you have to have a base class of any button on the ui.
 */


namespace UIInv
{
    public class UISelectable : MonoBehaviour
    {
        public int index;
        private Image img;
        public virtual void Interact(int si)
        {

        }
        public Vector2 size = Vector2.one * 25;
        public Vector3 offset = Vector3.zero;
        public Vector3 Position
        {
            get
            {
                return transform.position + offset;
            }
        }
        private void Start()
        {
            Refresh();
        }
        public void Refresh()
        {
            img = GetComponent<Image>();
            img.color = UISelect.instance.selectedIndex == index ? selected : notSelected;
        }
        public Color selected = new Color(0.8f, 1f, 0.8f);
        public Color notSelected = new Color(0.8f, 0.8f, 0.8f);

        public void OnSelected()
        {
            
            img.color = selected;
        }
        public void OnUnSelected()
        {
            img.color = notSelected;
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(Position, new Vector3(size.x, size.y, 20));
        }
    }
}