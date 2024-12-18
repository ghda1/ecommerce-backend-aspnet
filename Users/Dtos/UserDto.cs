// User Dto to return specific data 
public class UserDto
{

    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsAdmin { get; set; }
    public List<Address> Addresses { get; set; }
}