using System.Collections.Generic;
using Cinemachine;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using Zenject;

public class CameraStateManager : MonoBehaviour
{
	[SerializeField] private List<CinemachineVirtualCamera> _vCameras = new List<CinemachineVirtualCamera>();

	[Inject] private StaticCameraMovement _staticCameraMovement;
	[Inject] private StaticDialogueCameraMovement _staticDialogueCameraMovement;

	private CinemachineVirtualCamera _mainVirtualCamera;
	private CinemachineVirtualCamera _dialogueVirtualCamera;

	private void Awake()
	{
		_mainVirtualCamera = _staticCameraMovement.GetComponent<CinemachineVirtualCamera>();
		_dialogueVirtualCamera = _staticDialogueCameraMovement.GetComponent<CinemachineVirtualCamera>();

		_vCameras.Add(_mainVirtualCamera);
		_vCameras.Add(_dialogueVirtualCamera);

		ActivateMainCamera();
	}

	public void ActivateMainCamera()
	{
		SwitchCamera(_mainVirtualCamera);
	}

	public void ActivateDialogueCamera()
	{
		SwitchCamera(_dialogueVirtualCamera);
	}

	public void ActivateSpecificCamera(string name)
	{
		ResetAllPriorities();

		var vcam = GameObject.Find(name).GetComponent<CinemachineVirtualCamera>();
		vcam.Priority = 10;
	}

	private void SwitchCamera(CinemachineVirtualCamera vcam)
	{
		ResetAllPriorities();
		vcam.Priority = 10;
	}

	public void ResetAllPriorities()
	{
		foreach (CinemachineVirtualCamera vcam in _vCameras)
			vcam.Priority = 0;
	}

	public void RegisterLua()
	{
		Lua.RegisterFunction("ActivateDialogueCamera", this, SymbolExtensions.GetMethodInfo(() => ActivateDialogueCamera()));
		Lua.RegisterFunction("ActivateMainCamera", this, SymbolExtensions.GetMethodInfo(() => ActivateMainCamera()));
		Lua.RegisterFunction("ActivateSpecificCamera", this, SymbolExtensions.GetMethodInfo(() => ActivateSpecificCamera("")));
	}

	public void UnregisterLua()
	{
		Lua.UnregisterFunction("ActivateDialogueCamera");
		Lua.UnregisterFunction("ActivateMainCamera");
		Lua.UnregisterFunction("ActivateSpecificCamera");
	}
}