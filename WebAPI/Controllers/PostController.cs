using Microsoft.AspNetCore.Mvc;
using Repository.EfCore.Abstarct;
using Repository;
using Repository.Contract;
using entity.Models;
using DTO;
using Microsoft.EntityFrameworkCore;
using Repository.EfCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace WebAPI.Controllers
{

    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }


        [HttpGet]
        [Route("/[controller]")]
        public async Task<IActionResult> Get()
        {
            var posts = await _postRepository.GetAll()
                .Select(post => new GetDTO
                {
                    PostId = post.PostId,
                    Title = post.Title,
                    Description = post.Description,
                    Content = post.Content,
                    Url = post.Url,
                    Image = post.Image,
                    PublishedOn = post.PublishedOn,
                    IsActive = post.IsActive,
                    UserId = post.UserId,
                    User = post.User != null ? new UserDTO
                    {
                        Id = post.User.Id,
                        UserName = post.User.UserName,
                        Email = post.User.Email
                    } : null,

                    Likes = post.Likes.Select(l => new LikeDTO
                    {
                        PostId = l.PostId,
                        UserId = l.UserId,
                        LikedOn = l.LikedOn
                    }).ToList(),
                    Comments = post.Comments.Select(c => new CommentDTO
                    {
                        Text = c.Text,
                        PublishedOn = c.PublishedOn,
                        UserId = c.UserId,
                        PostId = c.PostId
                    }).ToList(),
                    LikesCount = post.Likes.Count,
                    CommentsCount = post.Comments.Count
                })
                .ToListAsync();

            return Ok(posts);
        }
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (userId == null)
            {
                throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı");
            }


            var post = _postRepository.GetAll()
                .Where(post => post.PostId == id).AsEnumerable()
                .Select(post => new GetDTO
                {
                    PostId = post.PostId,
                    Title = post.Title,
                    Description = post.Description,
                    Content = post.Content,
                    Url = post.Url,
                    Image = post.Image,
                    PublishedOn = post.PublishedOn,
                    IsActive = post.IsActive,
                    UserId = post.UserId,
                    User = post.User != null ? new UserDTO
                    {
                        Id = post.User.Id,
                        UserName = post.User.UserName,
                        Email = post.User.Email
                    } : null,
                    Likes = post.Likes.Select(l => new LikeDTO
                    {
                        PostId = l.PostId,
                        UserId = l.UserId,
                        LikedOn = l.LikedOn
                    }).ToList(),
                    Comments = post.Comments.Select(c => new CommentDTO
                    {
                        Text = c.Text,
                        PublishedOn = c.PublishedOn,
                        UserId = int.Parse(userId),
                        PostId = post.PostId

                    }).ToList(),
                    LikesCount = post.Likes.Count,
                    CommentsCount = post.Comments.Count
                })
                .FirstOrDefault();
            if (post?.UserId != int.Parse(userId) || userRole=="admin")
            {
                return Forbid("Yalnızca kendi gönderilerinizi güncelleyebilirsiniz"); // Kullanıcı kendi gönderisi değilse
            }
            if (post == null)
            {
                return NotFound(); // Eğer id'ye sahip post bulunamazsa NotFound döndür
            }

            return Ok(post);
        }
        [Authorize]
        [HttpPost("/create")]
        public IActionResult Create([FromBody] PostDTO postDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı");
            }
            if (postDTO == null)
            {
                return BadRequest("Post cannot be null"); // Optional: Return a bad request if post is null
            }
            var post = new Post
            {
                Title = postDTO.Title,
                Description = postDTO.Description,
                Image = postDTO.Image,
                IsActive = postDTO.IsActive,
                UserId = int.Parse(userId.ToString()),

            };
            _postRepository.Insert(post); // Assuming Insert method in repository handles saving PostDTO to database

            return Ok(); // HTTP 200 OK
        }

        [Authorize]
        [HttpPut("/put/{id}")]
        public IActionResult Update(int id, [FromBody] PostDTO postDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı");
            }

            if (postDTO == null)
            {
                return BadRequest("Post cannot be null"); // İsteği doğru biçimde oluşturduğunuzdan emin olun.
            }

            try
            {
                // Get the existing post from repository
                var existingPost = _postRepository.GetById(id);

                if (existingPost == null)
                {
                    return NotFound($"Post with ID {id} not found"); // Post bulunamazsa
                }

                // kendi ıd si eşleşiyormu sectıgı post
                if (existingPost.UserId != int.Parse(userId))
                {
                    return Forbid("Yalnızca kendi gönderilerinizi güncelleyebilirsiniz"); // Kullanıcı kendi gönderisi değilse
                }

                // Update the existing post properties
                existingPost.Title = postDTO.Title;
                existingPost.Description = postDTO.Description;
                existingPost.Image = postDTO.Image;
                existingPost.IsActive = postDTO.IsActive;

                // Perform update operation
                _postRepository.Update(id,existingPost);

                return Ok("Post updated successfully"); // Başarılı bir şekilde yapıldı
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); // Post bulunamadıysa
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // İstisna fırlatılırsa, hata kodu 500 döndür
            }
        }

        [Authorize]
        [HttpDelete("/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı");
            }
            var existingPost = _postRepository.GetById(id); if (existingPost == null)

            if (existingPost?.UserId != int.Parse(userId))
            {
                return Forbid("Yalnızca kendi gönderilerinizi güncelleyebilirsiniz"); // Kullanıcı kendi gönderisi değilse
            }

            _postRepository.Delete(id);
            return Ok();

        }
       
        }

    }


 

