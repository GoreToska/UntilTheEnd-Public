using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDataHolder : MonoBehaviour, IInteractable
{
	[SerializeField] public EnvironmentObjectData Data;

	private HighlightComponent _highlightComponent;

	private void Awake()
	{
		_highlightComponent = GetComponentInChildren<HighlightComponent>();
	}

	private void Destruct()
	{
		_highlightComponent.SetGlow(0, this.gameObject);
		Destroy(this);
	}

	public void StartInteraction()
	{
		UIManager.Instance.ShowInspectionWindow(Data.Description, Data.Voice.length);
		MusicManager.Instance.PlayInspectionPhrase(Data.Voice);
		Destruct();
	}
}
