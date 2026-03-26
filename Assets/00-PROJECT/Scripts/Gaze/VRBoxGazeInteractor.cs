using UnityEngine;
using UnityEngine.UI;

namespace VRBoxCustom
{
    /// <summary>
    /// Attach this script to your Main Camera.
    /// It casts a ray looking for objects in the "interactableLayer".
    /// </summary>
    public class VRBoxGazeInteractor : MonoBehaviour
    {
        [Header("Raycast Settings")]
        [Tooltip("Maximum distance to interact with objects.")]
        public float maxDistance = 10f;
        
        [Tooltip("The layer where your interactable objects are.")]
        public LayerMask interactableLayer;

        [Header("Gaze Settings")]
        [Tooltip("How many seconds the user must look at the object to click it.")]
        public float timeToInteract = 2f;
        
        [Header("UI Feedback (Optional)")]
        [Tooltip("Assign an Image (Filled -> Radial 360) here to see the loading progress.")]
        public Image reticleProgress;

        private VRBoxGazeInteractable currentInteractable;
        private float gazeTimer = 0f;

        void Update()
        {
            // Cast a ray from the camera's center forward
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            // Optional: Draw debug line in the Scene view
            Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);

            if (Physics.Raycast(ray, out hit, maxDistance, interactableLayer))
            {
                VRBoxGazeInteractable interactable = hit.collider.GetComponentInParent<VRBoxGazeInteractable>();
                
                if (interactable != null)
                {
                    if (currentInteractable != interactable)
                    {
                        // User started looking at a new interactable
                        if (currentInteractable != null) currentInteractable.GazeExit();
                        currentInteractable = interactable;
                        currentInteractable.GazeEnter();
                        gazeTimer = 0f;
                    }

                    // User is still looking at the object, add time
                    gazeTimer += Time.deltaTime;

                    // Update UI Reticle if assigned
                    if (reticleProgress != null)
                    {
                        reticleProgress.fillAmount = gazeTimer / timeToInteract;
                    }

                    // Trigger the Interaction!
                    if (gazeTimer >= timeToInteract)
                    {
                        currentInteractable.GazeInteract();
                        
                        // We reset the timer so it doesn't spam click.
                        // You can adjust this to your liking (e.g. only click once until looked away).
                        gazeTimer = 0f; 
                        if (reticleProgress != null) reticleProgress.fillAmount = 0;
                    }
                }
            }
            else
            {
                // Looking at nothing
                if (currentInteractable != null)
                {
                    currentInteractable.GazeExit();
                    currentInteractable = null;
                }
                
                gazeTimer = 0f;
                if (reticleProgress != null) reticleProgress.fillAmount = 0;
            }
        }
    }
}
