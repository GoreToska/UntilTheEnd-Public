using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _trees;
    [SerializeField] private List<GameObject> _transforms;

    private void Start()
    {
        StartCoroutine(SpawnTree());
    }

    private IEnumerator SpawnTree()
    {
        while (true)
        {
            var tree = Instantiate(_trees[Random.Range(0, _trees.Count)], _transforms[Random.Range(0, _transforms.Count)].transform);
            tree.transform.SetParent(null);

            yield return new WaitForSeconds(Random.Range(0.2f, 2f));
        }

    }
}
