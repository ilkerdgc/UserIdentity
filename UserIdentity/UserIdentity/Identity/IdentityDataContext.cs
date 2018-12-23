using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserIdentity.Identity
{
    public class IdentityDataContext: IdentityDbContext<ApplicationUser> // burada aynı code firs ile database oluştururken ki DbContext sınıfını kullanmamız gibi IdentityDbContext sınıfını kullanıyoruz ve bir de generic olarak kullanacağımız ApplicationUser sınıfını tanımlamamız gerekiyor.
    {
        public IdentityDataContext():base("identityConnection") // burada webconfig de bir connectionstring belirtmek için consractor ın base ine yani IdentityDbContext in içerisine bir connectionstring gönderiyoruz
        {

        }
    }
}