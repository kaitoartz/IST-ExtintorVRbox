using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onEnter;

    [SerializeField]
    private UnityEvent _onExit;

    private void OnTriggerEnter(Collider other)
    {
        _onEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        _onExit.Invoke();
    }
}
