using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PandCManager : MonoBehaviour
{
	[SerializeField] private LayerMask _pandcLayerMask;

	private Camera _camera;

	private void OnEnable()
	{
		_camera = Camera.main;
		InputReader.PointAndClick += PerformRatcast;
	}

	private void OnDisable()
	{
		InputReader.PointAndClick -= PerformRatcast;
	}

	private void PerformRatcast()
	{
		if (Physics.Raycast(_camera.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hitInfo, 1000f, _pandcLayerMask))
		{
			hitInfo.collider.GetComponent<IInteractable>().StartInteraction();
		}
	}
}
