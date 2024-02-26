using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class NPCNavigationStopTrigger : MonoBehaviour
{
    [SerializeField] private string _NPCName;
    [SerializeField] private Transform _lookAt;
    [SerializeField] private float _stopRotationTime = 0.8f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == _NPCName & other.transform.root.GetComponent<NavMeshAgent>().destination != null)
        {
            other.transform.root.GetComponent<NavMeshAgent>().SetDestination(other.transform.root.position);
            other.transform.root.GetComponent<NavMeshAgent>().isStopped = true;

            if (_lookAt)
                StartCoroutine(LookAt(other.transform.root.transform));
            else
                Destroy(this.gameObject);
        }
    }

    IEnumerator LookAt(Transform transform)
    {
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(_lookAt.position.x - transform.position.x, 0, _lookAt.position.z - transform.position.z));
        float time = 0;

        while (time < _stopRotationTime)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time / _stopRotationTime);

            time += Time.deltaTime;

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
