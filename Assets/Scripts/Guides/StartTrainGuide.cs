using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class StartTrainGuide : MonoBehaviour
{
    [SerializeField] private List<GameObject> _pages;
    [SerializeField] private List<GameObject> _toggles;
    [SerializeField] private GameObject _onStartDialogueTrigger;
    [SerializeField] private GameObject _guideCanvas;
    [SerializeField] private UnityEngine.Rendering.Volume _volume;
    [SerializeField] private Toggle _firstToggle;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Sprite _activeImage;
    [SerializeField] private Sprite _defaultImage;

    private Sequence _sequence;
    private int _currentPage;
    private int _previousPage;

    private void Awake()
    {
        _sequence = DOTween.Sequence();
        _currentPage = 0;

        //��������� �������
        //_toggles[_currentPage].GetComponent<Image>().sprite = _activeImage;

        StartCoroutine(Wait());
    }

    public void EnableDialogue()
    {
        // depth off
        DOTween.To(() => _volume.weight, x => _volume.weight = x, 0, 1f);

        _onStartDialogueTrigger.SetActive(true);
        Destroy(_guideCanvas);
        //Destroy(this.gameObject);
    }

    public void SetVisible(int page)
    {
        if (page == _currentPage)
            return;

        _previousPage = _currentPage;
        _currentPage = page;

        _toggles[_previousPage].GetComponent<Image>().sprite = _defaultImage;
        _toggles[_currentPage].GetComponent<Image>().sprite = _activeImage;

        StartCoroutine(FadePage(1f));
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);

        _inputReader.DisableAllInput();

        yield break;
    }

    private IEnumerator FadePage(float speed)
    {
        _pages[_currentPage].GetComponent<Canvas>().enabled = true;

        _sequence.Kill();
        print(_sequence);

        _sequence.Append(_pages[_currentPage].GetComponent<CanvasGroup>().DOFade(1, speed));
        _sequence.Append(_pages[_previousPage].GetComponent<CanvasGroup>().DOFade(0, speed));
        print(_sequence);

        yield return _sequence.Play().WaitForCompletion();

        _pages[_previousPage].GetComponent<Canvas>().enabled = false;

        yield break;
    }
}
