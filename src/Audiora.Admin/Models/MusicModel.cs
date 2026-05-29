namespace Audiora.Admin.Models;

public class MusicModel
{
    public string Id             { get; set; } = "";
    public string Title          { get; set; } = "";
    public string ArtistName     { get; set; } = "";
    public string AlbumTitle     { get; set; } = "";
    public string Genre          { get; set; } = "";
    public string Duration       { get; set; } = "";
    public long   StreamCount    { get; set; }
    public bool   IsExplicit     { get; set; }
    public string CoverImageUrl  { get; set; } = "";
    public string AudioUrl       { get; set; } = "";
}