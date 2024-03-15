using System.Linq;
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
	}

	public void SetCurrentLocation()
	{
		_button.image.sprite = _currentImage;
		//RemoveListeners();

		if (!_mapManager.LocationsList.ContainsLocation(_name))
			return;

		_mapManager.LocationsList.Locations.First(i => i.LocationName == _name).IsAvailable = true;
		_mapManager.LocationsList.Locations.First(i => i.LocationName == _name).IsCurrent = true;
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

	public void UnsetCurrentLocation()
	{
		EnableLocation();
		_button.image.sprite = _enabledImage;

		//AddListeners();

		if (!_mapManager.LocationsList.ContainsLocation(_name))
			return;

		_mapManager.LocationsList.Locations.First(i => i.LocationName == _name).IsAvailable = true;
		_mapManager.LocationsList.Locations.First(i => i.LocationName == _name).IsCurrent = false;
	}

	private void SetInteractable()
	{
		_button.interactable = true;
	}

	private void UnsetInteractable()
	{
		_button.interactable = false;
	}

	public void DisableLocation()
	{
		_button.transform.gameObject.SetActive(false);
	}

	public void EnableLocation()
	{
		_mapManager.EnableFastTravels += SetInteractable;
		_mapManager.DisableFastTravels += UnsetInteractable;

		//AddListeners();

		_button.transform.gameObject.SetActive(true);
		UnsetInteractable();

		if (!_mapManager.LocationsList.ContainsLocation(_name))
			return;

		_mapManager.LocationsList.Locations.First(i => i.LocationName == _name).IsAvailable = true;
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
