using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity.Models
{
    public class Like
    {
        public int Id { get; set; }
        public DateTime LikedOn { get; set; }
        public int UserId { get; set; }
        public AppUser ?User { get; set; }
        public int PostId { get; set; }
        public  Post ?Post { get; set; } 
    }

}
