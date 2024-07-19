using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CommentDTO
    {

        public string ?Text { get; set; }
        public DateTime PublishedOn { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
    }
}
