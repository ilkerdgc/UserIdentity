using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserIdentity.Identity
{
    public class ApplicationUser: IdentityUser // IdentityUser sınıfı uygulamamızda üyelik oluşturan kullanıcıların bilgilerini taşıyan sınıftır. ApplicationUser sınıfını oluşturup IdentityUser sınıfından miras almamızın sebebi IdentityUser temel sınıftır ve biz bu temel sınıfı genişletmek için oluşturduk.
    {
        public string Name { get; set; }
    }
}