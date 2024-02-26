using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Evidence", menuName = "UTE/Evidence report")]
public class EvidenceReport : AEvidence
{
    [SerializeField] string _variable;
    [SerializeField] string _personName;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Sprite _personSprite;

    public Sprite Sprite
    {
        get { return _sprite; }
    }

    public Sprite PersonSprite
    {
        get { return _personSprite; }
    }

    public string PersonName
    {
        get { return _personName; }
    }
    //TODO: JSON 
}
