using UnityEngine;
using UnityEngine.InputSystem;

public class PandCManager : MonoBehaviour
{
	[SerializeField] private LayerMask _pandcLayerMask;

	private Camera _camera;

	private void OnEnable()
	{
		_camera = Camera.main;
		InputReader.PointAndClick += PerformRaycast;
	}

	private void OnDisable()
	{
		InputReader.PointAndClick -= PerformRaycast;
	}

	private void PerformRaycast()
	{
		if (Physics.Raycast(_camera.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hitInfo, 1000f, _pandcLayerMask))
		{
			hitInfo.collider.GetComponent<IInteractable>().StartInteraction();
		}
	}
}
