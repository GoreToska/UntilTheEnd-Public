using PixelCrushers.DialogueSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GGDestinationTrigger : MonoBehaviour
{
    [SerializeField] private Transform _lookAt;
    [SerializeField] private string _variableName;

    private void FixedUpdate()
    {
        if (!DialogueLua.GetVariable(_variableName).asBool)
            return;

        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<NavMeshAgent>().enabled)
            {
                other.gameObject.transform.LookAt(_lookAt);
                StartCoroutine(Wait(other.gameObject.GetComponent<NavMeshAgent>()));
            }
        }
    }

    private IEnumerator Wait(NavMeshAgent navMesh)
    {
        yield return new WaitForSeconds(0.3f);

        navMesh.isStopped = true;

        Destroy(this);
    }
}
