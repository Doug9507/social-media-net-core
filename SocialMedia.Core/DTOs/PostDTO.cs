using System;

namespace SocialMedia.Core.DTOs
{
    public class PostDTO
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
