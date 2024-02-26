using UnityEngine;
using TMPro;
using PixelCrushers.DialogueSystem;

public class UIManager : MonoBehaviour
{
    [HideInInspector] public static UIManager instance;

    [Header("Journal pages")]
    [SerializeField] private Canvas _journalCanvas;
    [SerializeField] private Canvas _mapPageCanvas;
    [SerializeField] private Canvas _casePageCanvas;
    [SerializeField] private Canvas _questsPageCanvas;

    [Header("Quest page")]
    [SerializeField] private Canvas _activeQuestInfoPage;
    [SerializeField] private Canvas _doneQuestInfoPage;

    [Header("Skills")]
    [SerializeField] private Canvas _startSkillsCanvas;

    [Header("Guide Canvases")]
    [SerializeField] private Canvas _startGuideCanvas;

    [Header("Evidence Canvas")]
    [SerializeField] private Canvas _evidenceCanvas;

    [Header("Main Canvas")]
    [SerializeField] private Canvas _mainCanvas;

    [Header("Inspection Canvas")]
    [SerializeField] private Canvas _inspectionPageCanvas;
    [SerializeField] private TMP_Text _inspectionDescription;

    [Header("Other Canvases")]
    [SerializeField] private Canvas _loadingCanvas;
    [SerializeField] private Canvas _menuCanvas;

    [Header("Input reader")]
    [SerializeField] private InputReader _inputReader = default;

    private Canvas _currentCanvas;

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

        _journalCanvas.enabled = false;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _currentCanvas = _casePageCanvas;
        DisableAllWindows();
        OnCloseJournal();
    }

    private void OnEnable()
    {
        _inputReader.OpenJournal += OnOpenJournal;
        _inputReader.ExitEvent += OnCloseJournal;
        //_inputReader.OpenMenuEvent += OnOpenMenu;
        //_inputReader.CloseMenuEvent += OnCloseMenu;

        PixelCrushers.SaveSystem.loadStarted += StartLoading;
        PixelCrushers.SaveSystem.loadEnded += EndLoading;
    }

    private void OnDisable()
    {
        _inputReader.OpenJournal -= OnOpenJournal;
        _inputReader.ExitEvent -= OnCloseJournal;
        //_inputReader.OpenMenuEvent -= OnOpenMenu;
        //_inputReader.CloseMenuEvent -= OnCloseMenu;

        PixelCrushers.SaveSystem.loadStarted -= StartLoading;
        PixelCrushers.SaveSystem.loadEnded -= EndLoading;
    }

    public void RegisterLua()
    {
        Lua.RegisterFunction("EndDialogue", this, SymbolExtensions.GetMethodInfo(() => EndDialogue()));
    }

    #region Game UI

    public void ShowMainCanvas()
    {
        UIAnimations.instance.FadeCanvas(1f, _mainCanvas.gameObject);
    }

    public void HideMainCanvas()
    {
        UIAnimations.instance.FadeCanvas(0f, _mainCanvas.gameObject);
    }

    public void EnableInspectText(bool value)
    {
        Debug.Log("Inspection here");
        //_inspectText.enabled = value;
    }

    #endregion

    #region Journal

    private void DisableAllWindows()
    {
        FadeOutAllWindows(1);
    }

    public void ShowInspectionWindow(string description, float voiceLenght)
    {
        _inspectionDescription.text = description;
        StartCoroutine(UIAnimations.instance.ShowReportAlert(_inspectionPageCanvas.gameObject, voiceLenght));
    }

    public void OnQuestsOpen()
    {
        FadeOutAllWindows(3);

        UIAnimations.instance.FadeCanvas(1f, _questsPageCanvas.gameObject);
        _currentCanvas = _questsPageCanvas;
    }

    public void OnActiveQuestOpen()
    {
        UIAnimations.instance.FadeCanvas(1f, _activeQuestInfoPage.gameObject);
        UIAnimations.instance.FadeCanvas(0f, _doneQuestInfoPage.gameObject);
    }

    public void OnDoneQuestOpen()
    {
        UIAnimations.instance.FadeCanvas(0f, _activeQuestInfoPage.gameObject);
        UIAnimations.instance.FadeCanvas(1f, _doneQuestInfoPage.gameObject);
    }

    public void OnEvidenceOpen()
    {
        UIAnimations.instance.FadeCanvas(1f, _evidenceCanvas.gameObject);
    }

    public void OnEvidenceClose()
    {
        UIAnimations.instance.FadeCanvas(0f, _evidenceCanvas.gameObject);
    }

    public void OnCaseOpen()
    {
        FadeOutAllWindows(1);

        UIAnimations.instance.FadeCanvas(1f, _casePageCanvas.gameObject);
        _currentCanvas = _casePageCanvas;
    }

    public void OnMapOpen()
    {
        FadeOutAllWindows(2);

        UIAnimations.instance.FadeCanvas(1f, _mapPageCanvas.gameObject);
        _currentCanvas = _casePageCanvas;

        //_currentCanvas = _mapPageCanvas;

        //if (!_journalCanvas.GetComponent<Canvas>().enabled)
        //{
        //    _journalCanvas.GetComponent<Canvas>().enabled = true;
        //    _journalCanvas.GetComponent<CanvasGroup>().alpha = 1f;
        //}

        //_inputReader.SwitchToJournalControls();
        ////FadeOutAllWindows(2);

        //OnOpenJournal();

        //_mapPageCanvas.enabled = true;
        //Debug.Log(_mapPageCanvas.enabled);
        ////_mapPageCanvas.GetComponent<CanvasGroup>().alpha = 1f;
    }

    public void SetMapCurrentPage()
    {
        _currentCanvas = _mapPageCanvas;
    }

    public void DisableStartSkillsCanvas()
    {
        UIAnimations.instance.FadeCanvas(0f, _startSkillsCanvas.gameObject);
    }

    public void EnableStartSkillsCanvas()
    {
        UIAnimations.instance.FadeCanvas(1f, _startSkillsCanvas.gameObject);
    }

    public void DisableGuideCanvas()
    {
        UIAnimations.instance.FadeCanvas(0f, _startGuideCanvas.gameObject);
    }

    public void OnOpenJournal()
    {
        UIAnimations.instance.FadeCanvasWithVolume(1f, _journalCanvas.gameObject);
        UIAnimations.instance.FadeCanvas(0f, _mainCanvas.gameObject);
        UIAnimations.instance.FadeCanvas(1f, _currentCanvas.gameObject);
        _inputReader.SwitchToJournalControls();
    }

    public void OnCloseJournal()
    {
        UIAnimations.instance.FadeCanvasWithVolume(0f, _journalCanvas.gameObject);
        UIAnimations.instance.FadeCanvas(1f, _mainCanvas.gameObject);
        UIAnimations.instance.FadeCanvas(0f, _currentCanvas.gameObject);
        _inputReader.SwitchToGameControls();
    }

    private void FadeOutAllWindows(int n)
    {
        //0 - all; 1 - journal; 2 - map; 3 - logs; 4 - options 
        switch (n)
        {
            case 0:
                UIAnimations.instance.FadeCanvas(0, _mapPageCanvas.gameObject);
                UIAnimations.instance.FadeCanvas(0, _questsPageCanvas.gameObject);
                //StartCoroutine(FadeSingleCanvas(0, _optionsPageObject));
                UIAnimations.instance.FadeCanvas(0, _casePageCanvas.gameObject);
                break;
            case 1:
                UIAnimations.instance.FadeCanvas(0, _mapPageCanvas.gameObject);
                UIAnimations.instance.FadeCanvas(0, _questsPageCanvas.gameObject);
                //StartCoroutine(FadeSingleCanvas(0, _optionsPageObject));
                break;
            case 2:
                UIAnimations.instance.FadeCanvas(0, _casePageCanvas.gameObject);
                UIAnimations.instance.FadeCanvas(0, _questsPageCanvas.gameObject);
                //StartCoroutine(FadeSingleCanvas(0, _optionsPageObject));
                break;
            case 3:
                UIAnimations.instance.FadeCanvas(0, _casePageCanvas.gameObject);
                UIAnimations.instance.FadeCanvas(0, _mapPageCanvas.gameObject);
                //StartCoroutine(FadeSingleCanvas(0, _optionsPageObject));
                break;
            case 4:
                UIAnimations.instance.FadeCanvas(0, _casePageCanvas.gameObject);
                UIAnimations.instance.FadeCanvas(0, _mapPageCanvas.gameObject);
                UIAnimations.instance.FadeCanvas(0, _questsPageCanvas.gameObject);
                break;

        }
    }

    #endregion

    #region Loading

    public void StartLoading()
    {
        _loadingCanvas.GetComponent<Canvas>().enabled = true;
        _loadingCanvas.GetComponent<CanvasGroup>().alpha = 1f;
    }

    public void EndLoading()
    {
        UIAnimations.instance.FadeLoadingCanvas(0f, _loadingCanvas.gameObject);
    }

    #endregion

    #region Menu

    public void OnCloseMenu()
    {
        UIAnimations.instance.FadeCanvas(0f, _menuCanvas.gameObject);
        _inputReader.SwitchToGameControls();
    }

    public void OnOpenMenu()
    {
        UIAnimations.instance.FadeCanvas(1f, _menuCanvas.gameObject);
        _inputReader.SwitchToMainMenuControls();
    }

    #endregion

    //  FIND OTHER PLACE FOR THIS
    public void EndDialogue()
    {
        PixelCrushers.DialogueSystem.DialogueManager.StopAllConversations();
    }
}