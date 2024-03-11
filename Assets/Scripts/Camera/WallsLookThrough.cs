using System.Collections.Generic;
using UnityEngine;

public class WallsLookThrough : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private LayerMask _wallMask;
    private static int _CutHeightID = Shader.PropertyToID("_CutoffHeight");
    private static int _PlayerPosID = Shader.PropertyToID("_PlayerPosition");

    [SerializeField] private float _dissolveSpeed = 3f; // maybe must be counted from the size
    [SerializeField] private float _thresholdHeight = 0.1f;

    private RaycastHit[] _hitObjects;
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
        Vector3 offset = _target.transform.position - CameraStateManager.Instance.GetComponent<Camera>().transform.position;
        _hitObjects = Physics.RaycastAll(CameraStateManager.Instance.GetComponent<Camera>().transform.position, offset.normalized, offset.magnitude, _wallMask);

        for (int i = 0; i < _hitObjects.Length; ++i)
        {
            if (!_hitsObjects.ContainsKey(_hitObjects[i].collider.gameObject.name))
            {
                _hitsObjects.Add(_hitObjects[i].collider.gameObject.name, _hitObjects[i]);
            }

            Material[] materials = _hitObjects[i].transform.GetComponent<Renderer>().materials;
            for (int j = 0; j < materials.Length; ++j)
            {
                if (materials[j].HasFloat(_CutHeightID))
                {
                    float lowBorder = _hitObjects[i].transform.GetComponent<Collider>().bounds.min.y + _thresholdHeight;
                    float curCutoffHeight = materials[j].GetFloat(_CutHeightID);
                    materials[j].SetVector(_PlayerPosID, CheckPlayerPos());

                    if (curCutoffHeight > lowBorder)
                    {
                        materials[j].SetFloat(_CutHeightID, curCutoffHeight - _dissolveSpeed * Time.deltaTime);
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
            for (int i = 0; i < _hitObjects.Length; ++i)
            {
                if (dictVal.Key == _hitObjects[i].collider.gameObject.name)
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
                    if (materials[k].HasFloat(_CutHeightID))
                    {
                        float highBorder = dictVal.Value.transform.GetComponent<Renderer>().bounds.max.y + 0.2f;
                        // 0.2f - to be sure there is no invisible parts
                        float curCutoffHeight = materials[k].GetFloat(_CutHeightID);

                        if (curCutoffHeight < highBorder)
                        {
                            materials[k].SetFloat(_CutHeightID, curCutoffHeight + _dissolveSpeed * Time.deltaTime);
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
        return CameraStateManager.Instance.GetComponent<Camera>().WorldToViewportPoint(gameObject.transform.position);
    }
}