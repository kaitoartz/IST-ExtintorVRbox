using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BoltInteraction : MonoBehaviour
{
    [Header("Control")]
    [Range(0, 90)]
    public float angle;
    public float angleValue;
    public float stableAngleValue;

    [Header("Feedback")]
    public AudioSource clickSound;
    public AudioSource usingSound;

    [Header("Grabbable Wrench")]
    public GameObject trueWrench;
    MeshRenderer meshTrueWrench;
    CapsuleCollider colTrueWrench;
    
    [Header("ClonedWrench")]
    public GameObject clonWrench;
    MeshRenderer meshClonWrench;
    public CapsuleCollider colClonWrench;

    [Header ("Gimmick Wrench")]
    public GameObject falseWrench;
    public MeshRenderer meshFalseWrench;
    public CapsuleCollider colFalseWrench;
    HingeJoint jointWrench;

    public UnityEvent onComplete;

    private void Awake()
    {
        trueWrench = GameObject.Find("WrenchTool");
        clonWrench = GameObject.Find("ClonWrench");
        meshTrueWrench = trueWrench.GetComponentInChildren<MeshRenderer>();
        colTrueWrench = trueWrench.GetComponentInChildren<CapsuleCollider>(); 
        jointWrench = falseWrench.GetComponent<HingeJoint>();
        meshClonWrench = clonWrench.GetComponentInChildren<MeshRenderer>();
        meshFalseWrench.enabled = false;
        colFalseWrench.isTrigger = true;
        meshClonWrench.enabled = false;
        colClonWrench.isTrigger = true;
        angleValue = jointWrench.limits.max;

    }
    private void OnEnable()
    {
        if (TickManager.Instance != null)
        {
            TickManager.Instance.OnVeryFastTick += VeryFastTick;
        }
    }
    private void OnDisable()
    {
        if (TickManager.Instance != null)
        {
            TickManager.Instance.OnVeryFastTick -= VeryFastTick;
        }
        CancelInvoke();
    }
    private void VeryFastTick()
    {
        if ((OnUsingWrench()) % 10 == 0)
        {
            clickSound.Play();
        }

        angle = Mathf.Round(jointWrench.angle);
        if (angle >= angleValue)
        {
            Debug.Log("Torque adjusted" + gameObject.name);
            ReturnWrench();
            onComplete.Invoke();
        }

    }
    //private void Update()
    //{
    //    if ((OnUsingWrench()) % 10 == 0)
    //    {
    //        clickSound.Play();
    //    }

    //    angle = Mathf.Round(jointWrench.angle);
    //    if (angle >= angleValue)
    //    {
    //        Debug.Log("Torque adjusted" + gameObject.name);
    //        ReturnWrench();
    //        onComplete.Invoke();
    //    }
    //}
    public float OnUsingWrench()
    {
        if (angle >= stableAngleValue) stableAngleValue = angle;
        return stableAngleValue;
    }
    public void ChangeWrench()
    {
        Debug.Log("Wrench has changed");
        usingSound.Play();
        meshTrueWrench.enabled = false;
        colTrueWrench.isTrigger = true;
        meshFalseWrench.enabled = true;
        colFalseWrench.isTrigger = false;
    }
    public void ReturnWrench()
    {
        Debug.Log("Task Completed");
        colClonWrench.isTrigger = false;
        meshClonWrench.enabled = true;
        meshFalseWrench.enabled = false;
        colFalseWrench.isTrigger = true;
    }
}
