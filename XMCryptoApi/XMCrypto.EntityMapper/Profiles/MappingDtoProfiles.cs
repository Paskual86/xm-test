using AutoMapper;
using XMCrypto.Domain.Entities;
using XMCrypto.Dtos;
using XMCrypto.Utils;

namespace XMCrypto.EntityMapper.Profiles
{
    public class MappingDtoProfiles : Profile
    {
        public MappingDtoProfiles()
        {
            CreateMap<CryptoProvider, CryptoProviderDto>().ReverseMap();
            CreateMap<BitCoinPrice, BitCoinPriceDto>()
                .ForMember(dest => dest.SellPrice, opt => opt.MapFrom(src => src.SellPrice.FormatPrice()))
                .ForMember(dest => dest.BuyPrice, opt => opt.MapFrom(src => src.BuyPrice.FormatPrice()))
                .ReverseMap();
        }
    }
}