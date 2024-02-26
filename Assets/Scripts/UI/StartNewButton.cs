using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartNewButton : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    public void StartNew()
    {
        _inventory.ClearInventory();
        StartCoroutine(ReloadSceneAsync());
    }

    IEnumerator ReloadSceneAsync()
    {
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(
            SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);

        while (!loadAsync.isDone)
        {
            Debug.Log("Loading...");
            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }
}
