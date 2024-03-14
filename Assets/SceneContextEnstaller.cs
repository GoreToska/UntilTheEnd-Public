using PixelCrushers.DialogueSystem.Wrappers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneContextEnstaller : MonoInstaller
{
	[SerializeField] private AttitudeManager _attitudeManager;
	[SerializeField] private SkillsManager _skillsManager;
	[SerializeField] private UIAnimations _uiAnimations;
	[SerializeField] private UIManager _uiManager;
	[SerializeField] private UTESceneManager _uteSceneManager;
	[SerializeField] private MapManager _mapManager;
	[SerializeField] private CameraStateManager _cameraStateManager;
	[SerializeField] private ReportManager _reportManager;
	[SerializeField] private DialogueEvidenceInteraction _dialogueEvidenceInteraction;
	[SerializeField] private WalkingDialogueActorsManager _walkingDialogueActorsManager;
	[SerializeField] private StaticCameraMovement _staticCameraMovement;
	[SerializeField] private StaticDialogueCameraMovement _staticDialogueCameraMovement;
	[SerializeField] private StaticCharacterMovement _staticCharacterMovement;
	[SerializeField] private EvidenceUIManager _evidenceUIManager;
	[SerializeField] private PlayerInteractionSystem _playerInteractionSystem;
	[SerializeField] private PromptManager _promptManager;
	[SerializeField] private MusicManager _musicManager;
	[SerializeField] private StandardDialogueUI _standardDialogueUI;
	[SerializeField] private CameraTarget _cameraTarget;
	[SerializeField] private UISoundManager _uiSoundManager;
	[SerializeField] private UIConclusion _uiConclusion;
	[SerializeField] private InspectionCamera _inspectionCamera;

	public override void InstallBindings()
	{
		Container.BindInstance(_attitudeManager).AsSingle().NonLazy();
		Container.BindInstance(_skillsManager).AsSingle().NonLazy();
		Container.BindInstance(_uiAnimations).AsSingle().NonLazy();
		Container.BindInstance(_uiManager).AsSingle().NonLazy();
		Container.BindInstance(_uteSceneManager).AsSingle().NonLazy();
		Container.BindInstance(_mapManager).AsSingle().NonLazy();
		Container.BindInstance(_cameraStateManager).AsSingle().NonLazy();
		Container.BindInstance(_reportManager).AsSingle().NonLazy();
		Container.BindInstance(_dialogueEvidenceInteraction).AsSingle().NonLazy();
		Container.BindInstance(_walkingDialogueActorsManager).AsSingle().NonLazy();
		Container.BindInstance(_staticCameraMovement).AsSingle().NonLazy();
		Container.BindInstance(_staticDialogueCameraMovement).AsSingle().NonLazy();
		Container.BindInstance(_staticCharacterMovement).AsSingle().NonLazy();
		Container.BindInstance(_evidenceUIManager).AsSingle().NonLazy();
		Container.BindInstance(_playerInteractionSystem).AsSingle().NonLazy();
		Container.BindInstance(_promptManager).AsSingle().NonLazy();
		Container.BindInstance(_musicManager).AsSingle().NonLazy();
		Container.BindInstance(_standardDialogueUI).AsSingle().NonLazy();
		Container.BindInstance(_cameraTarget).AsSingle().NonLazy();
		Container.BindInstance(_uiSoundManager).AsSingle().NonLazy();
		Container.BindInstance(_uiConclusion).AsSingle().NonLazy();
		Container.BindInstance(_inspectionCamera).AsSingle().NonLazy();
	}
}
