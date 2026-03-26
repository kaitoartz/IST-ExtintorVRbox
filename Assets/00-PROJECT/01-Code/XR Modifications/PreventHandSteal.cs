/*using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

// Este script hace que el objeto sea "fiel": no se va con otra mano si ya lo tienen agarrado.
public class PreventHandSteal : MonoBehaviour, IXRSelectFilter
{
    private IXRSelectInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<IXRSelectInteractable>();
    }

    public bool canProcess => true;

    public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        return CanProcessSelect(interactor);
    }

    public bool CanProcessSelect(IXRSelectInteractor interactor)
    {
        if (!interactable.isSelected)
        {
            return true;
        }

        bool isMyCurrentOwner = interactable.interactorsSelecting.Contains(interactor);

        return isMyCurrentOwner;
    }
}*/