using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FootstepsSounds", menuName = "UTE/FootstepsSounds")]
public class FootstepsSounds : ScriptableObject
{
    [Header("��� �����������, � ������� ��������� �����")]
    [SerializeField] private string _tag;

    [Header("�����")]
    [SerializeField] private List<AudioClip> _sounds;

    public string Tag
    {
        get { return _tag; }
    }

    public AudioClip Sound
    { get { return _sounds[Random.Range(0, _sounds.Count)]; } }
}
