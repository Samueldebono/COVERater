using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COVERater.API.Models;
using AutoMapper;
using COVERater.API.Bindings;
using COVERater.API.Dto;
using COVERater.API.Services;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COVERater.API.Controllers
{
    [Route("api")]
    [ApiController]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 10)]
    [HttpCacheValidation(MustRevalidate = true)]
    [Produces("application/json")] 
    public class ImageController: ControllerBase
    {
        private readonly ICoveraterRepository _CoveraterRepository;
        private readonly IMapper _mapper;

        public ImageController(ICoveraterRepository CoveraterRepository, IMapper mapper)
        {
            _CoveraterRepository = CoveraterRepository ?? throw new ArgumentNullException(nameof(CoveraterRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Authorize]
        [HttpGet("image/{id}")]
        public ActionResult<Image> GetImage(int id)
        {
            var image = _CoveraterRepository.GetImage(id);
            if (image == null)
                return NotFound();
            return Ok(image);
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpGet("images")]
        public ActionResult<List<ImageDto>> GetAllImages()
        {
            var images = _CoveraterRepository.GetImages();
            if (images == null)
                return NotFound();
            var results = _mapper.Map<List<ImageDto>>(images);
            return Ok(results);
        }

        [Authorize]
        [HttpGet("image")]
        public ActionResult<List<ImageDto>> GetRandomImage(SearchImageBinding binding)
        {
            var images = _CoveraterRepository.GetImages();
            if (images == null)
                return NotFound();
            if (binding.PreviousImageIds != null && binding.PreviousImageIds.Length > 0)
                images = images.Where(x => !binding.PreviousImageIds.Contains(x.ImageId));
            if (binding.ReturnRandom.HasValue && binding.ReturnRandom.Value)
            {
                Random rnd = new Random();
                images = images.OrderBy(x => rnd.Next()).ToList();
            }
            var results = _mapper.Map<ImageDto>(images.FirstOrDefault());
            return Ok(results);
        }

    }
}
