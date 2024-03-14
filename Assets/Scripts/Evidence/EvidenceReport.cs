using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Evidence", menuName = "UTE/Evidence report")]
public class EvidenceReport : AEvidence
{
    [field: SerializeField] public string Variable { get; private set; }
    [field: SerializeField] public string PersonName { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Sprite PersonSprite { get; private set; }
}
