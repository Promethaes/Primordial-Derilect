using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemyDataScriptableObject enemyData;
    public NavMeshAgent navMeshAgent;
    public float currentHP;
    public GameObject meleeHitbox;
    public AttackRangeBox attackRangeBox;
    public AttackRangeBox sightRangeBox;

    PlayerManager playerManager = null;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = enemyData.hp;
        if (playerManager == null)
            playerManager = FindObjectOfType<PlayerManager>();
    }

    public void DisableMeleeHitbox()
    {
        if (enemyData.isRanged)
            return;
        meleeHitbox.SetActive(false);
    }
    public void Attack()
    {
        //come back to this later
        if (!enemyData.isRanged)
        {
            meleeHitbox.SetActive(true);
        }
    }

    //maybe add armor?
    public void TakeDamage(float damage){
        currentHP -= damage;
        if(currentHP <= 0.0f)
            gameObject.gameObject.SetActive(false);
    }

    IEnumerator DieOnAttack()
    {
        yield return new WaitForSeconds(1.0f);//make this editor exposed?
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void DealDamage()
    {
        float damageValue = enemyData.damage;
        var direction = gameObject.transform.position - playerManager.pRigidBody.position;
        if (enemyData.hasExplosiveDamageFalloff)
            damageValue = enemyData.damage / (direction.magnitude / enemyData.explosiveDamageFalloff);


        if (enemyData.hasKnockback)
            playerManager.pRigidBody.AddForce(-direction.normalized * enemyData.knockback);

        if (enemyData.dieOnAttack)
            StartCoroutine("DieOnAttack");//change in the future to have an external system place vfx?

        playerManager.TakeDamage(damageValue);
    }
}
