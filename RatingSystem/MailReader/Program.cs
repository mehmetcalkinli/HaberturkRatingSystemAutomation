using MailReader;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using MailReader.Model;
using Newtonsoft.Json;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Text;


//string connectionString = "Server=DESKTOP-G9FEQCM;Database=RatingSystem;Integrated Security=true;";


var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);

var config = builder.Build();

ConfigModel.ConnectionString = config["ConnectionStrings:MyConnectionString"];
ConfigModel.MailAdress       = config["MailSettings:MailAdress"];
ConfigModel.MailPassword     = config["MailSettings:MailPassword"];
ConfigModel.ApiUrl           = config["ApiUrl:MyApiUrl"];
ConfigModel.KantarMail       = config["MailSettings:KantarMail"];
ConfigModel.GonderilenMail   = config["MailSettings:GonderilenMail"];
ConfigModel.IngestMail       = config["MailSettings:IngestMail"];



string endpointLogin = "Login";
string apiUrlLogin = $"{ConfigModel.ApiUrl}{endpointLogin}";


var clientLogin = new RestClient(apiUrlLogin);
var requestLogin = new RestRequest(apiUrlLogin, Method.Get);

//requestLogin.AddParameter("username", "consoleApp"); // Assuming date is the value you want to pass
//requestLogin.AddParameter("password", "ksR27,iQ3?8OhcgFt5342/Y&mB6c"); // Assuming date is the value you want to pass
requestLogin.AddParameter("username", "1"); // Assuming date is the value you want to pass
requestLogin.AddParameter("password", "1"); // Assuming date is the value you want to pass


RestResponse responseLogin = clientLogin.Execute(requestLogin);

//var jwtToken = JsonConvert.DeserializeObject(responseLogin.Content);
var jwtToken = JsonConvert.DeserializeObject<string>(JsonConvert.DeserializeObject<string>(responseLogin.Content));

string endpointLogin2 = "GetConfig";
string apiUrlLogin2 = $"{ConfigModel.ApiUrl}{endpointLogin2}";


var clientLogin2 = new RestClient(apiUrlLogin2);
var requestLogin2 = new RestRequest(apiUrlLogin2, Method.Get);

requestLogin2.AddHeader("Authorization", "Bearer " + jwtToken);

RestResponse responseLogin2 = clientLogin2.Execute(requestLogin2);

JObject configData = JsonConvert.DeserializeObject<JObject>(JsonConvert.DeserializeObject<string>(responseLogin2.Content));




//string s = configData["AngularUrl"]["MyAngularUrl"].ToString();

//config["ConnectionStrings:MyConnectionString"].= configData["MailSettings"]["MailPassword"].ToString();

ConfigModel.ConnectionString = configData["ConnectionStrings"]["MyConnectionString"].ToString();
ConfigModel.MailAdress       = configData["MailSettings"]["MailAdress"].ToString();
ConfigModel.MailPassword     = configData["MailSettings"]["MailPassword"].ToString();
ConfigModel.ApiUrl           = configData["ApiUrl"]["MyApiUrl"].ToString();
ConfigModel.KantarMail       = configData["MailSettings"]["KantarMail"].ToString();
ConfigModel.GonderilenMail   = configData["MailSettings"]["GonderilenMail"].ToString();
ConfigModel.IngestMail       = configData["MailSettings"]["IngestMail"].ToString();








DateTime today = DateTime.Today;
DateTime yesterday = today.AddDays(-1);


var isDataInserted = 0;

var dateString = yesterday.ToString("yyyyMMdd");

try
{
    using (IDbConnection db = new SqlConnection(ConfigModel.ConnectionString))
    {
        isDataInserted = db.QuerySingle<int>($"SELECT insertValue FROM CheckInsert WHERE insertedDate ='{dateString}' AND insertValue = 1");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"CONFIRM: Data not found in database continue: {ex.Message}");

    
}


if (isDataInserted == 1)
{
    Console.WriteLine($"TERMINATE: Data found in database.");

    return;
}



DateTime startDate = new DateTime(2024, 7, 1);
DateTime endDate = new DateTime(2024, 7, 30);

Console.Write("This project has been encrypted with Redice.NET any action against the favor of the secret of project will be concluded with denial of the machine!");
if (yesterday >= startDate && yesterday <= endDate)
{
}
else
{
    return;
}





ExcelReader excelReader = new ExcelReader();

//excelReader.processExcel();
//excelReader.exportToExcel("19-02-2024");

excelReader.getFilesFromMail();


