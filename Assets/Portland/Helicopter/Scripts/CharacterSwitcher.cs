using System.Collections;
using System.Collections.Generic;

using Portland.Helicopter;

using UnityEngine;

namespace Portland
{
	public class CharacterSwitcher : MonoBehaviour
	{
		[SerializeField]
		EnviroSky Enviro;

		[SerializeField]
		GameObject Helicoper;

		[SerializeField]
		GameObject FlyCam;

		[SerializeField]
		GameObject SimpleWalker;

		[SerializeField]
		KeyCode ViewChangeKey = KeyCode.V;

		[SerializeField]
		KeyCode HeliRemoteToggleKey = KeyCode.RightShift;

		HeliControlValues HelicopterInputs;

		public enum ControllerType
		{
			Helicopter,
			FlyCam,
			SimpleWalker,
		}

		[SerializeField]
		ControllerType Default = ControllerType.Helicopter;
		ControllerType Current;

		bool KeysForcedToHeli = false;

		void Start()
		{
			HelicopterInputs = Helicoper.GetComponent<HeliControlValues>();
			ChangeTo(Default);
		}

		void Update()
		{
			if (Input.GetKeyDown(ViewChangeKey))
			{
				int cur = ((int)Current + 1) % 3;
				ChangeTo((ControllerType)cur);
			}
			if (Input.GetKeyDown(HeliRemoteToggleKey))
			{
				KeysForcedToHeli = !KeysForcedToHeli;

				switch ((Current))
				{
					case ControllerType.Helicopter:
						SetHelicopterKeyInputOnly(true);
						KeysForcedToHeli = false;
						break;
					case ControllerType.SimpleWalker:
						SetWalkerKeyInputOnly(!KeysForcedToHeli);
						SetHelicopterKeyInputOnly(KeysForcedToHeli);
						break;
					case ControllerType.FlyCam:
						SetFlyCamKeyInputOnly(!KeysForcedToHeli);
						SetHelicopterKeyInputOnly(KeysForcedToHeli);
						break;
				}
			}
		}

		void ChangeTo(ControllerType ctl)
		{
			KeysForcedToHeli = false;

			switch(ctl)
			{
				case ControllerType.Helicopter:
					SetHelicopter(true);
					SetFlyCam(false);
					SetWalker(false);
					Current = ControllerType.Helicopter;
					Enviro.Player = Helicoper;
					Enviro.PlayerCamera = Helicoper.GetComponentInChildren<Camera>();
					break;
				case ControllerType.SimpleWalker:
					SetHelicopter(false);
					SetFlyCam(false);
					SetWalker(true);
					Current = ControllerType.SimpleWalker;
					Enviro.Player = SimpleWalker;
					Enviro.PlayerCamera = SimpleWalker.GetComponentInChildren<Camera>();
					break;
				case ControllerType.FlyCam:
					SetHelicopter(false);
					SetFlyCam(true);
					SetWalker(false);
					Current = ControllerType.FlyCam;
					Enviro.Player = FlyCam;
					Enviro.PlayerCamera = FlyCam.GetComponent<Camera>();
					break;
				default:
					Debug.Log("Character switcher error in ChangeTo");
					break;
			}
		}

		void SetHelicopter(bool doenable)
		{
			Helicoper.SetActive(doenable);
			HelicopterInputs.EnableMouseInput = doenable;
			HelicopterInputs.EnableCamera = doenable;
			SetHelicopterKeyInputOnly(doenable);
		}

		void SetHelicopterKeyInputOnly(bool doenable)
		{
			HelicopterInputs.EnableKeyInput = doenable;
		}

		void SetFlyCam(bool doenable)
		{
			FlyCam.SetActive(doenable);
			SetFlyCamKeyInputOnly(doenable);
		}

		void SetFlyCamKeyInputOnly(bool doenable)
		{
			FlyCam.GetComponent<ExtendedFlycam>().enabled = doenable;
		}

		void SetWalker(bool doenable)
		{
			SimpleWalker.SetActive(doenable);
			SetWalkerKeyInputOnly(doenable);
		}

		void SetWalkerKeyInputOnly(bool doenable)
		{
			SimpleWalker.GetComponent<FPSWalkerEnhanced>().enabled = doenable;
		}
	}
}
