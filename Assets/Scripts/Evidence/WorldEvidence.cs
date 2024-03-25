using UnityEngine;
using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine.UI;
using Zenject;

public class WorldEvidence : MonoBehaviour, IViewableInteractable
{
	[SerializeField] protected Inventory _inventory;
	[SerializeField] protected EvidenceItem _evidenceItem;

	[SerializeField] public string _dialogueVariableName = "";
	protected HighlightComponent _highlightComponent;

	public EvidenceItem EvidenceItem { get { return _evidenceItem; } }

	[Inject] protected EvidenceUIManager _evidenceUIManager;
	[Inject] protected UIManager _uiManager;
	[Inject] protected PromptManager _promptManager;
	[Inject] protected UIAnimations _uIAnimations;
	[Inject] protected PlayerInteractionSystem _playerInteractionSystem;
	[Inject] protected InspectionCamera _inspectionCamera;
	[SerializeField] protected InputReader _inputReader;

	private void Awake()
	{
		_highlightComponent = GetComponent<HighlightComponent>();
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag != "Player")
			return;

		_playerInteractionSystem.AddInteractable(this);
		_promptManager.ActivatePromptEvidence(gameObject);
		_highlightComponent.SetGlow(1, this.gameObject);
	}

	protected virtual void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag != "Player")
			return;

		_playerInteractionSystem.RemoveInteractable(this);
		_highlightComponent.SetGlow(0, this.gameObject);
		_promptManager.DeactivatePromptEvidence();
	}

	public virtual void StartInteraction()
	{
		_inputReader.SwitchToInspectionControls();
		InputReader.AcceptEvent += OnInteractionView;
		AddButtonListener();
		StartInspect();
	}

	public virtual void StartInspect()
	{
		_highlightComponent.SetGlow(0, gameObject);
		_inspectionCamera.SpawnInspectableObject(this.gameObject, EvidenceItem);
		_promptManager.DeactivatePromptEvidence();

		

		_evidenceUIManager.SetEvidenceName(EvidenceItem.Name);
		_uiManager.OnEvidenceOpen();
		_inspectionCamera.InspectableObject = this;
		//TODO: Add animation
		_uiManager.HideMainCanvas();
	}

	protected virtual void AddButtonListener()
	{
		_evidenceUIManager.SetTakeButtonEvent(this);
	}

	protected virtual void OnDestroy()
	{
		InputReader.AcceptEvent -= OnInteractionView;
		_inputReader.SwitchToGameControls();
	}

	protected virtual void ClearEvidence()
	{
		_inspectionCamera.DestroyInspectableObject();
		Destroy(gameObject);
	}

	public virtual void OnInteractionView()
	{
		_inventory.AddEvidence(_evidenceItem);

		_uiManager.OnEvidenceClose();
		_uiManager.ShowMainCanvas();

		if (_dialogueVariableName != "")
		{
			DialogueLua.SetVariable(_dialogueVariableName, true);
		}

		_playerInteractionSystem.EndInteraction();
		_inputReader.SwitchToGameControls();
		_evidenceUIManager.RemoveButtonEvents();
		ClearEvidence();
	}
}