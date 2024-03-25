using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

[CreateAssetMenu(fileName = "MatchingConnect", menuName = "UTE/Matching Connect")]
public class EvidenceMatchingController : ScriptableObject
{
	public event UnityAction<int, AEvidence> PassEvidenceEvent = delegate { };
	private List<AEvidence> _activeEvidences = new List<AEvidence>();

	[SerializeField] private EvidenceMatchingModel _matching;
	[SerializeField] private Inventory _inventory;

	public void Initialize()
	{
		if (_matching == null || _inventory == null)
			Debug.LogError("EvidenceMatchingController properties are not assigned!");

		if (_activeEvidences.Count == 0)
		{
			_activeEvidences.Add(null);
			_activeEvidences.Add(null);
		}

		for (int i = 0; i < _activeEvidences.Count; i++)
		{
			if (_activeEvidences[i] != null)
			{
				PassEvidenceEvent.Invoke(i + 1, _activeEvidences[i]);
			}
		}
	}

	public void PassEvidence(AEvidence evidence)
	{
		for (int i = 0; i < _activeEvidences.Count; i++)
		{
			if (_activeEvidences[i] == evidence)
				return;

			else if (_activeEvidences[i] == null)
			{
				_activeEvidences[i] = evidence;
				PassEvidenceEvent.Invoke(i + 1, evidence);
				return;
			}
		}
	}

	public void ClearEvidence(int num)
	{
		if (_activeEvidences.Count == 0) 
			return;

		_activeEvidences[num - 1] = null;
	}

	public bool TryToMakeConclusion()
	{
		if (_activeEvidences[0] == null || _activeEvidences[1] == null)
			return false;

		EvidenceReport answer = _matching.FindMatch(_activeEvidences[0], _activeEvidences[1], out var variableName);

		if (answer == null || _inventory.HaveConclusion(answer))
		{
			return false;
		}

		if(variableName != string.Empty)
		{
			DialogueLua.SetVariable(variableName, true);
		}

		_inventory.AddConclusion(answer);

		return true;
	}
}