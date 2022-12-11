using System;
using System.Collections.Generic;

using UnityEngine;

namespace Portland.Helicopter
{
	public class HeliInput : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField, Tooltip("Control inputs from user, Timeline, or AI")]
		HeliControlValues Inputs;

		[Header("Input Asset Axis Names")]
		[SerializeField]
		string CyclicForBackInputAxis = "Vertical";
		[SerializeField]
		string CyclicLeftRightInputAxis = "Horizontal";
		[SerializeField]
		string PedalsLeftRightInputAxis = "Pedals";
		[SerializeField]
		string ThrottleInputAxis = "Throttle";

		// Update is called once per frame
		void Update()
		{
			Inputs.CyclicForBack = Input.GetAxis(CyclicForBackInputAxis);
			Inputs.CyclicLeftRight = Input.GetAxis(CyclicLeftRightInputAxis);
			Inputs.PedalsLeftRight = Input.GetAxis(PedalsLeftRightInputAxis);
			Inputs.Throttle = Input.GetAxis(ThrottleInputAxis);
		}

#if UNITY_EDITOR
		void OnValidate()
		{
			if (Inputs == null)
			{
				Inputs = GetComponent<HeliControlValues>();
			}
		}
#endif
	}
}
