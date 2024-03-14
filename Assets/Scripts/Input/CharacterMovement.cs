using UnityEngine;
using Zenject;

//Summary
// This class was used for character movement with non-static camera view. Now this class is legacy
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private GameObject _virtualCamera;
    [SerializeField] private float _movementSpeed;

    private Vector2 _inputVector;
    private Rigidbody _playerRigidbody;

    private void Awake()
    {
        if (_virtualCamera == null)
        {
            Debug.Log($"There is must ba a camera attached to player");
        }
    }

    private void OnEnable()
    {
        _playerRigidbody = GetComponent<Rigidbody>();

		InputReader.MoveEvent += OnMove;

        // TODO: other functions
    }

    private void OnDisable()
    {
		InputReader.MoveEvent -= OnMove;

        // TODO: other functions
    }

    private void FixedUpdate()
    {
        RotateCharacter();
        MoveCharacter();
    }

    private void OnMove(Vector2 movement)
    {
        _inputVector = movement;
    }

    private void MoveCharacter()
    {
        _playerRigidbody.velocity = (transform.forward * _inputVector.y * _movementSpeed) +
            (transform.right * _inputVector.x * _movementSpeed) + (transform.up * _playerRigidbody.velocity.y);
    }

    private void RotateCharacter()
    {
        if (_inputVector != Vector2.zero)
            transform.rotation = Quaternion.Euler(0, _virtualCamera.transform.rotation.eulerAngles.y, 0);
    }

    public Vector2 GetMovementVector
    {
        get { return _inputVector; }
    }
}