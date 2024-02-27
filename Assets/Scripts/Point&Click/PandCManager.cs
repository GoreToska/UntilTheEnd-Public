using UnityEngine;
using UnityEngine.InputSystem;

public class PandCManager : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    private Camera _camera;

    private RaycastHit _hit;
    private Ray _ray;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _inputReader.PointAndClick += RaycastReaction;
    }

    private void RaycastReaction()
    {
        var hits = Physics.RaycastAll(_camera.ScreenPointToRay(Mouse.current.position.ReadValue()));

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.GetComponent<EnvironmentDataHolder>())
            {
                var envObject = hit.collider.gameObject.GetComponent<EnvironmentDataHolder>();

                UIManager.Instance.ShowInspectionWindow(envObject.Data.Description, envObject.Data.Voice.length);
                MusicManager.instance.PlayInspectionPhrase(envObject.Data.Voice);
                envObject.Destruct();
            }
        }

        //_ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        //if (Physics.Raycast(_ray, out _hit))
        //{
        //    var envObject = _hit.transform.gameObject.GetComponent<EnvironmentDataHolder>();
        //    if (envObject)
        //    {
        //        _uiManager.ShowInspectionWindow(envObject.Data.Description, envObject.Data.Voice.length);
        //        _soundManager.PlayInspectionPhrase(envObject.Data.Voice);
        //        envObject.Destruct();
        //    }
        //}
    }
}
