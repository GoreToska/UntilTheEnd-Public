using Cinemachine;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine.Rendering;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

public class EvidenceFound : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private EvidenceItem _evidenceItem;

    private GameObject _inspectableObject;
    [SerializeField] public TMP_Text _evidenceName;
    [SerializeField] private Button _takeButton;

    [SerializeField] public string _dialogueVariableName = "";

    private float _fadeTime = 0.5f;
    private int _glowFactorID = Shader.PropertyToID("_GlowFactor");

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        SetGlow(1, this.gameObject);
        PromptManager.instance.ActivatePromptEvidence(gameObject);
        _inputReader.InteractEvent += OnInteract;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        SetGlow(0, this.gameObject);
        PromptManager.instance.DeactivatePromptEvidence();
        _inputReader.InteractEvent -= OnInteract;
    }

    public void DialogueEvidence()
    {
        if (!_takeButton)
            _takeButton = GameObject.Find("Take").GetComponent<Button>();

        if (!_evidenceName)
            _evidenceName = GameObject.Find("EvidenceName").GetComponent<TMP_Text>();

        UIAnimations.instance.DialogueFadeOut();
        _inputReader.SwitchToInspectionControls();
        _inputReader.TakeEvidenceEvent += OnTakeDialogueEvidence;

        StartInspect(true);
    }

    private void OnInteract()
    {
        _inputReader.SwitchToInspectionControls();

        _inputReader.TakeEvidenceEvent += OnTakeEvidence;
        _inputReader.InteractEvent -= OnInteract;
        //_inputReader.OpenJournal += OnTakeEvidence;

        StartInspect(false);
    }

    private void StartInspect(bool isDialogue)
    {
        _inspectableObject = Instantiate(gameObject, InspectionCamera.instance.gameObject.transform.GetChild(0));
        _inspectableObject.GetComponent<Rigidbody>().isKinematic = true;
        _inspectableObject.GetComponent<BoxCollider>().enabled = false;
        PromptManager.instance.DeactivatePromptEvidence();
        SetGlow(0, _inspectableObject);

        // TODO: inspectableObject to scriptable object
        InspectableObject inspectableObject = _inspectableObject.GetComponent<InspectableObject>();

        inspectableObject.transform.localPosition = Vector3.zero + inspectableObject.spawnPositionOffset;
        inspectableObject.transform.localRotation = Quaternion.Euler(Vector3.zero + inspectableObject.spawnRotationOffset);
        _evidenceName.text = inspectableObject.Name;

        UIManager.instance.OnEvidenceOpen();

        InspectionCamera.instance.inspectableObject = inspectableObject;
        InspectionCamera.instance.GetComponentInChildren<Camera>().enabled = true;

        //Добавить анимацию
        UIManager.instance.HideMainCanvas();

        if (isDialogue)
            _takeButton.onClick.AddListener(() => OnTakeDialogueEvidence());
        else
            _takeButton.onClick.AddListener(() => OnTakeEvidence());
    }

    private void OnTakeDialogueEvidence()
    {
        _inventory.AddEvidence(_evidenceItem);
        UIManager.instance.OnEvidenceClose();
        InspectionCamera.instance.GetComponentInChildren<Camera>().enabled = false;
        UIManager.instance.ShowMainCanvas();
        UIAnimations.instance.DialogueFadeIn();

        if (_dialogueVariableName != "")
        {
            DialogueLua.SetVariable(_dialogueVariableName, true);
        }

        Destroy(_inspectableObject);
        Destroy(gameObject);
        _inputReader.TakeEvidenceEvent -= OnTakeDialogueEvidence;
        _takeButton.onClick.RemoveListener(OnTakeDialogueEvidence);
    }

    private void OnTakeEvidence()
    {
        _inventory.AddEvidence(_evidenceItem);

        // TODO: Better funcs to switch canvases
        UIManager.instance.OnEvidenceClose(); // <- is better

        InspectionCamera.instance.GetComponentInChildren<Camera>().enabled = false;
        UIManager.instance.ShowMainCanvas();

        if (_dialogueVariableName != "")
        {
            DialogueLua.SetVariable(_dialogueVariableName, true);
        }

        Destroy(_inspectableObject);
        Destroy(gameObject);
        _inputReader.SwitchToGameControls();
        _takeButton.onClick.RemoveListener(OnTakeEvidence);
    }

    private void SetGlow(float value, GameObject gObject)
    {
        Transform[] allChildren = gObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in allChildren)
        {
            Renderer childRenderer = child.gameObject.GetComponent<Renderer>();
            if (childRenderer)
            {
                foreach (Material mat in childRenderer.materials)
                {
                    mat.SetFloat(_glowFactorID, value);
                }
            }
        }
    }

    private void OnDisable()
    {
        _inputReader.InteractEvent -= OnInteract;
        _inputReader.TakeEvidenceEvent -= OnTakeEvidence;
        _inputReader.SwitchToGameControls();
        //_inputReader.OpenJournal -= OnTakeEvidence;
    }
}