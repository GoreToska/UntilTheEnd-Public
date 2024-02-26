using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public enum ButtonFunctions
{
    OnCaseOpen,
    OnQuestsOpen,
    OnMapOpen,
    OnOptionsOpen
}

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private float _fadeAnimationSpeed;
    [SerializeField] private UIManager _manager;
    [SerializeField] private ButtonFunctions _buttonFunction;

    private Color _redColor = new(255, 102, 102);
    private Color _defaultColor = new(255, 255, 255);
    private Image _highlightImage;
    private TMP_Text _buttonLabel;
    private Tween _buttonTween;
    private Tween _textTween;
    private Toggle _buttonToggle;

    private void Awake()
    {
        _highlightImage = GetComponentInChildren<Image>();
        _buttonLabel = GetComponentInChildren<TMP_Text>();
        _buttonToggle = GetComponent<Toggle>();

        if (_fadeAnimationSpeed == 0)
            _fadeAnimationSpeed = 0.2f;

        _buttonToggle.onValueChanged.AddListener(delegate
        {
            ListenerEvent();
        });

    }

    private void ListenerEvent()
    {
        if (_buttonToggle.isOn)
        {
            SetHighlight(1);
            UIManager.instance.Invoke(_buttonFunction.ToString(), 0);
        }
        else
        {
            SetHighlight(0);
        }
    }

    private void SetHighlight(float value)
    {
        _buttonTween.Kill();
        _buttonTween = _highlightImage.DOFade(value, _fadeAnimationSpeed);

        if (value == 1)
        {
            _textTween.Kill();
            _textTween = _buttonLabel.DOColor(_redColor, _fadeAnimationSpeed);
        }
        else
        {
            _textTween.Kill();
            _textTween = _buttonLabel.DOColor(_defaultColor, _fadeAnimationSpeed);
        }
    }
}
