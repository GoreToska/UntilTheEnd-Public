using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Collections;
using Zenject;

public class UTESceneManager : MonoBehaviour
{
	[SerializeField] public const string _train = "Train";
	[SerializeField] public const string _estate = "Estate";
	[SerializeField] public const string _estateFirst = "EstateFirstFloor";
	[SerializeField] public const string _estateSecond = "EstateSecondFloor";
	[SerializeField] public const string _railway = "Railway";
	[SerializeField] public const string _court = "CourtHouse";
	[SerializeField] public const string _pub = "Pub";
	[SerializeField] public const string _loading = "Loading";

	[Inject] private MapManager _mapManager;
	[Inject] private PromptManager _promptManager;
	[Inject] private UIManager _uiManager;
	[Inject] private MusicManager _musicManager;
	[Inject] private StaticCharacterMovement _player;
	private static AsyncOperation _loadingOperation;

	private void Awake()
	{
		PixelCrushers.SaveSystem.ApplySavedGameData();
	}

	private void OnEnable()
	{
		InputReader.SaveGame += OnSaveGame;
		InputReader.LoadGame += OnLoadGame;
	}

	private void OnDisable()
	{
		InputReader.SaveGame -= OnSaveGame;
		InputReader.LoadGame -= OnLoadGame;
	}

	public void RegisterLua()
	{
		Lua.RegisterFunction("LoadRailway", this, SymbolExtensions.GetMethodInfo(() => LoadRailway()));
		Lua.RegisterFunction("LoadRailwaySpawn", this, SymbolExtensions.GetMethodInfo(() => LoadRailway("")));
		Lua.RegisterFunction("LoadEstate", this, SymbolExtensions.GetMethodInfo(() => LoadEstate()));
		Lua.RegisterFunction("LoadPub", this, SymbolExtensions.GetMethodInfo(() => LoadPub()));
	}

	public void UnregisterLua()
	{
		Lua.UnregisterFunction("LoadTrain");
		Lua.UnregisterFunction("LoadRailway");
		Lua.UnregisterFunction("LoadRailwaySpawn");
		Lua.UnregisterFunction("LoadEstate");
		Lua.UnregisterFunction("LoadPub");
	}

	public void LoadRailway()
	{
		if (CurrentScene == _railway)
			return;

		_mapManager.SetCurrentButton(_railway);
		_musicManager.StopAllMusic();

		LoadScene(_railway);
	}

	public void LoadRailway(string spawnpoint)
	{
		if (CurrentScene == _railway)
			return;

		_mapManager.SetCurrentButton(_railway);
		_musicManager.StopAllMusic();

		LoadScene(_railway, spawnpoint);
	}

	public void LoadEstate()
	{
		if (CurrentScene == _estate)
			return;

		_mapManager.SetCurrentButton(_estate);
		_musicManager.StopAllMusic();

		LoadScene(_estate);
	}

	public void LoadCourtHouse()
	{
		if (CurrentScene == _court)
			return;

		_mapManager.SetCurrentButton(_court);
		_musicManager.StopAllMusic();

		LoadScene(_court);
	}

	public void LoadPub()
	{
		if (CurrentScene == _pub)
			return;

		_mapManager.SetCurrentButton(_pub);
		_musicManager.StopAllMusic();

		LoadScene(_pub);
	}

	public void OnSaveGame()
	{
		PixelCrushers.SaveSystem.SaveToSlot(0);
		_mapManager.SavedLocation = SceneManager.GetActiveScene().name;
	}

	public void OnLoadGame()
	{
		_uiManager.OnCloseMenu();
		_mapManager.SetCurrentButtonLong(_mapManager.SavedLocation);
		_promptManager.DeactivatePrompts();

		PixelCrushers.SaveSystem.LoadScene(_loading);
		_loadingOperation = PixelCrushers.SaveSystem.currentAsyncOperation;

		_loadingOperation.completed += delegate { PixelCrushers.SaveSystem.LoadFromSlot(0); _loadingOperation = null; };
	}

	public void LoadScene(string locationName, string spawnPoint = null)
	{
		PixelCrushers.SaveSystem.BeforeSceneChange();
		PixelCrushers.SaveSystem.RecordSavedGameData();

		_loadingOperation = SceneManager.LoadSceneAsync(_loading, LoadSceneMode.Additive);

		if (spawnPoint != null)
		{
			_loadingOperation.completed += delegate { PixelCrushers.SaveSystem.LoadScene($"{locationName}@{spawnPoint}"); ; _loadingOperation = null; };
		}
		else
		{
			_loadingOperation.completed += delegate { PixelCrushers.SaveSystem.LoadScene(locationName); _loadingOperation = null; };
		}
	}

	//private AsyncOperation LoadLoadingScene()
	//{

	//	return PixelCrushers.SaveSystem.currentAsyncOperation;
	//	//return SceneManager.LoadSceneAsync(_loading, LoadSceneMode.Single);
	//}

	public static string CurrentScene { get { return SceneManager.GetActiveScene().name; } }

	public static string TrainName { get { return _train; } }

	public static string RailwayName { get { return _railway; } }

	public static string CourtName { get { return _court; } }

	public static string EstateName { get { return _estate; } }

	public static string EstateFirstName { get { return _estateFirst; } }

	public static string EstateSecondName { get { return _estateSecond; } }

	public static string PubName { get { return _pub; } }
}
