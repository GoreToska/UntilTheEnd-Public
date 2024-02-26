using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerRotation : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
