using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RecenterOrigin : MonoBehaviour
{
    [Tooltip("El objeto raíz del sistema XR (XR Rig / Camera Offset)")]
    public Transform xrRig;
    [Tooltip("La cámara principal (Main Camera)")]
    public Transform headCamera;
    [Tooltip("El punto exacto donde debe aparecer la cámara (Opcional, usa este objeto si está vacío)")]
    public Transform targetOrigin;

    void Start()
    {
        StartCoroutine(RecenterRoutine());
    }

    IEnumerator RecenterRoutine()
    {
        // Esperamos a que inicie el tracking
        yield return new WaitForSeconds(0.5f);

        // 1. Recentrado Nativo (Legacy)
        UnityEngine.XR.InputTracking.Recenter();

        // 2. Recentrado Nativo (Subsystems - Moderno)
        List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);
        foreach (var s in subsystems)
        {
            s.TryRecenter();
        }

        // Esperamos un frame para que el sistema aplique los cambios
        yield return null;

        // 3. Corrección Manual (La "A prueba de balas")
        PerformManualAlignment();

        Debug.Log("Listo el pollo: Cámara recentrada y alineada manualmente. 🗿");
    }

    void PerformManualAlignment()
    {
        // Autocompletar referencias si faltan
        if (headCamera == null) headCamera = Camera.main != null ? Camera.main.transform : null;
        if (xrRig == null && headCamera != null) xrRig = headCamera.parent;
        if (targetOrigin == null) targetOrigin = this.transform;

        if (xrRig != null && headCamera != null)
        {
            // A. Alineación de Rotación (Solo eje Y)
            // Rotamos el Rig para que la mirada de la cámara coincida con la del target
            float currentY = headCamera.eulerAngles.y;
            float targetY = targetOrigin.eulerAngles.y;
            float diffY = targetY - currentY;

            xrRig.Rotate(0, diffY, 0);

            // B. Alineación de Posición (Solo X y Z para respetar la altura del suelo)
            // Calculamos cuánto falta para llegar al target desde la cámara
            Vector3 currentPos = headCamera.position;
            Vector3 targetPos = targetOrigin.position;
            Vector3 diffPos = targetPos - currentPos;

            // Aplicamos el desplazamiento al Rig (manteniendo su altura original)
            xrRig.position += new Vector3(diffPos.x, 0, diffPos.z);
        }
    }
}