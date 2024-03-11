using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HighlightComponent : MonoBehaviour
{
	private float _fadeTime = 0.5f;
	private int _glowFactorID = Shader.PropertyToID("_GlowFactor");

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag != "Player")
			return;

		SetGlow(1, transform.root.gameObject);
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag != "Player")
			return;

		SetGlow(0, transform.root.gameObject);
	}

	public void SetGlow(float value, GameObject gObject)
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
