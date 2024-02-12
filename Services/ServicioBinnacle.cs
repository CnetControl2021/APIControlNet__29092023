using APIControlNet.Models;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;



namespace APIControlNet.Services
{
    public class ServicioBinnacle
    {
        private readonly CnetCoreContext context;
        
        public ServicioBinnacle(CnetCoreContext context)
        {
            this.context=context;
        }

        public async Task AddBinnacle(string usuarioId, string Ip, string name, Guid? storeId)
        {
            var addr = "";
            foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (n.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && n.OperationalStatus == OperationalStatus.Up)
                {
                    addr += n.GetPhysicalAddress().ToString();
                    break;
                }
            }

            var binnacle = new Binnacle
            {
                BinnacleId = Guid.NewGuid(),
                StoreId = storeId,
                Name = "Adicionar ",
                Response = "Adicion de registro " + name,
                UserId = usuarioId,
                BinnacleTypeId = 6,
                Description = "controlVolumetrico",
                ValueName = "Add",
                IpAddress = Ip,
                MacAddress = addr,
                Date = DateTime.Now,
                Updated = DateTime.Now,
                Active = true,
                Locked = false,
                Deleted = false
            };

            context.Add(binnacle);

            await context.SaveChangesAsync();
        }

        public async Task AddBinnacle2(string usuarioId, string Ip, string name, Guid? storeId, string Table)
        {
            var addr = "";
            foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (n.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && n.OperationalStatus == OperationalStatus.Up)
                {
                    addr += n.GetPhysicalAddress().ToString();
                    break;
                }
            }

            var binnacle = new Binnacle
            {
                BinnacleId = Guid.NewGuid(),
                StoreId = storeId,
                Name = "Adicionar ",
                Response = "Adicion de registro " + name + "de " + Table,
                UserId = usuarioId,
                BinnacleTypeId = 6,
                Description = "controlVolumetrico",
                ValueName = "Add",
                IpAddress = Ip,
                MacAddress = addr,
                Date = DateTime.Now,
                Updated = DateTime.Now,
                Active = true,
                Locked = false,
                Deleted = false
            };

            context.Add(binnacle);

            await context.SaveChangesAsync();
        }

        public async Task ManualBinnacle(string usuarioId, string Ip, string name, Guid? storeId)
        {
            var addr = "";
            foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (n.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && n.OperationalStatus == OperationalStatus.Up)
                {
                    addr += n.GetPhysicalAddress().ToString();
                    break;
                }
            }

            var binnacle = new Binnacle
            {
                BinnacleId = Guid.NewGuid(),
                StoreId = storeId,
                Name = name,
                Response = "Adicion de registro manual",
                UserId = usuarioId,
                BinnacleTypeId = 6,
                Description = "controlVolumetrico",
                ValueName = "Event Manual",
                IpAddress = Ip,
                MacAddress = addr,
                Date = DateTime.Now,
                Updated = DateTime.Now,
                Active = true,
                Locked = false,
                Deleted = false
            };

            context.Add(binnacle);

            await context.SaveChangesAsync();
        }

        public async Task EditBinnacle(string usuarioId, string Ip, string name, Guid? storeId)
        {
            var addr = "";
            foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (n.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && n.OperationalStatus == OperationalStatus.Up)
                {
                    addr += n.GetPhysicalAddress().ToString();
                    break;
                }
            }

            var binnacle = new Binnacle
            {
                BinnacleId = Guid.NewGuid(),
                StoreId = storeId,
                Name = "Actualizar",
                Response = "Actualizacion " + name,
                UserId = usuarioId,
                BinnacleTypeId = 6,
                Description = "controlVolumetrico",
                ValueName = "edit",
                IpAddress = Ip,
                MacAddress = addr,
                Date = DateTime.Now,
                Updated = DateTime.Now,
                Active = true,
                Locked = false,
                Deleted = false
            };

            context.Add(binnacle);
            await context.SaveChangesAsync();
        }

        public async Task deleteBinnacle(string usuarioId, string Ip, string name, Guid? storeId)
        {
            var addr = "";
            foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (n.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && n.OperationalStatus == OperationalStatus.Up)
                {
                    addr += n.GetPhysicalAddress().ToString();
                    break;
                }
            }

            var binnacle = new Binnacle
            {
                BinnacleId = Guid.NewGuid(),
                StoreId = storeId,
                Name = "Elimino",
                Response = "Se elimino registro " + name,
                UserId = usuarioId,
                BinnacleTypeId = 6,
                Description = "controlVolumetrico",
                ValueName = "delete",
                IpAddress = Ip,
                MacAddress = addr,
                Date = DateTime.Now,
                Updated = DateTime.Now,
                Active = true,
                Locked = false,
                Deleted = false
            };

            context.Add(binnacle);
            await context.SaveChangesAsync();
        }

        public async Task deleteBinnacle2(string usuarioId, string Ip, string name, Guid? storeId, string Table)
        {
            var addr = "";
            foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (n.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && n.OperationalStatus == OperationalStatus.Up)
                {
                    addr += n.GetPhysicalAddress().ToString();
                    break;
                }
            }

            var binnacle = new Binnacle
            {
                BinnacleId = Guid.NewGuid(),
                StoreId = storeId,
                //Name = "Elimino",
                Response = "Se elimino registro " + name + "de " + Table,
                UserId = usuarioId,
                BinnacleTypeId = 6,
                Description = "Evento controlVolumetrico",
                ValueName = "delete",
                IpAddress = Ip,
                MacAddress = addr,
                Date = DateTime.Now,
                Updated = DateTime.Now,
                Active = true,
                Locked = false,
                Deleted = false
            };

            context.Add(binnacle);
            await context.SaveChangesAsync();
        }


        public async Task deleteLogicBinnacle(string usuarioId, string Ip, string name, Guid? storeId)
        {
            var addr = "";
            foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (n.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && n.OperationalStatus == OperationalStatus.Up)
                {
                    addr += n.GetPhysicalAddress().ToString();
                    break;
                }
            }

            var binnacle = new Binnacle
            {
                BinnacleId = Guid.NewGuid(),
                StoreId = storeId,
                Name = "Borrado",
                Response = "borrado logico de resitro " + name,
                UserId = usuarioId,
                BinnacleTypeId = 6,
                Description = "controlVolumetrico",
                ValueName = "logic Delete",
                IpAddress = Ip,
                MacAddress = addr,
                Date = DateTime.Now,
                Updated = DateTime.Now,
                Active = true,
                Locked = false,
                Deleted = false
            };

            context.Add(binnacle);
            await context.SaveChangesAsync();
        }



        public async Task loginBinnacle(string usuarioId, string Ip, string name)
        {
            var addr = "";
            foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (n.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && n.OperationalStatus == OperationalStatus.Up)
                {
                    addr += n.GetPhysicalAddress().ToString();
                    break;
                }
            }

            var binnacle = new Binnacle
            {
                BinnacleId = Guid.NewGuid(),
                Name = "Login",
                Response = "Login correcto " + usuarioId,
                UserId = usuarioId,
                BinnacleTypeId = 6,
                Description = "controlVolumetrico",
                ValueName = "Login",
                IpAddress = Ip,
                MacAddress = addr,
                Date = DateTime.Now,
                Updated = DateTime.Now,
                Active = true,
                Locked = false,
                Deleted = false
            };

            context.Add(binnacle);
            await context.SaveChangesAsync();
        }

        public async Task logoutBinnacle(string usuarioId, string Ip, string name, Guid? storeId)
        {
            var addr = "";
            foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (n.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && n.OperationalStatus == OperationalStatus.Up)
                {
                    addr += n.GetPhysicalAddress().ToString();
                    break;
                }
            }

            var binnacle = new Binnacle
            {
                BinnacleId = Guid.NewGuid(),
                StoreId = storeId,
                Name = "Logout",
                Response = "Logout " + usuarioId,
                UserId = usuarioId,
                BinnacleTypeId = 6,
                Description = "controlVolumetrico",
                ValueName = "Logout",
                IpAddress = Ip,
                MacAddress = addr,
                Date = DateTime.Now,
                Updated = DateTime.Now,
                Active = true,
                Locked = false,
                Deleted = false
            };
            context.Add(binnacle);
            await context.SaveChangesAsync();
        }

        public async Task logoutBinnacle2(string usuarioId, string Ip, string name, Guid? storeId)
        {
            var addr = "";
            foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (n.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && n.OperationalStatus == OperationalStatus.Up)
                {
                    addr += n.GetPhysicalAddress().ToString();
                    break;
                }
            }

            var binnacle = new Binnacle
            {
                BinnacleId = Guid.NewGuid(),
                StoreId = storeId,
                Name = "Logout por inactividad",
                Response = "Logout por inactividad " + usuarioId,
                UserId = usuarioId,
                BinnacleTypeId = 6,
                Description = "controlVolumetrico",
                ValueName = "Logout inactividad",
                IpAddress = Ip,
                MacAddress = addr,
                Date = DateTime.Now,
                Updated = DateTime.Now,
                Active = true,
                Locked = false,
                Deleted = false
            };
            context.Add(binnacle);
            await context.SaveChangesAsync();
        }

        public async Task changePasswordBinnacle(string usuarioId, string Ip, string name, Guid? storeId)
        {
            var addr = "";
            foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (n.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && n.OperationalStatus == OperationalStatus.Up)
                {
                    addr += n.GetPhysicalAddress().ToString();
                    break;
                }
            }

            var binnacle = new Binnacle
            {
                BinnacleId = Guid.NewGuid(),
                StoreId = storeId,
                Name = "Cambio de password",
                Response = "Cambio de password " + name,
                UserId = usuarioId,
                BinnacleTypeId = 6,
                Description = "controlVolumetrico",
                ValueName = "Cambio password",
                IpAddress = Ip,
                MacAddress = addr,
                Date = DateTime.Now,
                Updated = DateTime.Now,
                Active = true,
                Locked = false,
                Deleted = false
            };

            context.Add(binnacle);
            await context.SaveChangesAsync();
        }

        public async Task AsignarRol(string usuarioId, string Ip, string name, Guid? storeId)
        {
            var addr = "";
            foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (n.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && n.OperationalStatus == OperationalStatus.Up)
                {
                    addr += n.GetPhysicalAddress().ToString();
                    break;
                }
            }

            var binnacle = new Binnacle
            {
                BinnacleId = Guid.NewGuid(),
                StoreId = storeId,
                Name = "Se asigno rol",
                Response = "Se asigno rol " + name,
                UserId = usuarioId,
                BinnacleTypeId = 6,
                Description = "controlVolumetrico",
                ValueName = "Asinacion de rol",
                IpAddress = Ip,
                MacAddress = addr,
                Date = DateTime.Now,
                Updated = DateTime.Now,
                Active = true,
                Locked = false,
                Deleted = false
            };

            context.Add(binnacle);
            await context.SaveChangesAsync();
        }

        public async Task errorLoginBinnacle(string usuarioId, string Ip, string name)
        {
            var addr = "";
            foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (n.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && n.OperationalStatus == OperationalStatus.Up)
                {
                    addr += n.GetPhysicalAddress().ToString();
                    break;
                }
            }

            var binnacle = new Binnacle
            {
                BinnacleId = Guid.NewGuid(),
                Name = "Login incorrecto",
                Response = "Login incorrecto " + usuarioId,
                UserId = usuarioId,
                BinnacleTypeId = 6,
                Description = "controlVolumetrico",
                ValueName = "Add",
                IpAddress = Ip,
                MacAddress = addr,
                Date = DateTime.Now,
                Updated = DateTime.Now,
                Active = true,
                Locked = false,
                Deleted = false
            };

            context.Add(binnacle);
            await context.SaveChangesAsync();
        }


    }
}
