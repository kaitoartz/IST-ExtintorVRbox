using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRope : MonoBehaviour
{
    [SerializeField]GameObject bone;
    [SerializeField]Renderer rope;
    public enum RopeState
    {
        None, Using
    }
    [SerializeField] private RopeState state;
    void Start()
    {
        rope = GetComponentInChildren<Renderer>();
    }
    private void OnEnable()
    {
        if (TickManager.Instance != null)
        {
            TickManager.Instance.OnFastTick += FastTick;
        }
    }
    private void OnDisable()
    {
        if (TickManager.Instance != null)
        {
            TickManager.Instance.OnFastTick -= FastTick;
        }
        CancelInvoke();
    }
    private void FastTick()
    {
        switch (state)
        {
            case RopeState.None:
                rope.GetComponent<Renderer>().enabled = true;
                break;
            case RopeState.Using:
                rope.GetComponent<Renderer>().enabled = false;
                break;
            default:
                break;
        }
    }
    //void Update()
    //{
    //    switch (state)
    //    {
    //        case RopeState.None:
    //            rope.GetComponent<Renderer>().enabled=true;
    //            break;
    //        case RopeState.Using:
    //            rope.GetComponent<Renderer>().enabled=false;
    //            break;
    //        default:
    //            break;
    //    }
    //}
    public void StartUsing()
    {
        state = RopeState.Using;
        HideEverything(bone);
    }
    public void QuitUsing()
    {
        state=RopeState.None;
        ShowEverything(bone);
    }
    void HideEverything(GameObject toHide)
    {
        if (toHide.GetComponentInChildren<GameObject>() != null) 
            if (toHide.GetComponentInChildren<Collider>() != null)
            {
                HideEverything(toHide.GetComponentInChildren<GameObject>().gameObject);
            }
            else HideEverything(toHide.GetComponentInChildren<GameObject>().gameObject.GetComponentInChildren<GameObject>().gameObject);
        toHide.GetComponent<Collider>().enabled = false;
    }
    void ShowEverything(GameObject toHide)
    {
        if (toHide.GetComponentInChildren<GameObject>() != null) ShowEverything(toHide.GetComponentInChildren<GameObject>());
        toHide.GetComponent<Collider>().enabled = true;
    }
}
