using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [Tooltip("Object in case menu of journal, that contains quest groups.")]
    [SerializeField] private GameObject _activeQuestGroup;
    [SerializeField] private GameObject _doneQuestGroup;
    [SerializeField] private Canvas _activeQuestCanvas;
    [SerializeField] private Canvas _doneQuestCanvas;

    [Tooltip("Object in case menu of journal, that should contain information about quest.")]
    [SerializeField] private GameObject _questInfoPrefab;
    [SerializeField] private TMP_Text _questInfoTextPrefab;

    //[SerializeField] private QuestsInfos _questsInfos;

    //[Header("Active quest prefabs")]
    //[SerializeField] private GameObject _activeQuestTogglePrefab;
    //[SerializeField] private GameObject _activeQuestInfoPrefab;

    //[Header("Succsess quest prefabs")]
    //[SerializeField] private GameObject _succsessQuestTogglePrefab;
    //[SerializeField] private GameObject _succsessQuestInfoPrefab;

    //[Header("Failure quest prefabs")]
    //[SerializeField] private GameObject _failureQuestTogglePrefab;
    //[SerializeField] private GameObject _failureQuestInfoPrefab;

    //[Header("Abandoned quest prefabs")]
    //[SerializeField] private GameObject _abandonedQuestTogglePrefab;
    //[SerializeField] private GameObject _abandonedQuestInfoPrefab;

    //[Header("Grantable quest prefabs")]
    //[SerializeField] private GameObject _grantableQuestTogglePrefab;
    //[SerializeField] private GameObject _grantableQuestInfoPrefab;

    //[Header("ReturnToNPC quest prefabs")]
    //[SerializeField] private GameObject _returnToNPCQuestTogglePrefab;
    //[SerializeField] private GameObject _returnToNPCQuestInfoPrefab;

    [Header("Quest states strings (you can check it in QuestState).")]
    [SerializeField] private const string _unassigned = "Unassigned";
    [SerializeField] private const string _active = "Active";
    [SerializeField] private const string _success = "Success";
    [SerializeField] private const string _failure = "Failure";
    [SerializeField] private const string _abandoned = "Abandoned";
    [SerializeField] private const string _grantable = "Grantable";
    [SerializeField] private const string _returnToNPC = "ReturnToNPC";

    private static QuestManager _instance = null;

    private void Awake()
    {
        Lua.RegisterFunction("UpdateEntryInfo", this, SymbolExtensions.GetMethodInfo(() => UpdateEntryInfo("", (int)0)));

        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        foreach (var quest in QuestLog.GetAllQuests(true, null))
        {
            UpdateQuest(quest, QuestLog.GetQuestState(quest));
            QuestLog.AddQuestStateObserver(quest, LuaWatchFrequency.EndOfConversation, UpdateQuest);
            //Debug.Log("Update");
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_activeQuestGroup.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(_doneQuestGroup.GetComponent<RectTransform>());
    }

    private void OnEnable()
    {
        foreach (var quest in QuestLog.GetAllQuests(true, null))
        {
            UpdateQuest(quest, QuestLog.GetQuestState(quest));
            QuestLog.AddQuestStateObserver(quest, LuaWatchFrequency.EndOfConversation, UpdateQuest);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_activeQuestGroup.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(_doneQuestGroup.GetComponent<RectTransform>());
    }

    private void OnDisable()
    {
        //QuestLog.RemoveAllQuestStateObservers();
    }

    private void UpdateQuest(string name, QuestState state)
    {
        switch (state)
        {
            case QuestState.Unassigned:
                break;
            case QuestState.Active:
                UpdateToActive(name);
                break;
            case QuestState.Success:
                UpdateToSuccess(name);
                break;
            case QuestState.Failure:
                UpdateToFailure(name);
                break;
            case QuestState.Abandoned:
                break;
            case QuestState.Grantable:
                break;
            case QuestState.ReturnToNPC:
                break;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_activeQuestGroup.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(_doneQuestGroup.GetComponent<RectTransform>());
    }

    #region Update UIs
    private void UpdateToActive(string name)
    {
        Debug.Log("Adding quest");

        if (_activeQuestGroup.transform.Find(name))
            return;

        var quest = Instantiate(_questInfoPrefab, _activeQuestGroup.transform);
        quest.name = name;
        quest.transform.GetChild(0).GetComponent<TMP_Text>().text = QuestLog.GetQuestTitle(name);

        var description = Instantiate(_questInfoTextPrefab, quest.transform.GetChild(2));
        description.text = QuestLog.GetQuestDescription(name);
        description.name = "Description";

        var entry1 = Instantiate(_questInfoTextPrefab, quest.transform.GetChild(2));
        entry1.text = QuestLog.GetQuestEntry(name, 1);
        entry1.name = "Entry";

        Debug.Log(quest);

    }

    private void UpdateEntryInfo(string name, int number)
    {
        var quest = _activeQuestGroup.transform.Find(name);
        var entry = Instantiate(_questInfoTextPrefab, quest.transform.GetChild(2));
        entry.name = "Entry";
        entry.text = QuestLog.GetQuestEntry(name, number);
    }

    private void UpdateToSuccess(string name)
    {
        if (_doneQuestGroup.transform.Find(name))
            return;

        if (_activeQuestGroup.transform.Find(name))
        {
            Destroy(_activeQuestGroup.transform.Find(name).gameObject);
        }

        var quest = Instantiate(_questInfoPrefab, _doneQuestGroup.transform);
        quest.name = name;
        quest.transform.GetChild(0).GetComponent<TMP_Text>().text = QuestLog.GetQuestTitle(name);

        var description = Instantiate(_questInfoTextPrefab, quest.transform.GetChild(2));
        description.text = QuestLog.GetQuestDescription(name, QuestState.Success);
        description.name = "Description";
    }

    private void UpdateToFailure(string name)
    {
        if (_doneQuestGroup.transform.Find(name))
            return;

        if (_activeQuestGroup.transform.Find(name))
        {
            Destroy(_activeQuestGroup.transform.Find(name));
        }

        var quest = Instantiate(_questInfoPrefab, _doneQuestGroup.transform);
        quest.name = name;
        quest.transform.GetChild(0).GetComponent<TMP_Text>().text = QuestLog.GetQuestTitle(name);

        var description = Instantiate(_questInfoTextPrefab, quest.transform.GetChild(2));
        description.text = QuestLog.GetQuestDescription(name, QuestState.Failure);
        description.name = "Description";
    }

    private void UpdateToAbandoned(string name)
    {

    }

    private void UpdateToGrantable(string name)
    {

    }

    private void UpdateToReturnToNPC(string name)
    {

    }

    #endregion
}
