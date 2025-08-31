using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            List<Walk> walks = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);

            List<WalkDto> walksDto = mapper.Map<List<WalkDto>>(walks);

            return Ok(walksDto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Walk walk = mapper.Map<Walk>(request);
            walk = await walkRepository.CreateAsync(walk);

            WalkDto walkDto = mapper.Map<WalkDto>(walk);

            return Ok(walkDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Walk? walk = await walkRepository.GetByIdAsync(id);
            if (walk == null) return NotFound();

            WalkDto walkDto = mapper.Map<WalkDto>(walk);
            return Ok(walkDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto request)
        {
            Walk? walk = mapper.Map<Walk>(request);
            walk = await walkRepository.UpdateAsync(id, walk);
            if (walk == null) return NotFound();

            WalkDto walkDto = mapper.Map<WalkDto>(walk);

            return Ok(walkDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Walk? walk = await walkRepository.DeleteAsync(id);
            if (walk == null) return NotFound();

            WalkDto walkDto = mapper.Map<WalkDto>(walk);

            return Ok(walkDto);
        }
    }
}
