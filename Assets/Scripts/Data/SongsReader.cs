using System.IO;
using UnityEngine;


public class SongsReader {
    string _songsListPath = "Music/Songs/data.json";
    string _dataFilePath;

    public SongsReader() {
        _dataFilePath = Path.Combine(Application.streamingAssetsPath, _songsListPath);
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

    public void SetData(SongDataList songDataList) {
        for (int i = 0; i < songDataList.songData.Count; i++)
        {
            Debug.Log(songDataList.songData[i].title);
            Debug.Log(songDataList.songData[i].artist);
            Debug.Log(songDataList.songData[i].path);
            Debug.Log(songDataList.songData[i].duration);
            for (int j = 0; j < songDataList.songData[i].levels.Length; j++)
            {
                Debug.Log(songDataList.songData[i].levels[j].level);
                Debug.Log(songDataList.songData[i].levels[j].path);
            }
        }
    }
}