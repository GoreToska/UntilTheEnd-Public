using PixelCrushers.DialogueSystem;
using Zenject;

public class DialogueEvidence : WorldEvidence
{
	public override void StartInspect()
	{
		base.StartInspect();

		_uIAnimations.DialogueFadeOut();
	}

	public override void OnInteractionView()
	{
		_inventory.AddEvidence(_evidenceItem);

		_uiManager.OnEvidenceClose();
		_uiManager.ShowMainCanvas();
		_uIAnimations.DialogueFadeIn();

		if (_dialogueVariableName != "")
		{
			DialogueLua.SetVariable(_dialogueVariableName, true);
		}

		_playerInteractionSystem.EndInteraction();
		_evidenceUIManager.RemoveButtonEvents();
		ClearEvidence();
	}
}