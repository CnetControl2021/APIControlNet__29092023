using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Menu
    {
        public int MenuId { get; set; }
        public int? ParentMenuId { get; set; }
        public string PageName { get; set; }
        public string MenuName { get; set; }
        public string IconName { get; set; }
    }
}
