using AspNetIdentity.DomainModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentity.Data
{
    //自动生成数据库迁移(旧版本)
    //Enable-Migrations -EnableAutomaticMigrations
    //Add-Migration InitialCreate //Add-Migration -Name InitialCreate -OutPutDir Data/Migrations
    //Update-Database -Verbose
    //参考地址：https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/powershell

    //推荐使用新版本
    //参考地址：https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Core Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Core Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
