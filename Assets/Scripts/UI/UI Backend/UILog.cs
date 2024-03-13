using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILog : MonoBehaviour
{
    [SerializeField] private GameObject _logExample;
    [SerializeField] private GameObject _logsHelp;
    [SerializeField] private Canvas _logsPannel;
    [SerializeField] private Inventory _inventory;

    private void Awake()
    {
        //LogsInit();
        //_inventory.ChangeLogEvent += AddLog;
    }

    //перепроектировать позже с учетом системы
    public void AddLog(LogsInfo logsInfo)
    {
        if (_inventory.LogsInfos.Count > 0)
        {
            _logsHelp.SetActive(false);
        }

        var log = Instantiate(_logExample, _logsPannel.transform.GetChild(1).transform);
        log.GetComponentInChildren<TMP_Text>().text = logsInfo.Text;
    }

    private void LogsInit()
    {
        List<LogsInfo> _logsInfos = _inventory.LogsInfos;

        foreach (LogsInfo logInfo in _logsInfos)
        {
            AddLog(logInfo);
        }
    }

    public Canvas Panel
    {
        get { return _logsPannel; }
    }
}
