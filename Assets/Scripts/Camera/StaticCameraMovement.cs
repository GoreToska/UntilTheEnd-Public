using Cinemachine;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Zenject;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class StaticCameraMovement : MonoBehaviour
{
    // input
    [SerializeField] private InputReader _inputReader = default;
    private CinemachineVirtualCamera _virtualCamera;

    // settings
    private float _maxCameraOffset = 4.2f;
    private float _minCameraOffset = 3f;
    [SerializeField] private float _zoomSensitivity = 1f;

	[Inject] private CameraTarget _cameraTarget;

	private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();

        if (_virtualCamera == null)
        {
            // TODO: if none, create new through code
            Debug.LogError("CameraMovement requires CinemachineVirtualCamera component");
        }
    }
	private void Start()
	{
        _virtualCamera.Follow = _cameraTarget.gameObject.transform;
    }

    private void OnEnable()
    {
		InputReader.ZoomEvent += OnZoom;

        _virtualCamera.Follow = _cameraTarget.gameObject.transform;
    }

    private void OnDisable()
    {
		InputReader.ZoomEvent -= OnZoom;
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
