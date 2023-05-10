using System;
using System.Threading.Tasks;
using BloggerApi.Models;
using BloggerApi.Persistence;
using BloggerApi.Queries;
using Microsoft.AspNetCore.Http;

namespace BloggerApi.Services {
  public class UserService {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<User> _repository;

    public UserService(IHttpContextAccessor httpContextAccessor, IRepository<User> repository) {
      _repository = repository;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<dynamic> GetCurrentUserAsync() {
      var id = _httpContextAccessor.HttpContext.User.Identity.Id();
      var user = await _repository.GetUserByIdAsync(id);

      if (user == null) {
        return null;
      }

      var result = new {
        id = _httpContextAccessor.HttpContext.User.Identity.Id(),
        firstname = user.FirstName,
        lastname = user.LastName,
        email = user.Email,
        role = user.Role,
        active = user.Active
      };

      return result;
    }

    public async Task<dynamic> GetUserAsync(long id) {
      var user = await _repository.GetUserByIdAsync(id);

      var result = new {
        id = user?.Id,
        firstName = user?.FirstName,
        lastName = user?.LastName,
        email = user?.Email,
        role = user?.Role,
        active = user?.Active
      };

      return result;
    }

    public async Task<dynamic> AddUserAsync(dynamic model) {
      // TODO: add model input validation
      if (model.email == null) {
        return null;
      }

      var user = await _repository.GetUserByEmailAsync((string) model.email);

      if (user != null) {
        return new {
          success = false,
          message = "email already in use"
        };
      }

      user = await _repository.AddOrUpdateAsync(new User {
        FirstName = model.firstname,
        LastName = model.lastname,
        Email = model.email,
        Role = Role.CLIENT,
        Active = true,
        Password = model.password,
        CreatedDate = DateTime.Now, // could be handled by repository
        ModifiedDate = null // could be handled by the repository
      });

      var result = new {
        success = true,
        user = new {
          id = user.Id,
          firstname = user.FirstName,
          lastname = user.LastName,
          email = user.Email,
          role = user.Role,
          active = user.Active
        }
      };

      return result;
    }

    public async Task<dynamic> UpdateUserAsync(dynamic model) {
      // TODO: add model input validation
      var user = await _repository.AddOrUpdateAsync(new User {
        Email = model.email,
        FirstName = model.firstname,
        LastName = model.lastname
      });

      var updatedUser = await _repository.GetUserByIdAsync(user.Id);

      var result = new {
        email = updatedUser.Email,
        firstName = updatedUser.FirstName,
        lastName = updatedUser.LastName
      };

      return result;
    }
  }
}