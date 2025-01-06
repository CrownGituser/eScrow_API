using Microsoft.Extensions.Options;

namespace SMFG_API_New
{
    public class ExceptionHandling : IExceptionHandling
    {
        private readonly IConfiguration _config;
        private string ConnectionString;
        private string GetFileName;
        private string _ContentRootPath;

        public ExceptionHandling(IConfiguration configuration, IOptions<FluentApiSettings> options, IWebHostEnvironment env)
        {
            _config = configuration;
            _ContentRootPath = env.ContentRootPath;
            ConnectionString = _config["ConnectionStrings:DefaultConnection"];
            Console.WriteLine(ConnectionString);
            GetFileName = options.Value.LogFilePath;
        }

        public void LogError(Exception ex)
        {
            var G1 = _ContentRootPath;
            string FilePath = Path.Combine(G1, @"Logs");
            if (!Directory.Exists(FilePath))
                Directory.CreateDirectory(FilePath);
            //  var TempPath = conManager.GetConnectionString("CtrPath:LogFilePath");//"LogFilePath"];  //_configuration.GetValue<string>("LogFilePath");
            if (FilePath != null)
            {
                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += string.Format("Message: {0}", ex.Message);
                message += Environment.NewLine;
                message += string.Format("StackTrace: {0}", ex.StackTrace);
                message += Environment.NewLine;
                message += string.Format("Source: {0}", ex.Source);
                message += Environment.NewLine;
                message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                string path = Path.Combine(FilePath, "Filegene.txt");
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(message);
                    writer.Close();
                }
            }
        }


    }
    public interface IExceptionHandling
    {
        public void LogError(Exception ex);
    }
    public class FluentApiSettings
    {
        public string LogFilePath { get; set; }
    } 
}
