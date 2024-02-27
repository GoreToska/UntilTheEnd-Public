using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvidenceInfo : MonoBehaviour
{
    [HideInInspector] public static EvidenceInfo instance;

    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private float _animationSpeed = 0.2f;
    [SerializeField] private Image _image;  

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ShowInfo(string text, string name, Sprite portrait)
    {
        _infoText.text = text;
        _name.text = name;
        _image.sprite = portrait;
        
        UIAnimations.Ønstance.FadeCanvas(1f, GetComponent<Canvas>().gameObject);
    }

    public void HideInfo()
    {
        UIAnimations.Ønstance.FadeCanvas(0f, GetComponent<Canvas>().gameObject);
    }
}
