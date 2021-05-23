using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoor : MonoBehaviour
{
    public GameObject doorLeft;
    public Transform lerpPointLeft;
    Vector3 originalDoorLeftPos;

    public GameObject doorRight;
    public Transform lerpPointRight;
    Vector3 originalDoorRightPos;

    public float lerpSpeed = 2.0f;


    private void Start()
    {
        originalDoorLeftPos = doorLeft.transform.localPosition;
        originalDoorRightPos = doorRight.transform.localPosition;
    }


    float u = 0.0f;
    IEnumerator OpenDoor()
    {
        u = 0.0f;
        while (u < 1.0f)
        {
            u += Time.deltaTime * lerpSpeed;

            doorLeft.transform.localPosition = Vector3.Lerp(originalDoorLeftPos, lerpPointLeft.localPosition, u);
            doorRight.transform.localPosition = Vector3.Lerp(originalDoorRightPos, lerpPointRight.localPosition, u);
            yield return null;
        }

    }

    IEnumerator CloseDoor()
    {
        u = 1.0f;
        while (u > 0.0f)
        {
            u -= Time.deltaTime* lerpSpeed;

            doorLeft.transform.localPosition = Vector3.Lerp(originalDoorLeftPos, lerpPointLeft.localPosition, u);
            doorRight.transform.localPosition = Vector3.Lerp(originalDoorRightPos, lerpPointRight.localPosition, u);
            yield return null;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StopCoroutine("CloseDoor");
            StartCoroutine("OpenDoor");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StopCoroutine("OpenDoor");
            StartCoroutine("CloseDoor");
        }
    }

}
