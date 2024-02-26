using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    [HideInInspector] public static MapManager instance;

    [Header("������ ������� ����� �������� ���, ��� ���������� ����� �������")]
    [SerializeField] private List<LocationButton> _mapButtons;
    [SerializeField] private List<UnityEngine.UI.Toggle> _topBarButtons;
    [SerializeField] private GameObject _closeButton;
    [SerializeField] private UnityEngine.UI.Toggle _mapButton;
    private UnityEngine.UI.Toggle _lastButton;
    private string _savedLocation;

    public event UnityAction EnableFastTravels = delegate { };
    public event UnityAction DisableFastTravels = delegate { };

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

        DisableFastTravel();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterLua()
    {
        Lua.RegisterFunction("OpenMap", this, SymbolExtensions.GetMethodInfo(() => OpenMap()));
        Lua.RegisterFunction("CloseMap", this, SymbolExtensions.GetMethodInfo(() => CloseMap()));
        Lua.RegisterFunction("EnableLocation", this, SymbolExtensions.GetMethodInfo(() => EnableLocation("")));
    }

    public void OpenMap()
    {
        foreach (var button in _topBarButtons)
        {
            if (button.isOn)
            {
                _lastButton = button;
            }
        }

        _lastButton.isOn = false;
        _mapButton.isOn = true;
        EnableFastTravel();
        //UIManager.instance.SetMapCurrentPage();
        UIManager.instance.OnOpenJournal();
        _closeButton.SetActive(true);
    }

    public void CloseMap()
    {
        DisableFastTravel();
        _closeButton.SetActive(false);
        UIManager.instance.OnCloseJournal();
        _lastButton.isOn = true;
    }

    public void SetCurrentButton(string name)
    {
        foreach (var button in _mapButtons)
        {
            if (button.Name == SceneManager.GetActiveScene().name)
            {
                button.UnsetCurrent();
            }
        }

        foreach (var button in _mapButtons)
        {
            if (button.Name == name)
            {
                button.SetCurrent();
            }
        }
    }

    public void SetCurrentButtonLong(string name)
    {
        foreach (var button in _mapButtons)
        {
            button.UnsetCurrent();
        }

        foreach (var button in _mapButtons)
        {
            if (button.Name == name)
            {
                button.SetCurrent();
            }
        }
    }

    public void EnableLocation(string name)
    {
        foreach (var button in _mapButtons)
        {
            if (button.Name == name)
            {
                button.EnableLocation();
            }
        }
    }

    public void DisableLocation(string name)
    {
        foreach (var button in _mapButtons)
        {
            if (button.Name == name)
                button.DisableLocation();
        }
    }

    public void EnableFastTravel()
    {
        EnableFastTravels.Invoke();
    }

    public void DisableFastTravel()
    {
        DisableFastTravels.Invoke();
    }

    public static string ActiveScene
    { get { return SceneManager.GetActiveScene().ToString(); } }

    public string SavedLocation
    {
        get { return _savedLocation; }
        set { _savedLocation = value; }
    }
}
