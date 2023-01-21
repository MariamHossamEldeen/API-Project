﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Ahmed Nasr",
                    Email = "ahmedNasr@hotmail.com" , 
                    UserName = "Ahmed.Nasr" , 
                    PhoneNumber = "010000000000"
                };
                await userManager.CreateAsync(user , "P@ssw0rd");
            }
        }

        
    }
}
