using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LocationButton : MonoBehaviour
{
	[Header("�� ��������� ������� �������� ����������� �������")]
	[SerializeField] private Sprite _enabledImage;
	[SerializeField] private Sprite _currentImage;
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

		if (!_mapManager.LocationsList.ContainsLocation(_name))
			return;

		_mapManager.LocationsList.Locations.First(i => i.LocationName == _name).IsAvailable = true;
		_mapManager.LocationsList.Locations.First(i => i.LocationName == _name).IsCurrent = true;
	}

	public void UnsetCurrentLocation()
	{
		EnableLocation();
		_button.image.sprite = _enabledImage;

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

		_button.transform.gameObject.SetActive(true);
		UnsetInteractable();

		if (!_mapManager.LocationsList.ContainsLocation(_name))
			return;

		_mapManager.LocationsList.Locations.First(i => i.LocationName == _name).IsAvailable = true;
	}

	public string Name
	{
		get { return _name; }
	}
}
