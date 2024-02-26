using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CameraInterim", menuName = "UTE/Camera Interim")]
public class CameraInterim : ScriptableObject
{
    public event UnityAction DialogueActiveEvent = delegate { };
    public event UnityAction DialogueDisactiveEvent = delegate { };

    public void ActivateDialogue()
    {
        DialogueActiveEvent.Invoke();
    }

    public void DisactivateDialogue()
    {
        DialogueDisactiveEvent.Invoke();
    }
}