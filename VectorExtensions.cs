using UnityEngine;

namespace Utilities {
	public static class VectorExtensions {
		public static Vector3 SetX(this Vector3 vec, float x) {
			return new Vector3(x, vec.y, vec.z);
		}
		public static Vector3 SetY(this Vector3 vec, float y) {
			return new Vector3(vec.x, y, vec.z);
		}
		public static Vector3 SetZ(this Vector3 vec, float z) {
			return new Vector3(vec.x, vec.y, z);
		}

		public static Vector2 SetX(this Vector2 vec, float x) {
			return new Vector2(x, vec.y);
		}
		public static Vector2 SetY(this Vector2 vec, float y) {
			return new Vector2(vec.x, y);
		}
		/// <summary>
		/// Extension method to quickly convert a vector3 to a vector 2, dropping the z component.
		/// </summary>
		public static Vector2 V2(this Vector3 vector3) {
			return new Vector2(vector3.x, vector3.y);
		}

		public static Vector3 V3(this Vector2 vector2, float zComponent = 0) {
			return new Vector3(vector2.x, vector2.y, zComponent);
		}
	}
}