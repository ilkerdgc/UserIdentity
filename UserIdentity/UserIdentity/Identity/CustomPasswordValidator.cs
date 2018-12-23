using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace UserIdentity.Identity
{
    public class CustomPasswordValidator: PasswordValidator
    {
        // extra kriterleri bu aşamada oluşturuyoruz


        // Burada ValidateAsync i eziyoruz. yani ValidateAsync in içerisindeki sürecin içerisine ekstra bir kriter ekliyoruz.
        // Bunu yapmamış olsak zaten PasswordValidator içerisindeki ValidateAsync zaten PasswordValidator içerisindeki 
        // kriterleri işin içine katıyor. Dolayısıyla biz bu işin içine katılan kriterlerin yanına yeni bir kriter oluşturuyoruz.
        public override async Task<IdentityResult> ValidateAsync(string password) 
        {
            var result = await base.ValidateAsync(password); /// burada normal işlemine devam et. buradan gelen hataları da result un içerisine at ben bu hataların üzerine extra hata varsa ekleyecem diyoruz

            if (password.Contains("12345")) // kendi parola kriterimizi ve hatasını oluşturuyoruz
            {
                var errors = result.Errors.ToList();
                errors.Add("Parola ardışık sayısal ifade içeremez!");
                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}