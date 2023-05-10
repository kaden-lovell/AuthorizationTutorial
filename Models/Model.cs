using System;

namespace BloggerApi.Models {
  public class Model {
    public long Id { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
  }
}