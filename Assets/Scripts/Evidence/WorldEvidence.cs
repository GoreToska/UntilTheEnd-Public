using UnityEngine;
using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine.UI;

public class WorldEvidence : MonoBehaviour, IViewableInteractable
{
	[SerializeField] protected InputReader _inputReader = default;
	[SerializeField] protected Inventory _inventory;
	[SerializeField] protected EvidenceItem _evidenceItem;

	protected GameObject _inspectableObject;

	[SerializeField] public string _dialogueVariableName = "";
	protected PlayerInteractionSystem _playerInteractionComponent;
	protected HighlightComponent _highlightComponent;

	public EvidenceItem EvidenceItem { get { return _evidenceItem; } }

	private void Awake()
	{
		_highlightComponent = GetComponent<HighlightComponent>();
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag != "Player")
			return;

		_playerInteractionComponent = other.GetComponent<PlayerInteractionSystem>();
		_playerInteractionComponent.AddInteractable(this);
		PromptManager.instance.ActivatePromptEvidence(gameObject);
		_highlightComponent.SetGlow(1, this.gameObject);
	}

	protected virtual void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag != "Player")
			return;

		_playerInteractionComponent.RemoveInteractable(this);
		_highlightComponent.SetGlow(0, this.gameObject);
		PromptManager.instance.DeactivatePromptEvidence();
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

		PromptManager.instance.DeactivatePromptEvidence();
		_highlightComponent.SetGlow(0, _inspectableObject);

		// TODO: inspectableObject to scriptable object
		_inspectableObject.transform.localPosition = Vector3.zero + EvidenceItem.SpawnPositionOffset;
		_inspectableObject.transform.localRotation = Quaternion.Euler(Vector3.zero + EvidenceItem.SpawnRotationOffset);

		EvidenceUIManager.Instance.SetEvidenceName(EvidenceItem.Name);

		UIManager.Instance.OnEvidenceOpen();
		InspectionCamera.Instance.InspectableObject = this;
		//Добавить анимацию
		UIManager.Instance.HideMainCanvas();
	}

	protected virtual void AddButtonListener()
	{
		EvidenceUIManager.Instance.SetTakeButtonEvent(this);
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

		UIManager.Instance.OnEvidenceClose();
		UIManager.Instance.ShowMainCanvas();

		if (_dialogueVariableName != "")
		{
			DialogueLua.SetVariable(_dialogueVariableName, true);
		}

		_playerInteractionComponent.EndInteraction();
		_inputReader.SwitchToGameControls();
		EvidenceUIManager.Instance.RemoveButtonEvents();
		ClearEvidence();
	}
}