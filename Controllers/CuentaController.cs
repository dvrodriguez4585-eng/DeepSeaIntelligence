using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DeepSeaIntelligence.Controllers
{
    public class EntrarVM
    {
        [Required(ErrorMessage = "Escribe tu correo."), EmailAddress(ErrorMessage = "El correo no es válido.")]
        public string Email { get; set; } = "";
        [Required(ErrorMessage = "Escribe tu contraseña."), DataType(DataType.Password)]
        public string Password { get; set; } = "";
        public bool Recordarme { get; set; }
        public string? ReturnUrl { get; set; }
    }

    public class RegistroVM
    {
        [Required(ErrorMessage = "Escribe tu correo."), EmailAddress(ErrorMessage = "El correo no es válido.")]
        public string Email { get; set; } = "";
        [Required(ErrorMessage = "Escribe una contraseña."), DataType(DataType.Password)]
        public string Password { get; set; } = "";
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden."), DataType(DataType.Password)]
        public string Confirmar { get; set; } = "";
    }

    [AllowAnonymous]
    public class CuentaController : Controller
    {
        private readonly SignInManager<IdentityUser> _sm;
        private readonly UserManager<IdentityUser> _um;
        public CuentaController(SignInManager<IdentityUser> sm, UserManager<IdentityUser> um) { _sm = sm; _um = um; }

        [HttpGet]
        public IActionResult Entrar(string? returnUrl) => View(new EntrarVM { ReturnUrl = returnUrl });

        [HttpPost]
        public async Task<IActionResult> Entrar(EntrarVM m)
        {
            if (!ModelState.IsValid) return View(m);
            var r = await _sm.PasswordSignInAsync(m.Email, m.Password, m.Recordarme, false);
            if (r.Succeeded)
                return !string.IsNullOrEmpty(m.ReturnUrl) && Url.IsLocalUrl(m.ReturnUrl) ? LocalRedirect(m.ReturnUrl) : Redirect("/");
            ModelState.AddModelError("", "Correo o contraseña incorrectos.");
            return View(m);
        }

        [HttpGet]
        public IActionResult Registro() => View(new RegistroVM());

        [HttpPost]
        public async Task<IActionResult> Registro(RegistroVM m)
        {
            if (!ModelState.IsValid) return View(m);
            var u = new IdentityUser { UserName = m.Email, Email = m.Email };
            var r = await _um.CreateAsync(u, m.Password);
            if (r.Succeeded)
            {
                await _sm.SignInAsync(u, false);
                return Redirect("/");
            }
            foreach (var e in r.Errors) ModelState.AddModelError("", e.Description);
            return View(m);
        }

        [HttpPost]
        public async Task<IActionResult> Salir()
        {
            await _sm.SignOutAsync();
            return RedirectToAction(nameof(Entrar));
        }
    }
}