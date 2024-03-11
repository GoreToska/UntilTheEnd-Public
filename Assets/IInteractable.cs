public interface IInteractable
{
	public void StartInteraction();
}

public interface IViewableInteractable : IInteractable
{
	public void OnInteractionView();
}