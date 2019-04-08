using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GameOverManagerXR : MonoBehaviour {

    public PlayerHealthXR playerHealth;       // Reference to the player's health.
    public Animator animator;                          // Reference to the animator component.
    public UnityEvent OnGameOver;
    public TextMeshProUGUI gameOverText;
    public Image crosshairImage;

    void Update() {
        // If the player has run out of health...
        if(playerHealth.currentHealth <= 0) {
            // ... tell the animator the game is over.
            animator.SetTrigger("GameOver");
            gameOverText.gameObject.SetActive(true);
            crosshairImage.gameObject.SetActive(false);

            StartCoroutine(Wait());

        }
    }

    public void GameOver() {
        OnGameOver.Invoke();
    }

    private IEnumerator Wait() {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("MainMenu");
    }
}