using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InspectFound : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private TMP_Text _helpText;
    [SerializeField] private TMP_Text _commentText;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private string _comment;
    [SerializeField] private UIManager _manager;

    private void Awake()
    {
        _canvas.SetActive(true);
        _commentText.enabled = false;
        _helpText.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        _inputReader.InspectEvent += OnInspect;
        _manager.EnableInspectText(true);
    }

    private void OnTriggerExit(Collider other)
    {
        _inputReader.InspectEvent -= OnInspect;
        _manager.EnableInspectText(false);
    }

    private void OnInspect()
    {
        // logic
        StartCoroutine(InspectionCoroutine());

    }

    private void OnDestroy()
    {
        _inputReader.InspectEvent -= OnInspect;
    }

    IEnumerator InspectionCoroutine()
    {
        _commentText.text = _comment;
        _commentText.enabled = true;
        _manager.EnableInspectText(false);

        _audioSource.clip = _clip;
        _audioSource.Play();

        yield return new WaitForSeconds(5);

        _commentText.enabled = false;

        Destroy(this);
    }
}
