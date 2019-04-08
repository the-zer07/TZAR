using UnityEngine;

public class EnemyAttackXR : MonoBehaviour {

    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;


    private Animator animator;
    private string animatorParameterNameForPlayerDead = "PlayerDead";

    private GameObject player;
    private Collider playerDamageZone;
    private PlayerHealthXR playerHealth;
    private EnemyHealthXR enemyHealth;
    private bool playerInRange;
    private float timer;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealthXR>();
        playerDamageZone = playerHealth.playerDamageZone;

        enemyHealth = GetComponent<EnemyHealthXR>();
        animator = GetComponent<Animator>();
    }


    private void OnCollisionStay(Collision other) {
        // If the entering collider is the player...
        if(other.gameObject.tag.Equals("Player")) {
            // ... the player is in range.
            playerInRange = true;
        }
    }


    private void OnCollisionExit(Collision other) {
        // If the exiting collider is the player...
        if(other.gameObject.tag.Equals("Player")) {
            // ... the player is no longer in range.
            playerInRange = false;
        }
    }


    private void Update() {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0) {
            // ... attack.
            Attack();
        }

        if(playerHealth.currentHealth <= 0)
            Destroy(gameObject);
    }


    private void Attack() {
        // Reset the timer.
        timer = 0f;

        animator.SetBool("IsAttack", true);

        // If the player has health to lose...
        if(playerHealth.currentHealth > 0) {
            // ... damage the player.
            playerHealth.TakeDamage(attackDamage);
        }
    }
}