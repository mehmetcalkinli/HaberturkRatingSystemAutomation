using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailReader.Model
{
    public static class ConfigModel
    {
        public static string ConnectionString { get; set; }
        public static string MailAdress { get; set; }
        public static string MailPassword { get; set; }
        public static string ApiUrl { get; set; }
        public static string KantarMail{ get; set; }
        public static string GonderilenMail { get; set; }
        public static string IngestMail { get; set; }

    }
}
