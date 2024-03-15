using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

[CreateAssetMenu(fileName = "InputReader", menuName ="UTE/InputReader")]
public class InputReader : ScriptableObject, GameInput.IGameControlsActions, GameInput.IInspectionControlsActions, GameInput.IMenuControlsActions, GameInput.IMainMenuControlsActions
{
	//Gameplay delegates
	public static event UnityAction<Vector2> MoveEvent = delegate { };
	public static event UnityAction<Vector2> LookEvent = delegate { };
	public static event UnityAction<float> ZoomEvent = delegate { };
	public static event UnityAction InteractEvent = delegate { };
	public static event UnityAction InspectEvent = delegate { };
	public static event UnityAction OpenMenuEvent = delegate { };
	public static event UnityAction OpenJournal = delegate { };
	public static event UnityAction PointAndClick = delegate { };
	public static event UnityAction SaveGame = delegate { };
	public static event UnityAction LoadGame = delegate { };
	public static event UnityAction Sprint = delegate { };
	public static event UnityAction StopSprint = delegate { };

	//Inspecrion delegates
	public static event UnityAction<bool> ClickEvent = delegate { };
	public static event UnityAction<Vector2> RotateEvent = delegate { };
	public static event UnityAction<float> ZoomEvidenceEvent = delegate { };
	public static event UnityAction AcceptEvent = delegate { };

	//Menu delegates
	public static event UnityAction MouseClickEvent = delegate { };
	public static event UnityAction ExitEvent = delegate { };

	//Main menu delegates
	public static event UnityAction CloseMenuEvent = delegate { };

	private static GameInput _gameInput;

	private void OnEnable()
	{
		if (_gameInput == null)
			_gameInput = new GameInput();

		_gameInput.GameControls.SetCallbacks(this);
		_gameInput.InspectionControls.SetCallbacks(this);
		_gameInput.MenuControls.SetCallbacks(this);
		_gameInput.MainMenuControls.SetCallbacks(this);
		Debug.Log("AWAKE");
	}

	#region Enable/Disable Controls

	public static void SwitchToInspectionControls()
	{
		DisableAllInput();
		_gameInput.InspectionControls.Enable();
	}

	public static void SwitchToMainMenuControls()
	{
		DisableAllInput();
		_gameInput.MainMenuControls.Enable();
	}

	public static void SwitchToGameControls()
	{
		DisableAllInput();
		_gameInput.GameControls.Enable();
		Debug.Log("Enable Game Controls");
	}

	public static void SwitchToJournalControls()
	{
		DisableAllInput();
		_gameInput.MenuControls.Enable();
		Debug.Log("Switch to Journal Controls");
	}

	public static void DisableAllInput()
	{
		_gameInput.GameControls.Disable();
		_gameInput.InspectionControls.Disable();
		_gameInput.MenuControls.Disable();
		_gameInput.MainMenuControls.Disable();
		// TODO: add other interfaces callbacks
	}
	#endregion

	#region Game Controls
	public void OnSave(InputAction.CallbackContext context)
	{
		if (context.performed && SaveGame != null)
			SaveGame.Invoke();
	}

	public void OnLoad(InputAction.CallbackContext context)
	{
		if (context.performed && LoadGame != null)
			LoadGame.Invoke();
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		MoveEvent.Invoke(context.ReadValue<Vector2>());
	}

	public void OnLook(InputAction.CallbackContext context)
	{
		LookEvent.Invoke(context.ReadValue<Vector2>());
	}

	public void OnZoom(InputAction.CallbackContext context)
	{
		ZoomEvent.Invoke(context.ReadValue<float>());
	}

	public void OnInteract(InputAction.CallbackContext context)
	{
		if (context.performed && InteractEvent != null)
		{
			InteractEvent.Invoke();
		}
	}

	public void OnInspect(InputAction.CallbackContext context)
	{
		if (context.performed && InspectEvent != null)
		{
			InspectEvent.Invoke();
		}
	}

	public void OnJournal(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			OpenJournal?.Invoke();
		}
	}

	public void OnPointAndClick(InputAction.CallbackContext context)
	{
		if (context.performed)
			PointAndClick?.Invoke();
	}

	public void OnMenu(InputAction.CallbackContext context)
	{
		if (context.performed)
			OpenMenuEvent?.Invoke();
	}

	public void OnSprint(InputAction.CallbackContext context)
	{
		if (context.performed)
			Sprint?.Invoke();
		if (context.canceled)
			StopSprint?.Invoke();
	}

	#endregion

	#region Inspection Controls
	public void OnRotate(InputAction.CallbackContext context)
	{
		RotateEvent.Invoke(context.ReadValue<Vector2>());
	}

	public void OnClick(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			ClickEvent.Invoke(true);
		}
		else if (context.canceled)
		{
			ClickEvent.Invoke(false);
		}
	}

	public void OnZoomEvidence(InputAction.CallbackContext context)
	{
		ZoomEvidenceEvent.Invoke(context.ReadValue<float>());
	}

	public void OnTakeEvidence(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			AcceptEvent.Invoke();
		}
	}
	#endregion

	#region Menu Controls

	public void OnExit(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			ExitEvent?.Invoke();
		}
	}

	public void OnMouseClick(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			MouseClickEvent.Invoke();
		}
	}

	#endregion

	#region Main Menu Controls

	public void OnCloseMainMenu(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			CloseMenuEvent.Invoke();
			Debug.Log("Esc");
		}
	}

	#endregion

	#region Button events

	public void TakeEvidenceInvoke()
	{
		AcceptEvent.Invoke();
		AcceptEvent = null;
	}

	public void OpenJournalInvoke()
	{
		OpenJournal.Invoke();
	}

	#endregion
}