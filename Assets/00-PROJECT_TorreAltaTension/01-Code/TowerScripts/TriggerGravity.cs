using Autohand;
using UnityEngine;

public class TriggerGravity : MonoBehaviour
{
    public GameObject autoHandPlayer;
    public bool useGravity = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            autoHandPlayer = other.gameObject;
            autoHandPlayer.GetComponent<AutoHandPlayer>().useGrounding = useGravity;
            Debug.Log("FlyingDrag = 0F;");
        }
    }
}
