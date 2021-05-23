using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlacement : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> Rooms;
    
    public bool placeRandomRoomAtStart = false;
    private void Start()
    {
        Rooms = new List<GameObject>();
        var rooms = FindObjectsOfType<Room>();
        foreach (var r in rooms){
            Rooms.Add(r.gameObject);
            //r.gameObject.SetActive(false);
        }

        if (placeRandomRoomAtStart)
            PlaceRandomRoom();
        
    }

    public void PlaceRandomRoom()
    {
        foreach (var r in Rooms)
            r.SetActive(false);

        int index = Random.Range(0, Rooms.Count);

        Rooms[index].transform.rotation = Quaternion.identity;

        var rot = gameObject.transform.parent.rotation;
        Rooms[index].transform.rotation = rot ;
        Rooms[index].transform.position = gameObject.transform.position;

        Rooms[index].SetActive(true);
    }

    public void PlaceAirlock(){
        AirlockManager.airlockManager.PlaceAirlock(gameObject.transform.rotation,gameObject.transform.position);
    }
}
