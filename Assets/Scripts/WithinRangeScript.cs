using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//checks to see if something is within range
public class WithinRangeScript : MonoBehaviour
{
    [SerializeField]
    enum CollisionCheckType
    {
        OnTriggerStay,
        OnTriggerEnter,
    }

    //the tag to check against in collision checking
    public string tagToCheck;

    //The type of checking this script will do
    [SerializeField]
    CollisionCheckType collisionCheckType = CollisionCheckType.OnTriggerStay;

    public bool disableOnExit = false;
    public UnityEvent withinRangeEvent;
    public UnityEvent exitRangeEvent;
    
    [HideInInspector]
    public GameObject eventEntity;

    private void OnTriggerEnter(Collider other)
    {
        if (collisionCheckType != CollisionCheckType.OnTriggerEnter
            || other.gameObject.tag != tagToCheck)
            return;
        eventEntity = other.gameObject;
        withinRangeEvent.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        if (collisionCheckType != CollisionCheckType.OnTriggerStay
            || other.gameObject.tag != tagToCheck)
            return;
        eventEntity = other.gameObject;
        withinRangeEvent.Invoke();

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != tagToCheck)
            return;
        exitRangeEvent.Invoke();
        eventEntity = null;
        if(disableOnExit)
            gameObject.SetActive(false);
    }
}
