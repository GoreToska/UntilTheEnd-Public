using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [HideInInspector] public static MusicManager instance;

    [SerializeField] private AudioMixerGroup _musicAudioMixer;
    [SerializeField] private List<AudioClip> _TrainMusicClips;
    [SerializeField] private List<AudioClip> _RailwayMusicClips;
    [SerializeField] private List<AudioClip> _TrainSoundsMusicClips;
    [SerializeField] private List<AudioClip> _RainMusicClips;
    [SerializeField] private List<AudioClip> _CourtSoundsMusicClips;
    [SerializeField] private List<AudioClip> _EstateSoundsMusicClips;
    [SerializeField] private List<AudioClip> _PubSoundsMusicClips;

    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _rainAudioSource;
    [SerializeField] private AudioSource _trainAudioSource;
    [SerializeField] private AudioSource _voiceAudioSource;

    [SerializeField] private List<FootstepsSounds> _sounds;

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

    private void OnDisable()
    {
        StopAllMusic();
    }

    private void FixedUpdate()
    {
        PlayBackgroundSounds();
        PlayAmbient();
    }

    public void PlayFootSound(string tag, AudioSource source)
    {
        foreach (var footstep in _sounds)
        {
            if (tag != footstep.Tag)
                continue;

            source.PlayOneShot(footstep.Sound);
        }
    }

    private void PlayAmbient()
    {
        if (_musicAudioSource.isPlaying)
            return;

        if (UTESceneManager.CurrentScene == UTESceneManager.TrainName)
        {
            _musicAudioSource.PlayOneShot(_TrainMusicClips[Random.Range(0, _TrainMusicClips.Count)]);
        }

        if (UTESceneManager.CurrentScene == UTESceneManager.RailwayName)
        {
            _musicAudioSource.PlayOneShot(_RailwayMusicClips[Random.Range(0, _TrainMusicClips.Count)]);
        }

        if (UTESceneManager.CurrentScene == UTESceneManager.CourtName)
        {
            _musicAudioSource.PlayOneShot(_CourtSoundsMusicClips[Random.Range(0, _CourtSoundsMusicClips.Count)]);
        }

        if (UTESceneManager.CurrentScene == UTESceneManager.EstateName ||
            UTESceneManager.CurrentScene == UTESceneManager.EstateFirstName ||
            UTESceneManager.CurrentScene == UTESceneManager.EstateSecondName)
        {
            _musicAudioSource.PlayOneShot(_EstateSoundsMusicClips[Random.Range(0, _EstateSoundsMusicClips.Count)]);
        }
    }

    private void PlayBackgroundSounds()
    {
        if (UTESceneManager.CurrentScene == UTESceneManager.TrainName)
        {
            TrainSounds();
            LiteRain();
        }

        if (UTESceneManager.CurrentScene == UTESceneManager.RailwayName)
        {
            _musicAudioSource.PlayOneShot(_RailwayMusicClips[Random.Range(0, _TrainMusicClips.Count)]);
        }
    }

    private void LiteRain()
    {
        if (_rainAudioSource.isPlaying)
            return;

        _rainAudioSource.PlayOneShot(_RainMusicClips[0]);
    }

    private void TrainSounds()
    {
        if (_trainAudioSource.isPlaying)
            return;

        _trainAudioSource.PlayOneShot(_TrainSoundsMusicClips[0]);
    }

    public void PlayInspectionPhrase(AudioClip audioClip)
    {
        _voiceAudioSource.Stop();
        _voiceAudioSource.PlayOneShot(audioClip);
    }

    public void StopAllMusic()
    {
        _musicAudioSource.Stop();
        _rainAudioSource.Stop();
        _trainAudioSource.Stop();
        _voiceAudioSource.Stop();
    }
}
