using PixelCrushers.DialogueSystem;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using Zenject;

public class StartConversation : MonoBehaviour
{
	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private GameObject _player;

	[Header("If conversation starts with specified conversant.")]
	[SerializeField] private GameObject _conversant;
	[SerializeField] private string _conversationName;

	[Inject] private PromptManager _promptManager;
	[Inject] private UIAnimations _uiAnimations;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag != "Player")
			return;

		//  If conversant is manually specified
		if (_conversant)
		{
			_promptManager.ActivatePromptDialogue(_conversant);
		}
		else
		{
			_promptManager.ActivatePromptDialogue(gameObject);
		}

		InputReader.InteractEvent += OpenDialogue;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag != "Player")
			return;

		_promptManager.DeactivatePromptDialogue();
		InputReader.InteractEvent -= OpenDialogue;
	}

	private void OpenDialogue()
	{
		_inputReader.SwitchToJournalControls();

		_promptManager.DeactivatePromptDialogue();
		InputReader.InteractEvent -= OpenDialogue;

		_uiAnimations.DialogueFadeIn();

		if (_conversant)
			DialogueManager.StartConversation(_conversationName, _player.transform, _conversant.transform);
		else
			DialogueManager.StartConversation(_conversationName, _player.transform, this.transform);
	}
}