using System.IO;
using UnityEngine;

public class SongsReader {
    string _songsListPath = "Music/Songs/data.json";
    string _dataFilePath;

    public SongsReader() {
        string absoluteAssetsPath = Application.dataPath;
        string projectFolderPath = absoluteAssetsPath.Substring(0, absoluteAssetsPath.Length - "Assets".Length);
        string resourcesFolderPath = Path.Combine(projectFolderPath, "Assets/Resources");
        _dataFilePath = Path.Combine(resourcesFolderPath, _songsListPath);
    }

    //public void SaveData() {
    //    List<songData> playersList = new List<songData>();
    //    playersList.Add(new songData("Abby", 39));
    //    playersList.Add(new songData("Bobby", 96));

    //    songDataList songDataList = new songDataList();
    //    songDataList.songData = playersList;

    //    string jsonData = JsonUtility.ToJson(songDataList, true);
    //    PlayerPrefs.SetString("PlayerList", jsonData);

    //    File.WriteAllText(_dataFilePath, jsonData);
    //}

    public SongDataList ReadSongs() {
        if (File.Exists(_dataFilePath)) {
            string jsonData = File.ReadAllText(_dataFilePath);
            SongDataList songDataList = JsonUtility.FromJson<SongDataList>(jsonData);
            return songDataList;
        }
        return null;
    }

    public void PrintData(SongDataList songDataList) {
        foreach (SongData songData in songDataList.songData) {
            Debug.Log(songData.title);
            Debug.Log(songData.artist);
            Debug.Log(songData.path);
            Debug.Log(songData.duration);
            foreach (SongLevel songLevel in songData.levels) {
                Debug.Log(songLevel.level);
                Debug.Log(songLevel.path);
                
                foreach (LevelRanking ranking in songLevel.levelRanking) {
                    Debug.Log(ranking.username);
                    Debug.Log(ranking.score);
                    Debug.Log(ranking.max_combo);
                    Debug.Log(ranking.accuracy);
                }
            }
        }
    }
}