using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    SingleBulletFire gun;
    private void Start()
    {
        var guns = FindObjectsOfType<SingleBulletFire>();
        foreach (var g in guns)
            if (g.gameObject.activeSelf)
                gun = g;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.name.Contains("Bullet"))
        {
            gameObject.SetActive(false);
            if (other.gameObject.tag == "Enemy")
            {
                var enemy = other.gameObject.GetComponentInChildren<Enemy>();
                enemy.TakeDamage(gun.gunInfo.damage);
            }
        }
    }
}
