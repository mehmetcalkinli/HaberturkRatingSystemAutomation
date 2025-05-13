using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RatingAPI.Business;
using RatingAPI.Helpers;
using RatingAPI.Interfaces;
using RatingAPI.Model;
using RatingAPI.Services;
using System.Data;
using System.IO.Compression;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;
//using AuthorizeAttribute = RatingAPI.Helpers.AuthorizeAttribute;

namespace RatingAPI.Controllers
{


    [ApiController]
    [Route("[controller]")]
    [Authorize]

    public class RatingSystemController : ControllerBase
    {
        //private string connectionString = "Server=DESKTOP-G9FEQCM;Database=RatingSystem;Integrated Security=true;";

        //List<ExcelDataModel> excelDataModelList = new List<ExcelDataModel>();



        private IUserService _userService;

        public RatingSystemController(IUserService userService)
        {
            _userService = userService;
        }



        //[HttpGet]
        //[Route("GetExcelData")]

        //public (List<ExcelDataModel>, List<ExcelDataModel>, ExcelDataModel) GetExcelData()
        //{
        //    RatingSystemBusiness ratingSystemBusiness = new RatingSystemBusiness();

        //    List<ExcelDataModel> s1  = ratingSystemBusiness.GetExcelData().Item1;
        //    List<ExcelDataModel> s2 = ratingSystemBusiness.GetExcelData().Item2;
        //    ExcelDataModel s3 = ratingSystemBusiness.GetExcelData().Item3;

        //    ratingSystemBusiness.GetExcelData();

        //    return (s1, s2, s3);


        //}

        
        
        
        [AllowAnonymous]
        [HttpGet("Login")]
        public string Login(string username, string password)
        {
            RatingSystemBusiness ratingSystemBusiness = new RatingSystemBusiness();

            var resultToken = _userService.Authenticate(username, password);


            string serializedString = JsonConvert.SerializeObject(resultToken);

            return serializedString;


            //return _userService.Authenticate(username, password);


            //return ratingSystemBusiness.Login(username, password);

        }



        [HttpGet]
        [Route("GetExcelData")]

        public IActionResult GetExcelData(string inputDate)
        {
            RatingSystemBusiness ratingSystemBusiness = new RatingSystemBusiness();

            var excelData = ratingSystemBusiness.GetExcelData(inputDate);

            //List<ExcelDataModel> s1 = ratingSystemBusiness.GetExcelData(inputDate).Item1;
            //List<ExcelDataModel> s2 = ratingSystemBusiness.GetExcelData(inputDate).Item2;
            //List<ExcelDataModel> s3 = ratingSystemBusiness.GetExcelData(inputDate).Item3;

            List<ExcelDataModel> s1 = excelData.Item1;
            List<ExcelDataModel> s2 = excelData.Item2;
            List<ExcelDataModel> s3 = excelData.Item3;

            //ratingSystemBusiness.ExportToExcel(s1,s2,s3);

            return Ok(new { propertyName1 = s1, propertyName2 = s2, propertyName3 = s3 });


        }

        [HttpGet]
        [Route("GetValidData")]

        public List<string> GetValidData(string monthInput, string yearInput)
        {
            RatingSystemBusiness ratingSystemBusiness = new RatingSystemBusiness();

            return ratingSystemBusiness.GetValidData(monthInput, yearInput);
        }





        [HttpGet]
        [Route("DownloadFiles")]

        public async Task<IActionResult> DownloadFiles(string dateString)
        {

            DirectoryInfo directoryInfo = Directory.GetParent(Directory.GetCurrentDirectory());
            string mailReaderPath = Path.Combine(directoryInfo.FullName, "MailReader", "bin", "Debug", "net8.0");




            string zipFileName = "ExcelFilesToUpload.zip";
            string zipFilePath = Path.Combine(mailReaderPath + @"\Completed", zipFileName);



            if (System.IO.File.Exists(zipFilePath))
            {
                System.IO.File.Delete(zipFilePath);
            }
            


            string monthNumber = dateString.Substring(3, 2);
            string yearNumber = dateString.Substring(6, 4);



            string rtgFileName = "RTG_" + monthNumber + "_" + yearNumber + ".xlsx";
            string shareFileName = "SHARE_" + monthNumber + "_" + yearNumber + ".xlsx";
            string avgFileName = "ORTALAMA_" + yearNumber + ".xlsx";




            string directoryRtg = mailReaderPath + @"\Completed\" + rtgFileName;
            string directoryShare = mailReaderPath + @"\Completed\" + shareFileName;
            string directoryAvg = mailReaderPath + @"\Completed\" + avgFileName;








            // Create a new zip archive
            using (ZipArchive zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
            {
                // Add the files to the zip archive
                zipArchive.CreateEntryFromFile(directoryRtg, rtgFileName);
                zipArchive.CreateEntryFromFile(directoryShare, shareFileName);
                zipArchive.CreateEntryFromFile(directoryAvg, avgFileName);
            }

            // Read the zip file as bytes
            byte[] zipBytes = await System.IO.File.ReadAllBytesAsync(zipFilePath);

            // Delete the temporary zip file
            System.IO.File.Delete(zipFilePath);

            // Return the zip file
            return File(zipBytes, "application/zip", zipFileName);





            //var path = "C:\\Users\\mehme\\source\\repos\\C# repos\\RatingSystem\\RatingAPI\\ExcelFiles\\SUBAT_Rtg_11li.xlsx";

            //if (!System.IO.File.Exists(path))
            //    return NotFound();

            //Byte[] bytes = await System.IO.File.ReadAllBytesAsync(path);


            //return File(bytes, "application/octet-stream", "Example.xlsx");

        }



        [HttpGet]
        [Route("AddUser")]

        public void AddUser(string username, string password)
        {
            RatingSystemBusiness ratingSystemBusiness = new RatingSystemBusiness();

            if (username == "admin" || username == "consoleApp")
            {
                return;
            }

            ratingSystemBusiness.AddUser(username, password);
        }


        [HttpGet]
        [Route("ChangePassword")]

        public void ChangePassword(string username,string newPassword)
        {
            RatingSystemBusiness ratingSystemBusiness = new RatingSystemBusiness();

            if (username == "consoleApp")
            {
                return;
            }

            ratingSystemBusiness.ChangePassword(username, newPassword);
        }



        [HttpGet]
        [Route("DeleteUser")]

        public void DeleteUser(string username)
        {
            RatingSystemBusiness ratingSystemBusiness = new RatingSystemBusiness();

            if (username=="admin" || username == "consoleApp")
            {
                return;
            }
            ratingSystemBusiness.DeleteUser(username);
        }




        [HttpGet]
        [Route("SetConfig")]

        //public void SetConfig(string MailAdresi, string MailŞifresi, string connString, string KantarMediaMail, string IngestMail, string GönderilenMail, string APIURL, string ArayüzURL)
        public void SetConfig(string MailAdresi, string MailŞifresi, string KantarMediaMail, string IngestMail, string GonderilenMail)

        {
            RatingSystemBusiness ratingSystemBusiness = new RatingSystemBusiness();

            //ratingSystemBusiness.SetConfig(MailAdresi, MailŞifresi, connString, KantarMediaMail, IngestMail, GönderilenMail, APIURL, ArayüzURL);
            ratingSystemBusiness.SetConfig(MailAdresi, MailŞifresi,  KantarMediaMail, IngestMail, GonderilenMail);

        }


        [HttpGet]
        [Route("GetConfig")]

        public string GetConfig()
        {
            RatingSystemBusiness ratingSystemBusiness = new RatingSystemBusiness();

            return JsonConvert.SerializeObject(ratingSystemBusiness.GetConfig());
        }









    }
}
