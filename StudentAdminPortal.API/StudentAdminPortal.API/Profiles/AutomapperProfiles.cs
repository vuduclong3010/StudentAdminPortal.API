using AutoMapper;
using DataModel = StudentAdminPortal.API.DataModels;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Profiles.AfterMaps;

namespace StudentAdminPortal.API.Profiles
{
    public class AutomapperProfiles:Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<DataModel.Student, Student>()
                .ReverseMap();

            CreateMap<DataModel.Gender, Gender>()
                .ReverseMap();

            CreateMap<DataModel.Address, Address>()
                .ReverseMap();

            //CreateMap<UpdateStudentRequest, DataModel.Student>()
            //    .ForMember(dest => dest.Address.PhysicalAddress, opt => opt.MapFrom(src => src.PhysicalAddress))
            //    .ForMember(dest => dest.Address.PostalAddress, opt => opt.MapFrom(src => src.PostalAddress));

            CreateMap<UpdateStudentRequest, DataModel.Student>()
                .AfterMap<UpdateStudentRequestAfterMap>();

            CreateMap<AddStudentRequest, DataModel.Student>()
                .AfterMap<AddStudentRequestAfterMap>();
        }
    }
}
