using UnityEngine;
using UnityEngine.Events;

public class EvidenceCard : MonoBehaviour
{
    public event UnityAction<AEvidence> EvidenceCardActivateEvent = delegate { };
    private AEvidence _evidenceItem;

    private void Awake()
    {
        if (_evidenceItem == null)
        {
            Debug.LogError("Evidence info is not attached" +
                "to the card!");
        }
    }

    public AEvidence EvidenceItemData
    {
        get { return _evidenceItem; }
        set { _evidenceItem = value; }
    }

    public void OnCardActivate()
    {
        Debug.Log($"OnCardActivate - {_evidenceItem.Name}");
        EvidenceCardActivateEvent.Invoke(_evidenceItem);
    }
}