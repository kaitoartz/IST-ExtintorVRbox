using UnityEngine;
using UnityEngine.InputSystem;

namespace VRBoxCustom
{
    /// <summary>
    /// Control para mover al jugador usando física (Rigidbody) para un movimiento y feel más suave y realista.
    /// Simula el input de WASD pero guiado por el delta del control/mouse.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class VRBoxMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("Velocidad máxima de caminata.")]
        public float maxSpeed = 3.5f;
        
        [Tooltip("Qué tan rápido acelera hasta alcanzar la velocidad máxima.")]
        public float acceleration = 10f;
        
        [Tooltip("Qué tan rápido se detiene cuando sueltas el control.")]
        public float deceleration = 15f;
        
        [Header("References")]
        [Tooltip("La cámara principal para saber hacia dónde caminar.")]
        public Transform headCamera;

        private Rigidbody rb;
        private Vector2 inputDirection;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            
            // IMPORTANTÍSIMO: Evita que el CapsuleCollider se caiga al suelo (efecto "muñeco de trapo")
            rb.freezeRotation = true;
            
            // Si bien usamos físicas, queremos una respuesta limpia frente a colisiones continuas
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            inputDirection = Vector2.zero;

            // Leer Input (Mouse del control Chino)
            if (Mouse.current != null)
            {
                // Extraemos el desplazamiento
                Vector2 rawDelta = Mouse.current.delta.ReadValue();
                
                // Normalizamos el delta para que se comporte de 0 a 1 (como WASD o un Stick tradicional), 
                // asegurando velocidad estable sin importar si mueves el "ratón" lento o rápido.
                if (rawDelta.sqrMagnitude > 0.05f) 
                {
                    inputDirection = rawDelta.normalized;
                }
            }

            // Fallback para probar en PC con teclado (WASD)
            if (Keyboard.current != null)
            {
                if (Keyboard.current.wKey.isPressed) inputDirection.y += 1f;
                if (Keyboard.current.sKey.isPressed) inputDirection.y -= 1f;
                if (Keyboard.current.aKey.isPressed) inputDirection.x -= 1f;
                if (Keyboard.current.dKey.isPressed) inputDirection.x += 1f;
                
                if (inputDirection.sqrMagnitude > 1f) inputDirection.Normalize();
            }
        }

        void FixedUpdate()
        {
            Transform camTransform = headCamera != null ? headCamera : Camera.main.transform;
            
            // Calcular la dirección basada en dónde mira la cámara (evitando mirar al cielo/suelo)
            Vector3 forward = camTransform.forward;
            Vector3 right = camTransform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            // Meta de dirección y velocidad
            Vector3 targetDirection = (forward * inputDirection.y + right * inputDirection.x).normalized;
            Vector3 targetVelocity = targetDirection * maxSpeed;

            // Diferencia entre la velocidad meta y la actual (ignorar Eje Y para no afectar gravedad/saltos)
            Vector3 velocityChange = targetVelocity - rb.linearVelocity;
            velocityChange.y = 0f;

            // Decidir si aceleramos o frenamos
            float accelRate = (targetDirection.sqrMagnitude > 0.01f) ? acceleration : deceleration;
            
            // Aplicar la fuerza suavizada en el Rigidbody
            rb.AddForce(velocityChange * accelRate, ForceMode.Acceleration);
        }
    }
}
