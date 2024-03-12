using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LocationButton : MonoBehaviour
{
    [Header("По умолчанию ставить картинку выключенной локации")]
    [SerializeField] private Sprite _enabledImage;
    [SerializeField] private Sprite _currentImage;
    [SerializeField] private bool _enabledOnStart = false;
    [SerializeField] private string _name;

    private Button _button;

    [Inject] private MapManager _mapManager;

	private void Awake()
    {
        _button = GetComponent<Button>();

        if (UTESceneManager.CurrentScene == _name)
        {
            SetCurrent();
        }
        else
        {
            UnsetCurrent();
        }

		if (!_enabledOnStart)
		{
			DisableLocation();
			return;
		}
	}

    public void SetCurrent()
    {
        _button.image.sprite = _currentImage;
        RemoveListeners();
    }

    public void RemoveListeners()
    {
        _button.onClick.RemoveListener(() => _mapManager.CloseMap());
        _button.onClick.RemoveListener(() => _mapManager.DisableFastTravel());
    }

    public void AddListeners()
    {
        _button.onClick.AddListener(() => _mapManager.CloseMap());
        _button.onClick.AddListener(() => _mapManager.DisableFastTravel());
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
		_mapManager.EnableFastTravels += EnableFastTravel;
        _mapManager.DisableFastTravels += DisableFastTravel;

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
