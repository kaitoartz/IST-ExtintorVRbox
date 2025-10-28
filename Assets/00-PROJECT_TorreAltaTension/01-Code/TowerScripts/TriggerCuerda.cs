using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCuerda : MonoBehaviour
{
    public PlayerRope _playerRope;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerRope.StartUsing();
        }
    }
}
