using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using MvcCoreSaSAzureStorage.Helpers;
using MvcCoreSaSAzureStorage.Models;
using System.Threading.Tasks;

namespace MvcCoreSaSAzureStorage.Controllers
{
    public class MigracionController : Controller
    {
        private HelperXML helper;
        private IConfiguration configuration;
        public MigracionController(HelperXML helper, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.helper = helper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string accion)
        {
            //EN ESTE METODO LO QUE NECESITAMOS SON LAS KEYS DE AZURE
            string azureKeys = this.configuration.GetValue<string>("AzureKeys:StorageAccount");
            TableServiceClient tableService = new TableServiceClient(azureKeys);
            TableClient tableclient = tableService.GetTableClient("alumnos");
            await tableclient.CreateIfNotExistsAsync();
            List<Alumno> alumnos = this.helper.GetAlumnos();
            foreach (Alumno alumno in alumnos)
            {
                await tableclient.AddEntityAsync<Alumno>(alumno);
            }
            ViewData["MENSAJE"] = "Migracion realizada correctamente";
            return View();
        }
    }
}
