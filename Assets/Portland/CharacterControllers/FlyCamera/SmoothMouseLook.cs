// original source: http://forum.unity3d.com/threads/a-free-simple-smooth-mouselook.73117/
// Very simple smooth mouselook modifier for the MainCamera in Unity
// by Francis R. Griffiths-Keam - www.runningdimensions.com 
// Modified: Escape key for hide/show & lock/unlock mouse

using UnityEngine;

namespace Portland
{
	public class SmoothMouseLook : MonoBehaviour
	{
		public bool ControlPause;

		bool m_paused;
		Vector2 _mouseAbsolute;
		Vector2 _smoothMouse;

		public Vector2 clampInDegrees = new Vector2(360, 180);
		public Vector2 sensitivity = new Vector2(2, 2);
		public Vector2 smoothing = new Vector2(3, 3);
		public Vector2 targetDirection;

		void OnEnable()
		{
			// Set target direction to the camera's initial orientation.
			targetDirection = transform.localEulerAngles;//.rotation.eulerAngles;
			if (ControlPause)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}

		void LateUpdate()
		{
			if (ControlPause)
			{
				if (m_paused)
				{
					if (Input.GetMouseButtonDown(0))
					{
						Debug.Log("Unpause");
						m_paused = false;
						Cursor.lockState = CursorLockMode.Locked;
						Cursor.visible = false;
					}
					else
					{
						return;
					}
				}
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					Debug.Log("Pause");
					m_paused = true;
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
					return;
				}
			}

			// Allow the script to clamp based on a desired target value.
			Quaternion targetOrientation = Quaternion.Euler(targetDirection);

			// Get raw mouse input for a cleaner reading on more sensitive mice.
			var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

			// Scale input against the sensitivity setting and multiply that against the smoothing value.
			mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

			// Interpolate mouse movement over time to apply smoothing delta.
			_smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
			_smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

			// Find the absolute mouse movement value from point zero.
			_mouseAbsolute += _smoothMouse;

			// Clamp and apply the local x value first, so as not to be affected by world transforms.
			if (clampInDegrees.x < 360)
				_mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

			var xRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);
			transform.localRotation = xRotation;

			// Then clamp and apply the global y value.
			if (clampInDegrees.y < 360)
				_mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

			var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
			transform.localRotation *= yRotation;
			transform.rotation *= targetOrientation;
		}
	}
}