using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * Copyright SentientDragon5 2022
 * Please Attribute
 * 
 * This is used to actually select and use items within the menu.
 */

namespace UIInv
{
    public class UISelect : MonoBehaviour
    {
        public static UISelect instance;
        private void Awake()
        {
            if (instance == null)
                instance = this;
        }
        public List<UISelectable> elements = new List<UISelectable>();
        public UISelectable selected;
        public int selectedIndex = 0;
        public Vector2 pos;
        public Vector3 dir;
        public int closest;
        public float dirMag = 150;


        void Start()
        {
            UIInventory.instance.OnRecreate.AddListener(ResetListCaller);
            Inv3.Save3.instance.onLoad.AddListener(UIInventory.instance.CreateUI);
            StartCoroutine(ResetList(1));
        }
        [ContextMenu("SetList")]
        public void SetList()
        {
            //Debug.Log("CALLED");
            elements = GetComponentsInChildren<UISelectable>(true).ToList();
            int i = elements.Count -1 ;
            while(i >= 0)
            {
                if (elements[i] == null)
                {
                    elements.RemoveAt(i);
                }
                else
                    i--;
            }
            for (int s = 0; s < elements.Count; s++)
            {
                elements[s].index = s;
                elements[s].Refresh();
            }
        }
        void ResetListCaller()
        {
            StartCoroutine(ResetList(1));
        }
        IEnumerator ResetList(int frames)
        {
            for (int i = 0; i < frames; i++)
            {
                yield return new WaitForEndOfFrame();
            }
            SetList();
        }


        void Update()
        {
            if (!UIInventory.PAUSED)
                return;
            selected = elements[selectedIndex];
            
            Vector3 inV3 = GetInputV3();
            closest = ClosestToSelected(inV3 * GetDirMag(inV3));
            if (closest != selectedIndex && closest != -1)
            {
                selected.OnUnSelected();
                selectedIndex = closest;
                elements[selectedIndex].OnSelected();

                selected = elements[selectedIndex];
            }
            if(Input.GetKeyDown(KeyCode.Q))
            {
                selected.Interact(selectedIndex);
            }
            //selected = GetElementInDirection(GetInput());
            //pos = CavPos(selected.transform);
            //dir = GetInput();
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Vector3 inV3 = GetInputV3();
            if(selected != null)
                Gizmos.DrawCube(inV3 * GetDirMag(inV3) + selected.Position, Vector3.one * 10);
        }
        public float GetDirMag(Vector3 dir)
        {
            if (selected == null)
                return 0;
            return Mathf.Abs(selected.size.x * dir.x + selected.size.y * dir.y);
        }

        public int ClosestToSelected(Vector3 offset)
        {
            int indexClosest = -1;
            float closestDist = 10000000;//Vector3.Distance(positions[selectedIndex], positions[0];
            for (int i = 0; i < elements.Count; i++)
            {
                //if (i != selectedIndex)
                float dist = Vector3.Distance(selected.Position + offset, elements[i].Position);
                if (closestDist > dist)
                {
                    closestDist = dist;
                    indexClosest = i;
                }
            }
            return indexClosest;
        }
        public List<int> ClosestToSelectedAll(Vector3 offset)
        {
            UISelectable[] elms = new UISelectable[elements.Count];
            elements.CopyTo(elms);
            List<UISelectable> list = elements.ToList();
            List<int> lint = new List<int>();
            for (int i = 0; i < elements.Count; i++)
            {
                lint.Add(i);
            }
            lint = lint.OrderBy(i => Vector2.Distance(elements[i].Position, elements[selectedIndex].Position + offset)).ToList();
            return lint;
        }

        public UISelectable GetElementInDirection(Vector2 dir)
        {
            if (dir.sqrMagnitude == 0)
                return selected;
            if (selected == null)
                selected = elements[0];
            if (selected == null)
                return null;
            UISelectable current = selected;
            Vector2 curPos = new Vector2(current.transform.position.x, current.transform.position.y);
            List<UISelectable> otherElems = new List<UISelectable>();
            foreach (UISelectable g in elements)
            {
                if (!g.Equals(selected))
                    otherElems.Add(g);
            }
            float dirMag = 1;
            Vector2 checkPoint = curPos + (Vector2)dir * dirMag;
            //Sort by dist
            otherElems = otherElems.OrderBy(i => Vector2.Distance(curPos, CavPos(i.transform))).ToList();
            return otherElems[0];//closest GO from offseted point.
        }
        public static Vector2 CavPos(Transform t)
        {
            return new Vector2(t.position.x, t.position.y);
        }

        public Vector2 GetInput()
        {
            Vector2 o = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.W))
            {
                o += Vector2.up;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                o += Vector2.left;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                o += Vector2.down;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                o += Vector2.right;
            }
            if (o.sqrMagnitude > 0)
                Debug.Log(o);
            return o;
        }
        public Vector3 GetInputV3()
        {
            Vector3 o = Vector3.zero;
            if (Input.GetKeyDown(KeyCode.W))
            {
                o += Vector3.up;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                o += Vector3.left;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                o += Vector3.down;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                o += Vector3.right;
            }
            return o;
            //return new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        }
    }
}