using GestionEmpleado.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GestionEmpleado.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous]
        // GET: LoginController
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Create(Usuario usuarioLogin)
        {
            Usuario usuarioAdministrador, usuarioConsultante;
            List<Claim> claims;

            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Index",usuarioLogin);
                }

                usuarioAdministrador = new Usuario();
                usuarioAdministrador.Nombre = "Administrador";
                usuarioAdministrador.Correo = "administrador@igssgt.org";
                usuarioAdministrador.Roles = new List<Rol>();
                usuarioAdministrador.Roles.Add(new Rol { Nombre = "Administrador" });
                usuarioAdministrador.Roles.Add(new Rol { Nombre = "Consultante" });

                usuarioConsultante = new Usuario();
                usuarioConsultante.Nombre = "Consultante";
                usuarioConsultante.Correo = "consultanter@igssgt.org";
                usuarioConsultante.Roles = new List<Rol>();
                usuarioConsultante.Roles.Add(new Rol { Nombre = "Consultante" });

                if(usuarioLogin.Correo == "administrador@igssgt.org")
                {
                    claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuarioAdministrador.Nombre),
                        new Claim("Correo",usuarioAdministrador.Correo)
                    };

                    foreach(Rol rol in usuarioAdministrador.Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, rol.Nombre));
                    }
                }
                else
                {
                    claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuarioConsultante.Nombre),
                        new Claim("Correo",usuarioConsultante.Correo)
                    };

                    foreach (Rol rol in usuarioConsultante.Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, rol.Nombre));
                    }
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index","Login");
        }
    }

    public class Rol
    {
        public string Nombre { get; set; }
    }
}
