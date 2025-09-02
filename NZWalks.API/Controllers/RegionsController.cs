using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            List<Region> regions = await regionRepository.GetAllAsync();
            List<RegionDto> regionsDto = mapper.Map<List<RegionDto>>(regions);

            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Region? region = await regionRepository.GetByIdAsync(id);
            if (region == null)
                return NotFound();

            RegionDto regionDto = mapper.Map<RegionDto>(region);
            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto request)
        {
            Region region = mapper.Map<Region>(request);
            region = await regionRepository.CreateAsync(region);

            RegionDto regionDto = mapper.Map<RegionDto>(region);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Region? region = mapper.Map<Region>(request);
            region = await regionRepository.UpdateAsync(id, region);

            if (region == null)
                return NotFound();

            RegionDto regionDto = mapper.Map<RegionDto>(region);

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Region? region = await regionRepository.DeleteAsync(id);
            if (region == null)
                return NotFound();

            RegionDto regionDto = mapper.Map<RegionDto>(region);
            return Ok(regionDto);
        }
    }
}
