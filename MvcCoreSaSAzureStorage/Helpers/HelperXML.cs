using MvcCoreSaSAzureStorage.Models;
using System.Xml.Linq;

namespace MvcCoreSaSAzureStorage.Helpers
{
    public class HelperXML
    {
        private XDocument document;

        public HelperXML()
        {
            string pathResourceXML = "MvcCoreSaSAzureStorage.Documents.alumnos_tables.xml";
            Stream? stream = this.GetType().Assembly.GetManifestResourceStream(pathResourceXML);

            if (stream == null)
            {
                throw new InvalidOperationException($"Embedded resource not found: {pathResourceXML}");
            }

            this.document = XDocument.Load(stream);
        }

        public List<Alumno> GetAlumnos()
        {
            var consulta = from datos in this.document.Descendants("alumno")
                           select new Alumno
                           {
                               IdAlumno = int.Parse(datos.Element("idalumno").Value),
                               Curso = datos.Element("curso").Value,
                               Nombre = datos.Element("nombre").Value,
                               Apellidos = datos.Element("apellidos").Value,
                               Nota = int.Parse(datos.Element("nota").Value)
                           };
            return consulta.ToList();
        }
    }
}
