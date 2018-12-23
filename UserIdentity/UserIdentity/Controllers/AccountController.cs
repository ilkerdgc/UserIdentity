using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserIdentity.Identity;
using UserIdentity.Models;

namespace UserIdentity.Controllers
{
    // Burada kullanıcı kayıt kullanıcı giriş ya da kullanıcı oturumu kapat sistemden çıkış işlemlerimizi yapıyoruz.

    [Authorize]
    public class AccountController : Controller
    {
        // kullanıcı kayıt kullanıcı giriş ya da kullanıcı oturumu kapat gibi işlemleri yapabilmek için önce UserManager
        // oluşturmamız gerekiyor...

        private UserManager<ApplicationUser> userManager;

        public AccountController()
        {
            var userStore = new UserStore<ApplicationUser>(new IdentityDataContext());
            userManager = new UserManager<ApplicationUser>(userStore);



            userManager.PasswordValidator = new CustomPasswordValidator()// şifre kriterleri için CustomPasswordValidator adında model oluşturuldu ve PasswordValidator classı genişletilerek extra istediğimiz özellikleri tanımlayabildik
            {
                RequireDigit = true, // kullanıcı porola girerken mutlaka sayısal ifade girmesi gerekir
                RequiredLength = 7, // kullanıcı porola girerken mutlaka 7 karakter olması gerekir
                RequireLowercase = true, // kullanıcı porola girerken mutlaka bir küçük harf olması gerekir
                RequireUppercase = true, // kullanıcı porola girerken mutlaka bir büyük harf girmesi gerekir
                RequireNonLetterOrDigit = true // kullanıcı porola girerken mutlaka bir alfa numerik değer olması gerekir
            };

            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                RequireUniqueEmail = true, // her kullanıcının bir tane mail adresi olması için kullanılır
                AllowOnlyAlphanumericUserNames = false // alphanumeric karakter içerisin mi içermesin mi
            };
        }


        
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous] // action metodu dışarıya açmak için kullanılır.yani dışarıdan erişilebilinir olur
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous] 
        public ActionResult Register(Register registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                user.UserName = registerModel.UserName;
                user.Email = registerModel.Email;

                var result = userManager.Create(user, registerModel.Password); // UserManager ile kayıt oluşturuyoruz ve bize geriye IdetityResult döndürecek ve bunu kullanıcaz.

                if (result.Succeeded) // eğer kayıt başarılı oluşturulmuş ise kullanıcıyı login sayfasına yönlendiriyoruz.
                {
                    userManager.AddToRole(user.Id, "User"); // kullanıcıyı login e göndermeden önce kullanıcıyı bir role atıyabiliriz
                    return RedirectToAction("Login");
                }
                else // eğer kayıt başarısız ise IdetityResult bize error listesi gönderir ve biz bu listeyi dolaşıp yazdırabiliriz.
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item); // burada ilk parametre bizden key istiyor yani register model içerisindeki özelliklerden birinin hatası mı yoksa genel bir hata mı diye belitmemiz için. biz register modeline ait genel bir hata olarak algılatmak için boş bırakıyoruz.
                    }
                }
            }

            return View(registerModel); // eğer model isvalid false ise kullanıcıyı o ana kadar girdiği veriler ile beraber tekrar register forma yönlendiriyoruz.
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl) // eğer login in HttpGet ini çağırdığımızda eğer  querysitring içerisinde bir ReturnUrl varsa bunu alıp login view ine gönderiyoruz. daha sonra loginde sayfa post olduktan sonra da buradaki returnUrl değerini loginin httppost action metoduna aktarmış oluyoruz. Burada asıl yapılmak istenen returnUrl değerini hettpget metodundan httppost metoduna taşıyabilmek.
        {

            if (HttpContext.User.Identity.IsAuthenticated) // true ise kullanıcı zaten login olmuştur. login olduğu halde login sayfasına yönlendiriliyorsa demek ki kullanıcı yetkisi olmayan bir alana gidiyor demektir. dolayısıyla login sayfası değil hata sayfası görünmelidir
            {
                return View("Error", new string[] { "Erişim hakkınız yok" });
            }

            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel loginModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.Find(loginModel.UserName, loginModel.Password); // kullanıcının login olabilmesi için modelden gelen model ile karşılaştırma yapıyoruz
                if (user == null)
                {
                    ModelState.AddModelError("", "Yanlış kullanıcı adı veya parola");
                }
                else
                { // burada önce karşılaştırma sonucu eğer kullanıcı varsa kullanıcının tarayıcısına kullanıcının tanımlamasıyla alakalı bir cookie çerez bırakmamaız gerekiyor
                    var authManager = HttpContext.GetOwinContext().Authentication; // buradaki authManager benim uygulamadaki login veya logaut işlemlerimi yerine getirecek nesnedir ve oluşturacağımız cookie yide kullanıcının tarayıcısına authManager ile gönderiyoruz

                    var identity = userManager.CreateIdentity(user, "ApplicationCookie"); //Burada bir cookie oluşturuyoruz
                    var autProperties = new AuthenticationProperties() // oluşturduğumuz cookie yi kullanıcının tarayıcısına gönderirken yanında birkaç özellik de gönderebiliyoruz. örneğin kullanıcının cookie si kalıcı olarak oluşturulsun yani bir sonraki geldiğinde kullanıcı tanınsın mı, beni hatırlasın checkbox indan seçilen true ya da false değer "rememberme" gibi özellikler
                    {
                        IsPersistent = true // varsayılan olarak true ya eşitliyoruz
                    };

                    authManager.SignOut(); // kullanıcı daha öceden giriş yapmışsa daha önceden cookie varsa olmadığından emin olmak için SignOut diyoruz
                    authManager.SignIn(autProperties, identity); // daha sonra kullanıcıyı sisteme dahil ediyoruz

                    return Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl); // sisteme giriş yapıldıktan sonra yönlendirme yapıyoruz ve returnUrl boş veya boşluk karakteri var ise kullanıcıyı ana dizine yani home/index e yönlendiriyoruz diğer durumda ise returnUrl içinde ne varsa o dizine yönlendirilecek.
                }
            }

            ViewBag.returnUrl = returnUrl; // modelstate false ise tekrar sayfayı gönderiyoruz ve bunun yanında da tekrar returnUrl bilgisini gönderiyoruz

            return View(loginModel);
        }

        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;

            authManager.SignOut();

            return RedirectToAction("Login");
        }

    }
}