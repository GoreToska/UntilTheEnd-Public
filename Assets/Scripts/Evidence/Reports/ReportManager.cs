using DG.Tweening;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ReportManager : MonoBehaviour
{
    [SerializeField] private List<EvidenceReport> _listOfReports;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GameObject _alert;
    [SerializeField] private float _fadeTime = 0.2f;
    [SerializeField] private float _showTime = 5f;

    [Inject] private UISoundManager _soundManager;

	private Sequence _sequence;

	public void RegisterLua()
    {
        Lua.RegisterFunction("AddReport", this, SymbolExtensions.GetMethodInfo(() => AddReport(string.Empty)));
    }

    public void UnregisterLua()
    {
        Lua.UnregisterFunction("AddReport");
    }

    private void AddReport(string reportName)
    {
        Debug.Log("Add report");
        if (!_alert)
            _alert = GameObject.Find("AlertPanel");

        StartCoroutine(ShowAlert(_alert, _showTime));
        _inventory.AddEvidence(FindReportWithName(reportName));
		_soundManager.PlaySuccessConclusionSound();
    }

    private EvidenceReport FindReportWithName(string name)
    {
        Debug.Log("Find report");

        foreach (EvidenceReport report in _listOfReports)
        {
            if (report.Name == name)
                return report;
        }
        return null;
    }

    private IEnumerator ShowAlert(GameObject alert, float seconds)
    {
        Debug.Log("Show alert");

        _sequence.Kill();

        _sequence.Append(alert.GetComponent<CanvasGroup>().DOFade(1f, _fadeTime));

        yield return new WaitForSeconds(seconds);

        _sequence.Kill();
        _sequence.Append(alert.GetComponent<CanvasGroup>().DOFade(0, _fadeTime));

        yield break;

    }
}
