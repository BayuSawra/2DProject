using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;



public class FadeCanvas : MonoBehaviour
{
    [Header("监听事件")]
    public FadeEventSO fadeEvent;
    public Image fadeImage;

    void OnEnable()
    {
        fadeEvent.OnEventRaised += OnFadeEvent;
    }

    void OnDisable()
    {
        fadeEvent.OnEventRaised -= OnFadeEvent;
    }
    private void OnFadeEvent(Color target, float duration, bool fadeIn)
    {
        fadeImage.DOBlendableColor(target, duration);
    }

}
