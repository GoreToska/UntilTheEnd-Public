using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WalkAnimController : MonoBehaviour
{
    [SerializeField] private GameObject _pistol;
    [SerializeField] private AudioClip _shot;
    
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (_navMeshAgent)
            _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
    }

    private void PlayShotSound()
    {
        GetComponent<AudioSource>().PlayOneShot(_shot);
    }

    private void Rotate(float value)
    {
        transform.Rotate(0, value, 0);
    }

    IEnumerator StartRotate(float value)
    {
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(transform.position.x + value, transform.position.y, transform.position.z + value));
        float time = 0;

        while (time < 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time / 1f);

            time += Time.deltaTime;

            yield return null;
        }
    }

    private void ShowPistol()
    {
        _pistol.SetActive(true);
    }

    private void HidePistol()
    {
        _pistol.SetActive(false);
    }
}
