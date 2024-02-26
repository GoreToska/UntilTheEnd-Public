using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyNPCTrigger : MonoBehaviour
{
    [SerializeField] private string _name ="";

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == _name)
        {
            Destroy(other.gameObject);
            Destroy(this);
        }
    }
}
