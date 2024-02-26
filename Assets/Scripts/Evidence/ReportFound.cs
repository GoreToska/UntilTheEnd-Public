using UnityEngine;

public class ReportFound : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private EvidenceReport _evidenceReport;

    private void OnTriggerEnter(Collider other)
    {
        _inputReader.InteractEvent += OnInteract;
    }

    private void OnTriggerExit(Collider other)
    {

        _inputReader.InteractEvent -= OnInteract;
    }

    private void OnInteract()
    {
        Debug.Log("OnInteract");

        _inventory.AddEvidence(_evidenceReport);
        _inputReader.InteractEvent -= OnInteract;
        Destroy(gameObject);
    }
}