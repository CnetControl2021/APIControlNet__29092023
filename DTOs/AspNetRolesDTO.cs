namespace APIControlNet.DTOs
{
    public class AspNetRolesDTO
    {
        public string Id  { get; set; } 
        public string Name { get; set; }

        public string Description { get; set; } //Tabla RolesPermission
        public string RoleId { get; set; }  //Tabla RolesPermission

    }
}
