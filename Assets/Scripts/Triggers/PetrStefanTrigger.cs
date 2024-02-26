using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class PetrStefanTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(DialogueLua.GetVariable("PetrIsHelping").asBool == true)
            {
                Destroy(GameObject.Find("PetrAkimovNPC"));
            }

            if (DialogueLua.GetVariable("StefanIsHelping").asBool == true)
            {
                Destroy(GameObject.Find("StefanNPC"));
            }

            if (DialogueLua.GetVariable("StefanIsHelping").asBool == true ||
                DialogueLua.GetVariable("PetrIsHelping").asBool == true)
            {
                Destroy(GameObject.Find("EngineerNPC"));
            }
        }
    }
}
