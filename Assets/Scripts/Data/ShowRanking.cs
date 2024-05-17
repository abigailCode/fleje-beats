using System.Collections;
using TMPro;
using UnityEngine;

public class ShowRanking : MonoBehaviour {
    [SerializeField] GameObject _scoreContent;
    [SerializeField] GameObject _rankingContent;

    void Awake() {
        SetUp();
    }

    void SetUp() {
        Transform scoreHeader = _scoreContent.transform.GetChild(0).transform;
        scoreHeader.GetChild(0).GetComponent<TMP_Text>().text = PlayerPrefs.GetString("song.title");
        scoreHeader.GetChild(1).GetComponent<TMP_Text>().text = PlayerPrefs.GetString("song.level");
        _scoreContent.transform.Find("Score").GetComponent<TMP_Text>().text = $"SCORE: {PlayerPrefs.GetInt("score")}";
        //_scoreContent.transform.Find("MaxCombo").GetComponent<TMP_Text>().text = $"MAX COMBO: {PlayerPrefs.GetInt("max_combo")}";
        //_scoreContent.transform.Find("Accuracy").GetComponent<TMP_Text>().text = $"ACCURACY: {PlayerPrefs.GetFloat("accuracy")}%";

        SetUpRankingList();
    }

    void SetUpRankingList() {
        LevelRankingArrayWrapper wrapper = JsonUtility.FromJson<LevelRankingArrayWrapper>(PlayerPrefs.GetString("song.level.rankings"));
        LevelRanking[] levelRankings = wrapper.levelRankingArray;

        for (int i = 0; i < levelRankings.Length; i++) {
            GameObject ranking = _rankingContent.transform.GetChild(i).gameObject;
            ranking.transform.SetParent(_rankingContent.transform, false);
            ranking.transform.GetChild(0).GetComponent<TMP_Text>().text = levelRankings[i].username;
            ranking.transform.GetChild(1).Find("Score").GetComponent<TMP_Text>().text = $"SCORE: {levelRankings[i].score}";
            //ranking.transform.GetChild(1).Find("MaxCombo").GetComponent<TMP_Text>().text = $"MAX COMBO: {levelRankings[i].max_combo}";
            //ranking.transform.GetChild(1).Find("Accuracy").GetComponent<TMP_Text>().text = $"ACCURACY: {levelRankings[i].accuracy}%";
        }
    }

    void UpdateRanking() {
        RankingWriter rankingWriter = new();
        rankingWriter.UpdateRanking();
        SetUpRankingList();
    }

    public void OnClick() {
        UpdateRanking();
        //Show keyboard
    }
}
