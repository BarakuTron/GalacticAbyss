using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NexusDamageScript : EnemyReceiveDamage
{

    public Animator animator;
    private bool alive = true;

    public override void DealDamage(float damage)
    {
        if(alive) {
            healthBar.SetActive(true);
            health -= damage;
            CheckDeath2();
            healthBarSlider.value = CalculateHealthPercentage();
        }
    }

    private void CheckDeath2()
    {
        if (health <= 0)
        {

            alive = false;
                
            //add nexus death animation
            animator.SetTrigger("Die");

            //if it is, then the player has won
            // GameManager.Instance.PlayerWon();
    


           // Destroy(gameObject);
        }
    }

    

}
