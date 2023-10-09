using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainWave.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public Role Role { get; set; }
        public ICollection<Article>? Articles { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Saving>? Savings { get; set; }
        public ICollection<Following>? Followings { get; set; }

    }
}
