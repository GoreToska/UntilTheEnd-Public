using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class EvMatchingUI : MonoBehaviour
{
	[SerializeField] private EvidenceSlot _slotOne;
	[SerializeField] private EvidenceSlot _slotTwo;
	[SerializeField] private EvMatchConnectUI _matchingConnect;

	[Inject] private UISoundManager _soundManager;
	[Inject] private UIConclusion _uiConclusion;

	private void Awake()
	{
		if (_matchingConnect == null || _slotOne == null
			|| _slotTwo == null)
		{
			Debug.Log($"{_matchingConnect}, {_slotOne}, {_slotTwo}");
			Debug.LogError("Matching UI is not filled!");
		}

		FullClearSlot(1);
		FullClearSlot(2);

		_matchingConnect.PassEvidenceEvent += FillSlot;
		_matchingConnect.Initialize();
	}

	private void OnDisable()
	{
		FullClearSlot(1);
		FullClearSlot(2);
	}

	public void FillSlot(int num, AEvidence evidence)
	{
		if (evidence.EvidenceType == EEvidenceType.Item)
		{
			DetermingCurrentSlot(num).SetData(evidence as EvidenceItem);
		}
		else if (evidence.EvidenceType == EEvidenceType.Report)
		{
			DetermingCurrentSlot(num).SetData(evidence as EvidenceReport);
		}
	}

	public void FullClearSlot(int num)
	{
		DetermingCurrentSlot(num).ClearData();
		_matchingConnect.ClearEvidence(num);
		//TODO: Finish with sprite
	}

	public void MakeConclusion()
	{
		if(_matchingConnect.TryToMakeConclusion())
		{
			_soundManager.PlaySuccessConclusionSound();
			_uiConclusion.ShowConclusionSuccess(1f);
		}
		else
		{
			_uiConclusion.ShowConclusionsAlertMessage();
		}
	}

	private EvidenceSlot DetermingCurrentSlot(int num)
	{
		if (num != 1 && num != 2)
		{
			Debug.LogError("Incorrect evidence slot number!");
			return null;
		}

		if (num == 1)
		{
			return _slotOne;
		}
		else
		{
			return _slotTwo;
		}
	}
}