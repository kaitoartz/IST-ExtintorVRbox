using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RotateTween : MonoBehaviour
{
    public float angle;
    public float duration;
    private void Start()
    {
        Tween tween = gameObject.transform.DOLocalRotate(new Vector3(transform.localRotation.x, transform.localRotation.y, angle), duration, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart);
        tween.Play();
    }
}
