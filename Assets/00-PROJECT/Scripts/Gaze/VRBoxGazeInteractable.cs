using UnityEngine;
using UnityEngine.Events;

namespace VRBoxCustom
{
    /// <summary>
    /// Attach this script to the 3D objects you want to interact with.
    /// Make sure the object is on the "Interactable" collision layer.
    /// </summary>
    public class VRBoxGazeInteractable : MonoBehaviour
    {
        [Header("Events (Drag & Drop Functions Here)")]
        [Tooltip("When the user first looks at the object")]
        public UnityEvent onGazeEnter;
        
        [Tooltip("When the user looks away from the object")]
        public UnityEvent onGazeExit;
        
        [Tooltip("When the gaze timer finishes (The 'Click' action)")]
        public UnityEvent onGazeInteract;

        // Visual Feedback for hovering (optional)
        private Renderer rend;
        private Color originalColor;
        [Header("Visual Feedback")]
        public bool colorChangeOnHover = true;
        public Color hoverColor = Color.yellow;

        void Start()
        {
            rend = GetComponent<Renderer>();
            if (rend != null) originalColor = rend.material.color;
        }

        public void GazeEnter()
        {
            if (colorChangeOnHover && rend != null) rend.material.color = hoverColor;
            onGazeEnter?.Invoke();
        }

        public void GazeExit()
        {
            if (colorChangeOnHover && rend != null) rend.material.color = originalColor;
            onGazeExit?.Invoke();
        }

        public void GazeInteract()
        {
            Debug.Log("[VR Gaze] Interactuated with: " + gameObject.name);
            onGazeInteract?.Invoke();
        }
    }
}
