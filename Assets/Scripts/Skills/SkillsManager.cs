using PixelCrushers.DialogueSystem;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
    [SerializeField] private CharacterSkills _skills;

	public void RegisterLua()
    {
        Lua.RegisterFunction("CheckLaw", this, SymbolExtensions.GetMethodInfo(() => CheckLaw((double)0)));
        Lua.RegisterFunction("CheckCharter", this, SymbolExtensions.GetMethodInfo(() => CheckCharter((double)0)));
        Lua.RegisterFunction("CheckSavvy", this, SymbolExtensions.GetMethodInfo(() => CheckSavvy((double)0)));
    }

    public void UnregisterLua()
    {
        Lua.UnregisterFunction("CheckLaw");
		Lua.UnregisterFunction("CheckCharter");
		Lua.UnregisterFunction("CheckSavvy");
	}

	#region CheckSkills
	private bool CheckLaw(double value)
    {
        if (_skills.Law >= (int)value)
            return true;
        else
            return false;
    }

    private bool CheckCharter(double value)
    {
        if (_skills.Charter >= (int)value)
            return true;
        else
            return false;
    }

    private bool CheckSavvy(double value)
    {
        if (_skills.Savvy >= (int)value)
            return true;
        else
            return false;
    }
    #endregion

    #region AddSkills
    public void AddLaw()
    {
        _skills.Law++;
    }

    public void AddCharter()
    {
        _skills.Charter++;
    }

    public void AddSavvy()
    {
        _skills.Savvy++;
    }
    #endregion

    #region SetSkills
    public void SetLaw(int value)
    {
        _skills.Law = value;
    }

    public void SetCharter(int value)
    {
        _skills.Charter = value;
    }

    public void SetSavvy(int value)
    {
        _skills.Savvy = value;
    }
    #endregion
}
