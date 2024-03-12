using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Location List", menuName = "UTE/Locations")]
public class LocationsList : ScriptableObject
{
	[SerializeField] public List<Location> Locations;

	public bool ContainsLocation(string name)
	{
		foreach (var location in Locations)
		{
			if (location.LocationName == name)
				return true;
		}

		return false;
	}

	public bool ContainsLocation(string name, out Location location)
	{
		foreach (var item in Locations)
		{
			if (item.LocationName == name)
			{
				location = item;
				return true;
			}
		}

		location = null;
		return false;
	}
}

[System.Serializable]
public class Location
{
	public string LocationName;
	public bool IsCurrent = false;
	public bool IsAvailable = false;
}