using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvidenceInteraction : MonoBehaviour
{
	[HideInInspector] public static DialogueEvidenceInteraction Instance;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
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

	public void RegisterLua()
	{
		Lua.RegisterFunction("TakeEvidence", this, SymbolExtensions.GetMethodInfo(() => TakeEvidence("")));
	}

	public void TakeEvidence(string name)
	{
		GameObject.Find(name).GetComponent<IInteractable>().StartInteraction();
	}
}
