using PixelCrushers.DialogueSystem;
using UnityEngine;

public class LuaRegistration : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    [HideInInspector] public static LuaRegistration instance;

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

        RegisterAllLua();
    }

    private void RegisterAllLua()
    {
        AttitudeManager.instance.RegisterLua();
        SkillsManager.instance.RegisterLua();
        UIAnimations.instance.RegisterLua();
        UIManager.instance.RegisterLua();
        UTESceneManager.instance.RegisterLua();
        MapManager.instance.RegisterLua();
        CameraStateManager.Ønstance.RegisterLua();
        ReportManager.instance.RegisterLua();
        DialogueEvidence.instance.RegisterLua();
        WalkingDialogueActorsManager.instance.RegisterLua();
        _inventory.RegisterLua();   

        Lua.RegisterFunction("Destroy", this, SymbolExtensions.GetMethodInfo(() => Destroy("")));
        Lua.RegisterFunction("Activate", this, SymbolExtensions.GetMethodInfo(() => Activate("")));
    }

    #region Lua Utilities
    private void Destroy(string name)
    {
        Destroy(GameObject.Find(name));
    }

    private void Activate(string name)
    {
        foreach (var item in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (item.name == name)
            {
                item.SetActive(true);
                return;
            }
        }
    }

    #endregion
}
