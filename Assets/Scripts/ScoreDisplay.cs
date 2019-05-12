using UnityEngine;
using TMPro;

[RequireComponent (typeof (TextMeshProUGUI))]
public class ScoreDisplay : MonoBehaviour {

    TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI> ();
    }

    public void UpdateScore(int score)
    {
        scoreText.SetText ($"Score: {score}");
    }
}
