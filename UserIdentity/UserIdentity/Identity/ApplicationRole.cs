using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserIdentity.Identity
{
    public class ApplicationRole: IdentityRole // IdentityRole sınıfı uygulamamızda üyelik oluşturan kullanıcıların bilgilerini taşıyan sınıftır. ApplicationRole sınıfını oluşturup IdentityRole sınıfından miras almamızın sebebi IdentityRole temel sınıftır ve biz bu temel sınıfı genişletmek için oluşturduk.
    {
        public string Description { get; set; }
    }
}