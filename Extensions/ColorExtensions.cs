
using UnityEngine;

namespace Utilities {
	public static class ColorExtensions {
		private const float tintShadePercentage = 0.2f;
		public static Color Tint(this Color color) {
			Color.RGBToHSV(color, out float h, out float s, out float v);
			return Color.HSVToRGB(h, s, v + tintShadePercentage);
		}

		public static Color Shade(this Color color) {
			Color.RGBToHSV(color, out float h, out float s, out float v);
			return Color.HSVToRGB(h, s, v - tintShadePercentage);
		}

		public static Color SetA(this Color color, float alpha) {
			return new Color(color.r, color.g, color.b, alpha);
		}
	}
}