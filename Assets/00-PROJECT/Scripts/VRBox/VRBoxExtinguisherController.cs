using UnityEngine;
using UnityEngine.InputSystem;

namespace VRBoxCustom
{
    /// <summary>
    /// Hace que el extintor siga la cabeza del jugador (cámara).
    /// Pulsa Clic Izquierdo o Derecho del Mouse para alternar (activar/desactivar) el disparo automático.
    /// </summary>
    public class VRBoxExtinguisherController : MonoBehaviour
    {
        [Header("Extinguisher Components")]
        [Tooltip("El modelo físico del extintor (el GameObject que debe seguir y ser visible).")]
        public Transform extinguisherModel;
        
        [Tooltip("El script ExtinguisherDischarge original para expulsar las partículas y audio.")]
        public ExtinguisherDischarge extinguisherDischarge;

        [Header("Follow Settings")]
        [Tooltip("La cámara principal del jugador para seguir su vista.")]
        public Transform headCamera;
        
        [Tooltip("Offset del extintor respecto a la cámara (X: derecha, Y: abajo, Z: adelante).")]
        public Vector3 positionOffset = new Vector3(0.3f, -0.4f, 0.5f);
        
        [Tooltip("Ajuste de rotación inicial. Si el modelo del extintor mira hacia un lado (ej: Y en 90 o 180), modifícalo aquí.")]
        public Vector3 rotationOffset = Vector3.zero;
        
        [Tooltip("Velocidad de seguimiento. Usa 0 para que no haya delay (seguimiento rígido).")]
        public float followSpeed = 12f;
        
        private bool isSprayingToggle = false;

        private void Update()
        {
            // Detectar clic de Mouse Izquierdo o Derecho para ALTERNAR el spray
            if (Mouse.current != null)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)
                {
                    ToggleSpray();
                }
            }
            
            // Si quieres también usar la tecla Espacio en tu PC para testear
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                ToggleSpray();
            }
            
            // Seguir la cámara con retraso (Delay hover)
            if (headCamera != null && extinguisherModel != null)
            {
                Vector3 targetPosition = headCamera.TransformPoint(positionOffset);
                
                // Combinar la rotación base de la cámara con el offset configurado por si el modelo 3D no está "derecho" por defecto
                Quaternion targetRotation = headCamera.rotation * Quaternion.Euler(rotationOffset);

                if (followSpeed > 0f)
                {
                    extinguisherModel.position = Vector3.Lerp(extinguisherModel.position, targetPosition, Time.deltaTime * followSpeed);
                    extinguisherModel.rotation = Quaternion.Slerp(extinguisherModel.rotation, targetRotation, Time.deltaTime * followSpeed);
                }
                else
                {
                    extinguisherModel.position = targetPosition;
                    extinguisherModel.rotation = targetRotation;
                }
            }
        }

        private void ToggleSpray()
        {
            isSprayingToggle = !isSprayingToggle;

            if (extinguisherDischarge != null)
            {
                if (isSprayingToggle)
                {
                    if (!extinguisherDischarge.isActiveAndEnabled)
                    {
                        extinguisherDischarge.gameObject.SetActive(true);
                    }
                    extinguisherDischarge.StartSpraying();
                }
                else
                {
                    extinguisherDischarge.StopSpraying();
                }
            }
        }
    }
}
