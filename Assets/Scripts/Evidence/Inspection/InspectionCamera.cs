using System;
using UnityEngine;

public class InspectionCamera : MonoBehaviour
{
	[HideInInspector] public static InspectionCamera Instance;

	[NonSerialized] public WorldEvidence InspectableObject;

	[SerializeField] private InputReader _inputReader;

	[SerializeField] private Transform _objectRotator;
	[SerializeField] private Vector3 _targetPosition;
	private Vector2 _targetRotation;
	private float _zoom;
	[SerializeField] private Vector3 _initialSpawnOffset = Vector3.down * 5f;

	[Header("Rotation & Zoom Settings")]
	[SerializeField] private float _rotationSpeed = 50f;
	[SerializeField] private float _zoomSpeed = 10f;


	public void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

	}

	private void OnEnable()
	{
		InputReader.ClickEvent += OnCLick;
		InputReader.ZoomEvidenceEvent += OnZoom;

		_objectRotator.localPosition = _initialSpawnOffset;
		_objectRotator.localRotation = Quaternion.Euler(Vector3.zero + new Vector3(_targetRotation.x, _targetRotation.y, 0));
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
		InspectableObject.transform.Rotate(new Vector3(_targetRotation.y, -_targetRotation.x, 0) * Time.deltaTime * _rotationSpeed, Space.World);
	}

	private void ZoomInOut()
	{
		_targetPosition = new Vector3(
			Mathf.Clamp(_targetPosition.x + _zoom, InspectableObject.EvidenceItem.MinMaxZoomX.x, InspectableObject.EvidenceItem.MinMaxZoomX.y), 
			_targetPosition.y, 
			Mathf.Clamp(_targetPosition.z - _zoom, InspectableObject.EvidenceItem.MinMaxZoomZ.x, InspectableObject.EvidenceItem.MinMaxZoomZ.y));

		_objectRotator.localPosition = Vector3.Lerp(_objectRotator.localPosition, _targetPosition,
			Time.deltaTime * _zoomSpeed);
	}
}
