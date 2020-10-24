using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectFish.Models;
using Newtonsoft.Json;

namespace ProjectFish.Controllers
{
    public class AccountController : Controller
    {
        private readonly ProjectFishContext _context;

        public AccountController(ProjectFishContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            string session = HttpContext.Session.GetString("user");
            if (session != null)
            {
                int accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session));
                ViewBag.error = "Logged in " + accountId;

                return View();
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public IActionResult Login(Account account)
        {

            AccountMethods methods = new AccountMethods();
            string errormsg = "";

            int accountId = methods.getAccount(account, out errormsg);

            if (accountId < 1)
            {
                ViewBag.error = errormsg;
                return View();
            }
            else
            {
                var acc = _context.Account.FindAsync(accountId);
                var name = acc.Result.Mail;

                string session = JsonConvert.SerializeObject(accountId);
                HttpContext.Session.SetString("user", session);
                HttpContext.Session.SetString("username", name);
                //return View();
                return RedirectToAction("Index", "Compositions");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Account account)
        {
            AccountMethods methods = new AccountMethods();
            string errormsg = "";

            int accountId = methods.CreateAccount(account, out errormsg);
            
            if (accountId < 1)
            {
                ViewBag.error = errormsg;
                return View();
            }
            else
            {
                accountId = methods.getAccount(account, out errormsg);

                var acc = _context.Account.FindAsync(accountId);
                var name = acc.Result.Mail;


                string session = JsonConvert.SerializeObject(accountId);
                HttpContext.Session.SetString("user", session);
                HttpContext.Session.SetString("username", name);

                return RedirectToAction("Index", "Compositions");
            }

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");
            HttpContext.Session.Remove("username");
            return RedirectToAction("Index", "Home");
        }

    }
}
