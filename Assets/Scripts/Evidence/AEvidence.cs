using PixelCrushers.DialogueSystem;
using UnityEngine;

public abstract class AEvidence : ScriptableObject
{
    [SerializeField] protected string _name;
    [TextArea]
    [SerializeField] private string _description;
    [SerializeField] protected int _id;
    [SerializeField] protected ELocations _location;
    [SerializeField] protected EEvidenceType _type;
    [SerializeField] protected string _npcName;
    [SerializeField] public LogsInfo _logsInfo;
    // TODO: skills

    public EEvidenceType EvidenceType
    {
        get { return _type; }
    }

    public string Name
    {
        get { return _name; }
    }

    public string NPCName
    {
        get { return PixelCrushers.DialogueSystem.CharacterInfo.GetLocalizedDisplayNameInDatabase(DialogueLua.GetActorField(_npcName, "Name").asString); }
    }

    public int ID
    {
        get { return _id; }
    }

    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }
}