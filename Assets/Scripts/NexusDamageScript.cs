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
            CheckDeath();
            healthBarSlider.value = CalculateHealthPercentage();
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        Destroy(gameObject);
        //find a GameManager tagged element
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManager.GetComponent<PlayerStats>().WinGame();
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {

            alive = false;
                
            //add nexus death animation
            animator.SetTrigger("Die");

            //if it is, then the player has won
            // GameManager.Instance.PlayerWon();
    
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    

}
