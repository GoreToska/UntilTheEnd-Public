using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLook : MonoBehaviour
{
    [SerializeField] private List<Material> _circleMaterials;
    [SerializeField] private float _maxRadius = 1.0f;
    [SerializeField] private float _minRadius = 0f;
    [SerializeField] private float _changeRadiusSpeed = 4.0f;

    [SerializeField] CameraInterim cameraInterim;

    private void OnEnable()
    {
        cameraInterim.DialogueActiveEvent += ActDialoguePos;
        cameraInterim.DialogueDisactiveEvent += DisactDialoguePos;
    }

    private void OnDisable()
    {
        cameraInterim.DialogueActiveEvent -= ActDialoguePos;
        cameraInterim.DialogueDisactiveEvent -= DisactDialoguePos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        foreach (Material mat in _circleMaterials)
        {
            StartCoroutine(ChangeRadiusUp(_maxRadius, mat));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        foreach (Material mat in _circleMaterials)
        {
            StartCoroutine(ChangeRadiusDown(_minRadius, mat));
        }
    }

    private IEnumerator ChangeRadiusUp(float value, Material mat)
    {
        float currentRadius = mat.GetFloat("_Radius");
        while (currentRadius < value)
        {
            mat.SetFloat("_Radius",
                currentRadius + Time.deltaTime * _changeRadiusSpeed);
            currentRadius = mat.GetFloat("_Radius");

            yield return new WaitForSeconds(Time.deltaTime);
        }

        mat.SetFloat("_Radius", value);
        yield break;
    }

    private IEnumerator ChangeRadiusDown(float value, Material mat)
    {
        float currentRadius = mat.GetFloat("_Radius");
        while (currentRadius > value)
        {
            mat.SetFloat("_Radius",
                currentRadius - Time.deltaTime * _changeRadiusSpeed);
            currentRadius = mat.GetFloat("_Radius");

            yield return new WaitForSeconds(Time.deltaTime);
        }

        mat.SetFloat("_Radius", value);
        yield break;
    }

    // ѕозици€ перса, ее относительное местоположение с учетом разрешени€ экрана
    #region ForChangingCamera
    private float dialogueX = 0.72f;
    private float dialogueY = 0.50f;
    private float defaultX = 0.50f;
    private float defaultY = 0.40f;
    private float changeSpeed = 0.1f;
    private static int TargetPosID = Shader.PropertyToID("_PlayerPosition");

    private void ActDialoguePos()
    {
        StartCoroutine(ChangePositionToDialogue(changeSpeed, dialogueX, dialogueY));
    }

    private void DisactDialoguePos()
    {
        StartCoroutine(ChangePositionToDialogue(-changeSpeed, defaultX, defaultY));
    }

    private IEnumerator ChangePositionToDialogue(float speed, float resX, float resY)
    {
        int readyMats = 0;
        while(readyMats != _circleMaterials.Count)
        {
            foreach (Material mat in _circleMaterials)
            {
                Vector2 curPos = mat.GetVector(TargetPosID);
                if(Mathf.Abs(curPos.x - resX) > 0.005)
                {
                    curPos.x += speed * Time.deltaTime;
                }
                if (Mathf.Abs(curPos.y - resY) > 0.005)
                {
                    curPos.y += speed * Time.deltaTime;
                }

                Vector2 old = mat.GetVector(TargetPosID);
                if (curPos == old)
                {
                    readyMats++;
                    mat.SetVector(TargetPosID, new Vector2(resX, resY));
                }
                else
                {
                    mat.SetVector(TargetPosID, curPos);
                }
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield break;
    }
    #endregion
}