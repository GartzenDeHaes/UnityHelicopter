using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portland
{
	public class LayeredObjectCullingLight : MonoBehaviour
	{
		private LayeredObjectCulling CullingSystem { get; set; }

		private void OnEnable()
		{
			CullingSystem = FindObjectOfType<LayeredObjectCulling>();
			if (CullingSystem == null)
			{
				Debug.LogWarning("No LayeredObjectCulling system found in OnEnable!");
				return;
			}
			CullingSystem.RegisterLight(GetComponent<Light>());
		}

		private void OnDisable()
		{
			if (CullingSystem == null)
			{
				//Debug.LogWarning("No LayeredObjectCulling system found in OnDisable!");
				return;
			}
			CullingSystem.UnRegisterLight(GetComponent<Light>());
		}
	}
}
