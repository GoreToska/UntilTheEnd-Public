using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Camera))]
public class InspectionCamera : MonoBehaviour
{
	[NonSerialized] public WorldEvidence InspectableObject;

	[SerializeField] private Transform _objectRotator;
	
	private GameObject _target;
	private Vector2 _targetRotation;
	private float _zoom;
	private Vector3 _targetPosition;

	[Header("Rotation & Zoom Settings")]
	[SerializeField] private float _degree = -60;
	[SerializeField] private float _rotationSpeed = 50f;
	[SerializeField] private float _zoomSpeed = 10f;
	[SerializeField] private float _zoomLerpSpeed = 10f;
	private Camera _camera;

	private void Awake()
	{
		_camera = GetComponent<Camera>();
		DisableCamera();
	}

	private void OnEnable()
	{
		InputReader.ClickEvent += OnCLick;
		InputReader.ZoomEvidenceEvent += OnZoom;
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
		_target = Instantiate(gameObject, _objectRotator);
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
