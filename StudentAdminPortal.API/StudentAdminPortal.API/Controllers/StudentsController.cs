using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }


        // --------------------------------- C1: ---------------------------------
        //[HttpGet]
        //[Route("[controller]")]

        //public IActionResult GetAllStudents()
        //{
        //    var students = _studentRepository.GetStudents();

        //    var domainModelStudents = new List<Student>();

        //    foreach (var student in students)
        //    {
        //        domainModelStudents.Add(new Student()
        //        {
        //            ID = student.ID,
        //            FirstName = student.FirstName,
        //            LastName = student.LastName,
        //            DateOfBirth = student.DateOfBirth,
        //            Email = student.Email,
        //            Mobile = student.Mobile,
        //            ProfileImageUrl = student.ProfileImageUrl,
        //            GenderId = student.GenderId,
        //            Address = new Address()
        //            {
        //                Id = student.Address.Id,
        //                PhysicalAddress = student.Address.PhysicalAddress,
        //                PostalAddress = student.Address.PostalAddress
        //            },
        //            Gender = new Gender()
        //            {
        //                Id = student.Gender.Id,
        //                Description = student.Gender.Description
        //            }
        //        });
        //    }

        //    return Ok(domainModelStudents);
        //}




        // --------------------------------- C1: Automapper ---------------------------------
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetStudentsAsync();

            return Ok(_mapper.Map<List<Student>>(students));
        }
    }
}
