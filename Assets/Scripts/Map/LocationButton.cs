using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LocationButton : MonoBehaviour
{
    [Header("По умолчанию ставить картинку выключенной локации")]
    [SerializeField] private Sprite _enabledImage;
    [SerializeField] private Sprite _currentImage;
    [SerializeField] private bool _enabledOnStart = false;
    [SerializeField] private string _name;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();

        if (!_enabledOnStart)
        {
            DisableLocation();
            return;
        }

        if (UTESceneManager.CurrentScene == _name)
        {
            SetCurrent();
        }
        else
        {
            UnsetCurrent();
        }
    }

    public void SetCurrent()
    {
        _button.image.sprite = _currentImage;
        RemoveListeners();
    }

    public void RemoveListeners()
    {
        _button.onClick.RemoveListener(() => MapManager.instance.CloseMap());
        _button.onClick.RemoveListener(() => MapManager.instance.DisableFastTravel());
    }

    public void AddListeners()
    {
        _button.onClick.AddListener(() => MapManager.instance.CloseMap());
        _button.onClick.AddListener(() => MapManager.instance.DisableFastTravel());
    }

    public void UnsetCurrent()
    {
        EnableLocation();
        _button.image.sprite = _enabledImage;

        AddListeners();
    }

    private void EnableFastTravel()
    {
        _button.interactable = true;
    }

    private void DisableFastTravel()
    {
        _button.interactable = false;
    }

    public void DisableLocation()
    {
        _button.transform.gameObject.SetActive(false);
        //MapManager.instance.EnableFastTravels -= EnableFastTravel;
        //MapManager.instance.DisableFastTravels -= DisableFastTravel;
    }

    public void EnableLocation()
    {
        MapManager.instance.EnableFastTravels += EnableFastTravel;
        MapManager.instance.DisableFastTravels += DisableFastTravel;

        AddListeners();

        _button.transform.gameObject.SetActive(true);
        DisableFastTravel();
    }

    public bool EnabledOnStart
    {
        get { return _enabledOnStart; }
        set { _enabledOnStart = value; }
    }

    public string Name
    {
        get { return _name; }
    }

    public Button Button
    {
        get { return Button; }
    }
}
