using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LogsInfo", menuName = "UTE/LogsInfo")]
public class LogsInfo : ScriptableObject
{
    [SerializeField] string _text;

    //TODO: do something
    public string Text
    {
        get { return _text; }
    }
}
