namespace ProjectAPI.Models;
public class Tracking
{
    public int TrackingID { get; set;}
    public string TrackingNumber { get; set;} = null!;
    public string Location { get; set;}= null!;
    public string Status { get; set;}= null!;
    public DateTime UpdatedAt { get; set;}
    
}