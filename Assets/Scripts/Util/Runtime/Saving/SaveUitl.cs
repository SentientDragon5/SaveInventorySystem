/*
Copywrite SentientDragon5 2022
MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.


Please Attribute


This was originally posted in the SaveInventorySystem repo by SentientDragon5 2022
https://github.com/SentientDragon5/SaveInventorySystem/blob/main/Assets/Scripts/Util/Runtime/Saving/SaveUitl.cs

8/23/22 - change format of Save and Load
5/22/22 - work on generic saving
5/2/22  - initial release

 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Saving
{
    /// <summary>
    /// Put 'using static Saving.SaveUtil;' in the header to access methods.
    /// Has a variety of methods useful when saving characters to files
    /// </summary>
    public static class SaveUtil
    {
        // <T> is called Generics
        // use it to be generic when coding
        // https://www.tutorialsteacher.com/csharp/csharp-generics

        /// <summary>
        /// This will actually save the data to a file
        /// </summary>
        /// <typeparam name="T">This is the type of the object</typeparam>
        /// <param name="obj">this is the object that will be saved</param>
        /// <param name="path">This is the path of where the file will be save to (like "/character0.chr")</param>
        public static void Save<T>(T obj, string path)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + path);
            bf.Serialize(file, (T)obj);
            file.Close();
        }

        /// <summary>
        /// SAFE Use this to actually load the game
        /// </summary>
        /// <typeparam name="T">This is the type it shall be loaded as</typeparam>
        /// <param name="path">This is the path the file will be found at (like "/character0.chr")</param>
        /// <returns></returns>
        public static bool Load<T>(string path, out T obj)
        {

            if (!File.Exists(Application.persistentDataPath + path))
            {
                Debug.LogWarning("There is no save data!");
                obj = default(T);
                return false;
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + path, FileMode.Open);
            T o = (T)bf.Deserialize(file);
            file.Close();
            obj = o;
            return true;
        }

        /// <summary>
        /// Will Get if the File exists. Do note that Load is safe and will return False if no data is found.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FileExists(string path)
        {
            return File.Exists(Application.persistentDataPath + path);
        }

        /// <summary>
        /// Converts Vector v to a float array for saving.
        /// </summary>
        public static float[] VectorToArr(Vector2 v)
        {
            float[] o = new float[2];
            o[0] = v.x;
            o[1] = v.y;
            return o;
        }
        /// <summary>
        /// Converts Vector v to a float array for saving.
        /// </summary>
        public static float[] VectorToArr(Vector3 v)
        {
            float[] o = new float[3];
            o[0] = v.x;
            o[1] = v.y;
            o[2] = v.z;
            return o;
        }
        /// <summary>
        /// Converts Vector v to a float array for saving.
        /// </summary>
        public static float[] VectorToArr(Vector4 v)
        {
            float[] o = new float[4];
            o[0] = v.x;
            o[1] = v.y;
            o[2] = v.z;
            o[3] = v.w;
            return o;
        }
        /// <summary>
        /// Converts float array v to a Vector for using.
        /// </summary>
        public static Vector2 ArrToVector2(float[] v)
        {
            return new Vector2(v[0], v[1]);
        }
        /// <summary>
        /// Converts float array v to a Vector for using.
        /// </summary>
        public static Vector3 ArrToVector3(float[] v)
        {
            return new Vector3(v[0], v[1], v[2]);
        }
        /// <summary>
        /// Converts float array v to a Vector for using.
        /// </summary>
        public static Vector4 ArrToVector4(float[] v)
        {
            return new Vector4(v[0], v[1], v[2], v[3]);
        }
    }

    /*
     * I would like to use generics to make a simple save system where you input the monobehavior and it can save it.
     * unfortunatly that is a lot of work and I need to spend more time on such activity.
     * 
     */

    /*
    public class Saveable<MonoBehavior>
    {
        public virtual M GetSave<M>(MonoBehavior monoBehavior) where M : Saveable<MonoBehavior>
        {
            Saveable<MonoBehavior> saveable = this;
            return (M)saveable;
        }

        public virtual void Load(MonoBehavior monoBehavior)
        {

        }
    }
   
    public class SaveA : Saveable<SimpleChar>
    {
        public SaveA GetSave(SimpleChar s)
        {
            return base.GetSave<SimpleChar>(s);
        }
    }
    */

    /*

    May 22 22
    I have determined it is still a lot of work
    I think that you would need to create an interface or base class derived from MonoBehavior for you to tell it which thing to load
    I want to learn how to use atributes like [Range()] public float number = 0.5f; to create my own for something like
    [SaveProperty]
    that would tell the base class? or something to save it
    i think it has been done before
    but still
    Yeah
    it is hard


    /*
    /*
    public class Saveable<M>
    {
        public List<object> list;
        public M m;
        public T GetT<T>(M m) where T : Saveable<M>
        {
            this.m = m;
            return (T)this;
        }
        public void Load<T>()
        {
            
        }
    }
    public class SaveA : Saveable<SD5VisualNovel.VNReader>
    {
        public bool level;

        public SaveA(SD5VisualNovel.VNReader v)
        {
            level = v.enabled;
        }
    }
    */
}