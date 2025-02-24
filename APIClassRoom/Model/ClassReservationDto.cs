namespace APIClassRoom.Model;

public class ClassReservationDto
{
    public Guid Id { get; set; }
    public string ClassName { get; set; }
    public int IdClass { get; set; }
    public int LevelId { get; set; }
    public string Department { get; set; }
    public string Block { get; set; }
    public string TeacherName { get; set; }
    public DateTime DateReserved { get; set; }
    public string TimeSlot { get; set; }
    public string Day { get; set; }
    public string Compensation { get; set; }
    public string Description { get; set; }
}
