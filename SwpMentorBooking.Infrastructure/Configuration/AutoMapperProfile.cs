using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SwpMentorBooking.Domain.Entities;
using SwpMentorBooking.Infrastructure.DTOs.ImportedUser;

namespace SwpMentorBooking.Infrastructure.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Mapping for StudentDTO
            CreateMap<CSVStudentDTO, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.ProfilePhoto, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.IsFirstLogin, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.AdminDetail, opt => opt.Ignore())
                .ForMember(dest => dest.FeedbackGivenByNavigations, opt => opt.Ignore())
                .ForMember(dest => dest.FeedbackGivenToNavigations, opt => opt.Ignore())
                .ForMember(dest => dest.MentorDetail, opt => opt.Ignore())
                .ForMember(dest => dest.StudentDetail, opt => opt.MapFrom(src =>
                    new StudentDetail()
                    {
                        StudentCode = src.StudentCode
                    }
                ))
                .ForMember(dest => dest.UserActivities, opt => opt.Ignore())
                .ForMember(dest => dest.UserSessions, opt => opt.Ignore());

            // Mapping for CSVMentorDTO
            CreateMap<CSVMentorDTO, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.ProfilePhoto, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.IsFirstLogin, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.AdminDetail, opt => opt.Ignore())
                .ForMember(dest => dest.FeedbackGivenByNavigations, opt => opt.Ignore())
                .ForMember(dest => dest.FeedbackGivenToNavigations, opt => opt.Ignore())
                .ForMember(dest => dest.MentorDetail, opt => opt.MapFrom(src =>
                    new MentorDetail()
                    {
                        MainProgrammingLanguage = src.MainProgrammingLanguage,
                        AltProgrammingLanguage = src.AltProgrammingLanguage,
                        Framework = src.Framework,
                        Education = src.Education,
                        AdditionalContactInfo = src.AdditionalContactInfo,
                        Description = src.Description,
                    }
                 ))
                .ForMember(dest => dest.StudentDetail, opt => opt.Ignore())
                .ForMember(dest => dest.UserActivities, opt => opt.Ignore())
                .ForMember(dest => dest.UserSessions, opt => opt.Ignore());
        }
    }
}
