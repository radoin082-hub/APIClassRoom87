namespace APIClassRoom.Model;

public class ClassRoom
{
    public int IdClass { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Localisation { get; set; } = string.Empty;
    public string Block { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;

    // Navigation property for Times
    public ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
}