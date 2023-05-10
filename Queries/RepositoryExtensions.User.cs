using System.Linq;
using System.Threading.Tasks;
using BloggerApi.Models;
using BloggerApi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BloggerApi.Queries {
  public static partial class RepositoryExtensions {
    public static async Task<User> GetUserByIdAsync(this IRepository<User> repository, long id) {
      var result =
        await repository
          .AsQueryable()
          .OfType<User>()
          .SingleOrDefaultAsync(x => x.Id == id);

      return result;
    }

    public static async Task<User> GetUserByEmailAsync(this IRepository<User> repository, string email) {
      var result =
        await repository
          .AsQueryable()
          .OfType<User>()
          .SingleOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

      return result;
    }
  }
}