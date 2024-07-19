using DTO;
namespace DTO
{
    public class GetDTO
    {
        public int PostId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? Url { get; set; }
        public string? Image { get; set; }
        public DateTime PublishedOn { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public UserDTO ?User { get; set; } 
        public List<LikeDTO> Likes { get; set; } = new List<LikeDTO>();
        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
    }
}
