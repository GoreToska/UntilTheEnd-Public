using PixelCrushers.DialogueSystem;
using UnityEngine;

public class TrainSleepTrigger : MonoBehaviour
{
    void OnTriggerStay()
    {
        if(DialogueLua.GetVariable("Sleeping").asBool)
        {
            GetComponent<DialogueSystemTrigger>().enabled = true;
        }
    }
}
