using System.Collections.Generic;
using Cinemachine;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class CameraStateManager : MonoBehaviour
{
    [HideInInspector] public static CameraStateManager Ønstance;

    [SerializeField] private List<CinemachineVirtualCamera> _vCameras = new List<CinemachineVirtualCamera>();
    [SerializeField] private CameraInterim _cameraInterim;

    private void Awake()
    {
        if (Ønstance == null)
        {
            Ønstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _vCameras.Add(StaticCameraMovement.instance.GetComponent<CinemachineVirtualCamera>());
        _vCameras.Add(StaticDialogueCameraMovement.Instance.GetComponent<CinemachineVirtualCamera>());

        ActivateMainCamera();
        //cameraInterim.DialogueActiveEvent += ActivateDialogueCamera;
    }

    public void ActivateMainCamera()
    {
        _cameraInterim.DisactivateDialogue();
        SwitchCamera(StaticCameraMovement.instance.GetComponent<CinemachineVirtualCamera>());
    }

    public void ActivateDialogueCamera()
    {
        _cameraInterim.ActivateDialogue();
        SwitchCamera(StaticDialogueCameraMovement.Instance.GetComponent<CinemachineVirtualCamera>());
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
}