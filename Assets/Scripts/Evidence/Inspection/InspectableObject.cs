using UnityEngine;

public class InspectableObject : MonoBehaviour
{
    [Header("Initial Settings")]
    public Vector3 spawnPositionOffset;
    public Vector3 spawnRotationOffset;
    public Vector2 minMaxZoomZ;
    public Vector2 minMaxZoomX;
    public float defaultZoomValue = 1f;

    [SerializeField] private string _name;
    [SerializeField] private string _description;

    public string Name
    {
        get { return _name; }
    }

    public string Description
    {
        get { return _description; }
    }
}