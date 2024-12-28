using AutoMapper;
using Bookify.web.Core.Models;

namespace Bookify.Web.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Category
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryFormViewModel, Category>().ReverseMap();
            //Authors
            CreateMap<Author, AuthorViewModel>();
            CreateMap<AuthorFormViewModel, Author>().ReverseMap();
            //Publisher
            CreateMap<Publisher, PublisherViewModel>();
            CreateMap<PublisherFormViewModel, Publisher>().ReverseMap();
        }
    }
}