using PixelCrushers.DialogueSystem;

public class DialogueEvidence : WorldEvidence
{
	public override void StartInspect()
	{
		base.StartInspect();

		UIAnimations.Instance.DialogueFadeOut();
	}

	public override void OnInteractionView()
	{
		_inventory.AddEvidence(_evidenceItem);

		UIManager.Instance.OnEvidenceClose();
		UIManager.Instance.ShowMainCanvas();
		UIAnimations.Instance.DialogueFadeIn();

		if (_dialogueVariableName != "")
		{
			DialogueLua.SetVariable(_dialogueVariableName, true);
		}

		PlayerInteractionSystem.Instance.EndInteraction();

		EvidenceUIManager.Instance.RemoveButtonEvents();
		ClearEvidence();
	}
}