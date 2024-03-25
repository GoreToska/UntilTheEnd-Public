using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _musicAudioMixer;
    [SerializeField] private List<AudioClip> _trainMusicClips;
    [SerializeField] private List<AudioClip> _railwayMusicClips;
    [SerializeField] private List<AudioClip> _trainSoundsMusicClips;
    [SerializeField] private List<AudioClip> _rainMusicClips;
    [SerializeField] private List<AudioClip> _courtSoundsMusicClips;
    [SerializeField] private List<AudioClip> _estateSoundsMusicClips;
    [SerializeField] private List<AudioClip> _pubSoundsMusicClips;
    [SerializeField] private List<FootstepsSounds> _footstepsSounds;

    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _rainAudioSource;
    [SerializeField] private AudioSource _trainAudioSource;
    [SerializeField] private AudioSource _voiceAudioSource;

    private void OnDisable()
    {
        StopAllMusic();
    }

    private void FixedUpdate()
    {
        PlayBackgroundSounds();
        PlayAmbient();
    }

    public void PlayFootstepSound(string tag, AudioSource source)
    {
		source.PlayOneShot(_footstepsSounds.Find(i=> i.Tag == tag).Sound);
    }

    private void PlayAmbient()
    {
        if (_musicAudioSource.isPlaying)
            return;

        if (UTESceneManager.CurrentScene == UTESceneManager.TrainName)
        {
            _musicAudioSource.PlayOneShot(_trainMusicClips[Random.Range(0, _trainMusicClips.Count)]);
        }

        if (UTESceneManager.CurrentScene == UTESceneManager.RailwayName)
        {
            _musicAudioSource.PlayOneShot(_railwayMusicClips[Random.Range(0, _trainMusicClips.Count)]);
        }

        if (UTESceneManager.CurrentScene == UTESceneManager.CourtName)
        {
            _musicAudioSource.PlayOneShot(_courtSoundsMusicClips[Random.Range(0, _courtSoundsMusicClips.Count)]);
        }

        if (UTESceneManager.CurrentScene == UTESceneManager.EstateName ||
            UTESceneManager.CurrentScene == UTESceneManager.EstateFirstName ||
            UTESceneManager.CurrentScene == UTESceneManager.EstateSecondName)
        {
            _musicAudioSource.PlayOneShot(_estateSoundsMusicClips[Random.Range(0, _estateSoundsMusicClips.Count)]);
        }
    }

    private void PlayBackgroundSounds()
    {
        if (UTESceneManager.CurrentScene == UTESceneManager.TrainName)
        {
            PlayTrainSounds();
            PlayLiteRain();
        }

        if (UTESceneManager.CurrentScene == UTESceneManager.RailwayName)
        {
            _musicAudioSource.PlayOneShot(_railwayMusicClips[Random.Range(0, _trainMusicClips.Count)]);
        }
    }

    private void PlayLiteRain()
    {
        if (_rainAudioSource.isPlaying)
            return;

        _rainAudioSource.PlayOneShot(_rainMusicClips[0]);
    }

    private void PlayTrainSounds()
    {
        if (_trainAudioSource.isPlaying)
            return;

        _trainAudioSource.PlayOneShot(_trainSoundsMusicClips[0]);
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
