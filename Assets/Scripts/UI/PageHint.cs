using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PageHint : MonoBehaviour
{
    [SerializeField] private Image _hintImage;
    private float _fadeTime = 0.2f;

    public void EnableHint()
    {
        AnimateOn(_hintImage);
    }

    public void DisableHint()
    {
        AnimateOff(_hintImage);
    }

    private void AnimateOff(Image image)
    {
        image.DOFade(0, _fadeTime).OnComplete(() => image.enabled = false);
    }

    private void AnimateOn(Image image)
    {
        image.enabled = true;
        image.DOFade(1, _fadeTime);
    }
}
