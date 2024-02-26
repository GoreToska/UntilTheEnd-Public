using System.Collections.Generic;
using UnityEngine;

public class WallsLookThrough : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask wallMask;
    private static int CutHeightID = Shader.PropertyToID("_CutoffHeight");
    private static int PlayerPosID = Shader.PropertyToID("_PlayerPosition");

    [SerializeField] private float dissolveSpeed = 3f; // maybe must be counted from the size
    [SerializeField] private float thresholdHeight = 0.1f;

    private RaycastHit[] hitObjects;
    private Dictionary<string, RaycastHit> _hitsObjects = new Dictionary<string, RaycastHit>();

    // TODO: Optimize
    // If it's already cut - dont't check it again
    private void FixedUpdate()
    {
        CheckWalls();
        UnCheckWalls();
    }

    private void CheckWalls()
    {
        Vector3 offset = target.transform.position - CameraStateManager.Ønstance.GetComponent<Camera>().transform.position;
        hitObjects = Physics.RaycastAll(CameraStateManager.Ønstance.GetComponent<Camera>().transform.position, offset.normalized, offset.magnitude, wallMask);

        for (int i = 0; i < hitObjects.Length; ++i)
        {
            if (!_hitsObjects.ContainsKey(hitObjects[i].collider.gameObject.name))
            {
                _hitsObjects.Add(hitObjects[i].collider.gameObject.name, hitObjects[i]);
            }

            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;
            for (int j = 0; j < materials.Length; ++j)
            {
                if (materials[j].HasFloat(CutHeightID))
                {
                    float lowBorder = hitObjects[i].transform.GetComponent<Collider>().bounds.min.y + thresholdHeight;
                    float curCutoffHeight = materials[j].GetFloat(CutHeightID);
                    materials[j].SetVector(PlayerPosID, CheckPlayerPos());

                    if (curCutoffHeight > lowBorder)
                    {
                        materials[j].SetFloat(CutHeightID, curCutoffHeight - dissolveSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    Color curColor = materials[j].GetColor("_BaseColor");
                    if (curColor.a > 0)
                        curColor.a = 0;

                    materials[j].SetColor("_BaseColor", curColor);
                }
            }
        }
    }

    private void UnCheckWalls()
    {
        bool check;
        foreach (KeyValuePair<string, RaycastHit> dictVal in _hitsObjects)
        {
            check = true;
            for (int i = 0; i < hitObjects.Length; ++i)
            {
                if (dictVal.Key == hitObjects[i].collider.gameObject.name)
                {
                    check = false;
                    break;
                }
            }

            if (check)
            {
                Material[] materials = dictVal.Value.transform.GetComponent<Renderer>().materials;
                for (int k = 0; k < materials.Length; ++k)
                {
                    if (materials[k].HasFloat(CutHeightID))
                    {
                        float highBorder = dictVal.Value.transform.GetComponent<Renderer>().bounds.max.y + 0.2f;
                        // 0.2f - to be sure there is no invisible parts
                        float curCutoffHeight = materials[k].GetFloat(CutHeightID);

                        if (curCutoffHeight < highBorder)
                        {
                            materials[k].SetFloat(CutHeightID, curCutoffHeight + dissolveSpeed * Time.deltaTime);
                        }
                        else
                        {
                            _hitsObjects.Remove(dictVal.Key);
                            return;
                        }
                    }
                    else
                    {
                        Color curColor = materials[k].GetColor("_BaseColor");
                        if (curColor.a < 1)
                            curColor.a = 1;

                        materials[k].SetColor("_BaseColor", curColor);
                    }
                }
            }
        }
    }

    private Vector2 CheckPlayerPos()
    {
        return CameraStateManager.Ønstance.GetComponent<Camera>().WorldToViewportPoint(gameObject.transform.position);
    }
}