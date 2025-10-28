using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RespawnEPP : MonoBehaviour
{
    [SerializeField] private bool isGrabbed;
    [SerializeField] private float toRespawn;
    private bool isInFloor;
    float timerToRespawn;
    Vector3 socket;
    Quaternion rotation;
    
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip respawnSound;
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (toRespawn == 0) toRespawn = 1;
        socket = transform.position;
        rotation=transform.rotation;
        timerToRespawn=Mathf.Infinity;
    }

    private void OnEnable()
    {
        if (TickManager.Instance!=null)
        {
            TickManager.Instance.OnFastTick += FastTick;
        }
    }
    private void OnDisable()
    {
        if (TickManager.Instance != null)
        {
            TickManager.Instance.OnFastTick -= FastTick;
        }
        CancelInvoke();
    }
    void Update()
    {
        if (isInFloor&&!isGrabbed) 
        {
            timerToRespawn -= Time.deltaTime;
        }
        //if (timerToRespawn < 0)
        //{
        //    _audioSource.PlayOneShot(respawnSound);
        //    isInFloor = false;
        //    timerToRespawn=Mathf.Infinity;
        //    transform.rotation=rotation;
        //    GetComponent<Rigidbody>().position = socket;
        //    GetComponent<Rigidbody>().velocity=Vector3.zero;
        //    GetComponent<Rigidbody>().angularVelocity=Vector3.zero;
        //}
    }
    private void FastTick()
    {
        if (timerToRespawn < 0)
        {
            _audioSource.PlayOneShot(respawnSound);
            isInFloor = false;
            timerToRespawn = Mathf.Infinity;
            transform.rotation = rotation;
            GetComponent<Rigidbody>().position = socket;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
    public void ActiveGrab()
    {
        isGrabbed = true;
    }
    public void DeactivateGrab()
    {
        isGrabbed = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isGrabbed)
        {
            if (other.tag == "RespawnFloor")
            {
                Debug.Log("To respawn " + name);
                isInFloor = true;
                timerToRespawn = toRespawn;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isGrabbed)
        {
            if (collision.gameObject.tag == "RespawnFloor")
            {
                Debug.Log("To respawn " + name);
                isInFloor = true;
                timerToRespawn = toRespawn;
            }
        }
    }
}