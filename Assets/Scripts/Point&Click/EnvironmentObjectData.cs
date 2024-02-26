using UnityEngine;

[CreateAssetMenu(fileName = "Environment Object", menuName = "UTE/Environment Object")]
public class EnvironmentObjectData : ScriptableObject
{
    [SerializeField] private string _description;
    [SerializeField] private AudioClip _voice;

    public string Description { get { return _description; } }
    public AudioClip Voice { get { return _voice; } }
}
