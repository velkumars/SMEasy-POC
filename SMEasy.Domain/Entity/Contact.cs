using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xen.Entity;

namespace SMEasy.Domain.Entity
{
    public partial class Contact : BaseEFEntity
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Cellphone { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime Born { get; set; }
    }
}

