using APIControlNet.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace APIControlNet.DTOs.Seeding
{
    public class SeedingVersion
    {
        //public static void Seed(ModelBuilder modelBuilder)
        //{
        //    var version = new VersionDTO { SystemId = 3, VersionId = "1.0", RevisionId = "1", UserName ="Jose Manuel Salazar",
        //        UserNameCheck = "Octavio Moya",
        //        Description = "Version Inicial",
        //        VersionDate = new DateTime(2023-05-01),
        //        Active = true,
        //        Locked = false,
        //        Deleted = false
        //    };

        //    modelBuilder.Entity<VersionDTO>().HasData(version);
        //}

        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<CnetCoreContext>();
                //context.Database.EnsureCreated();

                if (!context.Versions.Any(x => x.SystemId == 3))
                {
                    context.Versions.AddRange(new List<Models.Version>()
                    {
                        new Models.Version
                        {
                            SystemId = 3,
                            VersionId = "1.0",
                            RevisionId = "1.0",
                            UserName = "Jose Manuel Salazar",
                            UserNameCheck = "Octavio Moya",
                            Description = "Version Inicial",
                            VersionDate = new DateTime(05-01-2023),
                            Active = true,
                            Locked = false,
                            Deleted = false
                        },
                        new Models.Version
                        {
                            SystemId = 3,
                            VersionId = "1.1",
                            RevisionId = "1.1",
                            UserName = "Jose Manuel Salazar",
                            UserNameCheck = "Octavio Moya",
                            Description = "Version Inicial",
                            VersionDate = new DateTime(05-05-2023),
                            Active = true,
                            Locked = false,
                            Deleted = false
                         }
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}

