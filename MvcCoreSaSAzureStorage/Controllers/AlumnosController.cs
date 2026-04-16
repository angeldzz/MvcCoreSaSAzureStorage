using Microsoft.AspNetCore.Mvc;
using MvcCoreSaSAzureStorage.Models;
using MvcCoreSaSAzureStorage.Services;

namespace MvcCoreSaSAzureStorage.Controllers
{
    public class AlumnosController : Controller
    {
        private ServiceAzureAlumnos service;
        public AlumnosController(ServiceAzureAlumnos service)
        {
            this.service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string curso)
        {
            List<Alumno> alumnos = await this.service.GetAlumnosAsync(curso);
            return View(alumnos);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create
            (Alumno alumno)
        {
            ViewData["Mensaje"] = "Alumno creado correctamente";
            await this.service.CreateAlumnoAsync(alumno.IdAlumno, alumno.Nombre, alumno.Apellidos, alumno.Nota);
            return RedirectToAction("Index");
        }
    }
}
