using System.Collections.Generic;
using UnityEngine;

public class HideObstacles : MonoBehaviour
{
    [SerializeField] private List<GameObject> _simpleObstacles;
    [SerializeField] private List<GameObject> _glassObstacles;
    [SerializeField] private float _hidingSpeed = 1f;
    private bool _hidingIsActive = false;
    private bool _showingIsActive = false;

    private void FixedUpdate()
    {
        if (_hidingIsActive)
            Hiding();

        if (_showingIsActive)
            Showing();
    }

    public void StartHiding()
    {
        _hidingIsActive = true;
        _showingIsActive = false;
    }

    public void StopHiding()
    {
        _hidingIsActive = false;
        _showingIsActive = true;
    }

    private void Hiding()
    {
        int count = 0;

        foreach (GameObject obstacle in _simpleObstacles)
        {
            if (HideObstacle(obstacle))
                count++;
        }

        foreach (GameObject glassObstacle in _glassObstacles)
        {
            if (HideObstacle(glassObstacle))
                count++;
        }

        if (count == _simpleObstacles.Count + _glassObstacles.Count)
            _hidingIsActive = false;
    }

    private void Showing()
    {
        int count = 0;

        foreach (GameObject obstacle in _simpleObstacles)
        {
            if (ShowObstacle(obstacle, 1.0f))
                count++;
        }

        foreach (GameObject glassObstacle in _glassObstacles)
        {
            if (ShowObstacle(glassObstacle, 0.3f))
                count++;
        }

        if (count == _simpleObstacles.Count + _glassObstacles.Count)
            _showingIsActive = false;
    }

    private bool HideObstacle(GameObject obstacle)
    {
        Material[] materials = obstacle.transform.GetComponent<Renderer>().materials;
        int count = 0;
        foreach (Material material in materials)
        {
            Color curColor = material.GetColor("_BaseColor");
            if (curColor.a > 0f)
            {
                curColor.a = Mathf.Clamp(curColor.a - _hidingSpeed * Time.deltaTime, 0f, 1f);
                material.SetColor("_BaseColor", curColor);
            }
            else
            {
                count++;
            }
        }

        if (count == materials.Length)
            return true;
        else
            return false;
    }

    private bool ShowObstacle(GameObject obstacle, float alphaMaxLimit)
    {
        Material[] materials = obstacle.transform.GetComponent<Renderer>().materials;
        int count = 0;
        foreach (Material material in materials)
        {
            Color curColor = material.GetColor("_BaseColor");
            if (curColor.a < alphaMaxLimit)
            {
                curColor.a = Mathf.Clamp(curColor.a + _hidingSpeed * Time.deltaTime, 0f, alphaMaxLimit);
                material.SetColor("_BaseColor", curColor);
            }
            else
            {
                count++;
            }
        }

        if (count == materials.Length)
            return true;
        else
            return false;
    }
}