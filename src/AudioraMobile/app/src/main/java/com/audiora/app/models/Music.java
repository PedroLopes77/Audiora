package com.audiora.app.models;

public class Music {
    private String id;
    private String title;
    private String audioUrl;
    private String coverImageUrl;
    private int durationSeconds;
    private String duration;
    private String genre;
    private long streamCount;
    private boolean isExplicit;
    private String artistName;
    private String albumTitle;

    public String getId()             { return id; }
    public String getTitle()          { return title; }
    public String getAudioUrl()       { return audioUrl; }
    public String getCoverImageUrl()  { return coverImageUrl; }
    public int getDurationSeconds()   { return durationSeconds; }
    public String getDuration()       { return duration; }
    public String getGenre()          { return genre; }
    public long getStreamCount()      { return streamCount; }
    public boolean isExplicit()       { return isExplicit; }
    public String getArtistName()     { return artistName; }
    public String getAlbumTitle()     { return albumTitle; }
}