using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetworkApp.Core.Application.Helpers.Sessions;
using SocialNetworkApp.Core.Application.Interfaces.Repositories;
using SocialNetworkApp.Core.Application.Interfaces.Services;
using SocialNetworkApp.Core.Application.ViewModels.Replies;
using SocialNetworkApp.Core.Application.ViewModels.Users;
using SocialNetworkApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.Services
{
    public class GenericService<ViewModel, SaveViewModel, Entity> : IGenericService<ViewModel, SaveViewModel, Entity>
        where ViewModel : class
        where SaveViewModel : class
        where Entity : class
    {
        private readonly IGenericRepository<Entity> _genericrepository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<Entity> repository, IMapper mapper)
        {
            _genericrepository = repository;
            _mapper = mapper;
        }

        public virtual async Task<List<ViewModel>> GetAllViewModel()
        {
            var entitylist = await _genericrepository.GetAllAsync();

            return _mapper.Map<List<ViewModel>>(entitylist);

        }
        public virtual async Task<SaveViewModel> CreateViewModel(SaveViewModel vm)
        {
            Entity entity = _mapper.Map<Entity>(vm);


            entity = await _genericrepository.AddAsync(entity);

            SaveViewModel savevm = _mapper.Map<SaveViewModel>(entity);

            return savevm;

        }
        public virtual async Task<SaveViewModel> GetByIdViewModel(int id)
        {
            Entity entity = await _genericrepository.GetByIdAsync(id);

            SaveViewModel entityViewModel = _mapper.Map<SaveViewModel>(entity);

            return entityViewModel;

        }

        public virtual async Task UpdateViewModel(SaveViewModel vm, int id)
        {
            Entity entity = _mapper.Map<Entity>(vm);

            await _genericrepository.UpdateAsync(entity,id);
        }

        public virtual async Task DeleteViewModel(int id)
        {
            Entity entity = await _genericrepository.GetByIdAsync(id);

            await _genericrepository.DeleteAsync(entity);
        }
    }
}
