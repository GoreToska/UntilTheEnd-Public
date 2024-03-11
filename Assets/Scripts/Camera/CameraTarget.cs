using UnityEngine;

public class CameraTarget : MonoBehaviour
{
	[HideInInspector] public static CameraTarget Instance;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		DontDestroyOnLoad(gameObject);
	}
}
