using UnityEngine;
using UnityEngine.UI;

public class SelectedHandler : MonoBehaviour
{
    public void RadioButtonListener()
    {
        if (GetComponent<Toggle>().isOn)
        {
            GetComponent<Animator>().SetTrigger("Selected");
        }
    }
}
