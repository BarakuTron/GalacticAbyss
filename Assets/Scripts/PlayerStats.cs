using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats playerStats;

    public GameObject player;
    public TextMeshProUGUI healthText;
    
    public Slider healthSlider;

    public float health;
    public float maxHealth = 100;

    private int gems;
    private int score;
    public TextMeshProUGUI gemsCounter;
    public TextMeshProUGUI scoreCounter;

    public PlayerHitSound hitSound;

    private bool isInvincible = false;

    void Awake()
    {
        if(playerStats == null)
        {
            playerStats = this;
            DontDestroyOnLoad(this);
        }
        else if(playerStats != this)
        {
            Destroy(gameObject);
        }  
    }
    
    void Start()
    {
        health = maxHealth;
        SetHealthUI(); 
    }

    public void DealDamage(float damage)
    {
        if(!isInvincible) 
        {
            hitSound.PlaySound();
            health -= damage;
            CheckDeath();
            SetHealthUI();
        }
    }

    public void HealCharacter(float heal)
    {
        health += heal;
        CheckOverheal();
        SetHealthUI();
    }

    private void SetHealthUI()
    {
        healthSlider.value = CalculateHealthPercentage();
        healthText.text = Mathf.Ceil(health).ToString() + " / " + Mathf.Ceil(maxHealth).ToString();
    }

    private void CheckOverheal()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {
            health = 0;
            Destroy(player);
        }
    }

    private float CalculateHealthPercentage()
    {
        return health / maxHealth;
    }

    public void AddGem() {
        gems++;
        SetGemUI();
    }

    public void AddScore(int scoreToAdd) {
        score += scoreToAdd;
        SetScoreUI();
    }

    private void SetGemUI()
    {
        gemsCounter.text = "Infinity Stones: " + gems.ToString() + "/5";
    }

    private void SetScoreUI()
    {
        scoreCounter.text = "Score : " + score.ToString();
    }

    // public void SetInvincible(bool status)
    // {
    //     isInvincible = status;
    //     //StartCoroutine(Invincible(timeDuration));
    // }

    IEnumerator Invincibility(float timeDuration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(timeDuration);
        isInvincible = false;
    }

    public void SetInvincible(float timeDuration)
    {
        if(!isInvincible) {
            StartCoroutine(Invincibility(timeDuration));
        }
    }
}
