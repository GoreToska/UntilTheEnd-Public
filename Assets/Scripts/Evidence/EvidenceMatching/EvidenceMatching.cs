using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

[CreateAssetMenu(fileName = "EvidenceMatching", menuName = "UTE/Evidence Matching")]
public class EvidenceMatching : ScriptableObject
{
	[System.Serializable]
	private struct EvidenceCombination
	{
		// TODO: custom == operator
		[SerializeField] private AEvidence _evidenceOne;
		[SerializeField] private AEvidence _evidenceTwo;
		[SerializeField] private EvidenceReport _report;
		[SerializeField] private string _variableName;

		public EvidenceCombination(AEvidence evOne, AEvidence evTwo)
		{
			_evidenceOne = evOne;
			_evidenceTwo = evTwo;
			_report = null;
			_variableName = null;
		}

		public readonly AEvidence EvidenceOne => _evidenceOne;
		public readonly AEvidence EvidenceTwo => _evidenceTwo;
		public readonly EvidenceReport Report => _report;
		public readonly string VariableName => _variableName;

		public static bool operator ==(EvidenceCombination combination1, EvidenceCombination combination2)
		{
			if (combination1.EvidenceOne == combination2.EvidenceOne && combination1.EvidenceTwo == combination2.EvidenceTwo)
				return true;
			else if (combination1.EvidenceOne == combination2.EvidenceTwo && combination1.EvidenceTwo == combination2.EvidenceOne)
				return true;
			else
				return false;
		}

		public static bool operator !=(EvidenceCombination combination1, EvidenceCombination combination2)
		{
			if (combination1.EvidenceOne == combination2.EvidenceOne && combination1.EvidenceTwo == combination2.EvidenceTwo)
				return false;
			else if (combination1.EvidenceOne == combination2.EvidenceTwo && combination1.EvidenceTwo == combination2.EvidenceOne)
				return false;
			else
				return true;
		}
	}

	[SerializeField] private List<EvidenceCombination> _evidenceCombinations;
	[SerializeField] private Inventory _inventory = default;

	private void OnEnable()
	{
		foreach (EvidenceCombination combination in _evidenceCombinations)
		{
			if (combination.Report == null || combination.EvidenceOne == null
				|| combination.EvidenceTwo == null)
				Debug.LogError("Preset evidence combinations must be filled!");
		}
	}

	public EvidenceReport FindMatch(AEvidence firstEvidence, AEvidence secondEvidence)
	{
		EvidenceCombination inEvidenceComb = new EvidenceCombination(firstEvidence, secondEvidence);

		foreach (EvidenceCombination combination in _evidenceCombinations)
		{
			if (inEvidenceComb == combination)
			{
				if (_inventory.HasConclusion(combination.Report))
					return null;

				if(combination.VariableName != string.Empty)
				{
					DialogueLua.SetVariable(combination.VariableName, true);
				}

				return combination.Report;
			}
		}

		return null;
	}
}
