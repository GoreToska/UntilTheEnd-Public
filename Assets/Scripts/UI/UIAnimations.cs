using System.Collections;
using UnityEngine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class UIAnimations : MonoBehaviour
{
    [HideInInspector] public static UIAnimations instance;

    [SerializeField] private Canvas _dialogueCanvas;
    [SerializeField] private Canvas _endGame;
    [SerializeField] private Volume _blur;
    [SerializeField] private const float _fadeTime = 0f;

    //private Tween _tween;

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
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterLua()
    {
        Lua.RegisterFunction("StartDialogueFade", this, SymbolExtensions.GetMethodInfo(() => StartDialogueFade((double)0, (double)0)));
        Lua.RegisterFunction("ShowEndCanvas", this, SymbolExtensions.GetMethodInfo(() => ShowEndCanvas()));
        Lua.RegisterFunction("StartFadeToBlack", this, SymbolExtensions.GetMethodInfo(() => StartFadeToBlack((double)0, (double)0)));
    }

    public void ShowEndCanvas()
    {
        if (!_endGame)
            _endGame = GameObject.Find("EndGame").GetComponent<Canvas>();

        EndGameCanvas();
    }

    public void StartFadeToBlack(double value, double speed)
    {
        Debug.Log("Fade to black here");
        //StartCoroutine(FadeToBlack((float)value, (float)speed));
    }

    //private IEnumerator FadeToBlack(float value, float speed)
    //{
    //    _mainCanvas.enabled = true;

    //    var tween = _mainCanvas.GetComponent<Image>().DOFade(value, speed).WaitForCompletion();

    //    yield return tween;

    //    yield break;
    //}

    public void StartDialogueFade(double value, double speed)
    {
        if (!_dialogueCanvas)
            _dialogueCanvas = GameObject.Find("DialogueCanvas").GetComponent<Canvas>();

        FadeDialogue((float)value);
    }

    public void DialogueFadeIn()
    {
        FadeDialogue(1f);
    }

    public void DialogueFadeOut()
    {
        FadeDialogue(0);
    }

    public void FadeCanvas(float value, GameObject gObject)
    {
        var canvas = gObject.GetComponent<Canvas>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(gObject.GetComponent<RectTransform>());
        if (value == 1f)
        {
            gObject.SetActive(true);
            canvas.enabled = true;
            canvas.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            gObject.SetActive(false);
            canvas.enabled = false;
            canvas.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    public void FadeCanvasWithVolume(float value, GameObject gObject)
    {
        var canvas = gObject.GetComponent<Canvas>();

        if (value == 1f)
        {
            canvas.gameObject.SetActive(true);
            canvas.enabled = true;
            canvas.GetComponent<CanvasGroup>().alpha = 1;
            _blur.weight = 1f;
        }
        else
        {
            canvas.gameObject.SetActive(false);
            canvas.enabled = false;
            canvas.GetComponent<CanvasGroup>().alpha = 0;
            _blur.weight = 0f;
        }
    }

    public IEnumerator ShowReportAlert(GameObject gObject, float seconds)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(gObject.GetComponent<RectTransform>());
        var canvas = gObject.GetComponent<Canvas>();


        gObject.SetActive(true);
        canvas.enabled = true;
        gObject.GetComponent<CanvasGroup>().alpha = 1f;

        yield return new WaitForSeconds(seconds + 3f);

        gObject.GetComponent<CanvasGroup>().alpha = 0f;
        gObject.SetActive(false);
        canvas.enabled = false;

        yield break;
    }

    public void FadeLoadingCanvas(float value, GameObject gObject)
    {
        var canvas = gObject.GetComponent<Canvas>();

        if (value == 1f)
        {
            canvas.enabled = true;
        }

        if (value == 0f)
        {
            canvas.enabled = false;
        }
    }

    private void FadeDialogue(float value)
    {
        if (value == 1)
        {
            _dialogueCanvas.enabled = true;
            _dialogueCanvas.GetComponent<CanvasGroup>().alpha = 1f;
        }
        else
        {
            _dialogueCanvas.enabled = false;
            _dialogueCanvas.GetComponent<CanvasGroup>().alpha = 0f;

        }
    }

    private void EndGameCanvas()
    {
        _endGame.GetComponent<Canvas>().enabled = true;
        _endGame.GetComponent<CanvasGroup>().alpha = 1f;
    }
}
