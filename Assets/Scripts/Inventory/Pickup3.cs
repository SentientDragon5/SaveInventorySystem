using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Saving.SaveUtil;
using System.Linq;

/*
 * Copyright SentientDragon5 2022
 * Please Attribute
 * 
 * Put this script on a world object to make it be able to be interacted with and be able to be picked up.
 */

namespace Inv3
{
    public class Pickup3 : MonoBehaviour//Interactable
    {
        public ItemAsset3 item;

        public void Interact(Transform t)
        {
            if(t.GetComponent<Inventory3>().Add(item, 1))
            {
                Destroy(gameObject);
            }
        }


        public void GetKeyNumbers(out int index)
        {
            ItemKey3.Key.GetIndexes(item, out index);
        }
        public ItemAsset3 GetItemAsset(int index)
        {
            return ItemKey3.Key.GetItem(index);
        }

        public static void Load(Transform parent, WorldSave3 save)
        {
            for (int i = 0; i < save.indexes.Length; i++)
            {
                GameObject go = Instantiate(ItemKey3.Key.GetItem(save.indexes[i]).prefab, parent);
                go.transform.parent = parent;
                go.transform.position = ArrToVector3(save.positons[i]);
            }
        }
        public static void Drop(Transform parent, ItemAsset3 item, Vector3 posWS)
        {
            if(item != null && item.index != -1)
            {
                GameObject go = Instantiate(item.prefab, parent);
                go.transform.parent = parent;
                go.transform.position = posWS;
            }
        }
    }
    [System.Serializable]
    public class WorldSave3
    {
        public int[] indexes;
        public float[][] positons;
        public WorldSave3(Pickup3[] pickups)
        {
            indexes = new int[pickups.Length];
            positons = new float[pickups.Length][];
            for (int i = 0; i < pickups.Length; i++)
            {
                pickups[i].GetKeyNumbers(out indexes[i]);
                positons[i] = VectorToArr(pickups[i].transform.position);
            }
        }
        public void Load(Transform parent)
        {
            //Destroy any pickups before loading new ones in to prevent dupes
            List<Pickup3> pickups = GetAllPickups(parent).ToList();
            while(pickups.Count >0)
            {
//#if UNITY_EDITOR
                //GameObject.DestroyImmediate(pickups[0].gameObject);
//#else
                //#endif
                GameObject g = pickups[0].gameObject;
                pickups.RemoveAt(0);
                GameObject.Destroy(g);
            }

            Pickup3.Load(parent, this);
        }
        public static WorldSave3 Save(Transform parent)
        {
            return new WorldSave3(GetAllPickups(parent));
        }

        /// <summary>
        /// Depreciated
        /// </summary>
        public static Pickup3[] GetAllPickups()
        {
            //This also gets prefabs. If you do this, you need to screen out the prefabs.
            // https://answers.unity.com/questions/218429/how-to-know-if-a-gameobject-is-a-prefab.html

            Pickup3[] pickups = Resources.FindObjectsOfTypeAll<Pickup3>();//Costly.
            Pickup3[] half = new Pickup3[pickups.Length / 2];
            for (int i = 0; i < pickups.Length / 2; i++)
            {
                half[i] = pickups[i];
            }
            Debug.Log(half.Length);
            foreach (Pickup3 p in half)
            {
                Debug.Log(p.gameObject.name);
            }
            return half;
        }
        // Make all pickups children of one object then save and load under that transform.
        public static Pickup3[] GetAllPickups(Transform p)
        {
            List<Pickup3> pickups = new List<Pickup3>();
            for (int i = 0; i < p.childCount; i++)
            {
                if(p.GetChild(i).TryGetComponent(out Pickup3 pick))
                {
                    pickups.Add(pick);
                }
            }
            return pickups.ToArray();
        }
    }
}