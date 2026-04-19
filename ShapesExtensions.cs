using Shapes;
using UnityEngine;

namespace Utilities {
	public static class ShapesExtensions {
		/// <summary>
		/// Provide it a point in world coordinates.
		/// Does not consider: rounded caps, dashing.
		/// </summary>
		public static bool ContainsPoint(this Disc disc, Vector2 point) {
			Vector2 discPos = disc.transform.position.V2();
			Vector2 pointLocalSpace = point - discPos;
			float mag = pointLocalSpace.magnitude;

			bool isTooFar = mag > disc.Radius + (disc.Thickness / 2);
			bool isTooClose = mag < disc.Radius - (disc.Thickness / 2);

			// # MATH
			float angleBetween = Mathf.Acos(Vector2.Dot(pointLocalSpace, Vector2.right) / pointLocalSpace.magnitude);
			// This feels hacky but I dunno how to properly compensate for acos returning the smaller of the two angles
			// Could be more generally expressed as Vector2.Dot(pointLocalSpace, Vector2.up) > 0 ig
			if(pointLocalSpace.y < 0) {
				angleBetween += Mathf.PI;
			}
			bool isAngleRight = angleBetween > disc.AngRadiansStart && angleBetween < disc.AngRadiansEnd;

			switch (disc.Type) {
				case DiscType.Disc:
					return !isTooFar;
				case DiscType.Ring:
					return !isTooFar && !isTooClose;
				case DiscType.Pie:
					return !isTooFar && isAngleRight;
				case DiscType.Arc:
					return !isTooFar && !isTooClose && isAngleRight;
			}

			return true;
		}
	}
}