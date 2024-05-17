using TMPro;
using UnityEngine;

public class Score : MonoBehaviour {
    public delegate void GameFinished();
    public static event GameFinished OnGameFinished;

    public int score = 0;
    TMP_Text _scoreText;

    void Start() {
        _scoreText = GetComponent<TMP_Text>();
        UpdateScore();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) IncreaseScore();
        if (Input.GetKeyDown(KeyCode.DownArrow)) FinishGame();
    }

    void OnEnable() => OnGameFinished += GoToRanking;

    void OnDisable() => OnGameFinished -= GoToRanking;

    void UpdateScore(int score = 0) => _scoreText.text =  score + " Points";

    void IncreaseScore() => UpdateScore(++score);

    void DecreaseScore() { if (score > 0) UpdateScore(--score); }

    void StoreScore() => PlayerPrefs.SetInt("score", score);

    void GoToRanking() {
        StoreScore();
        SCManager.instance.LoadScene("Ranking");
        AudioManager.instance.UnloadSong();
    }

    public static void FinishGame() => OnGameFinished.Invoke();
}