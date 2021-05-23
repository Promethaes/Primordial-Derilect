using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirlockManager : MonoBehaviour
{
    public static AirlockManager airlockManager = null;
    public GameObject AirlockPrefab;
    public GameObject StartDoor;

    void Start()
    {
        if (airlockManager == null)
            airlockManager = this;

    }


    public void PlaceAirlock(Quaternion rotation, Vector3 position)
    {
        StartDoor.SetActive(false);
        AirlockPrefab.transform.rotation = rotation;
        AirlockPrefab.transform.Rotate(new Vector3(0.0f,180.0f,0.0f),Space.World);
        AirlockPrefab.transform.position = position;
        AirlockPrefab.SetActive(true);
    }


}
