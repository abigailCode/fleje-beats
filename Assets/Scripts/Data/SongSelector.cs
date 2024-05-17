using MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;

public class SongSelector : MonoBehaviour {

    [SerializeField] GameObject _songButton;
    [SerializeField] GameObject _levelsPanel;
    SongLevel[] _levels;

    void Awake() {
        SetUp();
    }

    void SetUp() {
        SongDataList songDataList = new SongsReader().ReadSongs();

        foreach (SongData songData in songDataList.songData) {
            GameObject songButton = Instantiate(_songButton);
            songButton.transform.SetParent(gameObject.transform, false);

            songButton.transform.GetComponentInChildren<TMP_Text>().text = $"{songData.title} by {songData.artist}";
            songButton.GetComponent<PressableButton>().OnClicked.AddListener(() => OnClick(songData));
        }
    }

    public void OnClick(SongData song) {
        PlayerPrefs.SetString("song.title", song.title);
        PlayerPrefs.SetString("song.path", song.path);
        PlayerPrefs.SetInt("song.duration", song.duration);
        _levels = song.levels;
        _levelsPanel.SetActive(true);
    }

    public void SelectLevel(string level) {
        foreach (SongLevel songLevel in _levels) 
            if (songLevel.level == level) {
                PlayerPrefs.SetString("song.level", level);
                PlayerPrefs.SetString("song.level.path", songLevel.path);
                PlayerPrefs.SetString("song.level.rankings", JsonUtility.ToJson(new LevelRankingArrayWrapper { levelRankingArray = songLevel.levelRanking.ToArray() }));
                SCManager.instance.LoadScene("GameWip");
            }
    }

    public void HideLevelPanel() => _levelsPanel.SetActive(false);
}
