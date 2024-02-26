using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//  Õ≈ »—œŒÀ‹«”≈“—ﬂ
[CreateAssetMenu(fileName = "EvidenceIDManager", menuName = "UTE/Evidence ID Manager")]
public class EvidenceIDManager : ScriptableObject
{
    [SerializeField] private List<AEvidence> _evidences;
    public AEvidence SearchEvidence(int ID)
    {
        if(ID > _evidences.Count || ID < 1)
        {
            Debug.LogWarning("ID of evidence is incorrect");
            return null;
        }

        int min = 0;
        int max = _evidences.Count;
        while(min <= max)
        {
            int mid = Mathf.RoundToInt((min + max) / 2);

            if (ID == _evidences[mid].ID)
            {
                return _evidences[mid];
            }
            else if (ID < _evidences[mid].ID)
            {
                max = mid - 1;
            }
            else
            {
                min = mid + 1;
            }
        }

        return null;
    }

    // dev
    [ContextMenu("Sort Evidences")]
    public void SortEvidencesByID()
    {
        var sortedList = from evidence in _evidences
                         orderby evidence.ID
                         select evidence;

        _evidences = sortedList.ToList();
    }
}