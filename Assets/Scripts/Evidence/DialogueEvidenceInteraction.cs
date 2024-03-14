using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvidenceInteraction : MonoBehaviour
{
	public void RegisterLua()
	{
		Lua.RegisterFunction("TakeEvidence", this, SymbolExtensions.GetMethodInfo(() => TakeEvidence("")));
	}

	public void UnregisterLua()
	{
		Lua.UnregisterFunction("TakeEvidence");
	}

	public void TakeEvidence(string name)
	{
		GameObject.Find(name).GetComponent<IInteractable>().StartInteraction();
	}
}
