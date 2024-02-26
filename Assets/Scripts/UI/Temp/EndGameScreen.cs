using UnityEngine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;

public class EndGameScreen : MonoBehaviour
{
    private void Start()
    {
        Lua.RegisterFunction("ShowEndGame", this, SymbolExtensions.GetMethodInfo(() => ShowEndGame()));
    }

    private void ShowEndGame()
    {
        GetComponent<Canvas>().enabled = true;
        GetComponent<CanvasGroup>().DOFade(1, 3);
    }
}
