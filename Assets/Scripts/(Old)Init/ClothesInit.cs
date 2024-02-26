using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesInit : MonoBehaviour
{
    [SerializeField] private GameObject topPrefab;
    [SerializeField] private SkinnedMeshRenderer playerSkin;

    // Start is called before the first frame update
    void Start()
    {
        addClothes();
    }

    void addClothes()
    {
        SkinnedMeshRenderer[] renderers = topPrefab.GetComponents<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            renderer.bones = playerSkin.bones;
            renderer.rootBone = playerSkin.rootBone;
        }
    }
}
