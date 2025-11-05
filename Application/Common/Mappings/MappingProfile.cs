// src/Application/Common/Mappings/MappingProfile.cs
using AutoMapper;
using SaaS.MaundCalculator.Application.Features.Records.Queries;
using SaaS.MaundCalculator.Domain.Entities.Records;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SaaS.MaundCalculator.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<KapasRecord, KapasRecordDto>()
            .ForMember(dest => dest.Maund, opt => opt.MapFrom(src => src.ConvertToMaund()));

        CreateMap<KaniRecord, KaniRecordDto>()
            .ForMember(dest => dest.Maund, opt => opt.MapFrom(src => src.ConvertToMaund()));
    }
}