using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Util {
	// Lerping function. Straight from m***dy* which was straight from h*ng*****g, which was in turn straight from P*** t* H******, which was straight from Fl**red.
	public static IEnumerator Lerp(float duration, Action<float> perStep) {
		float timer = 0;
		while ((timer += Time.deltaTime) < duration) {
			perStep(timer / duration);
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
}