using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraMovement : MonoBehaviour
{
    // TODO: refactoring, delete serializes, only private fields through code

    // input
    [SerializeField] private InputReader _inputReader = default;
    private Vector2 _lookInputVector;

    private CinemachineFramingTransposer _transposer;
    private CinemachineVirtualCamera _virtualCamera;

    // settings
    private float _maxVerticalAngle = 75f;
    private float _minVerticalAngle = 25f;
    [SerializeField] private float _horizontalRotationSensitivity = 2f;
    [SerializeField] private float _verticalRotationSensitivity = 1f;
    private float _maxCameraOffset = 4.2f;
    private float _minCameraOffset = 3f;
    [SerializeField] private float _zoomSensitivity = 1f;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if( _virtualCamera == null )
        {
            // TODO: if none, create new through code
            Debug.Log("CameraMovement requires CinemachineVirtualCamera component");
        }

        _transposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (_transposer == null)
        {
            // TODO: if none, create new through code
            Debug.Log("CameraMovement requires transposer component");
        }
    }

    private void OnEnable()
    {
        _inputReader.LookEvent += OnLook;
        _inputReader.ZoomEvent += OnZoom;
    }

    private void OnDisable()
    {
        _inputReader.LookEvent -= OnLook;
        _inputReader.ZoomEvent -= OnZoom;
    }

    private void Update()
    {
        CalculateCameraTransform();
    }

    private void OnLook(Vector2 look)
    {
        _lookInputVector = look;
    }

    private void OnZoom(float zoom)
    {
        CalculateCameraOffset(zoom);
    }

    void CalculateCameraTransform()
    {
        if (_lookInputVector == Vector2.zero)
        {
            return;
        }

        transform.rotation *= Quaternion.AngleAxis(_lookInputVector.x * _horizontalRotationSensitivity, Vector3.up);
        transform.rotation *= Quaternion.AngleAxis(_lookInputVector.y * _verticalRotationSensitivity, Vector3.right);

        Vector3 angles = transform.localEulerAngles;
        angles.z = 0;

        //Clamp the Up/Down rotation
        if (angles.x < 180 && angles.x > _maxVerticalAngle)
        {
            angles.x = _maxVerticalAngle;
        }
        else if (angles.x < 180 && angles.x < _minVerticalAngle || angles.x > 180 && angles.x < _minVerticalAngle) 
        {
            angles.x = _minVerticalAngle;
        }
        
        transform.localEulerAngles = angles;
    }

    void CalculateCameraOffset(float zoomInput) 
    {
        float newCameraDistance = _virtualCamera.m_Lens.OrthographicSize + zoomInput / 100 * _zoomSensitivity * Time.deltaTime;
        if(newCameraDistance < _minCameraOffset)
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
