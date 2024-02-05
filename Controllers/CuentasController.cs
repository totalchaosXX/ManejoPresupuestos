using AutoMapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace ManejoPresupuesto.Controllers
{
    public class CuentasController : Controller
    {
        private readonly ITiposCuentasRepository _tiposCuentasRepository;
        private readonly IServicioUsuarios _servicioUsuarios;
        private readonly ICuentasRepository _cuentasRepository;
        private readonly IMapper _mapper;

        public CuentasController(ITiposCuentasRepository tiposCuentasRepository, IServicioUsuarios servicioUsuarios, ICuentasRepository cuentasRepository,
            IMapper mapper) 
        {
            _tiposCuentasRepository = tiposCuentasRepository;
            _servicioUsuarios = servicioUsuarios;
            _cuentasRepository = cuentasRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var cuentasConTipoCuenta = await _cuentasRepository.Buscar(usuarioId);

            var model = cuentasConTipoCuenta.GroupBy(x=> x.TipoCuenta).Select(grupo => new IndiceCuentasViewModel
            {
                TipoCuenta = grupo.Key,
                Cuentas = grupo.AsEnumerable()
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();

            var modelo = new CuentaCreacionViewModel();

            modelo.TiposCuentas = await ObtenerTiposCuentas(usuarioId);

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreacionViewModel cuenta)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();

            var tipoCuenta = await _tiposCuentasRepository.ObtenerPorId(cuenta.TipoCuentaId, usuarioId);

            if(tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if(!ModelState.IsValid)
            {
                cuenta.TiposCuentas = await ObtenerTiposCuentas(usuarioId);

                return View(cuenta);

            }

            await _cuentasRepository.Crear(cuenta);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var cuenta = await _cuentasRepository.ObtenerPorId(id, usuarioId);
            
            if(cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var model = _mapper.Map<CuentaCreacionViewModel>(cuenta);

            model.TiposCuentas = await ObtenerTiposCuentas(usuarioId);

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Editar(CuentaCreacionViewModel cuentaEditar)
        {
            if (!ModelState.IsValid)
            {
                return View(cuentaEditar);
            }

            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();

            var cuenta = await _cuentasRepository.ObtenerPorId(cuentaEditar.Id, usuarioId);

            if(cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var tipoCuenta = await _tiposCuentasRepository.ObtenerPorId(cuentaEditar.TipoCuentaId, usuarioId);

            if(tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await _cuentasRepository.Actualizar(cuentaEditar);

            return RedirectToAction("Index");
        }


        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas(int usuarioId)
        {
            var tiposCuentas = await _tiposCuentasRepository.Obtener(usuarioId);

            return tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));

        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();

            var cuenta = await _cuentasRepository.ObtenerPorId(id, usuarioId);

            if(cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(cuenta);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCuenta(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();

            var cuenta = await _cuentasRepository.ObtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await _cuentasRepository.Borrar(id);

            return RedirectToAction("Index");
        }
    }
}
