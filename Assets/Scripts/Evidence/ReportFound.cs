using UnityEngine;

public class ReportFound : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private EvidenceReport _evidenceReport;

    private void OnTriggerEnter(Collider other)
    {
		InputReader.InteractEvent += OnInteract;
    }

    private void OnTriggerExit(Collider other)
    {

		InputReader.InteractEvent -= OnInteract;
    }

    private void OnInteract()
    {
        Debug.Log("OnInteract");

        _inventory.AddEvidence(_evidenceReport);
		InputReader.InteractEvent -= OnInteract;
        Destroy(gameObject);
    }
}