using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonHint : MonoBehaviour
{
    private float _fadeTime = 0.2f;

    public void DisableHint()
    {
        var hint = this.transform.GetChild(1).GetComponent<Image>();
        if (hint)
        {
            Animate(hint);
        }
    }

    private void Animate(Image image)
    {
        image.DOFade(0, _fadeTime).OnComplete(() => DeleteHint(image));
    }

    private void DeleteHint(Image image)
    {
        Destroy(image);
        Destroy(this);
    }
}
