using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using static Saving.SaveUtil;

/*
 * Copyright SentientDragon5 2022
 * Please Attribute
 * 
 * Add this to the player to allow them to save and load themself.
 */

namespace Inv3
{
    public class Save3 : MonoBehaviour
    {
        public string characterPath = "/character0.chr";
        public string worldPath = "/world0.wld";
        public Transform pickupParent;
        private CharSave save;
        private WorldSave3 wSave;
        public UnityEvent onLoad;

        [ContextMenu("SAVE")]
        public void SAVE()
        {
            wSave = WorldSave3.Save(pickupParent);
            save = CharSave.Save(transform);

            Save(save, characterPath);
            Save(wSave, worldPath);
        }
        [ContextMenu("LOAD")]
        public void LOAD()
        {
            wSave = Load<WorldSave3>(worldPath);
            save = Load<CharSave>(characterPath);

            wSave.Load(pickupParent);
            save.Load(transform);
            onLoad.Invoke();
        }

        public bool AutoLoadSave = false;
        public static Save3 instance;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                return;
            if (AutoLoadSave)
                LOAD();
        }
        private void OnApplicationQuit()
        {
            if (AutoLoadSave)
                SAVE();
        }
        
    }

    /// <summary>
    /// This holds a compact and saveable class that can be saved. create it from the player then load it on to the player
    /// </summary>
    [System.Serializable]
    public class CharSave
    {
        public float[] position;
        public int[][] inventory;

        private CharSave(Vector3 pos, int[][] inv)
        {
            position = VectorToArr(pos);
            inventory = inv;
        }
        private void UnpackData(out Vector3 pos, out int[][] inv)
        {
            pos = ArrToVector3(position);
            inv = inventory;
        }

        /// <summary>
        /// Transform of the character you wish to set
        /// </summary>
        public void Load(Transform transform)
        {
            UnpackData(out Vector3 pos, out int[][] inv);
            transform.position = pos; // Note: if you are using a CharacterController component, I found that it would jump back.
            transform.GetComponent<Inventory3>().Load(inv);
        }
        /// <summary>
        /// Transform of the character you wish to save
        /// </summary>
        public static CharSave Save(Transform transform)
        {
            transform.GetComponent<Inventory3>().Save(out int[][] inv);
            return new CharSave(transform.position, inv);
        }
        /// <summary>
        /// the same as Save
        /// </summary>
        public CharSave(Transform transform)
        {
            Save(transform);
        }
    }
}
/*
 * Potential future 
 * Have a Monobehavior and a corresponding class that is part of an interface or inherits Save<T>() and Load<T>() where T is the MonoBehavior.
 * 
 */