using AutoMapper;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using DatingApp.API.Helpers;
using CloudinaryDotNet;
using DatingApp.API.Dtos;
using System.Security.Claims;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using System.Linq;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/UsersController/{userId}/photos")]
    [ApiController]
    public class PhotosControllers : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosControllers(IDatingRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _repo = repo;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
            
            
        }
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, PhotoForCreationDto photoForCreationDto)
        {
            if (userId !=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
               return Unauthorized();
               


            var userFromRepo = await _repo.GetUser(userId);

            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();
            if(file.Length>0){
                using(var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name,stream),
                        Transformation = new Transformation().Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = _cloudinary.Upload(uploadParams);
            }


        }
        photoForCreationDto.Url = uploadResult.Url.ToString();
        photoForCreationDto.PublicId = uploadResult.PublicId;

        var photo = _mapper.Map<Photo>(photoForCreationDto);

        if(!userFromRepo.Photos.Any(u => u.IsMain))
        photo.IsMain = true;

        userFromRepo.Add(photo);
        if(await _repo.SaveAll()){
            return Ok();

        }
        return BadRequest("could not add the photo")

    }
}
}
