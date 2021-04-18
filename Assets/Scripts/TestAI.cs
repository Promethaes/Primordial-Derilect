using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TestAI : MonoBehaviour
{
    public Transform destination;
    public NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
    }
    private void Update() {
        navMeshAgent.SetDestination(destination.position);
        
    }

}
