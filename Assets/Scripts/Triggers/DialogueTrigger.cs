using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _conversant;
    [SerializeField] private bool _destroyOnDialogue = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        _conversant.GetComponent<StartConversation>().Invoke("OpenDialogue", 0);

        if (_destroyOnDialogue)
            Destroy(this);
    }
}
