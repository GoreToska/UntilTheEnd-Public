using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDataHolder : MonoBehaviour
{
    [SerializeField] public EnvironmentObjectData Data;

    private float _fadeTime = 0.5f;
    private int _glowFactorID = Shader.PropertyToID("_GlowFactor");

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        SetGlow(1, this.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        SetGlow(0, this.gameObject);
    }

    public void Destruct()
    {
        SetGlow(0, this.gameObject);
        Destroy(this);
    }

    private void SetGlow(float value, GameObject gObject)
    {
        Transform[] allChildren = gObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in allChildren)
        {
            Renderer childRenderer = child.gameObject.GetComponent<Renderer>();
            if (childRenderer)
            {
                foreach (Material mat in childRenderer.materials)
                {
                    mat.SetFloat(_glowFactorID, value);
                }
            }
        }
    }
}
