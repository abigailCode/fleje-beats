using MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;

public class SongSelector : MonoBehaviour {

    [SerializeField] GameObject levelsPanel;
    SongLevel[] levels;

    void Start() {
        SetUp();
    }

    void SetUp() {
        SongDataList songDataList = new SongsReader().ReadSongs();

        foreach (SongData songData in songDataList.songData) {
            GameObject songButton = Instantiate(Resources.Load("Prefabs/SongButton")) as GameObject;
            songButton.transform.SetParent(gameObject.transform, false);

            songButton.transform.GetComponentInChildren<TMP_Text>().text = $"{songData.title} by {songData.artist}";
            songButton.GetComponent<PressableButton>().OnClicked.AddListener(() => OnClick(songData));
            levels = songData.levels;
        }
    }

    public void OnClick(SongData song) {
        PlayerPrefs.SetString("song.title", song.title);
        PlayerPrefs.SetString("song.path", song.path);
        PlayerPrefs.SetInt("song.duration", song.duration);
        levelsPanel.SetActive(true);
        print($"Selected song: {song.title}");
        print($"Selected song path: {song.path}");
    }

    public void SelectLevel(int level) {
        foreach (SongLevel songLevel in levels) 
            if (songLevel.level == level) 
                PlayerPrefs.SetString("song.level.path", songLevel.path);
    }

    public void HideLevelPanel() => levelsPanel.SetActive(false);
}
