using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyHealthXR : MonoBehaviour {
    public int startingHealth = 100;            // The amount of health the enemy starts the game with.
    public int currentHealth;                   // The current health the enemy has.
    public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
    public int scoreValue = 10;                 // The amount added to the player's score when the enemy dies.
    public AudioClip hurtClip;
    public Renderer[] renderers;

    [Header("Death")]
    public float deactivatingDelay = 0.3f;

    public AudioClip deathClip;                 // The sound to play when the enemy dies.


    private Animator animator;                              // Reference to the animator.

    private string animParameterNameForIsDead = "isDead";

    AudioSource enemyAudio;                     // Reference to the audio source.
    ParticleSystem hitParticles;                // Reference to the particle system that plays when the enemy is damaged.
    CapsuleCollider capsuleCollider;            // Reference to the capsule collider.
    bool isDead;                                // Whether the enemy is dead.
    bool isSinking;                             // Whether the enemy has started sinking through the floor.

    private Rigidbody rigidbody3D;
    private NavMeshAgent navMeshAgent;

    Transform player;               // Reference to the player's position.
    PlayerHealthXR playerHealth;      // Reference to the player's health.

    void Awake() {
        // Setting up the references.
        animator = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rigidbody3D = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealthXR>();

        Init();
    }

    void Init() {
        currentHealth = startingHealth;
        animator.SetBool("IsDead", false);
    }

    void Update() {
        // If the enemy should be sinking...
        if(isSinking) {
            // ... move the enemy down by the sinkSpeed per second.
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }

        // If the enemy and the player have health left...
        if(currentHealth > 0 && playerHealth.currentHealth > 0) {
            // ... set the destination of the nav mesh agent to the player.
            navMeshAgent.SetDestination(player.position);
        } else if(navMeshAgent.enabled) {
            // ... disable the nav mesh agent.
            navMeshAgent.enabled = false;
        }
    }

    public void TakeDamage(int amount) {
        if(isDead)
            return;

        enemyAudio.Play();
        currentHealth -= amount;

        if(currentHealth <= 0)
            Death();
    }

    public void TakeDamage(int amount, Vector3 hitPoint) {
        // If the enemy is dead...
        if(isDead)
            // ... no need to take damage so exit the function.
            return;

        // Play the hurt sound effect.
        enemyAudio.Play();

        // Reduce the current health by the amount of damage sustained.
        currentHealth -= amount;

        // Set the position of the particle system to where the hit was sustained.
        hitParticles.transform.position = hitPoint;

        // And play the particles.
        hitParticles.Play();

        // If the current health is less than or equal to zero...
        if(currentHealth <= 0) {
            // ... the enemy is dead.
            Death();
        }
    }

    void Death() {

        StartSinking();
        // The enemy is dead.
        isDead = true;

        // Turn the collider into a trigger so shots can pass through it.
        capsuleCollider.isTrigger = true;

        // Tell the animator that the enemy is dead.
        animator.SetBool("IsDead", true);

        // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
        enemyAudio.clip = deathClip;
        enemyAudio.Play();

        Destroy(gameObject);
    }

    // Call by Death Animation Event
    public void StartSinking() {
        // Find and disable the Nav Mesh Agent.
        navMeshAgent.enabled = false;

        // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
        rigidbody3D.isKinematic = true;

        // The enemy should no sink.
        isSinking = true;

        // Increase the score by the enemy's score value.
        ScoreManagerXR.Increase(scoreValue);

        StartCoroutine(Deactivate());
    }

    IEnumerator Deactivate() {
        yield return new WaitForSeconds(deactivatingDelay);

        SetRenderersEnabled(false);

        yield return new WaitForSeconds(deactivatingDelay);

        gameObject.SetActive(false);
    }

    public void Reset(Vector3 position, Quaternion rotation) {
        StartCoroutine(ResetCoroutine(position, rotation));
    }

    private IEnumerator ResetCoroutine(Vector3 position, Quaternion rotation) {
        yield return new WaitForFixedUpdate();

        navMeshAgent.Warp(position);
        rigidbody3D.rotation = rotation;

        yield return new WaitForFixedUpdate();

        SetRenderersEnabled(true);

        Init();

        navMeshAgent.enabled = true;

        isDead = false;

        capsuleCollider.isTrigger = false;

        animator.SetBool("IsDead", false);

        enemyAudio.clip = hurtClip;

        rigidbody3D.isKinematic = false;

        isSinking = false;
    }

    public void SetRenderersEnabled(bool enabled) {
        if(renderers.Length > 0) {
            for(int i = 0; i < renderers.Length; i++) {
                renderers[i].enabled = enabled;
            }
        } else {
            Debug.Log("Assign Renderer for EnemyHealthXR!");
        }
    }
}