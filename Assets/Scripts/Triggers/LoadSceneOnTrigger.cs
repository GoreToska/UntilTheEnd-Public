using UnityEngine;
using Zenject;

public class LoadSceneOnTrigger : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private string _sceneName;
    [SerializeField] private string _tempRusName;
    [SerializeField] private string _spawnPointName;

    [SerializeField] private bool _requireConfirmation;

    [Inject] private PromptManager _promptManager;

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
        if (_spawnPointName != null)
        {
			InputReader.InteractEvent -= OnInteract;
            PixelCrushers.SaveSystem.LoadScene($"{_sceneName}@{_spawnPointName}");
            _promptManager.DeactivatePromptLocation();
        }
        else
        {
            PixelCrushers.SaveSystem.LoadScene(_sceneName);
            _promptManager.DeactivatePromptLocation();
        }
    }

    public string Name
    {
        get { return _tempRusName; }
    }
}
