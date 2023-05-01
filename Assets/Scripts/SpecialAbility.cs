using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialAbility : MonoBehaviour
{
    [Header("TeleportAbility")]
    public Image teleportImage;
    public float teleportCooldown = 3f;
    public float teleportManaCost = 30f;
    bool isTeleportCooldown = false;

    [Header("FreezeAbility")]
    public Image freezeImage;
    public float freezeCooldown = 5f;
    public float freezeManaCost = 50f;
    public float freezeDuration = 3f;
    bool isFreezeCooldown = false;

    [Header("RealityAbility")]
    public Image realityImage;
    public float realityCooldown = 5f;
    public float realityManaCost = 20f;
    public float realityControlDuration = 3f;
    bool isRealityCooldown = false;

    [Header("InvincibleAbility")]
    public Image invincibleImage;
    public float invincibleCooldown = 6f;
    public float invincibleManaCost = 50f;
    public float invincibleDuration = 3f;
    bool isInvincibleCooldown = false;

    [Header("PowerAbility")]
    public Image powerImage;
    public float powerCooldown = 4f;
    public float powerManaCost = 40f;
    public float powerDuration = 3f;
    public float increasedDamageMultiplier = 1.5f;
    bool isPowerCooldown = false;

    [Header("Effects")]
    public GameObject teleportEffect;
    public GameObject freezeEffect;
    public GameObject invincibleEffect;
    public GameObject realityControlEffect;

    [Header("Mana")]
    public GameObject player;
    public TextMeshProUGUI manaText;
    public Slider manaSlider;

    public float manaRegenRate = 10f;
    float mana = 0f;
    float maxMana = 100f;

    bool isTeleporting = false;
    bool isFreezing = false;
    bool isInvincible = false;
    bool isControllingReality = false;
    bool isPower = false;

    bool teleportUnlocked;
    bool freezeUnlocked;
    bool realityUnlocked;
    bool invincibleUnlocked;
    bool powerUnlocked;

    void Start() 
    {
        mana = maxMana;
        SetManaUI();

        teleportImage.fillAmount = 1;
        freezeImage.fillAmount = 1;
        realityImage.fillAmount = 1;
        invincibleImage.fillAmount = 1;
        powerImage.fillAmount = 1;

        teleportUnlocked = false;
        freezeUnlocked = false;
        realityUnlocked = false;
        invincibleUnlocked = false;
        powerUnlocked = false;

        teleportImage.gameObject.SetActive(false);
        freezeImage.gameObject.SetActive(false);
        realityImage.gameObject.SetActive(false);
        invincibleImage.gameObject.SetActive(false);
        powerImage.gameObject.SetActive(false);
    }

    void Update()
    {
        // Regenerate mana over time
        mana += manaRegenRate * Time.deltaTime;
        mana = Mathf.Clamp(mana, 0f, maxMana);

        // Check if any ability can be activated
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isTeleporting && !isTeleportCooldown && teleportUnlocked)
        {
            // Teleport ability
            if (mana >= teleportManaCost)
            {
                StartCoroutine(Teleport());
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !isFreezing && !isFreezeCooldown && freezeUnlocked)
        {
            // Freeze ability
            if (mana >= freezeManaCost)
            {
                StartCoroutine(Freeze());
                isFreezeCooldown = true;
                freezeImage.fillAmount = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && !isPower && !isPowerCooldown && powerUnlocked)
        {
            // Increased damage ability
            if (mana >= powerManaCost)
            {
                StartCoroutine(IncreasedDamage());
                isPowerCooldown = true;
                powerImage.fillAmount = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && !isInvincible && !isInvincibleCooldown && invincibleUnlocked)
        {
            // Invincibility ability
            if (mana >= invincibleManaCost)
            {
                StartCoroutine(Invincibility());
                isInvincibleCooldown = true;
                invincibleImage.fillAmount = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !isControllingReality && !isRealityCooldown && realityUnlocked)
        {
            // Reality control ability
            if (mana >= realityManaCost)
            {
                StartCoroutine(RealityControl());
                isRealityCooldown = true;
                realityImage.fillAmount = 0;
            }
        }

        // Update the cooldown timers
        //Ability1
        if(isTeleportCooldown) {
            teleportImage.fillAmount += 1 / teleportCooldown * Time.deltaTime;
        }

        if(teleportImage.fillAmount == 1) 
        {
            isTeleportCooldown = false;
        }

        //Ability2
        if(isFreezeCooldown) {
            freezeImage.fillAmount += 1 / freezeCooldown * Time.deltaTime;
        }

        if(freezeImage.fillAmount == 1) 
        {
            isFreezeCooldown = false;
        }

        //Ability3
        if(isRealityCooldown) {
            realityImage.fillAmount += 1 / realityCooldown * Time.deltaTime;
        }

        if(realityImage.fillAmount == 1) 
        {
            isRealityCooldown = false;
        }

        //Ability4
        if(isInvincibleCooldown) {
            invincibleImage.fillAmount += 1 / invincibleCooldown * Time.deltaTime;
        }

        if(invincibleImage.fillAmount == 1) 
        {
            isInvincibleCooldown = false;
        }

        //Ability5
        if(isPowerCooldown) {
            powerImage.fillAmount += 1 / powerCooldown * Time.deltaTime;
        }

        if(powerImage.fillAmount == 1) 
        {
            isPowerCooldown = false;
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
        // Instantiate(teleportEffect, transform.position, Quaternion.identity);

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

            // Set Cooldown
            isTeleportCooldown = true;
            teleportImage.fillAmount = 0;

            // Reduce mana and start cooldown timer
            mana -= teleportManaCost;
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
        mana -= freezeManaCost;

        // Find all enemies with the EnemyAI script
        EnemyAI[] enemiesAI = FindObjectsOfType<EnemyAI>();

        float oldPatrolRadius = 0f;
        float oldDetectionRange = 0f;
        float oldPatrolSpeed = 0f;

        bool foundOldValues = false;

        // Loop through each enemy found and freeze it
        foreach (EnemyAI enemyAI in enemiesAI)
        {
            //if enemy is not destroyed
            if (enemyAI == null)
            {
                continue;
            }

            // Spawn freeze effect at location of enemy
            // Instantiate(freezeEffect, enemy.transform.position, Quaternion.identity, enemy.transform);
            //enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

            if(!foundOldValues) {
                oldPatrolRadius = enemyAI.patrolRadius;
                oldDetectionRange = enemyAI.detectionRange;
                oldPatrolSpeed = enemyAI.patrolSpeed;
                foundOldValues = true;
            }

            enemyAI.patrolRadius = 0f;
            enemyAI.detectionRange = 0f;
            enemyAI.patrolSpeed = 0f;
        }
        
        // Wait before unfreezing the enemy
        yield return new WaitForSeconds(freezeDuration);
        
        foreach (EnemyAI enemyAI in enemiesAI)
        {
            //if enemy is not destroyed
            if (enemyAI == null)
            {
                continue;
            }
            
            //put the default EnemyAI values back
            enemyAI.patrolRadius = oldPatrolRadius;
            enemyAI.detectionRange = oldDetectionRange;
            enemyAI.patrolSpeed = oldPatrolSpeed;
        }   

        isFreezing = false;
        yield return null;
    }

    IEnumerator IncreasedDamage()
    {
        isPower = true;
        mana -= powerManaCost;

        // Set increased damage multiplier for a set duration
        var originalDamageMultiplier = GetComponent<LaserShot>().damageMultiplier;
        GetComponent<LaserShot>().damageMultiplier = increasedDamageMultiplier;

        // Wait for a set duration
        yield return new WaitForSeconds(powerDuration);

        // Reset damage multiplier to original value
        GetComponent<LaserShot>().damageMultiplier = originalDamageMultiplier;
        
        isPower = false;
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;
        mana -= invincibleManaCost;

        // Spawn invincibility effect on player
        //Instantiate(invincibleEffect, transform.position, Quaternion.identity, transform);

        PlayerStats.playerStats.SetInvincible(invincibleDuration);
        
        isInvincible = false;
        yield return null;
    }

    IEnumerator RealityControl()
    {
        isControllingReality = true;
        mana -= realityManaCost;

         // Find all enemies with the EnemyAI script
        EnemyAI[] enemiesAI = FindObjectsOfType<EnemyAI>();

        // Original enemy max damage
        float originalMaxDamage = enemiesAI[0].maxDamage;
    
        foreach (EnemyAI enemyAI in enemiesAI)
        {
            //if enemy is not destroyed
            if (enemyAI == null)
            {
                continue;
            }

            //make enemies do less damage
            enemyAI.maxDamage = enemyAI.minDamage;
            
            //get the sprite renderer of enemyAI
            SpriteRenderer enemySpriteRenderer = enemyAI.GetComponent<SpriteRenderer>();
            Transform parentTransform = enemySpriteRenderer.gameObject.transform;
    
            //get the child sprite render of enemySpriteRenderer
            Transform childTransform = parentTransform.Find("ChickenSprite");
            SpriteRenderer childSpriteRenderer = childTransform.GetComponent<SpriteRenderer>();

            //set the enemySpriteRenderer to inactive:
            enemySpriteRenderer.enabled = false;
         
            //set the child sprite to active
            childSpriteRenderer.enabled = true;

            //transform to position of parent sprite
            childSpriteRenderer.transform.position = enemySpriteRenderer.transform.position;
        }

        // Wait before making the enemy normal again
        yield return new WaitForSeconds(realityControlDuration);
        
        foreach (EnemyAI enemyAI in enemiesAI)
        {
            //if enemy is not destroyed
            if (enemyAI == null)
            {
                continue;
            }

            //put the default EnemyAI maxDamage values back
            enemyAI.maxDamage = originalMaxDamage;

            //get the sprite renderer of enemyAI
            SpriteRenderer enemySpriteRenderer = enemyAI.GetComponent<SpriteRenderer>();
            Transform parentTransform = enemySpriteRenderer.gameObject.transform;
    
            //get the child sprite render of enemySpriteRenderer
            Transform childTransform = parentTransform.Find("ChickenSprite");
            SpriteRenderer childSpriteRenderer = childTransform.GetComponent<SpriteRenderer>();

            //set the enemySpriteRenderer back to active:
            enemySpriteRenderer.enabled = true;
         
            //set the child sprite back to inactive
            childSpriteRenderer.enabled = false;
        }   

        isControllingReality = false;
        yield return null;
    }

    // Unlocks abilities when collecting gems
    public void UpdateAbilities(int sceneIndex) {
        if (sceneIndex == 1) {
            teleportUnlocked = true;
            teleportImage.gameObject.SetActive(true);
        }
        if (sceneIndex == 2) {
            freezeUnlocked = true;
            freezeImage.gameObject.SetActive(true);
        }
        if (sceneIndex == 3) {
            realityUnlocked = true;
            realityImage.gameObject.SetActive(true);
        }
        if (sceneIndex == 4) {
            invincibleUnlocked = true;
            invincibleImage.gameObject.SetActive(true);
        }
        if (sceneIndex == 5) {
            powerUnlocked = true;
            powerImage.gameObject.SetActive(true);
        }
    }


}

