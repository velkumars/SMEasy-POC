using SMEasy.ViewModel.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xen.Entity;

namespace SMEasy.ViewModel.ViewModel
{
    [FluentValidation.Attributes.Validator(typeof(ContactValidator))]
    public class ContactViewModel : BaseEFEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Cellphone { get; set; }
        public bool IsActive { get; set; }
        public DateTime Born { get; set; }
    }
}
