using System;
using UnityEngine;
namespace Utilities {
	public class SetSeed : MonoBehaviour {

		public int seed = 0;
		[Tooltip("Randomizes seed (overwrites manually inputed value above)")]
		public bool randomizeSeed = false;

		void Start() {
			UnityEngine.Random.InitState(randomizeSeed ? (int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) : seed);
		}
	}
}