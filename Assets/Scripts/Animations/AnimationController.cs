using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private float _movementSpeed;
    private StaticCharacterMovement _staticMovement;
    private Animator _animator;

    private float _velocity;
    private float _acceleration;
    private bool _gravity;

    private void Awake()
    {
        _staticMovement = GetComponent<StaticCharacterMovement>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        CalculateStaticAnimation();
    }

    private void CalculateStaticAnimation()
    {
        if(_staticMovement.SecondaryMovementVector > 0)
        {
            _movementSpeed = _staticMovement.SecondaryMovementVector;
        }
        else
        {
            _movementSpeed = _staticMovement.StandardMovementVector;
        }

        _animator.SetFloat("Speed", _movementSpeed, 0.1f, Time.deltaTime);

        return;
    }

    public void ClothRebuild()
    {
        GetComponentInChildren<Cloth>().ClearTransformMotion();
    }

    public void DisableCloth()
    {
        GetComponentInChildren<Cloth>().ClearTransformMotion();
        _velocity = GetComponentInChildren<Cloth>().worldVelocityScale;
        GetComponentInChildren<Cloth>().worldVelocityScale = 0;
        _acceleration = GetComponentInChildren<Cloth>().worldAccelerationScale;
        GetComponentInChildren<Cloth>().worldAccelerationScale = 0;
        _gravity = GetComponentInChildren<Cloth>().useGravity;
        GetComponentInChildren<Cloth>().useGravity = false;
    }

    public void EnableCloth()
    {
        GetComponentInChildren<Cloth>().worldVelocityScale = _velocity;
        GetComponentInChildren<Cloth>().worldAccelerationScale = _acceleration;
        GetComponentInChildren<Cloth>().useGravity = _gravity;

        GetComponentInChildren<Cloth>().ClearTransformMotion();
    }
}