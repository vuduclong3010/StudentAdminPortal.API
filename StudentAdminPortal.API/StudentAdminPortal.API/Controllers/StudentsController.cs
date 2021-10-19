using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DateModels = StudentAdminPortal.API.DataModels;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly IImageRepository _imageRepository;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper, IImageRepository imageRepository)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _imageRepository = imageRepository;
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
        [Route("[controller]/{studentId:guid}"), ActionName("GetStudentAsync")]
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

        // Delete
        [HttpDelete]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
            if(await _studentRepository.Exists(studentId))
            {
                var student = await _studentRepository.DeleteStudent(studentId);
                return Ok(_mapper.Map<Student>(student));
            }

            return NotFound();
        }

        // Add
        [HttpPost]
        [Route("[controller]/Add")]
        public async Task<IActionResult> AddStudentAsync([FromBody] AddStudentRequest request)
        {
            var student = await _studentRepository.AddStudent(_mapper.Map<DateModels.Student>(request));

            return CreatedAtAction(nameof(GetStudentAsync), new { studentId = student.ID },
                _mapper.Map<Student>(student));
        }

        [HttpPost]
        [Route("[controller]/{studentId:guid}/upload-image")]
        public async Task<IActionResult> UploadImage([FromRoute] Guid studentId, IFormFile profileImage)
        {
            var validExtensions = new List<string>
            {
               ".jpeg",
               ".png",
               ".gif",
               ".jpg"
            };

            if (profileImage != null && profileImage.Length > 0)
            {
                var extension = Path.GetExtension(profileImage.FileName);
                if (validExtensions.Contains(extension))
                {
                    if (await _studentRepository.Exists(studentId))
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);

                        var fileImagePath = await _imageRepository.Upload(profileImage, fileName);

                        if (await _studentRepository.UpdateProfileImage(studentId, fileImagePath))
                        {
                            return Ok(fileImagePath);
                        }

                        return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading image");
                    }
                }

                return BadRequest("This is not a valid Image format");
            }

            return NotFound();
        }
    }
}