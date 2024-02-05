using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Controllers
{
    public class CategoriasController : Controller
    {
        public readonly ICategoriasRepository _categoriasRepository;
        private readonly IServicioUsuarios _servicioUsuarios;

        public CategoriasController(ICategoriasRepository categoriasRepository, IServicioUsuarios servicioUsuarios)
        {
            _categoriasRepository = categoriasRepository;
            _servicioUsuarios = servicioUsuarios;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();

            var categorias = await _categoriasRepository.Obtener(usuarioId);

            return View(categorias);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();

            categoria.UsuarioId= usuarioId;

            await _categoriasRepository.Crear(categoria);

            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var categoria = await _categoriasRepository.ObtenerPorId(id, usuarioId);

            if(categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Categoria categoriaEditar)
        {
            if (!ModelState.IsValid)
            {
                return View(categoriaEditar);
            }

            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var categoria = await _categoriasRepository.ObtenerPorId(categoriaEditar.Id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            categoriaEditar.UsuarioId = usuarioId;

            await _categoriasRepository.Actualizar(categoriaEditar);

            return RedirectToAction("Index");
        }

       
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var categoria = await _categoriasRepository.ObtenerPorId(id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCategoria(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var categoria = await _categoriasRepository.ObtenerPorId(id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await _categoriasRepository.Borrar(id);

            return RedirectToAction("Index");
        }
    }
}
