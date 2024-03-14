using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class InspectionCamera : MonoBehaviour
{
	[NonSerialized] public WorldEvidence InspectableObject;

	[SerializeField] private InputReader _inputReader;
	[SerializeField] private Transform _objectRotator;

	[SerializeField] private float _degree = -60;
	private GameObject _target;
	private Vector2 _targetRotation;
	private float _zoom;
	private Vector3 _targetPosition;
	//[SerializeField] private Vector3 _initialSpawnOffset = Vector3.down * 5f;

	[Header("Rotation & Zoom Settings")]
	[SerializeField] private float _rotationSpeed = 50f;
	[SerializeField] private float _zoomSpeed = 10f;
	[SerializeField] private float _zoomLerpSpeed = 10f;
	private Camera _camera;

	public Transform ObjectRotator { get { return _objectRotator; } }

	private void Awake()
	{
		//_defaultRotatorPosition = _objectRotator.localPosition;
		_camera = GetComponent<Camera>();
		DisableCamera();
	}

	private void OnEnable()
	{
		InputReader.ClickEvent += OnCLick;
		InputReader.ZoomEvidenceEvent += OnZoom;

		//ObjectRotator.localPosition = _defaultRotatorPosition;
		//ObjectRotator.localRotation = Quaternion.Euler(Vector3.zero + new Vector3(_targetRotation.x, _targetRotation.y, 0));
	}

	private void OnDisable()
	{
		InputReader.ClickEvent -= OnCLick;
		InputReader.ZoomEvent -= OnZoom;
	}

	private void Update()
	{
		if (InspectableObject == null)
			return;

		ZoomInOut();
		RotateObject();
	}

	private void OnZoom(float zoom)
	{
		_zoom = zoom;
	}

	private void OnCLick(bool isClicked)
	{
		if (isClicked)
		{
			InputReader.RotateEvent += OnRotate;
		}
		else
		{
			InputReader.RotateEvent -= OnRotate;
			_targetRotation = Vector3.zero;
		}
	}

	private void OnRotate(Vector2 rotation)
	{
		_targetRotation = rotation;
	}

	private void RotateObject()
	{
		_objectRotator.transform.Rotate(new Vector3(_targetRotation.y, -_targetRotation.x, 0) * Time.deltaTime * _rotationSpeed, Space.World);

		//		InspectableObject.transform.Rotate(new Vector3(_targetRotation.y, -_targetRotation.x, 0) * Time.deltaTime * _rotationSpeed, Space.World);
	}

	private void ZoomInOut()
	{
		float xInput = _targetPosition.x + _zoom / 100 * Mathf.Cos(Mathf.Deg2Rad * _degree) * _zoomSpeed;
		float yInput = _targetPosition.y;
		float zInput = _targetPosition.z + _zoom / 100 * Mathf.Sin(Mathf.Deg2Rad * _degree) * _zoomSpeed;

		xInput = Mathf.Clamp(xInput,
			-InspectableObject.EvidenceItem.MaxDistance * Mathf.Cos(Mathf.Deg2Rad * _degree),
			-InspectableObject.EvidenceItem.MinDistance * Mathf.Cos(Mathf.Deg2Rad * _degree));

		zInput = Mathf.Clamp(zInput,
			-InspectableObject.EvidenceItem.MinDistance * Mathf.Sin(Mathf.Deg2Rad * _degree),
			-InspectableObject.EvidenceItem.MaxDistance * Mathf.Sin(Mathf.Deg2Rad * _degree));

		_targetPosition = new Vector3(xInput, yInput, zInput);


		_objectRotator.localPosition = Vector3.Lerp(_objectRotator.localPosition,
			_targetPosition,
			_zoomLerpSpeed * Time.deltaTime
			);

		//_objectRotator.localPosition = Vector3.Lerp(
		//	_objectRotator.localPosition,
		//	new Vector3(
		//		Mathf.Clamp(_objectRotator.localPosition.x + _zoom *
		//		Mathf.Cos(Mathf.Deg2Rad * _degree) * _zoomSpeed, (float)(InspectableObject.EvidenceItem.MinDistance / Math.Cos(_degree)), (float)(InspectableObject.EvidenceItem.MaxDistance / Math.Cos(_degree))),
		//	_objectRotator.localPosition.y,
		//	Mathf.Clamp(_objectRotator.localPosition.z + _zoom *
		//	Mathf.Sin(Mathf.Deg2Rad * _degree) * _zoomSpeed, (float)(InspectableObject.EvidenceItem.MinDistance / Math.Sin(Mathf.Abs(_degree))), (float)(InspectableObject.EvidenceItem.MaxDistance * Math.Sin(Mathf.Abs(_degree))))),
		//	_zoomLerpSpeed * Time.deltaTime);

		//_objectRotator.localPosition = Vector3.Lerp(_objectRotator.localPosition,
		//	new Vector3(Mathf.Clamp(_objectRotator.localPosition.x + _defaultRotatorPosition.x + _zoom, InspectableObject.EvidenceItem.MinMaxZoomX.x, InspectableObject.EvidenceItem.MinMaxZoomX.y),
		//	_targetPosition.y,
		//	Mathf.Clamp(_objectRotator.localPosition.z + _defaultRotatorPosition.z + _zoom, InspectableObject.EvidenceItem.MinMaxZoomZ.x, InspectableObject.EvidenceItem.MinMaxZoomZ.y)), Time.deltaTime * _zoomSpeed);


		//_targetPosition = new Vector3(
		//	Mathf.Clamp(_targetPosition.x + _zoom, InspectableObject.EvidenceItem.MinMaxZoomX.x, InspectableObject.EvidenceItem.MinMaxZoomX.y),
		//	_targetPosition.y,
		//	Mathf.Clamp(_targetPosition.z - _zoom, InspectableObject.EvidenceItem.MinMaxZoomZ.x, InspectableObject.EvidenceItem.MinMaxZoomZ.y));

		//_objectRotator.localPosition = Vector3.Lerp(_objectRotator.localPosition, _targetPosition,
		//	Time.deltaTime * _zoomSpeed);
	}

	public void EnableCamera()
	{
		_camera.enabled = true;
	}

	public void DisableCamera()
	{
		_camera.enabled = false;
	}

	public void SpawnInspectableObject(GameObject gameObject, EvidenceItem evidence)
	{
		_target = Instantiate(gameObject, ObjectRotator);
		_target.GetComponent<Rigidbody>().isKinematic = true;
		_target.GetComponent<BoxCollider>().enabled = false;

		_target.transform.localPosition = evidence.SpawnPositionOffset;
		_target.transform.localRotation = Quaternion.Euler(Vector3.zero + evidence.SpawnRotationOffset);

		_targetPosition = new Vector3(-evidence.MinDistance * Mathf.Cos(Mathf.Deg2Rad * _degree), 0, -evidence.MinDistance * Mathf.Sin(Mathf.Deg2Rad * _degree));
		_objectRotator.localPosition = _targetPosition;

		EnableCamera();
	}

	public void DestroyInspectableObject()
	{
		Destroy(_target);
		DisableCamera();
	}
}
