using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext context;
        public SQLRegionRepository(NZWalksDbContext context)
        {
            this.context = context;
        }

        public async Task<Region> CreateAsync(Region region)
        {
           await context.Regions.AddAsync(region);
           await context.SaveChangesAsync();
           return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            Region? foundRegion = await context.Regions.FindAsync(id);
            if (foundRegion == null)
                return null;

            context.Regions.Remove(foundRegion);
            await context.SaveChangesAsync();
            return foundRegion;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await context.Regions.ToListAsync(); 
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await context.Regions.FindAsync(id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            Region? foundRegion = await context.Regions.FindAsync(id);
            if (foundRegion == null)
                return null;

            foundRegion.Code = region.Code;
            foundRegion.Name = region.Name;
            foundRegion.RegionImageUrl = region.RegionImageUrl;

            await context.SaveChangesAsync();

            return foundRegion;
        }
    }
}
