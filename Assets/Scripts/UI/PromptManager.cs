using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine;

public class PromptManager : MonoBehaviour
{
    [HideInInspector] public static PromptManager instance;


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

    private void Update()
    {
        if (_promptDialogue.activeSelf)
        {
            Vector3 newPromptPos = CameraStateManager.�nstance.GetComponent<Camera>().WorldToScreenPoint(_targetDialogue.transform.position) + new Vector3(0, _lift, 0);
            _promptDialogue.transform.position = newPromptPos;
        }

        if (_promptEvidence.activeSelf)
        {
            Vector3 newPromptPos = CameraStateManager.�nstance.GetComponent<Camera>().WorldToScreenPoint(_targetEvidence.transform.position) + new Vector3(0, _lift, 0);
            _promptEvidence.transform.position = newPromptPos;
        }

        if (_promtLocation.activeSelf)
        {
            Vector3 newPromptPos = CameraStateManager.�nstance.GetComponent<Camera>().WorldToScreenPoint(_targetLocation.transform.position) + new Vector3(0, _lift, 0);
            _promtLocation.transform.position = newPromptPos;
        }
    }

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
        _promptDialogue.transform.position = CameraStateManager.�nstance.GetComponent<Camera>().WorldToScreenPoint(_targetDialogue.transform.position) + new Vector3(0, _lift, 0);
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
        _evidenceName.text = target.GetComponent<InspectableObject>().Name;
        _promptEvidence.transform.position = CameraStateManager.�nstance.GetComponent<Camera>().WorldToScreenPoint(_targetEvidence.transform.position) + new Vector3(0, _lift, 0);
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
        _promtLocation.transform.position = CameraStateManager.�nstance.GetComponent<Camera>().WorldToScreenPoint(_targetLocation.transform.position) + new Vector3(0, _lift, 0);
        _promtLocation.SetActive(true);
    }

    public void DeactivatePromptLocation()
    {
        //if (!_promtLocation)
        //    _promtLocation = GameObject.Find("LocationPromt");
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