using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Portland
{
	public class DotProductEnabling : MonoBehaviour
	{
		[SerializeField]
		GameObject[] RendersToDisable;

		void Start()
		{
			if (RendersToDisable == null)
			{
				enabled = false;
			}
		}

		void Update()
		{
			if (Camera.main == null)
			{
				Debug.Log("camera null");
				return;
			}
			for (int x = 0; x < RendersToDisable.Length; x++)
			{
				var dot = Vector3.Dot(RendersToDisable[x].transform.position, Camera.main.transform.forward);
				bool activate = dot > 0;
				bool wasActive = RendersToDisable[x].activeSelf;
				if (activate != wasActive)
				{
					RendersToDisable[x].SetActive(activate);
					Debug.Log($"set active to {activate}");
				}
			}
		}
	}
}
