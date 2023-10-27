using AutoMapper;
using BrainWave.Core.Entities;

namespace BrainWave.WebUI.Models.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {
            this.CreateMap<User, UserViewModel>();
            this.CreateMap<Article, ArticleViewModel>();
            this.CreateMap<ArticleInputViewModel, Article>();
            this.CreateMap<User, ProfileInputViewModel>();
        }
    }
}
