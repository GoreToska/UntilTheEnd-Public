using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIConclusion : MonoBehaviour
{
    [HideInInspector] public static UIConclusion instance;

    [SerializeField] private GameObject _conclusionHelp;
    [SerializeField] private GameObject _conclusionsPanel;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private EvMatchConnectUI _matchingConnect;
    [SerializeField] private TMP_Text _conclusionsAllertText;
    [SerializeField] private Canvas _conclusionSuccessCanvas;
    [SerializeField] private TMP_Text _conclusionsMessageText;
    [SerializeField] private GameObject _reportExample;

    private float _fadeTime = 0.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ConclusionInit();

        //_conclusionsAllertText.enabled = false;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        _inventory.AddConclusionEvent += AddConclusion;
    }

    private void OnDisable()
    {
        _inventory.AddConclusionEvent -= AddConclusion;
    }

    //перепроектировать позже с учетом системы
    public void AddConclusion(EvidenceReport inConclusion, bool disableHint)
    {
        if (_inventory.ConclusionsInfos.Count == 0)
        {
            _conclusionHelp.SetActive(false);
        }

        _conclusionsMessageText.text = inConclusion.Description;

        //Перепроектировать после финала
        //var NewConclusion = Instantiate(_reportExample, _conclusionsPanel.transform.GetChild(1).transform);
        //NewConclusion.GetComponentInChildren<TMP_Text>().text = inConclusion.ReportText.ToString();

        //Add Hint disable
    }

    private void ConclusionInit()
    {
        List<EvidenceReport> conclusions = _inventory.ConclusionsInfos;

        foreach (var conclusion in conclusions)
        {
            AddConclusion(conclusion, true);
        }
    }

    private IEnumerator ConclusionsAlertMessage()
    {
        _conclusionsAllertText.text = "Похоже, улики несовместимы.";
        _conclusionsAllertText.enabled = true;

        _conclusionsAllertText.DOFade(1, _fadeTime);
        _conclusionsAllertText.transform.DOShakePosition(1, 3);

        yield return new WaitForSeconds(3);

        yield return _conclusionsAllertText.DOFade(0, _fadeTime).WaitForCompletion();

        _conclusionsAllertText.enabled = false;

        yield break;
    }

    private IEnumerator ConclusionSuccess(float value)
    {
        if (value == 1f)
        {
            _conclusionSuccessCanvas.enabled = true;
        }

        var tween = _conclusionSuccessCanvas.GetComponent<CanvasGroup>().DOFade(value, _fadeTime).WaitForCompletion();

        yield return tween;

        if (value == 0f)
        {
            _conclusionSuccessCanvas.enabled = false;
        }

        yield break;
    }

    public void ShowConclusionsAlertMessage()
    {
        StartCoroutine(ConclusionsAlertMessage());
    }

    public void ShowConclusionSuccess(float value)
    {
        StartCoroutine(ConclusionSuccess(value));
    }

    //public Canvas Panel
    //{
    //    get { return _conclusionsPanel; }
    //}
}
