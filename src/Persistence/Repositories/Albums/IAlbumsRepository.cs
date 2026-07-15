using RymCloneApi.src.Domain.Entities;
using System.Linq.Expressions;

namespace RymCloneApi.src.Persistence.Repositories.Albums
{
  public interface IAlbumsRepository : IRepository<Album>
  {
    public Task<Album?> GetMostRecentAlbumAsync();
  }
}
