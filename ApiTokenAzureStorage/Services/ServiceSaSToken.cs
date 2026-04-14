using Azure.Data.Tables;
using Azure.Data.Tables.Sas;

namespace ApiTokenAzureStorage.Services
{
    public class ServiceSaSToken
    {
        private TableClient tableAlumnos;
        public ServiceSaSToken(IConfiguration configuration)
        {
            string azureKeys = configuration.GetValue<string>("AzureKeys:StorageAccount");
            TableServiceClient tableService = new TableServiceClient(azureKeys);
            this.tableAlumnos = tableService.GetTableClient("alumnos");
        }
        public string GenerateToken(string curso)
        {
            // NECESITAMOS LOS PERMISOS DE ACCESO
            TableSasPermissions permisos = TableSasPermissions.Read;
            // EL ACCESO A TOKEN ESTA DELIMITADO POR UN TIMEPO DETERMINADO
            TableSasBuilder builder = this.tableAlumnos
                .GetSasBuilder(permisos, DateTimeOffset.UtcNow.AddMinutes(15));
            // EL ACCESO A LOS DATOS ES MEDIANTE ROWKEY Y PARTITIONKEY
            // SON STRING Y BAN DE FORMA ALFABETICA
            builder.PartitionKeyStart = curso;
            builder.PartitionKeyEnd = curso;
            //YA TENDREMOS EL TOKEN QUE ES UN ACCESO MEDIANTE URI
            Uri uriToken = this.tableAlumnos.GenerateSasUri(builder);
            string token = uriToken.AbsoluteUri;
            return token;
        }
    }
}
