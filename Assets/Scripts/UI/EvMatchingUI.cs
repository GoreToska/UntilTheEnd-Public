using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EvMatchingUI : MonoBehaviour
{
    [HideInInspector] public static EvMatchingUI instance;

    [SerializeField] private GameObject _slotOne;
    [SerializeField] private GameObject _slotTwo;
    [SerializeField] private EvMatchConnectUI _matchingConnect;

    private GameObject _tempSlot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (_matchingConnect == null || _slotOne == null
            || _slotTwo == null)
        {
            Debug.LogError("Matching UI is not filled!");
        }

        //FullClearSlot(1);
        //FullClearSlot(2);

        _matchingConnect.PassEvidenceEvent += FillSlot;
        _matchingConnect.Initialize();
    }

    public void FillSlot(int num, AEvidence evidence)
    {
        DetermingCurrentSlot(num);


        if(evidence.EvidenceType == EEvidenceType.Item)
        {
            EvidenceItem evidenceItem = evidence as EvidenceItem;
            _tempSlot.GetComponentsInChildren<Image>()[1].sprite = evidenceItem.EvidenceIcon;
            _tempSlot.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 1);
        }
        else if (evidence.EvidenceType == EEvidenceType.Report)
        {
            EvidenceReport evidenceReport = evidence as EvidenceReport;
            _tempSlot.GetComponentsInChildren<Image>()[1].sprite = evidenceReport.Sprite;
            _tempSlot.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 1);
        }
        //TODO: Check if it's report or evidence
    }

    public void FullClearSlot(int num)
    {
        DetermingCurrentSlot(num);
        ClearSingleSlot(_tempSlot);
        _matchingConnect.ClearEvidence(num);
        //TODO: Finish with sprite
    }

    public void ClearSingleSlot(GameObject slot)
    {
        slot.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 0);
    }

    public void MakeConclusion()
    {
        _matchingConnect.TryToMakeConclusion();
    }

    private void DetermingCurrentSlot(int num)
    {
        if (num < 1 || num > 2)
        {
            Debug.LogError("Incorrect evidence slot number!");
            return;
        }

        if (num == 1)
        {
            _tempSlot = _slotOne;
        }
        else
        {
            _tempSlot = _slotTwo;
        }
    }
}