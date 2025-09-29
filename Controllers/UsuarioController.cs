using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Jwt;
using proyecto_inmobiliaria.Services;

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
        public IActionResult Index(int paginaNro = 1, int tamPagina = 10)
        {
            var usuarios = _usuarioService.TodosLosUsuariosPaginados(paginaNro, tamPagina);

            int totalUsuarios = _usuarioService.CantidadUsuario();
            int totalPaginas = (int)Math.Ceiling(totalUsuarios / (double)tamPagina);

            ViewData["PaginaActual"] = paginaNro;
            ViewData["TotalPaginas"] = totalPaginas;

            return View(usuarios);
        }

        [HttpGet]
        public IActionResult Login()
        {
            var dto = new UsuarioRequestDTO(0, "", "", "", Enum.ERol.Empleado);
            return View("Login", dto);
        }

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
                    usuario.Rol.ToString()
                );

                Response.Cookies.Append("AuthToken", token, new CookieOptions
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
        public IActionResult Registro()
        {
            var dto = new UsuarioRequestDTO(0, "", "", "", Enum.ERol.Empleado);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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


    }
}