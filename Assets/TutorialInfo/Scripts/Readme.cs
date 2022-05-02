using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial", menuName = "Tutorial", order = 12)]
public class Readme : ScriptableObject {
	public Texture2D icon;
	public string title;
	public Section[] sections;
	public bool loadedLayout;
	
	[Serializable]
	public class Section {
		public string heading, text, linkText, url;
	}
}
