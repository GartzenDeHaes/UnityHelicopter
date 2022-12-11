using UnityEngine;

namespace Portland.Helicopter
{
	public class HeliController : MonoBehaviour
	{
		[Space]
		public float max_Rotor_Force = 22241.1081f;
		public float max_Rotor_Velocity = 7200f;
		public float StablisingConstant = 2f;
		
		float rotor_Velocity = 0.0f;
		//private float rotor_Rotation = 0.0f;
		public float max_tail_Rotor_Force = 15000.0f;

		[SerializeField, Tooltip("1=X, 2=Y, 3=Z")]
		int mainrotor_axis = 1;
		[SerializeField, Tooltip("1=X, 2=Y, 3=Z")]
		int tailrotor_axis = 1;
		//float max_Tail_Rotor_Velocity = 2200.0f;
		private float tail_rotor_Velocity = 0.0f;
		//private float tail_rotor_Rotation = 0.0f;
		[Tooltip("Force applied on cyclic forwards/backwards")]
		public float forward_Rotor_Torque_Multiplier = 1.5f;
		[Tooltip("Force applied on cyclic left/right")]
		public float sideways_Rotor_Torque_Multiplier = 2f;
		[Tooltip("Setting this value to 2 will default the throttle high enough to hover"), SerializeField]
		float Hover_Const = 0f;
		[SerializeField]
		float RotorSpeedIncreaseConstant = 2;
		[SerializeField]
		float RestoringTorqueMultiplier = 1;
		[SerializeField]
		float maxHeight = 10000f;
		//public GUIStyle gui;
		//public Rect labelPosition;
		[SerializeField]
		LayerMask GroundLayer = 0;
		//private float timer = 0f;
		//private bool cannotRestore = false;
		[HideInInspector]
		public float altitude;

		[Header("Components")]
		[SerializeField, Tooltip("Control inputs from user, Timeline, or AI")]
		HeliControlValues Inputs;
		[SerializeField]
		GameObject main_Rotor_GameObject;
		[SerializeField]
		GameObject tail_Rotor_GameObject;
		[SerializeField]
		GameObject CenterOfMass;
		[SerializeField]
		HeliHealth HeliEffects;
		[SerializeField]
		AudioSource MainAudio;
		[SerializeField]
		Rigidbody HeliRBody;

		[Header("Effects")]
		public GameObject Wind;
		public GameObject dust;
		public GameObject leave;

		bool main_Rotor_Active = true;
		bool tail_Rotor_Active = true;

		void Awake()
		{
		}

		void FixedUpdate()
		{
			if (HeliEffects.state == true)
			{
			}

			Vector3 torqueValue = Vector3.zero;
			Vector3 controlTorque;

			var cyclicForBk = Inputs.CyclicForBack;
			var cyclicLeftRight = Inputs.CyclicLeftRight;

			controlTorque = new Vector3(-cyclicForBk * forward_Rotor_Torque_Multiplier, 1, -cyclicLeftRight * sideways_Rotor_Torque_Multiplier);

			if (main_Rotor_Active == true)
			{
				torqueValue += (controlTorque * max_Rotor_Force * rotor_Velocity);

				if (altitude < maxHeight)
				{
					HeliRBody.AddRelativeForce(Vector3.up * max_Rotor_Force * rotor_Velocity);
				}
			}
			if (HeliRBody.velocity.y < 0 && rotor_Velocity < 0.3)
			{
				HeliRBody.AddForce(Vector3.down * 30000);
			}
			if (Vector3.Angle(Vector3.up, transform.up) < 80)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 360), Time.deltaTime * rotor_Velocity * StablisingConstant);
			}

			if (tail_Rotor_Active == true)
			{
				torqueValue -= (Vector3.up * max_tail_Rotor_Force * tail_rotor_Velocity);
			}
			//Fix v4.0 Helicopter not restoring z position when bent forward
			if (Inputs.CyclicForBack != 0)
			{
				if (Inputs.CyclicLeftRight == 0)
				{
					if (transform.localRotation.eulerAngles.z > 5 && transform.localRotation.eulerAngles.z < 80)
					{
						HeliRBody.AddRelativeTorque(0, 0, -rotor_Velocity * 1000 * transform.localRotation.eulerAngles.z * RestoringTorqueMultiplier);
					}
					if (transform.localRotation.eulerAngles.z > 190 && transform.localRotation.eulerAngles.z < 355)
					{
						HeliRBody.AddRelativeTorque(0, 0, rotor_Velocity * 1000 * (360 - transform.localRotation.eulerAngles.z) * RestoringTorqueMultiplier);
					}
				}
			}

			HeliRBody.AddRelativeTorque(torqueValue);
		}

		void dead()
		{
			Destroy(Wind);
			Destroy(GetComponent<HeliController>());
		}

		void Update()
		{
			if (HeliEffects.state == true)
			{
				dead();
			}
			if (main_Rotor_Active == true)
			{
				switch (mainrotor_axis)
				{
					case 1:
						main_Rotor_GameObject.transform.Rotate(rotor_Velocity * 40, 0, 0);
						break;
					case 2:
						main_Rotor_GameObject.transform.Rotate(0, rotor_Velocity * 40, 0);
						break;
					case 3:
						main_Rotor_GameObject.transform.Rotate(0, 0, rotor_Velocity * 40);
						break;
				}
			}
			if (tail_Rotor_Active == true)
			{
				switch (tailrotor_axis)
				{
					case 1:
						tail_Rotor_GameObject.transform.Rotate(tail_rotor_Velocity * 20, 0, 0);
						break;
					case 2:
						tail_Rotor_GameObject.transform.Rotate(0, tail_rotor_Velocity * 20, 0);
						break;
					case 3:
						tail_Rotor_GameObject.transform.Rotate(0, 0, tail_rotor_Velocity * 20);
						break;

				}
			}

			var hover_Rotor_Velocity = (HeliRBody.mass * Mathf.Abs(Physics.gravity.y) / max_Rotor_Force);
			var hover_Tail_Rotor_Velocity = (max_Rotor_Force * rotor_Velocity) / max_tail_Rotor_Force;

			if (Inputs.Throttle != 0.0)
			{
				rotor_Velocity += Inputs.Throttle * RotorSpeedIncreaseConstant * 0.001f;
			}
			else
			{
				rotor_Velocity = Mathf.Lerp(rotor_Velocity, hover_Rotor_Velocity, Time.deltaTime * Time.deltaTime * 5 * Hover_Const);
			}

			tail_rotor_Velocity = hover_Tail_Rotor_Velocity - Inputs.PedalsLeftRight;

			if (rotor_Velocity > 1.0)
			{
				rotor_Velocity = 1.0f;
			}
			else if (rotor_Velocity < 0.0)
			{
				rotor_Velocity = 0.0f;
			}

			MainAudio.pitch = rotor_Velocity;

			RaycastHit groundHit;
			Physics.Raycast(CenterOfMass.transform.position, -Vector3.up, out groundHit, 10000f, GroundLayer);
			altitude = groundHit.distance;

			extrafx();
		}

		void extrafx()
		{
			if (rotor_Velocity > 0.4)
			{
				Wind.SetActive(true);

			}
			else
			{
				Wind.SetActive(false);
			}


			if (HeliEffects.state == false && altitude < 15)
			{
				//dust.particleEmitter.emit = true;
				//dust.particleEmitter.minEnergy = 2.2;
				//dust.particleEmitter.maxEnergy = (rotor_Velocity / 1) * 6;
				//leave.particleEmitter.emit = true;
				//leave.particleEmitter.minEnergy = 2.2;
				//leave.particleEmitter.maxEnergy = (rotor_Velocity / 1) * 6;
			}
			else
			{
				//dust.particleEmitter.emit = false;
				//leave.particleEmitter.emit = false;
			}
		}

		//void OnGUI()
		//{
		//	GUI.Label(labelPosition, "\nVel up - " + rigidbody.velocity.y + "m/s"/*, gui*/);
		//}

#if UNITY_EDITOR
		void OnValidate()
		{
			if (HeliEffects == null)
			{
				HeliEffects = gameObject.GetComponent<HeliHealth>();
			}
			if (Inputs == null)
			{
				Inputs = GetComponent<HeliControlValues>();
			}
			if (HeliRBody == null)
			{
				HeliRBody = GetComponent<Rigidbody>();
			}
		}
#endif
	}
}
