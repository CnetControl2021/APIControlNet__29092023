//using APIControlNet.Models;
//using System;

//namespace APIControlNet.DTOs.Seeding
//{
//    public class SeedingVersion2 
//    {
//        public override void Seed(CnetCoreContext context)
//        {
//            IList<VersionDTO> versions = new List<VersionDTO>();

//            versions.Add(new VersionDTO()
//            {
//                SystemId = 3,
//                VersionId = "1.0",
//                RevisionId = "1",
//                UserName = "Jose Manuel Salazar",
//                UserNameCheck = "Octavio Moya",
//                Description = "Version Inicial",
//                VersionDate = new DateTime(2023-05-01),
//                Active = true,
//                Locked = false,
//                Deleted = false
//            });

//            context.Versions.AddRange((Models.Version)versions);
//            base.Seed(context);
//        }
//    }
//}
