using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIEvidence : MonoBehaviour
{
	[SerializeField] private GameObject _evidenceExample;
	[SerializeField] private GameObject _evidenceHelp;
	[SerializeField] private GameObject _evidencesScrollView;
	[SerializeField] private Inventory _inventory;
	[SerializeField] private EvidenceMatchingController _matchingConnect;
	[SerializeField] private GameObject _pageHint;
	private PageHint _hint;

	[Inject] private UISoundManager _uiSoundManager;

	private void Awake()
	{
		EvidencesInit();
		_hint = _pageHint.GetComponent<PageHint>();
	}

	private void OnEnable()
	{
		_inventory.AddEvidenceEvent += AddEvidence;
	}

	private void OnDisable()
	{
		_inventory.AddEvidenceEvent -= AddEvidence;
	}

	public void AddEvidence(EvidenceItem evidenceItem, bool disableHint)
	{
		if (_inventory.EvidenceInfos.Count >= 0)
		{
			_evidenceHelp.SetActive(false);
		}

		// Visual
		var evidence = Instantiate(_evidenceExample, _evidencesScrollView.transform.GetChild(1).transform);
		evidence.GetComponentsInChildren<Image>()[1].sprite = evidenceItem.EvidenceIcon;

		// Backend
		evidence.GetComponent<Button>().onClick.AddListener(() => _matchingConnect.PassEvidence(evidenceItem));
		evidence.GetComponent<Button>().onClick.AddListener(() => _uiSoundManager.PlayClickSound());
		evidence.GetComponent<InvokeReportInfo>().SetData(evidenceItem.Description, evidenceItem.Name, evidenceItem.EvidenceIcon);

		if (disableHint)
		{
			evidence.GetComponent<ButtonHint>().DisableHint();
		}
		else
		{
			_hint.EnableHint();
		}
	}

	private void EvidencesInit()
	{
		List<EvidenceItem> evidenceItems = _inventory.EvidenceInfos;

		foreach (EvidenceItem newEvidenceItem in evidenceItems)
		{
			AddEvidence(newEvidenceItem, true);
		}
	}

	//public Canvas Panel
	//{
	//    get { return _evidencesScrollView; }
	//}
}
