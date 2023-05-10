using Microsoft.EntityFrameworkCore;

namespace BloggerApi.Persistence {
  public interface IPersistenceContext {
    DbSet<TModel> Set<TModel>() where TModel : class;
  }
}