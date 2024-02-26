using PixelCrushers.DialogueSystem;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
    [HideInInspector] public static SkillsManager instance;
    [SerializeField] private CharacterSkills _skills;

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
        Lua.RegisterFunction("CheckLaw", this, SymbolExtensions.GetMethodInfo(() => CheckLaw((double)0)));
        Lua.RegisterFunction("CheckCharter", this, SymbolExtensions.GetMethodInfo(() => CheckCharter((double)0)));
        Lua.RegisterFunction("CheckSavvy", this, SymbolExtensions.GetMethodInfo(() => CheckSavvy((double)0)));
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
