namespace APIClassRoom.Model;

public class TimeSlot
{
    public int Id { get; set; }
    public string TimeSlotValue { get; set; } = string.Empty;
    public int IdClass { get; set; }

    public string Sunday { get; set; } = "N/A";
    public string Monday { get; set; } = "N/A";
    public string Tuesday { get; set; } = "N/A";
    public string Wednesday { get; set; } = "N/A";
    public string Thursday { get; set; } = "N/A";

    public ClassRoom Class { get; set; }

 
    public TimeSlot()
    {
        Sunday = Monday = Tuesday = Wednesday = Thursday = "N/A";
    }
}