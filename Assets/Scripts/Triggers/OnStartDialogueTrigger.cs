using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartDialogueTrigger : MonoBehaviour
{
    [SerializeField] private string _npcName;

    void Start()
    {
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(1);
        GameObject.Find(_npcName).GetComponent<StartConversation>().StartInteraction();
        Destroy(this.gameObject);
        
        yield break;
    }
}
