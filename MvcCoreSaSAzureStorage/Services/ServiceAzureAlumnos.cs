using Azure.Data.Tables;
using MvcCoreSaSAzureStorage.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace MvcCoreSaSAzureStorage.Services
{
    public class ServiceAzureAlumnos
    {
        private TableClient tableAlumnos;
        //NECESITAMOS LA URL DE ACCESO AL TOKEN
        private string UrlApi;
        public ServiceAzureAlumnos(IConfiguration configuration)
        {
            this.UrlApi = configuration.GetValue<string>("ApiUrls:ApiAzureToken");
        }
        public async Task<string> GetTokenAsync(string curso)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "token/" + curso;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add
                    (new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject keys = JObject.Parse(data);
                    string token = keys.GetValue("token").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }
        public async Task<List<Alumno>> GetAlumnosAsync(string curso)
        {
            string token = await this.GetTokenAsync(curso);
            Uri uri = new Uri(token);
            this.tableAlumnos = new TableClient(uri);
            List<Alumno> alumnos = new List<Alumno>();
            string filter = $"PartitionKey eq '{curso}'";
            var query = this.tableAlumnos.QueryAsync<Alumno>(filter: filter);
            await foreach (Alumno alumno in query)
            {
                alumnos.Add(alumno);
            }
            return alumnos;
        }
        public async Task CreateAlumnoAsync
            (int idAlumno, string nombre, 
            string apellidos, int nota)
        {
            string curso = "EN PROCESO";
            string token = await this.GetTokenAsync(curso);
            Alumno alumno = new Alumno();
            alumno.IdAlumno = idAlumno;
            alumno.Nombre = nombre;
            alumno.Apellidos = apellidos;
            alumno.Nota = nota;
            alumno.Curso = curso;
            Uri uriToken = new Uri(token);
            this.tableAlumnos = new TableClient(uriToken);
            await this.tableAlumnos.AddEntityAsync(alumno);
        }
    }
}
