using PixelCrushers.DialogueSystem;
using UnityEngine;
using Zenject;

public class LuaRegistration : MonoBehaviour
{
	[SerializeField] private Inventory _inventory;

	[Inject] private AttitudeManager _attitudeManager;
	[Inject] private SkillsManager _skillsManager;
	[Inject] private UIAnimations _uiAnimations;
	[Inject] private UIManager _uiManager;
	[Inject] private UTESceneManager _uteSceneManager;
	[Inject] private MapManager _mapManager;
	[Inject] private CameraStateManager _cameraStateManager;
	[Inject] private ReportManager _reportManager;
	[Inject] private DialogueEvidenceInteraction _dialogueEvidenceInteraction;
	[Inject] private WalkingDialogueActorsManager _walkingDialogueActorsManager;

	private void OnEnable()
	{
		RegisterAllLua();
	}

	private void OnDisable()
	{
		UnregisterAllLua();
	}

	private void RegisterAllLua()
	{
		_attitudeManager.RegisterLua();
		_skillsManager.RegisterLua();
		_uiAnimations.RegisterLua();
		_uiManager.RegisterLua();
		_uteSceneManager.RegisterLua();
		_mapManager.RegisterLua();
		_cameraStateManager.RegisterLua();
		_reportManager.RegisterLua();
		_dialogueEvidenceInteraction.RegisterLua();
		_walkingDialogueActorsManager.RegisterLua();
		_inventory.RegisterLua();

		Lua.RegisterFunction("Destroy", this, SymbolExtensions.GetMethodInfo(() => Destroy("")));
		Lua.RegisterFunction("Activate", this, SymbolExtensions.GetMethodInfo(() => Activate("")));
	}

	private void UnregisterAllLua()
	{
		_attitudeManager.UnregisterLua();
		_skillsManager.UnregisterLua();
		_uiAnimations.UnregisterLua();
		_uiManager.UnregisterLua();
		_uteSceneManager.UnregisterLua();
		_mapManager.UnregisterLua();
		_cameraStateManager.UnregisterLua();
		_reportManager.UnregisterLua();
		_dialogueEvidenceInteraction.UnregisterLua();
		_walkingDialogueActorsManager.UnregisterLua();
		_inventory.UnregisterLua();

		Lua.UnregisterFunction("Destroy");
		Lua.UnregisterFunction("Activate");
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
