using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(UserIdentity.IdentityConfig))]

// Owin, uygulama ve server arasında bir ara katmandır. Biz bu ara katman içerisinde uygulamaya istediğimiz ayarları 
// tanıtabiliyoruz. Örneğin biz uygulama içerisinde kullanıcının tarayıcısına bırakacağımız bir cokie aracılığıyla mı 
// kullanıcıyı tanıyalım ya da biz bu tanıma işlemini örneğin servisler içerisindeki tooken lar ile mi yapalım gibi 
// işlemlerimizi yani genel olarak identity içerisindeki kullanacağımız ayarları özelleştirebileceğimiz bir yapı.
// Uygulamaya biz facebook ile mi login olalım ya da google ile mi login olalım gibi standartları uygulamaya tanıtacağımız bir alan.
// Bunun için uygulamaya bir Owin Startup class eklememiz gerekiyor. Ekledikten sonra webconfig içerisinde uygulamaya tanıtmamız gerekiyor

namespace UserIdentity
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login") // Kullanıcı izni olmayan bir alana geldiği zaman gönderileceği yeri tanımladık.
            });

            // Burada kullanıcının tanımlanma yani authentication işlemi bizim cookie lere göre olacak. yani kullanıcının tarayıcısına bir cookie bırakılacak.
            
        }
    }
}
