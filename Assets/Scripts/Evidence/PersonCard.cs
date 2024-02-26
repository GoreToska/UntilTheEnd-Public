using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PersonCard", menuName = "UTE/People Card")]
public class PersonCard : ScriptableObject
{
    [SerializeField] private string _personName;
    [SerializeField] private Sprite _personSprite;
}
