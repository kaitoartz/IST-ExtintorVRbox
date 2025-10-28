using UnityEngine;
using DG.Tweening;

public class MoveAttachPointFollowingPlayer : MonoBehaviour
{
    [Header("Testing")]
    public bool isExact;
    public float playerZPOS;
    public bool active;
    [Header("References")]
    //public XRSocketInteractor hookSocket;
    public Transform attachSocketTransform;
    public Transform player;
    public float offset;

    [Header("Z Follow Up Limits")]
    [Range(0, 10)] public float maxLimit;
    [Range(0, -10)] public float minLimit;

    private void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }
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
        if (active)
        {

            playerZPOS = ZPOS(isExact);
            if (Mathf.Round(player.localPosition.z) <= minLimit || Mathf.Round(player.localPosition.z) >= maxLimit) return;
            if (playerZPOS == 0) return;
            attachSocketTransform.DOLocalMoveZ(playerZPOS + offset, 1f, false);
        }
    }
    //void Update()
    //{
    //    if (active)
    //    {

    //        playerZPOS = ZPOS(isExact);
    //        //if (isTracking)
    //        //{
    //        if (Mathf.Round(player.localPosition.z) <= minLimit || Mathf.Round(player.localPosition.z) >= maxLimit) return;
    //        if (playerZPOS == 0) return;
    //        //Mathf.Lerp(attachSocketTransform.position.z, Mathf.Round(player.gameObject.transform.position.z), 0.1f);
    //        attachSocketTransform.DOLocalMoveZ(playerZPOS+offset, 1f, false);
    //        //}
    //        //else attachSocketTransform.position.Set(attachSocketTransform.position.x,0,0);
    //    }
    //}

    public float ZPOS(bool isExact)
    {
        float result;
        if (isExact) result = player.localPosition.z;
        else result = Mathf.Round(player.localPosition.z);
        return result;
    }

    public void isActive()
    {
        active = true;
    }
}
