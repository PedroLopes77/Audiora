namespace Audiora.Admin.Models;

public class PagedResponse<T>
{
    public List<T> Data        { get; set; } = new();
    public int     TotalCount  { get; set; }
    public int     TotalPages  { get; set; }
    public bool    HasNext     { get; set; }
}