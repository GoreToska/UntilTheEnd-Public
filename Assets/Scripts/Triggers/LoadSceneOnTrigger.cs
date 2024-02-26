using UnityEngine;

public class LoadSceneOnTrigger : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private string _sceneName;
    [SerializeField] private string _tempRusName;
    [SerializeField] private string _spawnPointName;

    [SerializeField] private bool _requireConfirmation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        if (_requireConfirmation)
        {
            PromptManager.instance.ActivatePromptLocation(gameObject);
            _inputReader.InteractEvent += OnInteract;
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

        PromptManager.instance.DeactivatePromptLocation();

        if (_requireConfirmation)
        {
            _inputReader.InteractEvent -= OnInteract;
        }
    }

    private void OnInteract()
    {
        if (_spawnPointName != null)
        {
            _inputReader.InteractEvent -= OnInteract;
            PixelCrushers.SaveSystem.LoadScene($"{_sceneName}@{_spawnPointName}");
            PromptManager.instance.DeactivatePromptLocation();
        }
        else
        {
            PixelCrushers.SaveSystem.LoadScene(_sceneName);
            PromptManager.instance.DeactivatePromptLocation();
        }
    }

    public string Name
    {
        get { return _tempRusName; }
    }
}
