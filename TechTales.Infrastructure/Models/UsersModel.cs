using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Domain.entities;

namespace TechTales.Infrastructure.Models
{
    public partial class UsersModel
    {

        public int Id { get; set; }

        public string ImgProfile { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string? Role { get; set; }

        public DateTime? DateRegister { get; set; }

    }
}
