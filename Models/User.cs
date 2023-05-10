namespace BloggerApi.Models {
  public class User : Model {
    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public byte[] Password { get; set; }

    public string Role { get; set; }

    public bool Active { get; set; }
  }
}