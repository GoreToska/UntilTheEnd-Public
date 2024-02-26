using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvidence : MonoBehaviour
{
    [HideInInspector] public static DialogueEvidence instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        GameObject.Find(name).GetComponent<EvidenceFound>().DialogueEvidence();
    }
}
