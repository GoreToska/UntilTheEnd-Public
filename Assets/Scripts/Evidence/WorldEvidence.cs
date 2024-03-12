using UnityEngine;
using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine.UI;
using Zenject;

public class WorldEvidence : MonoBehaviour, IViewableInteractable
{
	[SerializeField] protected InputReader _inputReader = default;
	[SerializeField] protected Inventory _inventory;
	[SerializeField] protected EvidenceItem _evidenceItem;

	protected GameObject _inspectableObject;

	[SerializeField] public string _dialogueVariableName = "";
	protected HighlightComponent _highlightComponent;

	public EvidenceItem EvidenceItem { get { return _evidenceItem; } }

	[Inject] protected EvidenceUIManager _evidenceUIManager;
	[Inject] protected UIManager _uiManager;
	[Inject] protected PromptManager _promptManager;
	[Inject] protected UIAnimations _uIAnimations;
	[Inject] protected PlayerInteractionSystem _playerInteractionSystem;

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
		InstantiateEvidence();

		_promptManager.DeactivatePromptEvidence();
		_highlightComponent.SetGlow(0, _inspectableObject);

		// TODO: inspectableObject to scriptable object
		_inspectableObject.transform.localPosition = Vector3.zero + EvidenceItem.SpawnPositionOffset;
		_inspectableObject.transform.localRotation = Quaternion.Euler(Vector3.zero + EvidenceItem.SpawnRotationOffset);

		_evidenceUIManager.SetEvidenceName(EvidenceItem.Name);

		_uiManager.OnEvidenceOpen();
		InspectionCamera.Instance.InspectableObject = this;
		//Добавить анимацию
		_uiManager.HideMainCanvas();
	}

	protected virtual void AddButtonListener()
	{
		_evidenceUIManager.SetTakeButtonEvent(this);
	}

	protected virtual void OnDisable()
	{
		InputReader.AcceptEvent -= OnInteractionView;
		_inputReader.SwitchToGameControls();
	}

	protected virtual void InstantiateEvidence()
	{
		_inspectableObject = Instantiate(gameObject, InspectionCamera.Instance.gameObject.transform.GetChild(0));
		_inspectableObject.GetComponent<Rigidbody>().isKinematic = true;
		_inspectableObject.GetComponent<BoxCollider>().enabled = false;
		InspectionCamera.Instance.GetComponentInChildren<Camera>().enabled = false;
	}

	protected virtual void ClearEvidence()
	{
		InspectionCamera.Instance.GetComponentInChildren<Camera>().enabled = false;
		Destroy(_inspectableObject);
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