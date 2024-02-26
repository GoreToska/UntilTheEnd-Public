using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quests Infos", menuName = "UTE/Quests Infos")]
public class QuestsInfos : ScriptableObject
{
    [SerializeField] private List<GameObject> _activeQuests;
    [SerializeField] private List<GameObject> _doneQuests;

    public void AddActiveQuest(GameObject quest)
    {
        _activeQuests.Add(quest);
    }

    public void AddDoneQuest(GameObject quest)
    {
        _doneQuests.Add(quest);
    }

    public List<GameObject> ActiveQuests
    {
        get { return _activeQuests; }
    }

    public List<GameObject> DoneQuests
    {
        get { return _doneQuests; }
    }
}
