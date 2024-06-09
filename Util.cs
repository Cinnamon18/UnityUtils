using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Utilities {
	public class Util {
		// Lerping function. Straight from (al*ce)^* Straight from m*l*dyst*r, which was straight from h*ng*****g, which was straight from from P*** t* H******, which was straight from Fl**red.
		public static IEnumerator Lerp(float duration, Action<float> perStep) {
			float timer = 0;
			while ((timer += Time.deltaTime) < duration) {
				perStep(timer / duration);
				yield return null;
			}
			perStep(1);
		}

		/// <summary>
		/// Calls <code>perStep</code> with the arguments [0-1] by the provided AnimationCurve.
		/// </summary>
		public static IEnumerator CurveInterp(AnimationCurve animCurve, float duration, Action<float> perStep) {
			float timer = 0;
			while ((timer += Time.deltaTime) < duration) {
				perStep(animCurve.Evaluate(timer / duration));
				yield return null;
			}
			perStep(1);
		}

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

		public static void DoDestroyOnLoad(GameObject target) {
			SceneManager.MoveGameObjectToScene(target, SceneManager.GetActiveScene());
		}

		//Puts the passed gameObject on a timer before it's destroyed.
		public static IEnumerator DestructionCountdown(GameObject gO, int timer) {
			for (; timer > 0; timer -= 1) {
				yield return null;
			}
			GameObject.Destroy(gO);
		}
	}
}