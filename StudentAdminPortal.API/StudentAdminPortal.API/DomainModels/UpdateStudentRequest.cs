using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdminPortal.API.DomainModels
{
    public class UpdateStudentRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public long Mobile { get; set; }

        public Guid GenderId { get; set; }

        public string PhysicalAddress { get; set; }

        public string PostalAddress { get; set; }
    }
}
