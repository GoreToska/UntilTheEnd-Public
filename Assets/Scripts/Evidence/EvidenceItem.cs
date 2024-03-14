using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Evidence", menuName = "UTE/Evidence item")]
public class EvidenceItem : AEvidence
{
	[field: SerializeField] public Sprite EvidenceIcon { get; protected set; }
	[field: SerializeField] public Vector3 SpawnPositionOffset { get; protected set; }
	[field: SerializeField] public Vector3 SpawnRotationOffset { get; protected set; }
	[field: SerializeField] public float MaxDistance { get; protected set; }
	[field: SerializeField] public float MinDistance { get; protected set; }
}
