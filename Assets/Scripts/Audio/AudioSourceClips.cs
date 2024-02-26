using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceClips : MonoBehaviour
{
    [Header("������ ������ �� ������� � AudioSource")]
    [SerializeField] private List<AudioClip> _clips;

    [Header("�����, ������� ���������� ����� ����� ����� �������")]
    [SerializeField] private float _minTime;
    [SerializeField] private float _maxTime;

    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();

        StartCoroutine(PlayClip());
    }

    private float StartNewClip()
    {
        var clip = _clips[Random.Range(0, _clips.Count - 1)];
        _source.clip = clip;

        _source.Play();

        return clip.length + Random.Range(_minTime, _maxTime);
    }

    private IEnumerator PlayClip()
    {
        while (true)
        {
            var seconds = StartNewClip();

            yield return new WaitForSeconds(seconds);
        }
    }
}
