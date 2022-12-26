using System;

using UnityEngine;

namespace Portland.Helicopter
{
	/// <summary>
	/// Using a control values component allows inputs to come from different
	/// sources such as Timeline, animations, AI, new/old input system.
	/// </summary>
	public class HeliControlValues : MonoBehaviour
	{
		//[NonSerialized]
		[Range(-1, 1)]
		public float CyclicLeftRight = 0f;

		//[NonSerialized]
		[Range(-1, 1)]
		public float CyclicForBack = 0f;

		//[NonSerialized]
		[Range(-1, 1)]
		public float PedalsLeftRight = 0f;

		//[NonSerialized]
		[Range(-1, 1)]
		public float Throttle = 0f;

		public bool EnableCamera = true;
		public bool EnableMouseInput = true;
		public bool EnableKeyInput = true;
	}
}
