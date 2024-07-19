using Microsoft.AspNetCore.Mvc;
using Repository.EfCore.Abstarct;
using entity.Models;
using DTO;
using System.Linq;
using Repository.Contract;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeRepository _likeRepository;
       

        public LikeController(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
           
        }
        [Authorize]
        [HttpPost("create")]
        public IActionResult Create([FromBody] LikeDTO like)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı");
            }
            if (like == null)
            {
                return BadRequest(new { message = "Invalid data" });
            }

            var entity = new Like
            {
                Id = like.Id,
                LikedOn = like.LikedOn,
                PostId = like.PostId,
                UserId = int.Parse(userId.ToString()),
            };

            try
            {
                _likeRepository.Insert(entity);

               
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the like", error = ex.Message });
            }
        }
    }
}
