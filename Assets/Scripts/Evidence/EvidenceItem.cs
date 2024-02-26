using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Evidence", menuName = "UTE/Evidence item")]
public class EvidenceItem : AEvidence
{
    [SerializeField] private Sprite _sprite;

    public Sprite GetSprite
    {
        get { return _sprite; }
    }
}
