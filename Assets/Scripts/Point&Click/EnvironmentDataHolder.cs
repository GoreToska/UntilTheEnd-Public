using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnvironmentDataHolder : MonoBehaviour, IInteractable
{
	[SerializeField] public EnvironmentObjectData Data;

	private HighlightComponent _highlightComponent;

	[Inject] private UIManager _uiManager;
	[Inject] private MusicManager _musicManager;

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
		_uiManager.ShowInspectionWindow(Data.Description, Data.Voice.length);
		_musicManager.PlayInspectionPhrase(Data.Voice);
		Destruct();
	}
}
