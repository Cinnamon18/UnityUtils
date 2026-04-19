// Use like this: 
// public NamedPrefabStruct<GameObject>[] namedPrefabs;
// private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
// void Awake() {
// 	prefabs = NamedPrefab.dictFromNamedPrefabs(namedPrefabs);
// }

// TODO HAHA FIGURE OUT A WAY TO MAKE UNITY PLAY NICE W GENERICS

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities {
	public class NamedPrefab {
		[Serializable]
		public struct NamedPrefabStruct {
			public string name;
			public GameObject prefab;
		}

		[Serializable]
		public struct NamedSpriteStruct {
			public string name;
			public Sprite prefab;
		}

		public static Dictionary<string, GameObject> dictFromNamedPrefabs(IEnumerable<NamedPrefabStruct> namedPrefabs) {
			Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
			foreach (NamedPrefabStruct prefabEditor in namedPrefabs) {
				prefabs.Add(prefabEditor.name, prefabEditor.prefab);
			}
			return prefabs;
		}

		public static Dictionary<string, Sprite> dictFromNamedSprites(IEnumerable<NamedSpriteStruct> namedPrefabs) {
			Dictionary<string, Sprite> images = new Dictionary<string, Sprite>();
			foreach (NamedSpriteStruct prefabEditor in namedPrefabs) {
				images.Add(prefabEditor.name, prefabEditor.prefab);
			}
			return images;
		}
	}
}