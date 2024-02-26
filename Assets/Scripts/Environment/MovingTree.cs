using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingTree : MonoBehaviour
{
    [SerializeField] private float _range;
    [SerializeField] private float _duration;

    private Tween tween;

    private void OnEnable()
    {
        StartCoroutine(StartMoving());
    }

    private void FixedUpdate()
    {
        if (transform.position.x < -5)
        {
            tween.Kill();
            Destroy(gameObject);
        }
    }

    private IEnumerator StartMoving()
    {
        tween = transform.DOMoveX(_range, _duration);

        yield return tween.WaitForCompletion();
    }
}
