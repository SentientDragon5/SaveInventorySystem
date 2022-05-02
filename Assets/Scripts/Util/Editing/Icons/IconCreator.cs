/*
 * Copyright SentientDragon5 2022
 * Please Attribute
 * 
 * This script is used with the prefab in the folder above to create icons for your inventory items.
 */

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class IconCreator : MonoBehaviour
{
    public string assetPath;
    public string _name;
    private RenderTexture texture;
    private Camera cam;
    public Transform anchor;
    public Vector2 size = new Vector2(512, 512);

    public Vector3 offset = Vector3.one * 10;

    [ContextMenu("Auto Place to Anchor")]
    public void AutoPlaceCam()
    {
        Bounds bounds = anchor.GetComponent<Renderer>().bounds;
        transform.position = anchor.position + offset;
        transform.LookAt(anchor.position);
        //float orthoSize = bounds.extents.x * lookDir.x + bounds.extents.y * lookDir.y + bounds.extents.z * lookDir.z;

        cam = GetComponent<Camera>();
        cam.orthographic = true;
        //camera.orthographicSize = orthoSize;
    }
    private void OnDrawGizmosSelected()
    {
        if (anchor == null)
            return;
        Bounds bounds = anchor.GetComponent<Renderer>().bounds;
        Gizmos.DrawWireCube(bounds.center, bounds.extents * 2);
    }

    [ContextMenu("Single")]
    public void CreateSingle()
    {
        cam = GetComponent<Camera>();
        texture = new RenderTexture((int)size.x, (int)size.y, 24);

        //camera.transform.LookAt();
        cam.targetTexture = texture;
        cam.Render();
        Texture2D tex = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
        Rect rectReadPicture = new Rect(0, 0, texture.width, texture.height);
        RenderTexture.active = texture;
        tex.ReadPixels(rectReadPicture, 0, 0);
        Color32[] colors = tex.GetPixels32();
        int i = 0;
        Color32 transparent = colors[i];
        for (; i < colors.Length; i++)
        {
            if (colors[i].Equals(transparent))
            {
                colors[i] = new Color32();
            }
        }
        tex.SetPixels32(colors);
        RenderTexture.active = null;
        string cardPath = "Assets/" + assetPath + _name + "_icon" + ".png";
        byte[] bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(cardPath, bytes);
        AssetDatabase.ImportAsset(cardPath);
        TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(cardPath);
        ti.textureType = TextureImporterType.Sprite;
        ti.SaveAndReimport();
    }
}
#endif