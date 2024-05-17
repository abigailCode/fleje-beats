using System.IO;
using UnityEngine;

public class RankingWriter {
    string _songsListPath = "Music/Songs/data.json";
    string _dataFilePath;

    public RankingWriter() {
        string absoluteAssetsPath = Application.dataPath;
        string projectFolderPath = absoluteAssetsPath.Substring(0, absoluteAssetsPath.Length - "Assets".Length);
        string resourcesFolderPath = Path.Combine(projectFolderPath, "Assets/Resources");
        _dataFilePath = Path.Combine(resourcesFolderPath, _songsListPath);
    }

    public void UpdateRanking() {
        SongsReader songsReader = new();
        SongDataList songDataList = songsReader.ReadSongs() ?? new SongDataList();
        string username = PlayerPrefs.GetString("username", "Anonymous");
        int score = PlayerPrefs.GetInt("score", 0);
        int max_combo = PlayerPrefs.GetInt("max_combo", 0);
        float accuracy = PlayerPrefs.GetFloat("accuracy", 0);

        foreach (SongData songData in songDataList.songData) {
            if (songData.title == PlayerPrefs.GetString("song.title")) {
                foreach (SongLevel songLevel in songData.levels) {
                    if (songLevel.path == PlayerPrefs.GetString("song.level.path")) {
                        songLevel.AddRanking(new LevelRanking(username, score, max_combo, accuracy));
                        PlayerPrefs.SetString("song.level.rankings", JsonUtility.ToJson(new LevelRankingArrayWrapper { levelRankingArray = songLevel.levelRanking.ToArray() }));
                        break;
                    }
                }
            }
        }
        string jsonData = JsonUtility.ToJson(songDataList, true);
        File.WriteAllText(_dataFilePath, jsonData);
    }
}
