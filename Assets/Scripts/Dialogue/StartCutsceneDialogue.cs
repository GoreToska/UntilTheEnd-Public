using PixelCrushers.DialogueSystem;
using UnityEngine;

public class StartCutsceneDialogue : MonoBehaviour
{
    public void StartConversation(string conversationName)
    {
        DialogueManager.StartConversation(conversationName, this.transform);
    }
}
