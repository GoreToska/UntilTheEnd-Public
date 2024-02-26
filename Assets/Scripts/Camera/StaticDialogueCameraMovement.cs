using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class StaticDialogueCameraMovement : MonoBehaviour
{
    [HideInInspector] public static StaticDialogueCameraMovement Instance;

    // input
    [SerializeField] private InputReader _inputReader = default;
    private CinemachineVirtualCamera _virtualCamera;

    // settings
    private float _maxCameraOffset = 4.2f;
    private float _minCameraOffset = 3f;
    [SerializeField] private float _zoomSensitivity = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _virtualCamera = GetComponent<CinemachineVirtualCamera>();

        if (_virtualCamera == null)
        {
            // TODO: if none, create new through code
            Debug.LogError("CameraMovement requires CinemachineVirtualCamera component");
        }

    }

    private void Start()
    {
       // _virtualCamera.Follow = CameraTarget.Instance.gameObject.transform;
    }

    private void OnEnable()
    {
        _inputReader.ZoomEvent += OnZoom;

        _virtualCamera.Follow = CameraTarget.Instance.gameObject.transform;
    }

    private void OnDisable()
    {
        _inputReader.ZoomEvent -= OnZoom;
    }

    private void OnZoom(float zoom)
    {
        CalculateCameraOffset(zoom);
    }

    void CalculateCameraOffset(float zoomInput)
    {
        float newCameraDistance = _virtualCamera.m_Lens.OrthographicSize + zoomInput / 100 * _zoomSensitivity * Time.deltaTime;
        if (newCameraDistance < _minCameraOffset)
        {
            newCameraDistance = _minCameraOffset;
        }
        else if (newCameraDistance > _maxCameraOffset)
        {
            newCameraDistance = _maxCameraOffset;
        }

        _virtualCamera.m_Lens.OrthographicSize = newCameraDistance;
    }
}
