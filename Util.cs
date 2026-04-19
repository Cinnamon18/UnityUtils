using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Utilities {

	public class Util {
		public static System.Random rng = new System.Random();

		/// <summary>
		/// Lerping function. Calls the given action with values between 0 and 1.
		/// 
		/// Straight from al*c* sq**red, which was straight from m*l*dyst*r, which was straight from h*ng*****g, which was straight from from P*** t* H******, which was straight from Fl**red.
		/// </summary>
		/// <param name="durationSec">How long should the lerp last in seconds?</param>
		/// <param name="perStep">Action callback that will be invoked with args from [0, 1].</param>
		/// <returns></returns>
		public static IEnumerator Lerp(float durationSec, Action<float> perStep) {
			float timer = 0;
			while ((timer += Time.deltaTime) < durationSec) {
				perStep(timer / durationSec);
				yield return null;
			}
			perStep(1);
		}

		public static IEnumerator LerpRealtime(float durationSec, Action<float> perStep) {
			float timer = 0;
			while ((timer += Time.unscaledDeltaTime) < durationSec) {
				perStep(timer / durationSec);
				yield return null;
			}
			perStep(1);
		}

		/// <summary>
		/// Coroutine to fade an image in via alpha channel over a given time in seconds
		/// </summary>
		/// <param name="image">Image to fade in via alpha channel</param>
		/// <param name="time">Time for the fade to last in seconds</param>
		public static IEnumerator fadeIn(Image image, float time) {
			yield return Util.Lerp(time, (float progress) => {
				image.color = new Color(image.color.r, image.color.g, image.color.b, progress);
			});
		}

		public static IEnumerator fadeOut(Image image, float time) {
			yield return Util.Lerp(time, (float progress) => {
				image.color = new Color(image.color.r, image.color.g, image.color.b, 1 - progress);
			});
		}

		public static Vector3 LerpAction(Vector3 start, Vector3 end, float progress, Func<float, float> lerpFunction) {
			return Vector3.Lerp(start, end, lerpFunction(progress));
		}

		public static IEnumerator DoDelayed(int delayTimeMs, Action action) {
			yield return new WaitForSeconds(delayTimeMs / 1000f);
			action();
		}

		public static IEnumerator ParallelizeCoroutines(params Coroutine[] coroutines) {
			foreach(var coroutine in coroutines) {
				yield return coroutine;
			}
		}

		/// <summary>
		/// Revert a "don't destroy on load"
		/// </summary>
		public static void DoDestroyOnLoad(GameObject target) {
			SceneManager.MoveGameObjectToScene(target, SceneManager.GetActiveScene());
		}

		/// <summary>
		/// Returns the closest game object to the given mb from the provided list of other game objects
		/// </summary>
		public static MonoBehaviour FindClosestGOTo(MonoBehaviour mb, IEnumerable<MonoBehaviour> otherGOs) {
			MonoBehaviour closestGO = null;
			float closestSquareDist = float.MaxValue;
			foreach(MonoBehaviour otherGO in otherGOs) {
				if(Vector3.SqrMagnitude(otherGO.transform.position - mb.transform.position) < closestSquareDist) {
					closestSquareDist = Vector3.SqrMagnitude(otherGO.transform.position - mb.transform.position);
					closestGO = otherGO;
				}
			}
			return closestGO;
		}
	}

	public static class UtilExtensions {
		public static void LookAtLerp(this Transform transform, Vector2 targetPos, float turnRate) {
			transform.up = Vector3.Lerp(transform.up, targetPos - transform.position.V2(), turnRate);
		}

		public static void LookAtLerp(this GameObject gameObject, Vector2 targetPos, float turnRate) {
			gameObject.transform.LookAtLerp(targetPos, turnRate);
		}

		public static void LookAt2D(this Transform transform, Vector2 target) {
			transform.up = target - transform.position.V2();
		}

		public static void LookInDirection2D(this Transform transform, Vector2 direction) {
			transform.LookAt2D(transform.position.V2() + direction);
		}

		public static T GetRandomElement<T>(this IList<T> list) {
			if(list.Count == 0) {
				return default(T);
			}
			return list[Util.rng.Next(list.Count)];
		}

		// https://stackoverflow.com/a/2134210
		public static string KiloMegaFormat(this int num) {
			if(num == 0)
				return "0";
			if (num >= 100000000)
				return (num / 1000000).ToString("#,0M");
			if (num >= 10000000)
				return (num / 1000000).ToString("0.#") + "M";
			if (num >= 100000)
				return (num / 1000).ToString("#,0K");
			if (num >= 10000)
				return (num / 1000).ToString("0.#") + "K";
			return num.ToString("#,0");
		}

		public static string KiloMegaFormat(this float num) {
			if(num == 0) {
				return "0";
			} else if(num <= 1000) {
				return num.ToString("#.0#");
			} else {
				return KiloMegaFormat((int)num);
			}
		}

		public static string KiloMegaFormatOneDecimal(this float num) {
			if(num == 0) {
				return "0";
			} else if(num <= 1000) {
				return num.ToString("#.0");
			} else {
				return KiloMegaFormat(num);
			}
		}

		public static float Remap(this float input, float oldLow, float oldHigh, float newLow, float newHigh) {
			float t = Mathf.InverseLerp(oldLow, oldHigh, input);
			return Mathf.Lerp(newLow, newHigh, t);
		}
	}


	// Use like this: 
	// public NamedPrefabStruct<GameObject>[] namedPrefabs;
	// private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
	// void Awake() {
	// 	prefabs = NamedPrefab.dictFromNamedPrefabs(namedPrefabs);
	// }

	// TODO HAHA FIGURE OUT A WAY TO MAKE UNITY PLAY NICE W GENERICS

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
