using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UserIdentity.Identity;

namespace UserIdentity.Models
{
    // Burada sayfadan Action metoduna taşıyacağımız bilgileri getirmesi için giriş ve kayıt modelleri oluşturuyoruz

    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class Register
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }


    // role içerisindeki kullanıcıları seçme imkanı vermek için yani seçeceğimiz bir kullanıcyı o rolün içerisine atmak ya da çıkarmak için iki modele ihtiyacımız var.

    // buradaki amaç edit butonuna tıkladığımız zaman admin rolüne sahip olan kullanıcıları members olarak sayfa üzerine 
    // getircez. admin rolüne sahip olmayan kullanıcıları da nonmembers listesi ile sayfaya getircez. bunu  edit action ının  
    // get action metodundan döndürürken doldurup sayfaya göndericez. daha sonra members ve nonmembers listelerini 
    // foreach ile açacaz ve açtığımız kullanıcıların id lerini ise checkbox lara verecez ve seçtiğimiz checkbox lara 
    // göre tekrar edit action ın post una biz seçili id leri ya da  silinmesini istediğimiz kullanıcı id lerini de 
    // seçtirecez ve bunları da o rol içerisinden çıkartcak yani role atanmak istenen kullanıcılar ve o role içerisinden 
    // çıkartılmak istenen kullanıcıları burada belirtmemiz gerekiyor bunun için de RoleUpdateModel i kullanıcaz.
    public class RoleEditModel
    {
        public IdentityRole Role { get; set; } // role bilgilerini getircek

        public IEnumerable<ApplicationUser> Members { get; set; } // o role içerisindeki üyeleri yani kullanıcıları getircek

        public IEnumerable<ApplicationUser> NonMembers { get; set; } // o role içerisinde olmayan üyeleri yani kullanıcıları getircek
    }

    // bunlar da post a gelecek bilgiler
    public class RoleUpdateModel 
    {
        [Required]
        public string RoleName { get; set; }

        public string RoleId { get; set; }

        public string[] IdsToAdd { get; set; } // burada role içerisine eklenmesini istediğimiz checkbox ların atandığı userid lerini barındıracak olan yani buradan form üzerinden post action a gelecek olan kullanıcıların id bilgilerini o role içerisine atacaz

        public string[] IdsToDelete { get; set; } // burada role içerisinden silinmesini istediğimiz checkbox ların atandığı userid lerini barındıracak olan yani buradan form üzerinden post action a gelecek olan kullanıcıların id bilgilerini o role içerisinden silecez
    }

    // dolayısıyla edit in get inden RoleEditModel bilgileri gelecek bana ve bütün members ları ben foreach ile sayfa 
    // üzerinde açacam ve members lardan  her bir checkbox a karşılık gelen userid post a geldiğinde ise IdToDelete 
    // olarak gelecek, nonmembers lardan  gelen kullanıcıların id leri ise chackbox lara atancak ve IdsToAdd olacak 
    // yani üye değilse üye olmak için checkbox bilgisi posta gelebilcek ya da tam tersi.
}