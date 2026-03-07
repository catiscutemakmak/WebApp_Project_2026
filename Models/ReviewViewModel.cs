using System.ComponentModel.DataAnnotations;

public class ReviewViewModel
{
    public int UserProfileId { get; set; }
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Please Write Your Review")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Please rate this user")]
    [Range(1, 5, ErrorMessage = "Please rate this user")]
    public int Rating { get; set; }
}