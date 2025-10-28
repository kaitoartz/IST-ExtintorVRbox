using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableTwoAttach : XRGrabInteractable
{
    public bool isProp = false;
    public bool isWrench = false;
    public Transform leftAttachTransform;
    public Transform rightAttachTransform;
    public Transform centralAttachTransform;

    bool isDropped;
    public Transform attach;
    public float timeLeft = 10f;

    public GameObject canvas;
    private void Update()
    {
        if (isDropped)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                isDropped = false;
                timeLeft = 10f;
                transform.position = attach.position;
            }
        }
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (isProp)
        {
            attachTransform = leftAttachTransform;
            if (args.interactorObject.transform.CompareTag("Left Hand"))
            {
                if (attachTransform != leftAttachTransform) attachTransform = leftAttachTransform;
                canvas.SetActive(true);
            }
            if (args.interactorObject.transform.CompareTag("Right Hand"))
            {
                this.Drop();
                return;
            }
            if (args.interactorObject.transform.CompareTag("Socket"))
            {
                attachTransform = leftAttachTransform;
                return;
            }
        }
        if(!isProp)
        {
            if (args.interactorObject.transform.CompareTag("Left Hand"))
            {
                if (attachTransform != leftAttachTransform) attachTransform = leftAttachTransform;
            }
            if (args.interactorObject.transform.CompareTag("Right Hand"))
            {
                if (attachTransform != rightAttachTransform) attachTransform = rightAttachTransform;
            }
            if (args.interactorObject.transform.CompareTag("Socket"))
            {
                attachTransform = centralAttachTransform;
            }
        }
        if (isWrench)
        {
            attachTransform = rightAttachTransform;
            if (args.interactorObject.transform.CompareTag("Right Hand"))
            {
                if (attachTransform != rightAttachTransform) attachTransform = rightAttachTransform;
                canvas.SetActive(true);
            }
            if (args.interactorObject.transform.CompareTag("Left Hand"))
            {
                this.Drop();
                return;
            }
            if (args.interactorObject.transform.CompareTag("Socket"))
            {
                attachTransform = rightAttachTransform;
                return;
            }
        }
        base.OnSelectEntered(args);
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        Debug.Log("Has been Dropped");
        if(canvas != null)canvas.SetActive(false);
        if (args.interactorObject as XRSocketInteractor)
        {
            XRSocketInteractor prop = args.interactorObject as XRSocketInteractor;
            attach = prop.attachTransform;
        }
        isDropped = true;
        base.OnSelectExited(args);
    }
    //protected override void OnHoverEntered(HoverEnterEventArgs args)
    //{
    //    if (args.interactorObject.transform.CompareTag("Left Hand"))
    //    {
    //        attachTransform = leftAttachTransform;
    //    }
    //    else if (args.interactorObject.transform.CompareTag("Right Hand"))
    //    {
    //        attachTransform = rightAttachTransform;
    //    }
    //    base.OnHoverEntered(args);
    //}
    //protected override void OnFocusEntered(FocusEnterEventArgs args)
    //{
    //    if (args.interactorObject.transform.CompareTag("Left Hand"))
    //    {
    //        attachTransform = leftAttachTransform;
    //    }
    //    if (args.interactorObject.transform.CompareTag("Right Hand"))
    //    {
    //        attachTransform = rightAttachTransform;
    //    }
    //    else if (args.interactorObject.transform.CompareTag("Socket"))
    //    {
    //        attachTransform = centralAttachTransform;
    //    }
    //    base.OnFocusEntered(args);
    //}
}
