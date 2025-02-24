namespace APIClassRoom.Model;

public class User
{

    public Guid Id { get; set; }

    public string Email { get; set; }

    public string Password { get; set; } 

    public string FullName { get; set; }
   
    public string AvatarUrl { get; set; } /*= "school.png";*/
    public string RandomColor { get; set; }

    public UserRole Role { get; set; }
    public int? LevelId { get; set; }
}
