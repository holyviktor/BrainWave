using AutoMapper;
using BrainWave.Core.Entities;

namespace IdentityApi.Models.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {
            this.CreateMap<User, UserViewModel>();
        }
    }
}
