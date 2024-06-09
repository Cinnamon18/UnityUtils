using System;
using Shapes;
using UnityEngine;

#pragma warning disable CS8632 // Warning for nullables

namespace UI {
	[RequireComponent(typeof(Collider2D))]
	public class WorldspaceButton : MonoBehaviour {
		[Tooltip("Can be null")]
		[SerializeField] private ShapeRenderer? buttonBg;
		public bool IsButtonEnabled = true;
		public event Action OnClick;
		public event Action OnClickUp;

		private Color startingButtonColor;

		void Awake() {
			if(buttonBg != null) {
				startingButtonColor = buttonBg.Color;
			}
		}

		public void SetFocused(bool isFocused) {
			if(buttonBg == null) {
				return;
			}

			buttonBg.Color = isFocused ? PaletteManager.Palette.Focused : startingButtonColor;
		}

		private void OnMouseDown() {
			if(IsButtonEnabled) {
				OnClick?.Invoke();
			}
		}

		private void OnMouseUp() {
			if(IsButtonEnabled) {
				OnClickUp?.Invoke();
			}
		}
	}
}