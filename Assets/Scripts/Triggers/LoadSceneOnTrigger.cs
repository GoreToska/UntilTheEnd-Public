using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LoadSceneOnTrigger : MonoBehaviour
{
	[SerializeField] private string _sceneName;
	[SerializeField] private string _tempRusName;
	[SerializeField] private string _spawnPointName;

	[SerializeField] private bool _requireConfirmation;

	[Inject] private PromptManager _promptManager;
	[Inject] private UTESceneManager _sceneManager;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag != "Player")
			return;

		if (_requireConfirmation)
		{
			_promptManager.ActivatePromptLocation(gameObject);
			InputReader.InteractEvent += OnInteract;
		}
		else
		{
			OnInteract();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag != "Player")
			return;

		_promptManager.DeactivatePromptLocation();

		if (_requireConfirmation)
		{
			InputReader.InteractEvent -= OnInteract;
		}
	}

	private void OnInteract()
	{
		if (_spawnPointName != string.Empty)
		{
			InputReader.InteractEvent -= OnInteract;
			_sceneManager.LoadScene(_sceneName, _spawnPointName);
			//PixelCrushers.SaveSystem.LoadScene($"{_sceneName}@{_spawnPointName}");
			_promptManager.DeactivatePromptLocation();
		}
		else
		{
			_sceneManager.LoadScene(_sceneName);
			//PixelCrushers.SaveSystem.LoadScene(_sceneName);
			_promptManager.DeactivatePromptLocation();
		}
	}

	public string Name
	{
		get { return _tempRusName; }
	}
}
