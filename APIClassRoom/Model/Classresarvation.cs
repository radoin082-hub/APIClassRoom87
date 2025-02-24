using System.ComponentModel.DataAnnotations;

namespace APIClassRoom.Model;

public class ClassReservation
{
    public Guid Id { get; set; }
    public int IdClass { get; set; }
    [Required (ErrorMessage = "Department is required")]
    public string Department { get; set; }
    [Required (ErrorMessage = "Block is required")]
    public string Block { get; set; }
    
    public string TeacherName { get; set; }
    [Required (ErrorMessage = "DateReserved is required")]
    public DateOnly DateReserved { get; set; }
    [Required (ErrorMessage = "TimeSlot is required")]
    public string TimeSlot { get; set; }
    [Required (ErrorMessage = "Day is required")]
    public string Day { get; set; }
    [Required (ErrorMessage = "Level is required")]
    public int LevelId { get; set; }
    [Required (ErrorMessage = "Compensation is required")]
    public string Compensation { set; get; }
    [Required (ErrorMessage = "Description is required")]
    public string Description { set; get; }
}