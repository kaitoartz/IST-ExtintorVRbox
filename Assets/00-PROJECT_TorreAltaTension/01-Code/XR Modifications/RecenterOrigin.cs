using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RecenterOrigin : MonoBehaviour
{
    [SerializeField] private Transform head, origin, target;
    public InputActionProperty reCenterButton;

    public void Recenter()
    {
        Vector3 offset = head.position - origin.position;
        offset.y = 0;
        origin.position = target.position - offset;
        
        Vector3 targetForward = target.forward;
        targetForward.y = 0;
        Vector3 cameraForward = head.forward;
        cameraForward.y = 0;
        
        float angle = Vector3.SignedAngle(cameraForward, targetForward, Vector3.up);
        origin.RotateAround(head.position, Vector3.up, angle);
    }

    private void Start()
    {
        Recenter();
    }

    private void Update()
    {
        if (reCenterButton.action.WasPressedThisFrame())
        {
            Recenter();
        }
    }
}
