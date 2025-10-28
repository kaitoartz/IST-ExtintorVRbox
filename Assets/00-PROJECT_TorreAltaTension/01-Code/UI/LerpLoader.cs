using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class LerpLoader : MonoBehaviour
{
    private Image loader;
    //private Tween loaderTween;
    //private Tween colorTween;
    public Color colorCompleted;
    public Color colorStarted;
    [Range(0f , 10f)]
    public float tweenSpeed = 5f;
    public UnityEvent onLoadCompleteEvent;

    public void Awake()
    {
        loader = GetComponent<Image>();
        loader.fillAmount = 0;
    }
    private void Update()
    {
        bool isDone;
        if (loader.fillAmount == 1)
        {
            isDone = true;
            if (isDone)
            {
                onLoadCompleteEvent.Invoke();
                loader.fillAmount = 0;
                isDone = false;
            }

        }
    }
    public void LerpFillerStart()
    {
        loader.DOFillAmount(1, tweenSpeed);
        loader.DOColor(colorCompleted, tweenSpeed);
        //if(!loaderTween.IsPlaying())
        //{
        //    loaderTween = loader.DOFillAmount(1, tweenSpeed);
        //    colorTween = loader.DOColor(colorCompleted, tweenSpeed);
        //    loaderTween.Play();
        //    colorTween.Play();
        //}
        //loaderTween.OnComplete(() => onLoadCompleteEvent.Invoke());
    }
    public void LerpFillerEnd()
    {
        if(loader.fillAmount != 1)
        {
            loader.DOFillAmount(0, tweenSpeed);
            loader.DOColor(colorStarted, tweenSpeed);
        }
        //loaderTween = loader.DOFillAmount(0, tweenSpeed);
        //colorTween = loader.DOColor(colorStarted, tweenSpeed);
        //loaderTween.Play();
        //colorTween.Play();
    }
}
