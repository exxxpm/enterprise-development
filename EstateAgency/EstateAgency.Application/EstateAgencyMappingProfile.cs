using AutoMapper;
using EstateAgency.Application.Contracts.Application;
using EstateAgency.Application.Contracts.Counterparty;
using EstateAgency.Application.Contracts.Property;
using EstateAgency.Domain.Entitites;

namespace EstateAgency.Application;

public class EstateAgencyMappingProfile : Profile
{
    public EstateAgencyMappingProfile()
    {
        CreateMap<Counterparty, CounterpartyGetDto>();
        CreateMap<Counterparty, CounterpartyCreateEditDto>().ReverseMap();

        CreateMap<Property, PropertyGetDto>();
        CreateMap<Property, PropertyCreateEditDto>().ReverseMap();

        CreateMap<Domain.Entitites.Application, ApplicationGetDto>()
            .ForMember(dest => dest.Counterparty, opt => opt.MapFrom(src => src.Counterparty))
            .ForMember(dest => dest.Property, opt => opt.MapFrom(src => src.Property))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
        CreateMap<Domain.Entitites.Application, ApplicationCreateEditDto>().ReverseMap();
    }
}
