using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UISoundManager : MonoBehaviour
{
    [HideInInspector] public static UISoundManager instance;

    [SerializeField] private AudioMixerGroup _UISoundsAudioMixer;
    [SerializeField] private List<AudioClip> _UIClickClips;
    [SerializeField] private List<AudioClip> _UIConclusionSounds;
    [SerializeField] private AudioClip _UIDeleteConclusionClip;
    [SerializeField] private AudioSource _UIAudioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayClickSound()
    {
        _UIAudioSource.PlayOneShot(_UIClickClips[Random.Range(0, _UIClickClips.Count)]);
    }

    public void PlaySuccessConclusionSound()
    {
        _UIAudioSource.PlayOneShot(_UIConclusionSounds[Random.Range(0, _UIConclusionSounds.Count)]);
    }

    public void PlayDeleteConclusionSound()
    {
        _UIAudioSource.PlayOneShot(_UIDeleteConclusionClip);
    }
}
