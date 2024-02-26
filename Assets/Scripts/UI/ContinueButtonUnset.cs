using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContinueButtonUnset : MonoBehaviour
{
    private void Awake()
    {
        FixAnimation();
    }

    public void FixAnimation()
    {
        Animator animator = GetComponent<Animator>();
        animator.CrossFade("Normal", 0f);
        animator.Update(0f);
    }
}
