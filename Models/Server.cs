namespace BloggerApi.Models {
  public class Server : Model {
    public byte[] Key { get; set; }

    public byte[] IV { get; set; }
  }
}