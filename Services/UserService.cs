using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TinyDemo.Common.Data;
using TinyDemo.Common.Helpers;
using TinyDemo.Models;

namespace TinyDemo.Services
{
    public class UserService
    {
        private readonly DataContext context;

        public UserService(DataContext dataContext)
        {
            context = dataContext;
        }

        public async Task<IEnumerable<UserViewModel>> GetUsersAsync()
        {
            // TODO add pagination and filtering options according to business logic
            var users = await context.Users.ToListAsync();

            return users.ConvertAll(MapToViewModel);
        }

        public async Task<UserViewModel> GetUserAsync(int id)
        {
            // TODO add methods to find by name and / or email
            User user = await FindById(id);

            if (user == null)
            {
                throw new UserNotFoundException("User Not Found");
            }

            return MapToViewModel(user);
        }

        public async Task<UserViewModel> CreateUserAsync(UserViewModel userVM)
        {
            // TODO Add additional validations according to business logic
            var salt = Salt.Create();

            // TODO Add a business logic for default password
            var password = userVM.Password ?? "password";

            // TODO Add business logic for default role
            var roleId = 2;

            User user = new User
            {
                Id = userVM.Id,
                Email = userVM.Email,
                FirstName = userVM.FirstName,
                LastName = userVM.LastName,
                Address = userVM.Address,
                Phone = userVM.Phone,
                Password = Hash.Create(password, salt),
                Salt = salt,
                RoleId = roleId
            };

            context.Users.Add(user);

            await context.SaveChangesAsync();

            return MapToViewModel(user);
        }

        public async Task<UserViewModel> UpdateUserAsync(int id, UserViewModel userVM)
        {
            try
            {
                var user = await FindById(id);

                // TODO Add auto mapping

                if (!string.IsNullOrEmpty(userVM.Address))
                {
                    user.Address = userVM.Address;
                }

                if (!string.IsNullOrEmpty(userVM.FirstName))
                {
                    user.FirstName = userVM.FirstName;
                }


                if (!string.IsNullOrEmpty(userVM.LastName))
                {
                    user.LastName = userVM.LastName;
                }


                if (!string.IsNullOrEmpty(userVM.Phone))
                {
                    user.Phone = userVM.Phone;
                }

                // TODO Add additional validations according to business logic
                await context.SaveChangesAsync();

                return MapToViewModel(user);
            }
            catch(UserNotFoundException ex)
            {
                throw ex;
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;
            }
        }

        public async Task<bool> RemoveUserAsync(int id)
        {
            var user = await FindById(id);

            context.Users.Remove(user);

            var result = await context.SaveChangesAsync();

            return result > 0;
        }

        #region helpers

        private async Task<User> FindById(int id)
        {
            return await context.Users.FindAsync(id);
        }

        private UserViewModel MapToViewModel(User user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Phone = user.Phone
            };
        }

        #endregion
    }
}
