/*using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class AutoGrab : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private XRInteractionManager interactionManager;

    void Awake()
    {
        if (grabInteractable == null)
        {
            grabInteractable = GetComponent<XRGrabInteractable>();
            if (grabInteractable == null)
            {
                Debug.LogError("No se encontró XRGrabInteractable en el GameObject.", this);
            }
        }

        interactionManager = FindObjectOfType<XRInteractionManager>();

        if (interactionManager == null)
        {
            Debug.LogError("No se encontró un XRInteractionManager en la escena.", this);
        }
    }

    private void OnEnable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.hoverEntered.AddListener(OnHoverBegin);
        }
    }

    private void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.hoverEntered.RemoveListener(OnHoverBegin);
        }
    }

    private void OnHoverBegin(HoverEnterEventArgs args)
    {
        IXRSelectInteractor interactor = args.interactorObject as IXRSelectInteractor;

        if (interactor != null && interactionManager != null)
        {
            interactionManager.SelectEnter(interactor, grabInteractable);
        }
    }
}*/