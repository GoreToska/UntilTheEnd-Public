using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine;
using Zenject;

public class PromptManager : MonoBehaviour
{
    [SerializeField] private GameObject _promptDialogue;
    [SerializeField] private TMP_Text _NPCName;
    private GameObject _targetDialogue;

    [SerializeField] private GameObject _promptEvidence;
    [SerializeField] private TMP_Text _evidenceName;
    private GameObject _targetEvidence;

    [SerializeField] private GameObject _promtLocation;
    [SerializeField] private TMP_Text _locationName;
    private GameObject _targetLocation;

    [SerializeField] private int _lift = 175;
    private int _moveDistance = 150;

    private Camera _camera;

	private void Awake()
	{
		_camera = Camera.main;
	}

	private void Update()
    {
        if (_promptDialogue.activeSelf)
        {
            Vector3 newPromptPos = _camera.WorldToScreenPoint(_targetDialogue.transform.position) + new Vector3(0, _lift, 0);
            _promptDialogue.transform.position = newPromptPos;
        }

        if (_promptEvidence.activeSelf)
        {
            Vector3 newPromptPos = _camera.WorldToScreenPoint(_targetEvidence.transform.position) + new Vector3(0, _lift, 0);
            _promptEvidence.transform.position = newPromptPos;
        }

        if (_promtLocation.activeSelf)
        {
            Vector3 newPromptPos = _camera.WorldToScreenPoint(_targetLocation.transform.position) + new Vector3(0, _lift, 0);
            _promtLocation.transform.position = newPromptPos;
        }
    }
  
    private void OnDisable()
    {
        PixelCrushers.SaveSystem.loadStarted -= DeactivatePromptDialogue;
        PixelCrushers.SaveSystem.loadStarted -= DeactivatePromptEvidence;
        PixelCrushers.SaveSystem.loadStarted -= DeactivatePromptLocation;
    }

    private void OnEnable()
    {
        PixelCrushers.SaveSystem.loadStarted += DeactivatePromptDialogue;
        PixelCrushers.SaveSystem.loadStarted += DeactivatePromptEvidence;
        PixelCrushers.SaveSystem.loadStarted += DeactivatePromptLocation;
    }

    public void ActivatePromptDialogue(GameObject target)
    {
        _targetDialogue = target;
        _NPCName.text = target.GetComponent<DialogueActor>().actor;
        _promptDialogue.transform.position = _camera.WorldToScreenPoint(_targetDialogue.transform.position) + new Vector3(0, _lift, 0);
        _promptDialogue.SetActive(true);
    }

    public void DeactivatePromptDialogue()
    {
        if (!_targetDialogue)
            _targetDialogue = GameObject.Find("DialoguePrompt");

        _targetDialogue = null;
        _promptDialogue.SetActive(false);
    }

    public void ActivatePromptEvidence(GameObject target)
    {
        _targetEvidence = target;
        _evidenceName.text = target.GetComponent<WorldEvidence>().EvidenceItem.Name;
        _promptEvidence.transform.position = _camera.WorldToScreenPoint(_targetEvidence.transform.position) + new Vector3(0, _lift, 0);
        _promptEvidence.SetActive(true);
    }

    public void DeactivatePromptEvidence()
    {
        if (!_promptEvidence)
            _promptEvidence = GameObject.Find("EvidencePrompt");
        _targetEvidence = null;
        _promptEvidence.SetActive(false);
    }

    public void ActivatePromptLocation(GameObject target)
    {
        _targetLocation = target;
        _locationName.text = target.GetComponent<LoadSceneOnTrigger>().Name;
        _promtLocation.transform.position = _camera.WorldToScreenPoint(_targetLocation.transform.position) + new Vector3(0, _lift, 0);
        _promtLocation.SetActive(true);
    }

    public void DeactivatePromptLocation()
    {
        _targetLocation = null;
        _promtLocation.SetActive(false);
    }

    public void DeactivatePrompts()
    {
        DeactivatePromptDialogue();
        DeactivatePromptEvidence();
        DeactivatePromptLocation();
    }
}