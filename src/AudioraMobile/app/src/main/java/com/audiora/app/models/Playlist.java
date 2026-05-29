package com.audiora.app.models;

import java.util.List;

public class Playlist {
    private String id;
    private String name;
    private String description;
    private String coverImageUrl;
    private boolean isPublic;
    private String ownerName;
    private int totalTracks;
    private List<Music> musics;

    public String getId()             { return id; }
    public String getName()           { return name; }
    public String getDescription()    { return description; }
    public String getCoverImageUrl()  { return coverImageUrl; }
    public boolean isPublic()         { return isPublic; }
    public String getOwnerName()      { return ownerName; }
    public int getTotalTracks()       { return totalTracks; }
    public List<Music> getMusics()    { return musics; }
}