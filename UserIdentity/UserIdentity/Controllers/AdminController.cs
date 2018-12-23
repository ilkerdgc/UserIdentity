﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserIdentity.Identity;

namespace UserIdentity.Controllers
{
    // Burada kullanıcı listeleme, kullanıcı detaylarını görme gibi işlemleri yapıyoruz.

    public class AdminController : Controller
    {
        // kullanıcı listeleme, kullanıcı detaylarını görme gibi işlemleri yapabilmek için önce UserManager
        // oluşturmamız gerekiyor...

        private UserManager<ApplicationUser> userManager;

        public AdminController()
        {
            var userStore = new UserStore<ApplicationUser>(new IdentityDataContext());
            userManager = new UserManager<ApplicationUser>(userStore);
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View(userManager.Users); // veritabanına kayıt olmuş var olan tüm kullanıcıları view e göndererek sayfayda gösteriyoruz
        }
    }
}