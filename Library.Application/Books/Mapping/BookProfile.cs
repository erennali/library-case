using AutoMapper;
using Library.Application.Books.DTOs;
using Library.Domain.Entities;

namespace Library.Application.Books.Mapping;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<BookCreateDto, Book>();
        CreateMap<BookUpdateDto, Book>();
        CreateMap<Book, BookResponseDto>();
    }
}


