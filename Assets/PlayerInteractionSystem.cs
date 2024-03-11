using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteractionSystem : MonoBehaviour
{
	[HideInInspector] public static PlayerInteractionSystem Instance;

	private List<IInteractable> _interactables = new List<IInteractable>();
	private IInteractable _currentInteractable = null;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void OnEnable()
	{
		InputReader.InteractEvent += TryToInteract;
	}

	private void OnDisable()
	{
		InputReader.InteractEvent -= TryToInteract;
	}

	private void TryToInteract()
	{
		Debug.Log("Interact");
		if (_interactables.Count == 0 || _currentInteractable != null)
			return;

		_currentInteractable = _interactables[0];
		_currentInteractable.StartInteraction();
	}

	public void EndInteraction()
	{
		if (_currentInteractable == null)
			return;

		_interactables.Remove(_currentInteractable);
		_currentInteractable = null;
	}

	public void AddInteractable(IInteractable interactable)
	{
		if (_interactables.Contains(interactable))
			return;

		_interactables.Add(interactable);
	}

	public void RemoveInteractable(IInteractable interactable)
	{
		if (!_interactables.Contains(interactable))
			return;

		_interactables.Remove(interactable);
	}
}