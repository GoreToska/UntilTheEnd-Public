using System.Collections.Generic;
using UnityEngine;

public class PropsMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _direction;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _initialPosition;
    [SerializeField] private List<GameObject> _props;
    private GameObject _lastProp;

    private void Awake()
    {
        _lastProp = _props[0];
    }

    private void FixedUpdate()
    {
        foreach(GameObject prop in _props)
            MoveProp(prop);
    }

    private void OnTriggerEnter(Collider other)
    {
        //other.gameObject.transform.localPosition = _initialPosition;
        float length = _lastProp.GetComponent<Renderer>().bounds.size.x + 0.4f;
        other.gameObject.transform.localPosition = _lastProp.transform.localPosition - _direction * length;

        _lastProp = other.gameObject;
    }

    private void MoveProp(GameObject prop)
    {
        prop.transform.position += _direction * _speed * Time.deltaTime;
    }
}