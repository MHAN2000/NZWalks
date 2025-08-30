using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext context;
        private readonly IRegionRepository regionRepository;

        public RegionsController(NZWalksDbContext context, IRegionRepository regionRepository)
        {
            this.context = context;
            this.regionRepository = regionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            List<Region> regions = await context.Regions.ToListAsync();
            List<RegionDto> regionsDto = new List<RegionDto>();

            regions.ForEach(region =>
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl,
                });
            });

            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Region? region = await context.Regions.FindAsync(id);
            if ( region == null)
                return NotFound();

            RegionDto regionDto = new RegionDto() { Id = region.Id, Code = region.Code, Name = region.Name, RegionImageUrl = region.RegionImageUrl };
            return Ok(regionDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto request)
        {
            Region region = new Region { Code = request.Code, Name = request.Name, RegionImageUrl = request.RegionImageUrl };
            await context.Regions.AddAsync(region);
            await context.SaveChangesAsync();

            RegionDto regionDto = new RegionDto { Code = region.Code, Name = region.Name, Id = region.Id, RegionImageUrl = region.RegionImageUrl };

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto request)
        {
            Region? foundRegion = await context.Regions.FindAsync(id);
            if (foundRegion == null) return NotFound();

            foundRegion.RegionImageUrl = request.RegionImageUrl;
            foundRegion.Code = request.Code;
            foundRegion.Name = request.Name;

            await context.SaveChangesAsync();

            RegionDto region = new RegionDto { Id = foundRegion.Id, Code = foundRegion.Code, Name = foundRegion.Name, RegionImageUrl= foundRegion.RegionImageUrl };

            return Ok(region);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Region? region = await context.Regions.FindAsync(id);
            if (region == null) return NotFound();

            context.Regions.Remove(region);
            await context.SaveChangesAsync();

            RegionDto regionDto = new RegionDto { Code = region.Code, Name = region.Name, Id = region.Id, RegionImageUrl = region.RegionImageUrl };
            return Ok(regionDto);
        }
    }
}
