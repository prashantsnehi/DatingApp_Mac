namespace API.DTO;

public class LoginDto
{
    // [Required(ErrorMessage = "Username is required")]
    // [MinLength(3, ErrorMessage = "Username must contain at least 3 characters")]
    // [MaxLength(10, ErrorMessage = "Username is too long")]
    public string Username { get; set; }

    // [Required(ErrorMessage = "Password is required")]
    // [StringLength(8, MinimumLength = 4, ErrorMessage = "Length of password must be between 4 to 8")]
    public string Password { get; set; }
}
