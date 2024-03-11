using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

public class AttitudeManager : MonoBehaviour
{
    [HideInInspector] public static AttitudeManager instance;

    [SerializeField] private List<NPCAttitude> _npcAttitudes;

    private NPCAttitude _current;

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
        Lua.RegisterFunction("CheckAttitude", this, SymbolExtensions.GetMethodInfo(() => CheckAttitude((string)"", (string)"")));
        Lua.RegisterFunction("AddAttitude", this, SymbolExtensions.GetMethodInfo(() => AddAttitude((string)"", (double)0)));
        Lua.RegisterFunction("SubAttitude", this, SymbolExtensions.GetMethodInfo(() => SubAttitude((string)"", (double)0)));
    }

    #region Attitude Operations
    public bool CheckAttitude(string NPCName, string attitudeLVL)
    {
        FindNPCbyName(NPCName);

        if (attitudeLVL == "Bad")
            return _current.Bad();
        else if (attitudeLVL == "Neutral")
            return _current.Neutral();
        else if (attitudeLVL == "Good")
            return _current.Good();

        return false;
    }

    public void AddAttitude(string NPCName, double value)
    {
        FindNPCbyName(NPCName);
        Debug.Log(_current.name);

        _current.AddAttitude(value);
    }

    public void SubAttitude(string NPCName, double value)
    {
        FindNPCbyName(NPCName);
        Debug.Log(_current.name);
        _current.SubAttitude(value);
    }

    #endregion

    private void FindNPCbyName(string name)
    {
        foreach (var npc in _npcAttitudes)
        {
            if (npc.name == name)
            {
                _current = npc;
                return;
            }
        }

        Debug.LogError("Error! NPC SO not found!");
        return;
    }
}
