using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace LoginComplete.Models
{
    public class DemoContext:DbContext
    {
        public DemoContext():base("t")
        {

        }

        public DbSet<SiteUser> tblSiteUser { get; set; }
    }
}