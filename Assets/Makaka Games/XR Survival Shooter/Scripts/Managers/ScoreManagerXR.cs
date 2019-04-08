using UnityEngine;


using TMPro;

public class ScoreManagerXR : MonoBehaviour {

    public static int score;

    private TextMeshProUGUI text;

    private static bool isScoreChanged = false;

    void Awake() {
        // Set up the reference.
        text = GetComponent<TextMeshProUGUI>();
        // Reset the score.
        score = 0;
    }

    public static void Increase(int scoreValue) {
        score += scoreValue;
        isScoreChanged = true;
    }

    void UpdateUI() {
        text.text = score.ToString();
        isScoreChanged = false;
    }

    void Update() {
        if(isScoreChanged) {
            UpdateUI();
        }
    }
}