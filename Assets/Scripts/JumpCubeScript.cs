using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCubeScript : MonoBehaviour
{
    public PlayerManager playerManager;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Terrain")
            playerManager.canJump = true;
    }
}
