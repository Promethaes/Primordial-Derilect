using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeBox : MonoBehaviour
{
    public bool withinRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            withinRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            withinRange = false;
    }
}
