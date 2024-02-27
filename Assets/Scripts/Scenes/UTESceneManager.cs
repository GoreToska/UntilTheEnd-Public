using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Collections;

public class UTESceneManager : MonoBehaviour
{
    [HideInInspector] public static UTESceneManager instance;

    [SerializeField] private InputReader _inputReader;

    [SerializeField] public const string _train = "Train";
    [SerializeField] public const string _estate = "Estate";
    [SerializeField] public const string _estateFirst = "EstateFirstFloor";
    [SerializeField] public const string _estateSecond = "EstateSecondFloor";
    [SerializeField] public const string _railway = "Railway";
    [SerializeField] public const string _court = "CourtHouse";
    [SerializeField] public const string _pub = "Pub";


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

    private void OnEnable()
    {
        _inputReader.SaveGame += OnSaveGame;
        _inputReader.LoadGame += OnLoadGame;
    }

    private void OnDisable()
    {
        _inputReader.SaveGame -= OnSaveGame;
        _inputReader.LoadGame -= OnLoadGame;
    }

    public void RegisterLua()
    {
        Lua.RegisterFunction("LoadTrain", this, SymbolExtensions.GetMethodInfo(() => LoadTrain()));
        Lua.RegisterFunction("LoadRailway", this, SymbolExtensions.GetMethodInfo(() => LoadRailway()));
        Lua.RegisterFunction("LoadRailwaySpawn", this, SymbolExtensions.GetMethodInfo(() => LoadRailway("")));
        Lua.RegisterFunction("LoadEstate", this, SymbolExtensions.GetMethodInfo(() => LoadEstate()));
        Lua.RegisterFunction("LoadPub", this, SymbolExtensions.GetMethodInfo(() => LoadPub()));
    }

    public void LoadTrain()
    {
        if (CurrentScene == "Railway1")
            return;

        //Temp location
        MapManager.instance.SetCurrentButton("Railway1");
        MusicManager.instance.StopAllMusic(); //    place to on scene change event
        StartCoroutine(LoadScene("Railway1"));
    }

    public void LoadRailway()
    {
        if (CurrentScene == _railway)
            return;

        MapManager.instance.SetCurrentButton(_railway);
        MusicManager.instance.StopAllMusic();
        StartCoroutine(LoadScene(_railway));
    }

    public void LoadRailway(string spawnpoint)
    {
        if (CurrentScene == _railway)
            return;

        MapManager.instance.SetCurrentButton(_railway);
        MusicManager.instance.StopAllMusic();
        PixelCrushers.SaveSystem.LoadScene($"{_railway}@{spawnpoint}");

        //StartCoroutine(LoadScene(_railway, spawnpoint));
    }

    public void LoadEstate()
    {
        if (CurrentScene == _estate)
            return;

        MapManager.instance.SetCurrentButton(_estate);
        MusicManager.instance.StopAllMusic();
        StartCoroutine(LoadScene(_estate));
    }

    public void LoadCourtHouse()
    {
        if (CurrentScene == _court)
            return;

        MapManager.instance.SetCurrentButton(_court);
        MusicManager.instance.StopAllMusic();
        StartCoroutine(LoadScene(_court));
    }

    public void LoadPub()
    {
        if (CurrentScene == _pub)
            return;

        MapManager.instance.SetCurrentButton(_pub);
        MusicManager.instance.StopAllMusic();
        StartCoroutine(LoadScene(_pub));
    }

    public void OnSaveGame()
    {
        PixelCrushers.SaveSystem.SaveToSlot(0);
        MapManager.instance.SavedLocation = SceneManager.GetActiveScene().name;
    }

    public void OnLoadGame()
    {
        UIManager.Instance.OnCloseMenu();
        MapManager.instance.SetCurrentButtonLong(MapManager.instance.SavedLocation);
        PromptManager.instance.DeactivatePrompts();
        PixelCrushers.SaveSystem.LoadFromSlot(0);
    }

    private IEnumerator LoadScene(string locationName, string spawnPoint = null)
    {
        if (spawnPoint != null)
        {
            PixelCrushers.SaveSystem.LoadScene($"{locationName}@{spawnPoint}");
        }
        else
        {
            PixelCrushers.SaveSystem.LoadScene(locationName);
        }

        yield return null;
    }

    public static string CurrentScene { get { return SceneManager.GetActiveScene().name; } }

    public static string TrainName { get { return _train; } }

    public static string RailwayName { get { return _railway; } }

    public static string CourtName { get { return _court; } }

    public static string EstateName { get { return _estate; } }

    public static string EstateFirstName { get { return _estateFirst; } }

    public static string EstateSecondName { get { return _estateSecond; } }

    public static string PubName { get { return _pub; } }
}
