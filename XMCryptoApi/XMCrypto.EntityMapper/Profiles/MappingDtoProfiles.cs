using AutoMapper;
using XMCrypto.Domain.Entities;
using XMCrypto.Dtos;

namespace XMCrypto.EntityMapper.Profiles
{
    public class MappingDtoProfiles : Profile
    {
        public MappingDtoProfiles()
        {
            CreateMap<CryptoProvider, CryptoProviderDto>().ReverseMap();
        }
    }
}