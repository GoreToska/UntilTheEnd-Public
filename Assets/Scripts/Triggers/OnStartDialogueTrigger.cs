using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartDialogueTrigger : MonoBehaviour
{
    [SerializeField] private string _npcName;

    void Awake()
    {
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(1);
        GameObject.Find(_npcName).GetComponent<StartConversation>().Invoke("OpenDialogue", 0);
        Destroy(this.gameObject);
        
        yield break;
    }
}
