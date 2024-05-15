using TMPro;
using UnityEngine;

public class Score : MonoBehaviour {
    public int score = 0;
    TMP_Text _scoreText;

  
    void Start() {
        _scoreText = GetComponent<TMP_Text>();
        UpdateScore();
    }

    void UpdateScore(int score = 0) => _scoreText.text =  score + " Points";
    void IncreaseScore() => UpdateScore(++score);
    void DecreaseScore() { if (score > 0) UpdateScore(--score); }
}