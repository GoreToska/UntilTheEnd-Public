using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class StaticCharacterMovement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 1f;
    [SerializeField] private float _sprintSpeed = 1.6f;

    private float _currentSpeed = 1f;
    private int _movementMultiplier = 1;

    private Vector2 _inputVector;
    private Vector3 _directionForward;

    private NavMeshAgent _playerNavMesh;

    //New
    private Vector3 _movementVector;
    private Vector3 _lastDirection;
    private Vector3 _targetDirection;
    private float _lerpTime;

    [SerializeField]
    [Range(0f, 1f)]
    private float _smoothing = 0.25f;
    [SerializeField] private float _targetLerpSpeed = 1;

    [Inject] private StaticCameraMovement _staticCameraMovement;

	private void Start()
    {
        _directionForward.y = 0;

        _directionForward = _staticCameraMovement.gameObject.transform.forward.normalized;
    }

    private void OnEnable()
    {
        _playerNavMesh = GetComponent<NavMeshAgent>();
		InputReader.MoveEvent += OnMove;
		InputReader.Sprint += OnSprint;
		InputReader.StopSprint += OnSprintExit;
		// TODO: other functions
	}

    private void OnDisable()
    {
		InputReader.MoveEvent -= OnMove;
		InputReader.Sprint -= OnSprint;
		InputReader.StopSprint -= OnSprintExit;
    }

    private void Update()
    {
        MoveCharacter();
    }

    private void OnMove(Vector2 movement)
    {
        _inputVector = movement;
    }

    private void OnSprint()
    {
        _movementMultiplier = 2;
        _currentSpeed = _sprintSpeed;
    }

    private void OnSprintExit()
    {
        _movementMultiplier = 1;
        _currentSpeed = _walkSpeed;
    }

    private void MoveCharacter()
    {
        _movementVector = new Vector3(-_inputVector.x, 0, -_inputVector.y);
        _movementVector.Normalize();

        if (_movementVector != _lastDirection)
        {
            _lerpTime = 0f;
        }

        _lastDirection = _movementVector;
        _targetDirection = Vector3.Lerp(_targetDirection, _movementVector,
            Mathf.Clamp01(_lerpTime * _targetLerpSpeed * (1 - _smoothing)));

        _playerNavMesh.Move(_playerNavMesh.speed * Time.deltaTime * _targetDirection * _currentSpeed);

        RotateCharacter();

        _lerpTime += Time.deltaTime;
    }

    private void RotateCharacter()
    {
        Vector3 lookDirection = _movementVector;

        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDirection),
                Mathf.Clamp01(_lerpTime * _targetLerpSpeed * (1 - _smoothing)));
        }
    }

    public float StandardMovementVector
    {
        get { return _playerNavMesh.speed * _targetDirection.magnitude * _movementMultiplier; }
    }

    public float SecondaryMovementVector
    {
        get { return _playerNavMesh.velocity.magnitude * _movementMultiplier; }
    }

    public void SetNavigation(Transform transform)
    {
        _playerNavMesh.SetDestination(transform.position);
    }
}