using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities {

	public class UtilVFx {
		/// <summary>
		/// Fades a TMP_Text's alpha from one value to another over the given duration.
		/// </summary>
		public static IEnumerator FadeText(TMP_Text text, float from, float to, float duration) {
			yield return Util.Lerp(duration, (float t) => {
				text.alpha = Mathf.Lerp(from, to, t);
			});
		}

		/// <summary>
		/// Fades an Image's alpha from one value to another over the given duration.
		/// </summary>
		public static IEnumerator FadeImage(Image image, float from, float to, float duration) {
			yield return Util.Lerp(duration, (float t) => {
				Util.SetAlpha(image, Mathf.Lerp(from, to, t));
			});
		}
	}
}