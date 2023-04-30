using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialAbility : MonoBehaviour
{
    public GameObject teleportEffect;
    public float teleportDistance;

    public GameObject freezeEffect;
    public float freezeDuration;

    public float increasedDamageMultiplier;

    public GameObject invincibleEffect;
    public float invincibleDuration;

    public GameObject mindControlEffect;
    public float mindControlDuration;

    bool isTeleporting = false;
    bool isFreezing = false;
    bool isInvincible = false;
    bool isMindControlling = false;

    float mana = 0f;
    float maxMana = 100f;
    float manaRegenRate = 20f;
    float cooldownTime = 5f;
    float currentCooldownTime = 0f;

    public GameObject player;
    public TextMeshProUGUI manaText;
    public Slider manaSlider;

    void Start() 
    {
        mana = maxMana;
        SetManaUI();
    }

    void Update()
    {
        // Regenerate mana over time
        mana += manaRegenRate * Time.deltaTime;
        mana = Mathf.Clamp(mana, 0f, maxMana);

        // Check if any ability can be activated
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isTeleporting)
        {
            // Teleport ability
            if (mana >= 50f)
            {
                StartCoroutine(Teleport());
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !isFreezing)
        {
            // Freeze ability
            if (mana >= 25f)
            {
                StartCoroutine(Freeze());
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Increased damage ability
            if (mana >= 30f)
            {
                StartCoroutine(IncreasedDamage());
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && !isInvincible)
        {
            // Invincibility ability
            if (mana >= 40f)
            {
                StartCoroutine(Invincibility());
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && !isMindControlling)
        {
            // Mind control ability
            if (mana >= 50f)
            {
                StartCoroutine(MindControl());
            }
        }

        // Update cooldown timer for abilities
        if (currentCooldownTime > 0f)
        {
            currentCooldownTime -= Time.deltaTime;
            currentCooldownTime = Mathf.Clamp(currentCooldownTime, 0f, cooldownTime);
        }
        SetManaUI();
    }

    private void SetManaUI()
    {
        manaSlider.value = CalculateManaPercentage();
        manaText.text = Mathf.Ceil(mana).ToString() + " / " + Mathf.Ceil(maxMana).ToString();
    }

    private float CalculateManaPercentage()
    {
        return mana / maxMana;
    }

    IEnumerator Teleport()
    {
        isTeleporting = true;

        // Spawn teleport effect at current position
        Instantiate(teleportEffect, transform.position, Quaternion.identity);


        // Get the destination position of the teleport
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = -1;

        bool canTeleport = false;
        canTeleport = CanTeleport();
        Debug.Log(canTeleport);
    
        // Teleport the player if collision check passed
        if (canTeleport)
        {
            transform.position = targetPos;

            // Reduce mana and start cooldown timer
            mana -= 50f;
            currentCooldownTime = cooldownTime;
        }

        isTeleporting = false;
        yield return null;
    }

    private bool CanTeleport()
    {
        // Get the position of the mouse in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Find the first object with the "Map" tag and get its sprite renderer component
        GameObject mapObject = GameObject.FindGameObjectWithTag("Map");
        if (mapObject == null)
        {
            Debug.LogWarning("No objects found with tag 'Map'");
            return false;
        }
        SpriteRenderer mapSprite = mapObject.GetComponent<SpriteRenderer>();
        if (mapSprite == null)
        {
            Debug.LogWarning("Object with tag 'Map' does not have a SpriteRenderer component");
            return false;
        }

        // Check if the mouse position is inside the bounds of the map sprite
        Vector3 spriteCenter = mapSprite.bounds.center;
        Vector3 spriteExtents = mapSprite.bounds.extents;
        if (Mathf.Abs(mousePos.x - spriteCenter.x) <= spriteExtents.x &&
            Mathf.Abs(mousePos.y - spriteCenter.y) <= spriteExtents.y)
        {
            return true;
        }

        return false;
    }


    IEnumerator Freeze()
    {
        isFreezing = true;

    //     // Spawn freeze effect at location of all enemies
    //     var enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //     foreach (var enemy in enemies)
    //     {
    //         Instantiate(freezeEffect, enemy.transform.position, Quaternion.identity, enemy.transform);
    //     }

    //     // Freeze all enemies for a set duration
    //     foreach (var enemy in enemies)
    //     {
    //         var enemyController = enemy.GetComponent<EnemyController>();
    //         if (enemyController != null)
    //         {
    //             enemyController.Freeze(freezeDuration);
    //         }
    //     }

    //     // Reduce mana and start cooldown timer
    //     mana -= 25f;
    //     currentCooldownTime = cooldownTime;

        isFreezing = false;
        yield return null;
    }

    IEnumerator IncreasedDamage()
    {
        // Set increased damage multiplier for a set duration
        var originalDamageMultiplier = GetComponent<LaserShot>().damageMultiplier;
        GetComponent<LaserShot>().damageMultiplier = increasedDamageMultiplier;

        // Wait for a set duration
        yield return new WaitForSeconds(3f);

        // Reset damage multiplier to original value
        GetComponent<LaserShot>().damageMultiplier = originalDamageMultiplier;

        // Reduce mana and start cooldown timer
        mana -= 30f;
        currentCooldownTime = cooldownTime;
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;

        // Spawn invincibility effect on player
        Instantiate(invincibleEffect, transform.position, Quaternion.identity, transform);

        // Set player invincible for a set duration
        // GetComponent<PlayerMovement>().SetInvincible(invincibleDuration);
        PlayerStats.playerStats.SetInvincible(true);
        yield return new WaitForSeconds(3f);
        PlayerStats.playerStats.SetInvincible(false);

        // Reduce mana and start cooldown timer
        mana -= 40f;
        currentCooldownTime = cooldownTime;
        yield return null;
    }

    IEnumerator MindControl()
    {
        isMindControlling = true;

    //     // Spawn mind control effect at location of all enemies
    //     var enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //     foreach (var enemy in enemies)
    //     {
    //         Instantiate(mindControlEffect, enemy.transform.position, Quaternion.identity, enemy.transform);
    //     }

    //     // Force all enemies to target and shoot each other for a set duration
    //     foreach (var enemy in enemies)
    //     {
    //         var enemyController = enemy.GetComponent<EnemyController>();
    //         if (enemyController != null)
    //         {
    //             enemyController.TargetAndShootEachOther(mindControlDuration);
    //         }
    //     }

    //     // Reduce mana and start cooldown timer
    //     mana -= 50f;
    //     currentCooldownTime = cooldownTime;

        isMindControlling = false;
        yield return null;
    }
}
