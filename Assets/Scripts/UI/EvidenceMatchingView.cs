using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class EvidenceMatchingView : MonoBehaviour
{
	[SerializeField] private EvidenceSlot _firstSlot;
	[SerializeField] private EvidenceSlot _secondSlot;
	[SerializeField] private EvidenceMatchingController _matchingController;

	[Inject] private UISoundManager _soundManager;
	[Inject] private UIConclusion _uiConclusion;

	private void Awake()
	{
		if (_matchingController == null || _firstSlot == null
			|| _secondSlot == null)
		{
			Debug.Log($"{_matchingController}, {_firstSlot}, {_secondSlot}");
			Debug.LogError("Matching UI is not filled!");
		}

		ClearSlot(1);
		ClearSlot(2);

		_matchingController.PassEvidenceEvent += FillSlot;
		_matchingController.Initialize();
	}

	private void OnDisable()
	{
		ClearSlot(1);
		ClearSlot(2);
	}

	public void FillSlot(int num, AEvidence evidence)
	{
		if (evidence.EvidenceType == EEvidenceType.Item)
		{
			SetCurrentSlot(num).SetData(evidence as EvidenceItem);
		}
		else if (evidence.EvidenceType == EEvidenceType.Report)
		{
			SetCurrentSlot(num).SetData(evidence as EvidenceReport);
		}
	}

	public void ClearSlot(int num)
	{
		SetCurrentSlot(num).ClearData();
		_matchingController.ClearEvidence(num);
	}

	public void MakeConclusion()
	{
		if(_matchingController.TryToMakeConclusion())
		{
			_soundManager.PlaySuccessConclusionSound();
			_uiConclusion.ShowConclusionSuccess(1f);
		}
		else
		{
			_uiConclusion.ShowConclusionsAlertMessage();
		}
	}

	private EvidenceSlot SetCurrentSlot(int num)
	{
		if (num != 1 && num != 2)
		{
			Debug.LogError("Incorrect evidence slot number!");
			return null;
		}

		if (num == 1)
		{
			return _firstSlot;
		}
		else
		{
			return _secondSlot;
		}
	}
}