
using UnityEngine;

namespace Portland.Helicopter
{
	public class HeliCamera : MonoBehaviour
	{
		public Transform left;
		public Transform back;
		public Transform right;
		public Transform seat;

		private int i = 0;

		void Update()
		{
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
				following = true;
				target = left;
			}
			else if (i == 3)
			{
				following = true;
				target = right;
			}
			if (Input.GetKeyDown(KeyCode.F))
			{
				++i;
				if (i == 4)
				{
					i = 0;
				}
			}
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
			var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

			// Set the position of the camera on the x-z plane to:
			// distance meters behind the target
			transform.position = target.position;
			transform.position -= currentRotation * Vector3.forward * distance;

			// Set the height of the camera
			transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

			// Always look at the target
			transform.LookAt(target);
		}
	}
}
