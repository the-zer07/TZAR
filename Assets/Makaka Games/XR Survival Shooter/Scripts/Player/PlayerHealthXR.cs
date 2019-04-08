using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHealthXR : MonoBehaviour {

    public int startHealth = 100;                            
    public int currentHealth;                                   

    public Slider healthSlider;                                 
    public Image damageImage;                                   
    public AudioClip deathClip;                                 
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.
    public UnityEvent onDeath;
    public Collider playerDamageZone;                         

    private AudioSource playerAudio;                                      
    private PlayerShootingXR playerShooting;                             

    private bool isDead;                                                
    private bool isDamaged;                                               

    private Color defaultColour;

    private void Awake() {

        playerAudio = GetComponent<AudioSource>();
        playerShooting = GetComponentInChildren<PlayerShootingXR>();
        playerDamageZone = GetComponent<CapsuleCollider>();

        currentHealth = startHealth;
        defaultColour = damageImage.color;
    }

    private void Update() {
        if(isDamaged) {
            // ... transition the colour back to clear.
            damageImage.color = Color.Lerp(damageImage.color, flashColour, flashSpeed * Time.deltaTime);
        } else {
            // ... set the colour of the damageImage to the flash colour.
            damageImage.color = defaultColour;
        }
        // Reset the damaged flag.
        isDamaged = false;
    }

    public void TakeDamage(int amount) {
        // Set the damaged flag so the screen will flash.
        isDamaged = true;
        // Reduce the current health by the damage amount.
        currentHealth -= amount;
        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;
        // Play the hurt sound effect.
        playerAudio.Play();
        // If the player has lost all it's health and the death flag hasn't been set yet
        if(currentHealth <= 0 && !isDead) {
            Death();
        }
    }

    private void Death() {
        isDead = true;

        playerAudio.clip = deathClip;
        playerAudio.Play();

        playerShooting.enabled = false;
    }
}
