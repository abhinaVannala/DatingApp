using AutoMapper;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using DatingApp.API.Helpers;
using DatingApp.API.Dtos;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using DatingApp.API.Models;
using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

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

        [HttpGetAttribute("{id}",Name = "GetPhoto")]

        public async Task<IActionResult>GetPhoto(int id){
            var photoFromRepo = await _repo.GetPhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreationDto photoForCreationDto)
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
        var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);

        if(await _repo.SaveAll()){

            var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
            return CreatedAtRoute("GetPhoto", new{id = photo.Id}, photoToReturn);

        }
        return BadRequest("could not add the photo");}

    
        [HttpPost("{id}/setMain}")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var user = await _repo.GetUser(userId);

            if (!user.Photos.Any(p => p.ID == id))
                return Unauthorized();

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.isMain)
                return BadRequest("This is already the main photo.");

            var currentMainPhoto = await _repo.GetMainPhoto(userId);
            currentMainPhoto.isMain = false;

            photoFromRepo.isMain = true;

            if (await _repo.SaveAll())
            {
                return NoContent();
            }
            return BadRequest("could not set photo to main");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var user = await _repo.GetUser(userId);

            if (!user.Photos.Any(p => p.ID == id))
                return Unauthorized();

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.isMain)
                return BadRequest("You cannot delete you main photo.");

            if (photoFromRepo.PublicId != null)
            {
                var deleteparams = new DeletionParams(photoFromRepo.PublicId);

                var result = _cloudinary.Destroy(deleteparams);

                if (result.Result == "ok")
                {
                    _repo.Delete(photoFromRepo);
                }

                if(photoFromRepo.PublicId == null){
                _repo.Delete(photoFromRepo);
                }


                if (await _repo.SaveAll())
                 {
                   return Ok();
                 }

                return BadRequest("Could not delete photo.");

            }   

        }


    }
}

