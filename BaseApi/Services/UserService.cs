using System;
using System.Threading.Tasks;
using BaseApi.Domain.Entity.Model;
using BaseApi.Domain.Repositories;
using BaseApi.Domain.Response;
using BaseApi.Domain.UnitOfWork;

namespace BaseApi.Services
{
    public class UserService : GenericService<User>
    {
        public UserService(IGenericRepository<User> repository,IUnitOfWork unitOfWork)
            :base(repository,unitOfWork){}

        public override async Task<ObjectResponse<User>> AddAsync(User obj)
        {
            //find by email
            User check = await FindByEmailAsync(obj.Email);
            if (check != null) return new ObjectResponse<User>(Constants.ErrorMessages.UserErrors.EmailExist);

            return await base.AddAsync(check);
        }

        public override async Task<ObjectResponse<User>> UpdateAsync(User obj)
        {
            User toUpdate = await FindByIdAsync(obj.ID);
            if (toUpdate == null) return new ObjectResponse<User>(Constants.ErrorMessages.UserErrors.UserNotFound);

            //if user attempt to change email
            if (obj.Email != null && obj.Email != toUpdate.Email)
            {
                User checkEmail = await FindByEmailAsync(obj.Email);
                if (checkEmail != null) return new ObjectResponse<User>(Constants.ErrorMessages.UserErrors.EmailExist);
            }
            //TODO update all required fields
            toUpdate.Email = obj.Email ?? toUpdate.Email;
            toUpdate.FullName = obj.FullName ?? toUpdate.FullName;
            toUpdate.PassWord = obj.PassWord ?? toUpdate.PassWord;
            toUpdate.RefreshToken = obj.RefreshToken ?? toUpdate.RefreshToken;
            toUpdate.RefreshTokenExpirationDate = obj.RefreshTokenExpirationDate > DateTime.Now ?
                obj.RefreshTokenExpirationDate : toUpdate.RefreshTokenExpirationDate;

            return await base.UpdateAsync(toUpdate);
        }

        public override async Task<ObjectResponse<User>> DeleteAsync(User obj)
        {
            User toRemove = await FindByIdAsync(obj.ID);
            if (toRemove == null) return new ObjectResponse<User>(Constants.ErrorMessages.UserErrors.UserNotFound);

            return await base.DeleteAsync(obj);
        }

        private async Task<User> FindByIdAsync(long id)
        {
            return await Repository.FirstOrDefaultAsync(u => u.ID == id);
        }

        private async Task<User> FindByEmailAsync(string email)
        {
            return await Repository.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
