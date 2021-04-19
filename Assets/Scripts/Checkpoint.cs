using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var playerManager = other.gameObject.GetComponentInParent<PlayerManager>();
            playerManager.lastTouchedCheckpoint = this;
        }
    }
}
