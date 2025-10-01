using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Jwt;
using proyecto_inmobiliaria.Services;
using Microsoft.AspNetCore.Authorization;
using proyecto_inmobiliaria.Constants;
using System.Security.Claims;


namespace proyecto_inmobiliaria.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public UsuarioController(IUsuarioService usuarioService, JwtTokenGenerator jwtTokenGenerator)
        {
            _usuarioService = usuarioService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Administrador)]
        public IActionResult Index(int paginaNro = 1, int tamPagina = 10)
        {
            var usuarios = _usuarioService.TodosLosUsuariosPaginados(paginaNro, tamPagina);

            int totalUsuarios = _usuarioService.CantidadUsuario();
            int totalPaginas = (int)Math.Ceiling(totalUsuarios / (double)tamPagina);

            ViewData["PaginaActual"] = paginaNro;
            ViewData["TotalPaginas"] = totalPaginas;

            return View(usuarios);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            var dto = new UsuarioRequestDTO(0, "", "", "", Enum.ERol.Empleado, null!, "");
            return View("Login", dto);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string nombreUsuario, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(contrasena))
            {
                TempData["ErrorMensaje"] = "Usuario y contraseña son obligatorios.";
                return View();
            }

            try
            {
                var usuario = _usuarioService.Login(nombreUsuario, contrasena);

                var token = _jwtTokenGenerator.GenerateToken(
                    usuario.IdUsuario.ToString(),
                    usuario.Email,
                    usuario.NombreUsuario,
                    usuario.Rol.ToString()
                );

                Response.Cookies.Append("token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(120)
                });

                return RedirectToAction("Index", "Home");
            }
            catch (UnauthorizedAccessException ex)
            {
                TempData["ErrorMensaje"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = Roles.Administrador)]
        public IActionResult Registro()
        {
            var dto = new UsuarioRequestDTO(0, "", "", "", Enum.ERol.Empleado, null!, "");
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Administrador)]
        public IActionResult Registro(UsuarioRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            try
            {
                var nuevoUsuario = _usuarioService.AltaUsuario(dto);

                TempData["ExitoMensaje"] = $"Usuario '{nuevoUsuario.NombreUsuario}' creado correctamente.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMensaje"] = ex.Message;
                return View(dto);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (Request.Cookies["token"] != null)
            {
                Response.Cookies.Delete("token");
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public IActionResult ModificarUsuario(int idUsuario)
        {
            var usuario = _usuarioService.ObtenerPorId(idUsuario);
            return View(usuario);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult CambiarAvatar(int idUsuario, IFormFile AvatarFile)
        {
            if (AvatarFile == null || AvatarFile.Length == 0)
            {
                TempData["ErrorMensaje"] = "Debe seleccionar un archivo válido.";
                return RedirectToAction("Perfil", new { idUsuario });
            }

            try
            {
                var usuario = _usuarioService.CambiarAvatar(idUsuario, AvatarFile);
                TempData["ExitoMensaje"] = "Avatar actualizado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMensaje"] = ex.Message;
            }

            return RedirectToAction("Perfil", new { idUsuario });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult CambiarContrasena(int idUsuario, string NuevaContrasena)
        {
            if (string.IsNullOrWhiteSpace(NuevaContrasena))
            {
                TempData["ErrorMensaje"] = "La nueva contraseña no puede estar vacía.";
                return RedirectToAction("Perfil", new { idUsuario });
            }

            try
            {
                _usuarioService.CambiarContrasena(idUsuario, NuevaContrasena);
                TempData["ExitoMensaje"] = "Contraseña actualizada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMensaje"] = ex.Message;
            }

            return RedirectToAction("Perfil", new { idUsuario });
        }

        [HttpGet]
        [Authorize]
        public IActionResult Perfil()
        {
            var claimSub = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            int idUsuario = int.Parse(claimSub!.Value);

            var usuario = _usuarioService.ObtenerPorId(idUsuario);
            return View("Perfil", usuario);
        }

    }
}