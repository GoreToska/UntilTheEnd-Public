using UnityEngine;
using UnityEngine.Events;

public class CharacterFootSteps : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private LayerMask _layerMask;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayStepSound(string leg)
    {
        Vector3 startPoint;

        if (leg == "Left")
        {
            startPoint = _animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position;
        }
        else
        {
            startPoint = _animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position;
        }

        RaycastHit hit;
        if (Physics.Linecast(startPoint, startPoint + Vector3.down, out hit, _layerMask))
        {
            MusicManager.instance.PlayFootSound(hit.collider.tag, _audioSource);
        }
    }
}
