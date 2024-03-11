using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvidenceUIManager : MonoBehaviour
{
	[HideInInspector] public static EvidenceUIManager Instance;

	[SerializeField] private TMP_Text _evidenceName;
	[SerializeField] private Button _takeButton;

	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void SetEvidenceName(string name)
	{
		_evidenceName.text = name;
	}

	public void SetTakeButtonEvent(IViewableInteractable interactable)
	{
		_takeButton.onClick.AddListener(() => interactable.OnInteractionView());
	}

	public void RemoveButtonEvents()
	{
		_takeButton.onClick.RemoveAllListeners();
	}
}
