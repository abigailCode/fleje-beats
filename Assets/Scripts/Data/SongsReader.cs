using System.IO;
using UnityEngine;


public class SongsReader : MonoBehaviour {
    string _songsListPath = "Music/Songs/data.json";
    string dataFilePath;

    void Start() {
        dataFilePath = Path.Combine(Application.streamingAssetsPath, _songsListPath);
        ReadSongs();
    }

    //public void SaveData() {
    //    List<songData> playersList = new List<songData>();
    //    playersList.Add(new songData("Abby", 39));
    //    playersList.Add(new songData("Bobby", 96));

    //    songDataList songDataList = new songDataList();
    //    songDataList.songData = playersList;

    //    string jsonData = JsonUtility.ToJson(songDataList, true);
    //    PlayerPrefs.SetString("PlayerList", jsonData);

    //    File.WriteAllText(dataFilePath, jsonData);
    //}

    public SongDataList ReadSongs() {
        if (File.Exists(dataFilePath)) {
            string jsonData = File.ReadAllText(dataFilePath);
            SongDataList songDataList = JsonUtility.FromJson<SongDataList>(jsonData);
            return songDataList;
        }
        return null;
    }

    public void SetData(SongDataList songDataList) {
        for (int i = 0; i < songDataList.songData.Count; i++)
        {
            print(songDataList.songData[i].title);
            print(songDataList.songData[i].artist);
            print(songDataList.songData[i].path);
            print(songDataList.songData[i].duration);
            for (int j = 0; j < songDataList.songData[i].levels.Length; j++)
            {
                print(songDataList.songData[i].levels[j].level);
                print(songDataList.songData[i].levels[j].path);
            }
        }
    }
}