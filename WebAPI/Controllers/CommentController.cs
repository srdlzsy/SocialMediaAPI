using DTO;
using entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Contract;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        [Authorize]
        [HttpPost("create")]
        public IActionResult Create([FromBody] CommentDTO commentDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("Kullanıcı kimliği bulunamadı");
            }

            if (commentDto == null)
            {
                return BadRequest("Yorum boş olamaz");
            }

            if (string.IsNullOrWhiteSpace(commentDto.Text))
            {
                return BadRequest("Yorum içeriği boş olamaz");
            }

            var newComment = new Comment
            {
                Text = commentDto.Text,
                PublishedOn = DateTime.Now,
                PostId = commentDto.PostId,
                UserId = int.Parse(userId ?? ""),
                // Kullanıcı eşlemesi, varsayılan olarak repository veya servis katmanında ele alınmıştır
            };

            _commentRepository.Insert(newComment); // Comment'i veritabanına ekliyoruz

            return Ok(newComment); // HTTP 200 OK
        }
        [Authorize]
        [HttpPost("delete")]
        public IActionResult Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (userId == null)
            {
                return Unauthorized("Kullanıcı kimliği bulunamadı");
            }

            if (!int.TryParse(userId, out var currentUserId))
            {
                return BadRequest("Geçersiz kullanıcı kimliği");
            }

            var comment = _commentRepository.GetById(id);
            if (comment == null)
            {
                return NotFound(new { message = "Yorum bulunamadı" });
            }

            // Admin kontrolü
            if (userRole == "admin" || comment.UserId == currentUserId)
            {
                try
                {
                    _commentRepository.Delete(id);
                    return Ok(new { message = "Yorum başarıyla silindi" });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "Yorum silinirken bir hata oluştu", error = ex.Message });
                }
            }

            return Forbid("Yalnızca kendi yorumlarınızı silebilirsiniz veya admin yetkiniz yok");
        }



    }
}

