using AutoMapper;
using Library.Application.Members.DTOs;
using Library.Domain.Entities;

namespace Library.Application.Members.Mapping;

public class MemberProfile : Profile
{
    public MemberProfile()
    {
        CreateMap<MemberCreateDto, Member>();
        CreateMap<MemberUpdateDto, Member>();
        CreateMap<Member, MemberResponseDto>();
    }
}


