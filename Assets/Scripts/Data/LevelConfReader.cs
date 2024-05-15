using System.IO;
using UnityEngine;


public class LevelConfReader {
    string _songsListPath = "Music/Songs";
    string _dataFilePath;

    public LevelConfReader() {
        _dataFilePath = Path.Combine(Application.streamingAssetsPath, _songsListPath, PlayerPrefs.GetString("song.level.path"));
    }

    public SongLevelConfiguration ReadConf() {
        if (File.Exists(_dataFilePath)) {
            string jsonData = File.ReadAllText(_dataFilePath);
            SongLevelConfiguration songLevelConfiguration = JsonUtility.FromJson<SongLevelConfiguration>(jsonData);
            return songLevelConfiguration;
        }
        return null;
    }

    public void SetData(SongLevelConfiguration songLevelConfiguration) {
        Debug.Log(songLevelConfiguration.beatData);
        for (int i = 0; i < songLevelConfiguration.beatData.Count; i++) {
            Debug.Log(songLevelConfiguration.beatData[i].id);
            Debug.Log(songLevelConfiguration.beatData[i].time);
            Debug.Log(songLevelConfiguration.beatData[i].locationY);
            Debug.Log(songLevelConfiguration.beatData[i].locationX);
            Debug.Log(songLevelConfiguration.beatData[i].hit);
        }
    }
}