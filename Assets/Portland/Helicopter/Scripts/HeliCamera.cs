
using UnityEngine;

namespace Portland.Helicopter
{
	public class HeliCamera : MonoBehaviour
	{
		public Transform left;
		public Transform back;
		public Transform right;
		public Transform seat;
		public Transform infront;

		[SerializeField]
		HeliControlValues Inputs;

		[SerializeField]
		Camera TheCamera;
		[SerializeField]
		AudioListener TheListener;

		[SerializeField]
		SmoothMouseLook PilotViewController;

		[SerializeField]
		AudioSource InterorAudio;

		private int i = 0;

		void OnEnable()
		{
			//PilotViewController.enabled = false;
			TheCamera.enabled = Inputs.EnableCamera;
			TheListener.enabled = Inputs.EnableCamera;
			PilotViewController.enabled = i == 1 && Inputs.EnableMouseInput;
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				++i;
				if (i == 5)
				{
					i = 0;
				}
			}
			else if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				i = 0;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				i = 1;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				i = 2;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				i = 3;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				i = 4;
			}

			if (i == 0)
			{
				following = true;
				target = back;
			}
			else if (i == 1)
			{
				following = false;
				transform.position = seat.position;
				transform.rotation = seat.rotation;
			}
			else if (i == 2)
			{
				following = false;
				transform.position = infront.position;
				transform.rotation = infront.rotation;
			}
			else if (i == 3)
			{
				following = true;
				target = left;
			}
			else if (i == 4)
			{
				following = true;
				target = right;
			}

			InterorAudio.volume = i == 1 ? 1f : 0.25f;

			TheCamera.enabled = Inputs.EnableCamera;
			TheListener.enabled = Inputs.EnableCamera;
			PilotViewController.enabled = (i == 1 || i == 2) && Inputs.EnableMouseInput;
		}

		bool following = false;

		// The target we are following
		[SerializeField]
		public Transform target;
		// The distance in the x-z plane to the target
		[SerializeField]
		private float distance = 20.0f;
		// the height we want the camera to be above the target
		[SerializeField]
		private float height = 10.0f;

		[SerializeField]
		private float rotationDamping = 20f;
		[SerializeField]
		private float heightDamping = 20f;

		void LateUpdate()
		{
			// Early out if we don't have a target
			if (!following || !target)
			{
				return;
			}

			// Calculate the current rotation angles
			var wantedRotationAngle = target.eulerAngles.y;
			var wantedHeight = target.position.y + height;

			var currentRotationAngle = transform.eulerAngles.y;
			var currentHeight = transform.position.y;

			// Damp the rotation around the y-axis
			currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

			// Damp the height
			currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

			// Convert the angle into a rotation
			Quaternion currentRotation;
			currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

			// Set the position of the camera on the x-z plane to:
			// distance meters behind the target
			transform.position = target.position;
			transform.position -= currentRotation * Vector3.forward * distance;

			// Set the height of the camera
			transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

			// Always look at the target
			transform.LookAt(target);
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (PilotViewController == null)
			{
				PilotViewController = GetComponent<SmoothMouseLook>();
				TheCamera = GetComponent<Camera>();
				TheListener = GetComponent<AudioListener>();
				Inputs = GetComponentInParent<HeliControlValues>();
			}
		}
#endif
	}
}
