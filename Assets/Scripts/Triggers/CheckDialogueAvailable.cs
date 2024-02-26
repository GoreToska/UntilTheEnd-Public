using PixelCrushers.DialogueSystem;
using UnityEngine;

public class CheckDialogueAvailable : MonoBehaviour
{
    [SerializeField] private string _variableName;
    void FixedUpdate()
    {
        if(DialogueLua.GetVariable(_variableName).asBool)
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = true;
            Destroy(this.gameObject.GetComponent<CheckDialogueAvailable>());
        }
    }
}
