using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.Common.DataProvider;
using Org.Common.Domain;
using Org.Common.Exception;
using Org.Common.Manager;
using Org.Common.Model;

namespace Org.Services
{
    internal class UserManager : IUserManager
    {
        private readonly UserManager<EmployeePortalUser> _userManager;
        private readonly IDatabaseMigrationProvider _migrationProvider;
        private readonly SignInManager<EmployeePortalUser> _signInManager;

        public UserManager(UserManager<EmployeePortalUser> userManager, IDatabaseMigrationProvider migrationProvider, SignInManager<EmployeePortalUser> signInManager)
        {
            _userManager = userManager;
            _migrationProvider = migrationProvider;
            _signInManager = signInManager;
        }

        public async Task<User> Register(RegistrationModel model)
        {
            await _migrationProvider.MigrateDb();
            var result = await _userManager.CreateAsync(new EmployeePortalUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            }, model.Password);

            if (!result.Succeeded)
            {
                throw new BadRegistrationRequestException(result.Errors);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == model.Email);
          //  await _userManager.AddToRoleAsync(user, "Administrator");

            var roles = await _userManager.GetRolesAsync(user);
            return new User
            {
                Roles = roles.ToList(),
                Email = user.Email,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<User> Login(string login, string password)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == login);
            if (user == null)
            {
                throw new NotAuthorizedException("There is no user with provided login and password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);
            if (!result.Succeeded)
            {
                throw new NotAuthorizedException("There is no user with provided login and password");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return new User
            {
                Email = user.Email,
                Id = user.Id,
                Roles = roles.ToList(),
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
    }
}