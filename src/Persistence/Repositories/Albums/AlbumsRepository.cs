using Microsoft.EntityFrameworkCore;
using RymCloneApi.src.Domain.Entities;
using RymCloneApi.src.Persistence.Context.Interfaces;
using System.Linq.Expressions;

namespace RymCloneApi.src.Persistence.Repositories.Albums
{
  public class AlbumsRepository : Repository<Album>, IAlbumsRepository
  {
    public AlbumsRepository(IAppDbContext context) : base(context) { }

    public async Task<Album?> GetMostRecentAlbumAsync()
    {
     var alg = _table.OrderByDescending(al => al.CreatedAt).Include(al => al.Artist);

      return await alg.FirstOrDefaultAsync();
    }
  }
}
