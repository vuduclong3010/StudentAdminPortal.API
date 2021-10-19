using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repositories;
using System;
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

        [HttpGet]
        [Route("[controller]")]
        // --------------------------------- C2: Automapper ---------------------------------
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetStudentsAsync();

            return Ok(_mapper.Map<List<Student>>(students));
        }

        [HttpGet]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> GetStudentAsync([FromRoute] Guid studentId)
        {
            // Fetch Student Details
            var student = await _studentRepository.GetStudentAsync(studentId);

            // Return Student
            if(student == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Student>(student));
        }

        // Update Student Details
        [HttpPut]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId, [FromBody] UpdateStudentRequest request)
        {
            if (await _studentRepository.Exists(studentId))
            {
                // Update Details
                var updatedStudent = await _studentRepository.UpdateStudent(studentId, _mapper.Map<DataModels.Student>(request));

                if (updatedStudent != null)
                {
                    return Ok(_mapper.Map<Student>(updatedStudent));
                }
            }
            return NotFound();
        }
    }
}
