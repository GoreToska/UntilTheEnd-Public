using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Inventory", menuName = "UTE/Inventory")]
public class Inventory : ScriptableObject
{
    public event UnityAction<LogsInfo> ChangeLogEvent = delegate { };
    public event UnityAction<EvidenceItem, bool> AddEvidenceEvent = delegate { };
    public event UnityAction<EvidenceReport, bool> AddReportEvent = delegate { };
    public event UnityAction<EvidenceReport, bool> AddConclusionEvent = delegate { };

    [SerializeField] private List<EvidenceItem> _evidenceItems;
    [SerializeField] private List<LogsInfo> _logsInfos;
    [SerializeField] private List<EvidenceReport> _evidenceReports;
    [SerializeField] private List<EvidenceReport> _conclusions;

    public void RegisterLua()
    {
        Lua.RegisterFunction("HaveEvidence", this, SymbolExtensions.GetMethodInfo(() => HaveEvidence(string.Empty)));
    }

    public void UnregisterLua()
    {
        Lua.UnregisterFunction("HaveEvidence");
    }

    public void AddEvidence(AEvidence evidence)
    {
        if (evidence == null)
            return;

        switch (evidence.EvidenceType)
        {
            case EEvidenceType.Item:
                EvidenceItem item = evidence as EvidenceItem;
                if (CheckEvidenceItem(item))
                    return;

                _evidenceItems.Add(item);
                AddEvidenceEvent.Invoke(item, false);
                AddLog(evidence._logsInfo);
                break;

            case EEvidenceType.Report:
                EvidenceReport report = evidence as EvidenceReport;
                if (CheckEvidenceReport(report))
                    return;

                _evidenceReports.Add(report);
                AddReportEvent.Invoke(report, false);
                AddLog(evidence._logsInfo);
                break;
        }
    }

    public void AddLog(LogsInfo logsInfo)
    {
        if (logsInfo == null)
            return;

        _logsInfos.Add(logsInfo);
        ChangeLogEvent.Invoke(logsInfo);
    }

    public void AddConclusion(EvidenceReport inConclusion)
    {
        if (inConclusion == null)
            return;

        _conclusions.Add(inConclusion);
        AddLog(inConclusion._logsInfo);
        AddConclusionEvent.Invoke(inConclusion, false);
    }

    private bool CheckEvidenceReport(EvidenceReport report)
    {
        return _evidenceReports.Contains(report);
    }

    private bool CheckEvidenceItem(EvidenceItem item)
    {
        return _evidenceItems.Contains(item);
    }

    public void ClearInventory()
    {
        _evidenceItems.Clear();
        _logsInfos.Clear();
        _evidenceReports.Clear();
        _conclusions.Clear();
    }

    public bool HasConclusion(EvidenceReport report)
    {
        return _conclusions.Contains(report);
    }

    public List<LogsInfo> LogsInfos
    {
        get { return _logsInfos; }
    }

    public List<EvidenceItem> EvidenceInfos
    {
        get { return _evidenceItems; }
    }

    public List<EvidenceReport> ReportsInfos
    {
        get { return _evidenceReports; }
    }

    public List<EvidenceReport> ConclusionsInfos
    {
        get { return _conclusions; }
    }

    #region Lua

    private bool HaveEvidence(string evidenceItemName)
    {
        foreach (var evidence in _evidenceItems)
        {
            if (evidence.Name == evidenceItemName)
                return true;
        }
        return false;
    }

    #endregion
}
