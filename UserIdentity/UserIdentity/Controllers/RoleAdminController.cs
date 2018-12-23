using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserIdentity.Identity;
using UserIdentity.Models;

namespace UserIdentity.Controllers
{
    [Authorize(Roles = "Admin")] // admin rolüne sahip kullanıcı burayı görebilir onun haricinde herhangi bir kullanıcı burayı göremez
    public class RoleAdminController : Controller
    {
        // role listeleme, role ekleme gibi işlemleri yapabilmek için önce RoleManager oluşturmamız gerekiyor...

        private RoleManager<IdentityRole> roleManager;
        private UserManager<ApplicationUser> userManager;

        public RoleAdminController()
        {
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDataContext()));
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityDataContext())); // role işlemlerini yapmak için userManager a da ihtiyacımız olduğundan onu da tanımladık
        }

        // GET: RoleAdmin
        public ActionResult Index()
        {
            return View(roleManager.Roles); // role leri listelemek için view e listeyi gönderiyoruz
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string name)
        {
            if (ModelState.IsValid)
            {
                var result = roleManager.Create(new IdentityRole(name)); // RoleManager ile kayıt oluşturuyoruz ve bize geriye IdetityResult döndürecek ve bunu kullanıcaz.

                if (result.Succeeded) // eğer kayıt başarılı oluşturulmuş ise kullanıcıyı Index sayfasına yönlendiriyoruz.
                {
                    return RedirectToAction("Index");
                }
                else // eğer kayıt başarısız ise IdetityResult bize error listesi gönderir ve biz bu listeyi dolaşıp yazdırabiliriz.
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item); // burada ilk parametre bizden key istiyor yani name model içerisindeki özelliklerden birinin hatası mı yoksa genel bir hata mı diye belitmemiz için. biz register modeline ait genel bir hata olarak algılatmak için boş bırakıyoruz.
                    }
                }
            }
            return View(name);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var role = roleManager.FindById(id); // rolü database de buluyoruz

            if (role!=null) 
            {
                var result = roleManager.Delete(role); // role ü siliyoruz ve bize geriye IdetityResult döndürecek ve bunu kullanıcaz.

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors); // Delete action unun bir httpget metodu olmadığı için view ide yok demektir ve burada bir view oluşturuyoruz
                }
            }
            else
            {
                return View("Error", new string[] { "Role Bulunamadı" }); // eğer role bulunamadı ise oluşturacağımız error view ine yeni bir string dizi oluşturup gönderiyoruz. çünkü error view i bir string dizi modeli kullanıcak
            }
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            // şimdi buradaki görevimiz bize gelen id ile  role blgilerine ulaşıp role bilgilerinin yanında o role ait 
            // olan kullanıcıları members ları ya da o role içerisinde olmayan kullanıcıları nonmembers ları da bir 
            // RoleEditModel içerisinde paketleyip sayfa üzerine göndermek. 

            var role = roleManager.FindById(id); // id ye göre role ü buluyoruz

            var members = new List<ApplicationUser>(); // üye olanları tutacak
            var nonMembers = new List<ApplicationUser>(); // üye olmayanları tutacak

            foreach (var user in userManager.Users.ToList()) // usermanager aracılığı ile kullanıcıları tek tek dolaşıp kullanıcının bulunan role içerisinde olup olmadığını kontrol edecez
            {
                var list = userManager.IsInRole(user.Id, role.Name)? members : nonMembers; // gelen user ın o role içerisinde olup olmadığına göre  geriye sadece bir listenin türünü gönderecez. eğer sonuç true ise geriye members ı değilse nonmembers ı göndercez

                list.Add(user);
            }

            return View(new RoleEditModel() // yukarıda aldığımız ihtiyacımı olan bilgileri bir RoleEditModel oluşturup doldurarak view e gönderiyoruz
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public ActionResult Edit(RoleUpdateModel model) // checkbox lardan gelen model
        {
            IdentityResult result;

            if (ModelState.IsValid)
            {
                // bu foreach ile istediğimiz id leri bu role atamış oluyoruz
                foreach (var userId in model.IdsToAdd ?? new string[] { }) // model içerisinden gelen IdsToAdd null ise bu foreach in hata vermemesi için dizi olarak algılanması gerekiyor ve null ise boş bir string dizi olarak oluşmasını sağlıyoruz
                {
                    result = userManager.AddToRole(userId, model.RoleName); 
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }

                // bu foreach ile istediğimiz id leri bu role den silmiş oluyoruz
                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    result = userManager.RemoveFromRole(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                return RedirectToAction("Index");
            }

            return View("Error", new string[] { "aranılan role yok" });
        }
    }
}