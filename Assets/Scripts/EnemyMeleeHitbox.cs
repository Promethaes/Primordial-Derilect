using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeHitbox : MonoBehaviour
{
    public Enemy enemy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemy.DealDamage();
            gameObject.SetActive(false);
        }
    }
}
