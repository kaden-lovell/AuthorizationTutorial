using System;
using System.Linq;
using System.Threading.Tasks;
using BloggerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BloggerApi.Persistence {
  public class Repository<TModel> : IRepository<TModel> where TModel : Model {
    private readonly IPersistenceContext _persistenceContext;
    private readonly DataContext _dataContext;
    private readonly DbSet<TModel> _entities;

    public Repository(IPersistenceContext persistenceContext, DataContext dataContext) {
      _dataContext = dataContext;
      _persistenceContext = persistenceContext;
      _entities = dataContext.Set<TModel>();
    }

    public IQueryable AsQueryable() {
      return _persistenceContext.Set<TModel>();
    }

    public async Task<TModel> AddOrUpdateAsync(TModel model) {
      if (model == null) {
        return (await _entities.AddAsync(model)).Entity;
      }

      return _entities.Update(model).Entity;
    }

    public async Task DeleteAsync(TModel model) {
      if (model == null) {
        throw new ArgumentNullException("entity");
      }

      _entities.Remove(model);
      await _dataContext.SaveChangesAsync();
    }
  }
}