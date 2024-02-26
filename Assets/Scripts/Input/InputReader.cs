using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameControlsActions, GameInput.IInspectionControlsActions, GameInput.IMenuControlsActions, GameInput.IMainMenuControlsActions
{
    //Gameplay delegates
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction<Vector2> LookEvent = delegate { };
    public event UnityAction<float> ZoomEvent = delegate { };
    public event UnityAction InteractEvent = delegate { };
    public event UnityAction InspectEvent = delegate { };
    public event UnityAction OpenMenuEvent = delegate { };
    public event UnityAction OpenJournal = delegate { };
    public event UnityAction PointAndClick = delegate { };
    public event UnityAction SaveGame = delegate { };
    public event UnityAction LoadGame = delegate { };
    public event UnityAction Sprint = delegate { };
    public event UnityAction StopSprint = delegate { };

    //Inspecrion delegates
    public event UnityAction<bool> ClickEvent = delegate { };
    public event UnityAction<Vector2> RotateEvent = delegate { };
    public event UnityAction<float> ZoomEvidenceEvent = delegate { };
    public event UnityAction TakeEvidenceEvent = delegate { };

    //Menu delegates
    public event UnityAction MouseClickEvent = delegate { };
    public event UnityAction ExitEvent = delegate { };

    //Main menu delegates
    public event UnityAction CloseMenuEvent = delegate { };

    private GameInput _gameInput;

    void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
            _gameInput.GameControls.SetCallbacks(this);
            _gameInput.InspectionControls.SetCallbacks(this);
            _gameInput.MenuControls.SetCallbacks(this);
            _gameInput.MainMenuControls.SetCallbacks(this);
            // TODO: add other interfaces callbacks
        }

        // TODO: add function
        _gameInput.GameControls.Enable();
    }

    #region Enable/Disable Controls
    private void OnDisable()
    {
        DisableAllInput();
    }

    public void SwitchToInspectionControls()
    {
        DisableAllInput();
        _gameInput.InspectionControls.Enable();
    }

    public void SwitchToMainMenuControls()
    {
        DisableAllInput();
        _gameInput.MainMenuControls.Enable();
    }

    public void SwitchToGameControls()
    {
        DisableAllInput();
        _gameInput.GameControls.Enable();
    }

    public void SwitchToJournalControls()
    {
        DisableAllInput();
        _gameInput.MenuControls.Enable();
    }

    public void DisableAllInput()
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
        if (context.performed && OpenJournal != null)
        {
            OpenJournal.Invoke();
        }
    }

    public void OnPointAndClick(InputAction.CallbackContext context)
    {
        if (context.performed && PointAndClick != null)
            PointAndClick.Invoke();
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
            OpenMenuEvent.Invoke();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
            Sprint.Invoke();
        if(context.canceled)
            StopSprint.Invoke();
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
            TakeEvidenceEvent.Invoke();
        }
    }
    #endregion

    #region Menu Controls

    public void OnExit(InputAction.CallbackContext context)
    {
        if (context.performed && ExitEvent != null)
        {
            ExitEvent.Invoke();
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
        TakeEvidenceEvent.Invoke();
        TakeEvidenceEvent = null;
    }

    public void OpenJournalInvoke()
    {
        OpenJournal.Invoke();
    }

    #endregion
}