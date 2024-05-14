using System;
using System.Collections.Generic;

[Serializable]
public class SongData {

    public string title;
    public string artist;
    public string path;
    public int duration;
    public SongLevel[] levels;

    public SongData(string title, string artist, string path, int duration, SongLevel[] levels)
    {
        this.title = title;
        this.artist = artist;
        this.path = path;
        this.duration = duration;
        this.levels = levels;
    }
}


[Serializable]
public class SongLevel
{
    public int level;
    public string path;
    public SongLevel(int level, string path)
    {
        this.level = level;
        this.path = path;
    }
}

[Serializable]
public class SongDataList
{
    public List<SongData> songData;

    public SongDataList()
    {
        songData = new List<SongData>();
    }
}

[Serializable]
public class SongBeat
{
    public int id;
    public float time;
    public string locationY;
    public string locationX;
    public string hit;

    public SongBeat(int id, float time, string locationY, string locationX, string hit)
    {
        this.id = id;
        this.time = time;
        this.locationY = locationY;
        this.locationX = locationX;
        this.hit = hit;
    }
    
}

[Serializable]
public class SongLevelConfiguration
{
    public List<SongBeat> songData;

    public SongLevelConfiguration()
    {
        songData = new List<SongBeat>();
    }
}
