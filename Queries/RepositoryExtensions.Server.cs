using System.Linq;
using System.Threading.Tasks;
using BloggerApi.Models;
using BloggerApi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BloggerApi.Queries {
  public static partial class RepositoryExtensions {
    public static async Task<Server> GetServerAsync(this IRepository<Server> repository) {
      var result =
        await repository
          .AsQueryable()
          .OfType<Server>()
          .FirstOrDefaultAsync();

      return result;
    }
  }
}