using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zenject;

public class MapManager : MonoBehaviour
{
	[Header(" нопки локаций стоит называть так, как называетс€ сцена локации")]
	[SerializeField] private List<LocationButton> _mapButtons;
	[SerializeField] private List<UnityEngine.UI.Toggle> _topBarButtons;
	[SerializeField] private GameObject _closeButton;
	[SerializeField] private UnityEngine.UI.Toggle _mapButton;
	[SerializeField] public LocationsList LocationsList;

	private UnityEngine.UI.Toggle _lastButton;
	private string _savedLocation;

	public event UnityAction EnableFastTravels = delegate { };
	public event UnityAction DisableFastTravels = delegate { };

	[Inject] private UIManager _uiManager;
	[SerializeField] private InputReader _inputReader;

	private void Start()
	{
		foreach (var item in LocationsList.Locations)
		{
			if (item.IsAvailable)
			{
				_mapButtons.First(i => i.Name == item.LocationName).UnsetCurrentLocation();
				Debug.Log("Enable");
			}
			else
			{
				Debug.Log("Disable");
				_mapButtons.First(i => i.Name == item.LocationName).DisableLocation();
			}

			if (item.IsCurrent || SceneManager.GetActiveScene().name == item.LocationName)
			{
				_mapButtons.First(i => i.Name == item.LocationName).SetCurrentLocation();
			}
		}

		DisableFastTravel();
	}

	public void RegisterLua()
	{
		Lua.RegisterFunction("OpenMap", this, SymbolExtensions.GetMethodInfo(() => OpenMap()));
		Lua.RegisterFunction("CloseMap", this, SymbolExtensions.GetMethodInfo(() => CloseMap()));
		Lua.RegisterFunction("EnableLocation", this, SymbolExtensions.GetMethodInfo(() => EnableLocation("")));
	}

	public void UnregisterLua()
	{
		Lua.UnregisterFunction("OpenMap");
		Lua.UnregisterFunction("CloseMap");
		Lua.UnregisterFunction("EnableLocation");
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

		_inputReader.SwitchToJournalControls();
		_lastButton.isOn = false;
		_mapButton.isOn = true;
		EnableFastTravel();
		_uiManager.OnOpenJournal();
		_closeButton.SetActive(true);
	}

	public void CloseMap()
	{
		DisableFastTravel();
		_closeButton.SetActive(false);
		_uiManager.OnCloseJournal();
		_lastButton.isOn = true;
		_inputReader.SwitchToGameControls();
	}

	public void SetCurrentButton(string name)
	{
		foreach (var button in _mapButtons)
		{
			if (button.Name == SceneManager.GetActiveScene().name)
			{
				button.UnsetCurrentLocation();
			}
		}

		foreach (var button in _mapButtons)
		{
			if (button.Name == name)
			{
				button.SetCurrentLocation();
			}
		}
	}

	public void SetCurrentButtonLong(string name)
	{
		foreach (var button in _mapButtons)
		{
			button.UnsetCurrentLocation();
		}

		foreach (var button in _mapButtons)
		{
			if (button.Name == name)
			{
				button.SetCurrentLocation();
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

	public string SavedLocation
	{
		get { return _savedLocation; }
		set { _savedLocation = value; }
	}
}
