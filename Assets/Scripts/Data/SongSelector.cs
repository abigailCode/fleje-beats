using MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;

public class SongSelector : MonoBehaviour {

    [SerializeField] GameObject _songButton;
    [SerializeField] GameObject levelsPanel;
    SongLevel[] levels;

    void Start() {
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
        levels = song.levels;
        levelsPanel.SetActive(true);
    }

    public void SelectLevel(int level) {
        foreach (SongLevel songLevel in levels) 
            if (songLevel.level == level) 
                PlayerPrefs.SetString("song.level.path", songLevel.path);
    }

    public void HideLevelPanel() => levelsPanel.SetActive(false);
}