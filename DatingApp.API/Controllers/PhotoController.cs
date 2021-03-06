using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDatingRepository _repo;
        private readonly IOptions<CloudinarySettings> _cloudinaryconfig;
        private Cloudinary _cloudinary;

        public PhotoController(IDatingRepository repo, IMapper mapper,

        IOptions<CloudinarySettings> cloudinaryconfig)
        {
            _cloudinaryconfig = cloudinaryconfig;
            _repo = repo;
            _mapper = mapper;

            Account acc = new Account(_cloudinaryconfig.Value.CloudName,
                _cloudinaryconfig.Value.ApiKey,
                _cloudinaryconfig.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {

            var photofromrepo = await _repo.GetPhoto(id);
            var photoForReturn = _mapper.Map<PhotoForReturnDTO>(photofromrepo);
            return Ok(photoForReturn);

        }
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId,
        [FromForm] PhotoForCreationDTO photoForCreationDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var userfromrepo = await _repo.GetUser(userId);
            var file = photoForCreationDTO.File;
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            photoForCreationDTO.Url = uploadResult.Url.ToString();
            photoForCreationDTO.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDTO);

            if (!userfromrepo.Photos.Any(u => u.isMain))
                photo.isMain = true;

            userfromrepo.Photos.Add(photo);

            if (await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDTO>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.id }, photoToReturn);
            }
            return BadRequest("Problem occured on uploading Photo");
        }
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> setMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var userfromrepo = await _repo.GetUser(userId);
            if (!userfromrepo.Photos.Any(p => p.id == id))
            {
                return Unauthorized();
            }
            var photofromrepo = await _repo.GetPhoto(id);
            if (photofromrepo.isMain)
            {
                return BadRequest("This is already a main photo");
            }

            var currentMainPhoto = await _repo.GetMainPhoto(userId);
            currentMainPhoto.isMain = false;
            photofromrepo.isMain = true;
            if (await _repo.SaveAll())
                return NoContent();
            return BadRequest("Could not set photo to main");

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> deletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var userfromrepo = await _repo.GetUser(userId);
            if (!userfromrepo.Photos.Any(p => p.id == id))
            {
                return Unauthorized();
            }
            var photofromrepo = await _repo.GetPhoto(id);
            if (photofromrepo.isMain)
            {
                return BadRequest("You cannot delete main Photo");
            }
            if(photofromrepo.PublicId!=null){

            var deleteionParams = new DeletionParams(photofromrepo.PublicId);
            var result = _cloudinary.Destroy(deleteionParams);

            if(result.Result=="ok")
                _repo.Delete(photofromrepo);
            }
            if(photofromrepo.PublicId==null)
                _repo.Delete(photofromrepo);
            
            if(await _repo.SaveAll()){
                return Ok();
            }
            return BadRequest("Failed to Delete Photo");
        }
    }

}