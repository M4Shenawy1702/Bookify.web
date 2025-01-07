using AutoMapper;
using Bookify.web.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.Web.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Authors
            CreateMap<Author, AuthorViewModel>();
            CreateMap<AuthorFormViewModel, Author>().ReverseMap();
            CreateMap<Author, SelectListItem>()
                .ForMember(dest=>dest.Value,opt=>opt.MapFrom(src=>src.Id))
                .ForMember(dest=>dest.Text,opt=>opt.MapFrom(src=>src.Name));


            //Category
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryFormViewModel, Category>().ReverseMap();
            CreateMap<Category, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

            //Publisher
            CreateMap<Publisher, PublisherViewModel>();
            CreateMap<PublisherFormViewModel, Publisher>().ReverseMap();
            CreateMap<Publisher, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

            //Book
            CreateMap<BookFormViewModel, Book>()
                .ReverseMap()
                .ForMember(dest=>dest.Categories , opt=>opt.Ignore());
           
        }
    }
}