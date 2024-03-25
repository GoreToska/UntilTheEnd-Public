using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIReport : MonoBehaviour
{
    [SerializeField] private GameObject _reportExample;
    [SerializeField] private EvidenceInfo _reportInfo;
    [SerializeField] private GameObject _reportHelp;
    [SerializeField] private GameObject _reportsScrollView;
    [SerializeField] private Transform _reportsList;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private EvidenceMatchingController _matchingConnect;
    //[SerializeField] private GameObject _pageHint;
    [SerializeField] private PageHint _hint;

    [Inject] private UISoundManager _uiSoundManager;

	private List<string> eNPCNames = new();

    private void Awake()
    {
        ReportsInit();
    }

    private void OnEnable()
    {
        _inventory.AddReportEvent += AddReport;
    }

    private void OnDisable()
    {
        _inventory.AddReportEvent -= AddReport;
    }

    //перепроектировать
    public void AddReport(EvidenceReport evidenceReport, bool disableHint)
    {
        if (_inventory.ReportsInfos.Count >= 0)
        {
            if (_reportHelp)
                _reportHelp.SetActive(false);
        }

        if (!eNPCNames.Contains(evidenceReport.NPCName))
        {
            eNPCNames.Add(evidenceReport.NPCName);
        }

        var NewReport = Instantiate(_reportExample, _reportsList);
        NewReport.GetComponentsInChildren<Image>()[1].sprite = evidenceReport.Sprite;

        NewReport.GetComponent<Button>().onClick.AddListener(() => _matchingConnect.PassEvidence(evidenceReport));
        NewReport.GetComponent<Button>().onClick.AddListener(() => _uiSoundManager.PlayClickSound());
        NewReport.GetComponent<InvokeReportInfo>().SetData(evidenceReport.Description, evidenceReport.NPCName.ToString(), evidenceReport.Sprite);

        if (disableHint)
        {
            NewReport.GetComponent<ButtonHint>().DisableHint();
        }
        else
        {
            if (!_hint)
            {
                foreach (var item in Resources.FindObjectsOfTypeAll<PageHint>())
                {
                    if (item.name == "Journal")
                    {
                        _hint = item;
                    }
                }
            }

            _hint.EnableHint();
        }
    }

    private void ReportsInit()
    {
        List<EvidenceReport> reportinfo = _inventory.ReportsInfos;

        foreach (var report in reportinfo)
        {
            AddReport(report, true);
        }
    }

    //public Canvas Panel
    //{
    //    get { return _reportsScrollView; }
    //}
}
