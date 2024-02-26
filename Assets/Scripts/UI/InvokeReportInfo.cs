using UnityEngine;
using UnityEngine.EventSystems;

public class InvokeReportInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string _personName;
    private string _reportText;
    private Sprite _portrait;

    public void SetData(string text, string name, Sprite portrait)
    {
        _reportText = text;
        _personName = name;
        _portrait = portrait;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EvidenceInfo.instance.ShowInfo(_reportText, _personName, _portrait);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EvidenceInfo.instance.HideInfo();
    }
}
