using UnityEngine;
using Zenject;

public class CameraTarget : MonoBehaviour
{
	[Inject] private StaticCharacterMovement _character;

	private void Update()
	{
		this.transform.position = _character.transform.position;
	}
}
