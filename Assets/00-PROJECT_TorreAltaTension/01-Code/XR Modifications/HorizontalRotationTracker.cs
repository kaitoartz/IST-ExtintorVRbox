using UnityEngine;

public class HorizontalRotationTracker : MonoBehaviour
{

    [Tooltip("El transform principal (padre) del que se seguirá la rotación")] [SerializeField]
    private Transform parentTransform;

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
        Vector3 parentRotation = parentTransform.rotation.eulerAngles;
        Debug.Log("tick");
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x, 
            parentRotation.y, 
            transform.rotation.eulerAngles.z
        );
    }
}