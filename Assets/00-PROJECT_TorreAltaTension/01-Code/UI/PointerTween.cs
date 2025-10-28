using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PointerTween : MonoBehaviour
{
    public float offset;
    private void Start()
    {
        Tween tween = gameObject.transform.DOLocalMoveY(gameObject.transform.localPosition.y + offset, 1f, false).SetLoops(-1, LoopType.Yoyo);
        tween.Play();
    }
}
