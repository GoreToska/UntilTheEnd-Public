using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
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

        public AEvidence EvidenceOne
        {
            get { return _evidenceOne; }
        }

        public AEvidence EvidenceTwo
        {
            get { return _evidenceTwo; }
        }

        public EvidenceReport Report
        {
            get 
            { 
                return _report; 
            }
        }

        public string VariableName
        {
            get { return _variableName; }
        }
    }

    [SerializeField] private List<EvidenceCombination> _evidenceCombinations;
    [SerializeField] private Inventory _inventory = default;

    private void OnEnable()
    {
        foreach(EvidenceCombination combination in _evidenceCombinations)
        {
            if (combination.Report == null || combination.EvidenceOne == null 
                || combination.EvidenceTwo == null)
                Debug.LogError("Preset evidence combinations must be filled!");
        }
    }

    public EvidenceReport FindMatch(AEvidence inEvidenceOne, AEvidence inEvidenceTwo)
    {
        EvidenceCombination inEvidenceComb = new EvidenceCombination(inEvidenceOne, inEvidenceTwo);

        foreach (EvidenceCombination combination in _evidenceCombinations)
        {
            if (CompareEvidenceCombinations(inEvidenceComb, combination))
            {
                if (_inventory.HasConclusion(combination.Report))
                    return null;

                DialogueLua.SetVariable(combination.VariableName, true);
                return combination.Report;
            }
        }
        
        return null;
    }

    private bool CompareEvidenceCombinations(EvidenceCombination combOne, EvidenceCombination combTwo)
    {
        if (combOne.EvidenceOne == combTwo.EvidenceOne && combOne.EvidenceTwo == combTwo.EvidenceTwo)
            return true;
        else if (combOne.EvidenceOne == combTwo.EvidenceTwo && combOne.EvidenceTwo == combTwo.EvidenceOne)
            return true;
        else
            return false;
    }
}
