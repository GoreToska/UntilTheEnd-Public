using PixelCrushers.DialogueSystem;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class StartConversation : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private GameObject _player;

    [Header("If conversation starts with specified conversant.")]
    [SerializeField] private GameObject _conversant;
    [SerializeField] private string _conversationName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        //  If conversant is manually specified
        if (_conversant)
        {
            PromptManager.instance.ActivatePromptDialogue(_conversant);
        }
        else
        {
            PromptManager.instance.ActivatePromptDialogue(gameObject);
        }

        _inputReader.InteractEvent += OpenDialogue;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        PromptManager.instance.DeactivatePromptDialogue();
        _inputReader.InteractEvent -= OpenDialogue;
    }

    private void OpenDialogue()
    {
        _inputReader.SwitchToJournalControls();

        PromptManager.instance.DeactivatePromptDialogue();
        _inputReader.InteractEvent -= OpenDialogue;

        UIAnimations.Instance.DialogueFadeIn();

        if(_conversant)
            DialogueManager.StartConversation(_conversationName, _player.transform, _conversant.transform);
        else
            DialogueManager.StartConversation(_conversationName, _player.transform, this.transform);
    }
}