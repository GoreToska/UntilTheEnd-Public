using Cinemachine;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class StaticDialogueCameraMovement : MonoBehaviour
{
	// input
	[SerializeField] private InputReader _inputReader = default;
	private CinemachineVirtualCamera _virtualCamera;

	// settings
	private float _maxCameraDistance = 4.2f;
	private float _minCameraDistance = 3f;
	[SerializeField] private float _zoomSensitivity = 1f;
	[SerializeField] private float _zoomSpeed = 0.5f;

	[Inject] private CameraTarget _cameraTarget;

	private float _currentCameraDistance;

	private void Awake()
	{
		_virtualCamera = GetComponent<CinemachineVirtualCamera>();
		_virtualCamera.Follow = _cameraTarget.gameObject.transform;
		_currentCameraDistance = _minCameraDistance;
	}

	private void Update()
	{
		_virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(_virtualCamera.m_Lens.OrthographicSize, _currentCameraDistance, _zoomSpeed * Time.deltaTime);
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

	private void CalculateCameraOffset(float zoomInput)
	{
		_currentCameraDistance += zoomInput / 100 * _zoomSensitivity * Time.deltaTime;
		_currentCameraDistance = Mathf.Clamp(_currentCameraDistance, _minCameraDistance, _maxCameraDistance);
	}
}
