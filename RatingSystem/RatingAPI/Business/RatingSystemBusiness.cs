using RatingAPI.Model;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using static Dapper.SqlMapper;
using Microsoft.AspNetCore.Components.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Globalization; // Import EPPlus namespace
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RatingAPI.Business
{
    public class RatingSystemBusiness
    {
        private readonly string connectionString = "Server=DESKTOP-G9FEQCM;Database=RatingSystem;Integrated Security=true;";


        private readonly string connectionString2 = "Server=DESKTOP-G9FEQCM;Database=RatingSystem;User Id = APIlogin; Password=1;";



        private readonly string[] colorArrayTr = ["Kırmızı", "Açık Yeşil", "Siyah", "Koyu Mavi", "Sarı", "Pembe", "Kahverengi", "Açık Mavi", "Gri", "Bordo", "Koyu Yeşil"];

        private readonly string[] colorArrayRGB = ["rgb(255, 58, 0)", "rgb(0, 255, 0)", "rgb(0, 0, 0)", "rgb(0, 175, 240)", "rgb(255, 192, 0)", "rgb(255, 0, 255)", "rgb(150, 70, 0)", "rgb(0, 255, 238)", "rgb(155, 155, 155)", "rgb(102, 0, 51)", "rgb(42, 124, 38)"];

        private readonly string[] colorArrayEng = ["red", "green", "black", "blue", "yellow", "pink", "saddlebrown", "cyan", "grey", "maroon", "darkgreen"];


        private IConfigurationRoot configuration;

        private static string connectionStringAll;

        public RatingSystemBusiness()
        {
            // Set up configuration builder
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            // Build configuration
            configuration = builder.Build();

            connectionStringAll = configuration["ConnectionStrings:MyConnectionString"];
            ConfigModel.ConnectionString = connectionStringAll;

        }

        //public System.Boolean Login(string username, string password)
        //{


        //using (IDbConnection db = new SqlConnection(connectionString))
        //{
        //    // Modify the SQL query to select the login info based on the input username
        //    var loginInfo = db.QuerySingleOrDefault<LoginInfo>(
        //        $"SELECT LoginUsername, LoginPassword FROM LoginTable WHERE LoginUsername = @Username",
        //        new { Username = username }
        //    );

        //    if (loginInfo != null && loginInfo.LoginPassword == password)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }


        //}


        //}




        public (List<ExcelDataModel>, List<ExcelDataModel>, List<ExcelDataModel>) GetExcelData(string inputDate)
        {
            List<ExcelDataModel> dataListRTG = new List<ExcelDataModel>();
            List<ExcelDataModel> dataListShare = new List<ExcelDataModel>();



            DateTime parsedDate = DateTime.ParseExact(inputDate, "dd-MM-yyyy", null);

            // Format the date as "yyyyMMdd"
            string outputDate = parsedDate.ToString("yyyyMMdd");



            using (IDbConnection db = new SqlConnection(connectionStringAll))
            {
                //var sqls = db.Query<ExcelDataModel>($"Exec GetDataListRTG '{date}'").ToList();

                //List<ExcelDataModel> dataList = db.Query<ExcelDataModel>($"Exec GetDataListRTG '{date}'").ToList();

                //dataListRTG = db.Query<ExcelDataModel>($"Exec GetDataListRTG '20240219'").ToList();
                //dataListShare = db.Query<ExcelDataModel>($"Exec GetDataListShare '20240219'").ToList();
               

                dataListRTG = db.Query<ExcelDataModel>($"Exec GetDataListRTG '{outputDate}'").ToList();
                dataListShare = db.Query<ExcelDataModel>($"Exec GetDataListShare '{outputDate}'").ToList();

            }


            //SortExcelData(dataList[0]);

            foreach (var dataItem in dataListRTG)
            {
                SortExcelData(dataItem);

                dataItem.CheckZeroValues();
            }

            foreach (var dataItem in dataListShare)
            {
                SortExcelData(dataItem);

                dataItem.CheckZeroValues();
            }

            //CreateMonthlyData("20240219");

            List<ExcelDataModel> monthlyAverage = new List<ExcelDataModel>();

           // monthlyAverage = CreateMonthlyData("20240219");
            monthlyAverage = CreateMonthlyData(outputDate);


            return (dataListRTG,dataListShare, monthlyAverage);
        }

        public List<string> GetValidData(string monthInput, string yearInput)
        {
            //string dateString1 = $"{yearInput}-{monthInput}-01";
            //string dateString2 = $"{yearInput}-{monthInput}-31";

            //DateTime date1 = DateTime.ParseExact(dateString1, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            //DateTime date2 = DateTime.ParseExact(dateString2, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            string monthInstring = "";

            switch (monthInput)
            {
                case "Ocak":
                    monthInstring = "01";
                        break;
                case "Şubat":
                    monthInstring = "02";
                    break;
                case "Mart":
                    monthInstring = "03";
                    break;
                case "Nisan":
                    monthInstring = "04";
                    break;
                case "Mayıs":
                    monthInstring = "05";
                    break;
                case "Haziran":
                    monthInstring = "06";
                    break;
                case "Temmuz":
                    monthInstring = "07";
                    break;
                case "Ağustos":
                    monthInstring = "08";
                    break;
                case "Eylül":
                    monthInstring = "09";
                    break;
                case "Ekim":
                    monthInstring = "10";
                    break;
                case "Kasım":
                    monthInstring = "11";
                    break;
                case "Aralık":
                    monthInstring = "12";
                    break;
                default:
                    break;
            }

            int month = int.Parse(monthInstring);
            int year = int.Parse(yearInput);

            // Get the last day of the month
            int lastDayOfMonth = DateTime.DaysInMonth(year, month);

            // Create a DateTime object representing the last day of the month
            DateTime lastDayOfMonthDate = new DateTime(year, month, lastDayOfMonth);

            // Your SQL query with the date range from the 1st day to the last day of the month
            string startDateString = $"{yearInput}-{monthInstring}-01";
            string endDateString = $"{yearInput}-{monthInstring}-{lastDayOfMonth}";

            List< string> validDataList = new List<string>();  

            string checkQuery = $"SELECT FORMAT(insertedDate, 'dd-MM-yyyy') AS formattedDate FROM CheckInsert WHERE insertedDate >= '{startDateString}' AND insertedDate <= '{endDateString}' AND insertValue=1 ORDER BY insertedDate";


            using (IDbConnection db = new SqlConnection(connectionStringAll))
            {
                //db.Execute(checkQuery);


                validDataList= db.Query<string>(checkQuery).ToList();

            }
            return validDataList;

        }


        public List<ExcelDataModel> CreateMonthlyData(string date)
        {
            ExcelDataModel ORTALAMA = new ExcelDataModel();


            List<ExcelDataModel> totalDayList = new List<ExcelDataModel>();



            DateTime datetime = DateTime.ParseExact(date, "yyyyMMdd", null);

            datetime = new DateTime(datetime.Year, datetime.Month, 1);

            string newDateString = datetime.ToString("yyyyMMdd");


            DateTime datetimeStandard = DateTime.ParseExact(date, "yyyyMMdd", null);

            int availableMonthCount = 0;

            int monthCount = 1;
            int lastDay = DateTime.DaysInMonth(datetimeStandard.Year, datetimeStandard.Month); // Get last day of the month

            string query = $"SELECT * FROM TotalDay WHERE tableType='SHARE' AND tableDate={date}";



            decimal TotalNTV         = 0;
            decimal TotalHABERTURK   = 0;
            decimal TotalCNNTURK     = 0;
            decimal TotalHALKTV      = 0;
            decimal TotalKRT         = 0;
            decimal TotalAHABER      = 0;
            decimal TotalTRTHABER    = 0;
            decimal TotalTELE1       = 0;
            decimal TotalTV100       = 0;
            decimal TotalHABERGLOBAL = 0;
            decimal TotalSOZCUTV     = 0;

            decimal ABSESNTV         = 0;
            decimal ABSESHABERTURK   = 0;
            decimal ABSESCNNTURK     = 0;
            decimal ABSESHALKTV      = 0;
            decimal ABSESKRT         = 0;
            decimal ABSESAHABER      = 0;
            decimal ABSESTRTHABER    = 0;
            decimal ABSESTELE1       = 0;
            decimal ABSESTV100       = 0;
            decimal ABSESHABERGLOBAL = 0;
            decimal ABSESSOZCUTV     = 0;

            using (IDbConnection db = new SqlConnection(connectionStringAll))
            {
                //var vs = db.Query<ExcelDataModel>($"SELECT * FROM TotalDay WHERE tableType='SHARE' AND tableDate='{newDateString}'").ToList();

                //while (db.Query<ExcelDataModel>($"SELECT * FROM TotalDay WHERE tableType='SHARE' AND tableDate='{newDateString}'").ToList().Count != 0)
                while (monthCount< (lastDay+1))
                {


                    List<ExcelDataModel> vs = db.Query<ExcelDataModel>($"SELECT * FROM TotalDay WHERE tableType='SHARE' AND tableDate='{newDateString}'").ToList();


                    ExcelDataModel obj = db.Query<ExcelDataModel>($"SELECT * FROM TotalDay WHERE tableType='SHARE' AND tableDate='{newDateString}'").FirstOrDefault();

                    if (obj != null)
                    {
                        SortExcelData(obj);
                        obj.CheckZeroValues();
                        totalDayList.Add(obj);






                        TotalNTV += vs[0].TotalNTV;
                        TotalHABERTURK += vs[0].TotalHABERTURK;
                        TotalCNNTURK += vs[0].TotalCNNTURK;
                        TotalHALKTV += vs[0].TotalHALKTV;
                        TotalKRT += vs[0].TotalKRT;
                        TotalAHABER += vs[0].TotalAHABER;
                        TotalTRTHABER += vs[0].TotalTRTHABER;
                        TotalTELE1 += vs[0].TotalTELE1;
                        TotalTV100 += vs[0].TotalTV100;
                        TotalHABERGLOBAL += vs[0].TotalHABERGLOBAL;
                        TotalSOZCUTV += vs[0].TotalSOZCUTV;

                        ABSESNTV += vs[0].ABSESNTV;
                        ABSESHABERTURK += vs[0].ABSESHABERTURK;
                        ABSESCNNTURK += vs[0].ABSESCNNTURK;
                        ABSESHALKTV += vs[0].ABSESHALKTV;
                        ABSESKRT += vs[0].ABSESKRT;
                        ABSESAHABER += vs[0].ABSESAHABER;
                        ABSESTRTHABER += vs[0].ABSESTRTHABER;
                        ABSESTELE1 += vs[0].ABSESTELE1;
                        ABSESTV100 += vs[0].ABSESTV100;
                        ABSESHABERGLOBAL += vs[0].ABSESHABERGLOBAL;
                        ABSESSOZCUTV += vs[0].ABSESSOZCUTV;


                        availableMonthCount++;
                    }

                    DateTime datetime2 = DateTime.ParseExact(newDateString, "yyyyMMdd", null);

                    monthCount++;

                    if (monthCount == (lastDay+1))
                    {
                        break;
                    }

                    datetime2 = new DateTime(datetime2.Year, datetime2.Month, monthCount);

                    newDateString = datetime2.ToString("yyyyMMdd");
                   
                    //monthCount++;


                    
                }

            }
             List<decimal> TotalArray = new List<decimal>();
            List<decimal> ABSESArray = new List<decimal>();
            ExcelDataModel rankMonthlyAverage = new ExcelDataModel();

            ORTALAMA.TotalNTV         = TotalNTV         / (availableMonthCount);
            ORTALAMA.TotalHABERTURK   = TotalHABERTURK   / (availableMonthCount);
            ORTALAMA.TotalCNNTURK     = TotalCNNTURK     / (availableMonthCount);
            ORTALAMA.TotalHALKTV      = TotalHALKTV      / (availableMonthCount);
            ORTALAMA.TotalKRT         = TotalKRT         / (availableMonthCount);
            ORTALAMA.TotalAHABER      = TotalAHABER      / (availableMonthCount);
            ORTALAMA.TotalTRTHABER    = TotalTRTHABER    / (availableMonthCount);
            ORTALAMA.TotalTELE1       = TotalTELE1       / (availableMonthCount);
            ORTALAMA.TotalTV100       = TotalTV100       / (availableMonthCount);
            ORTALAMA.TotalHABERGLOBAL = TotalHABERGLOBAL / (availableMonthCount);
            ORTALAMA.TotalSOZCUTV     = TotalSOZCUTV     / (availableMonthCount);
                                                                       
            ORTALAMA.ABSESNTV         = ABSESNTV         / (availableMonthCount);
            ORTALAMA.ABSESHABERTURK   = ABSESHABERTURK   / (availableMonthCount);
            ORTALAMA.ABSESCNNTURK     = ABSESCNNTURK     / (availableMonthCount);
            ORTALAMA.ABSESHALKTV      = ABSESHALKTV      / (availableMonthCount);
            ORTALAMA.ABSESKRT         = ABSESKRT         / (availableMonthCount);
            ORTALAMA.ABSESAHABER      = ABSESAHABER      / (availableMonthCount);
            ORTALAMA.ABSESTRTHABER    = ABSESTRTHABER    / (availableMonthCount);
            ORTALAMA.ABSESTELE1       = ABSESTELE1       / (availableMonthCount);
            ORTALAMA.ABSESTV100       = ABSESTV100       / (availableMonthCount);
            ORTALAMA.ABSESHABERGLOBAL = ABSESHABERGLOBAL / (availableMonthCount);
            ORTALAMA.ABSESSOZCUTV     = ABSESSOZCUTV / (availableMonthCount);


            TotalArray.Add(ORTALAMA.TotalNTV        );
            TotalArray.Add(ORTALAMA.TotalHABERTURK  );
            TotalArray.Add(ORTALAMA.TotalCNNTURK    );
            TotalArray.Add(ORTALAMA.TotalHALKTV     );
            TotalArray.Add(ORTALAMA.TotalKRT        );
            TotalArray.Add(ORTALAMA.TotalAHABER     );
            TotalArray.Add(ORTALAMA.TotalTRTHABER   );
            TotalArray.Add(ORTALAMA.TotalTELE1      );
            TotalArray.Add(ORTALAMA.TotalTV100      );
            TotalArray.Add(ORTALAMA.TotalHABERGLOBAL);
            TotalArray.Add(ORTALAMA.TotalSOZCUTV);

            ABSESArray.Add(ORTALAMA.ABSESNTV        );
            ABSESArray.Add(ORTALAMA.ABSESHABERTURK  );
            ABSESArray.Add(ORTALAMA.ABSESCNNTURK    );
            ABSESArray.Add(ORTALAMA.ABSESHALKTV     );
            ABSESArray.Add(ORTALAMA.ABSESKRT        );
            ABSESArray.Add(ORTALAMA.ABSESAHABER     );
            ABSESArray.Add(ORTALAMA.ABSESTRTHABER   );
            ABSESArray.Add(ORTALAMA.ABSESTELE1      );
            ABSESArray.Add(ORTALAMA.ABSESTV100      );
            ABSESArray.Add(ORTALAMA.ABSESHABERGLOBAL);
            ABSESArray.Add(ORTALAMA.ABSESSOZCUTV);



            var tuples1 = TotalArray.Select((x, i) => (Value: x, OldIndex: i, NewIndex: -1))
                                        .OrderByDescending(x => x.Value)
                                        .Select((x, i) => (x.Value, x.OldIndex, NewIndex: i + 1))
                                        .OrderBy(x => x.OldIndex)
                                        .ToArray();

            var tuples2 = ABSESArray.Select((x, i) => (Value: x, OldIndex: i, NewIndex: -1))
                                       .OrderByDescending(x => x.Value)
                                       .Select((x, i) => (x.Value, x.OldIndex, NewIndex: i + 1))
                                       .OrderBy(x => x.OldIndex)
                                       .ToArray();


            rankMonthlyAverage.TotalNTV = tuples1[0].NewIndex;
            rankMonthlyAverage.TotalHABERTURK       = tuples1[1].NewIndex;
            rankMonthlyAverage.TotalCNNTURK         = tuples1[2].NewIndex;
            rankMonthlyAverage.TotalHALKTV          = tuples1[3].NewIndex;
            rankMonthlyAverage.TotalKRT             = tuples1[4].NewIndex;
            rankMonthlyAverage.TotalAHABER          = tuples1[5].NewIndex;
            rankMonthlyAverage.TotalTRTHABER        = tuples1[6].NewIndex;
            rankMonthlyAverage.TotalTELE1           = tuples1[7].NewIndex;
            rankMonthlyAverage.TotalTV100           = tuples1[8].NewIndex;
            rankMonthlyAverage.TotalHABERGLOBAL     = tuples1[9].NewIndex;
            rankMonthlyAverage.TotalSOZCUTV         = tuples1[10].NewIndex;

            rankMonthlyAverage.ABSESNTV         = tuples2[0].NewIndex;
            rankMonthlyAverage.ABSESHABERTURK   = tuples2[1].NewIndex;
            rankMonthlyAverage.ABSESCNNTURK     = tuples2[2].NewIndex;
            rankMonthlyAverage.ABSESHALKTV      = tuples2[3].NewIndex;
            rankMonthlyAverage.ABSESKRT         = tuples2[4].NewIndex;
            rankMonthlyAverage.ABSESAHABER      = tuples2[5].NewIndex;
            rankMonthlyAverage.ABSESTRTHABER    = tuples2[6].NewIndex;
            rankMonthlyAverage.ABSESTELE1       = tuples2[7].NewIndex;
            rankMonthlyAverage.ABSESTV100       = tuples2[8].NewIndex;
            rankMonthlyAverage.ABSESHABERGLOBAL = tuples2[9].NewIndex;
            rankMonthlyAverage.ABSESSOZCUTV     = tuples2[10].NewIndex;


            ORTALAMA.timebands = "ORTALAMA";
            ORTALAMA.tableDate = datetime;

            SortExcelData(ORTALAMA);
            ORTALAMA.CheckZeroValues();

           

            totalDayList.Add(ORTALAMA);




            rankMonthlyAverage.timebands = "Sıralama";

            SortExcelData(rankMonthlyAverage);
            rankMonthlyAverage.CheckZeroValues();


            totalDayList.Add(rankMonthlyAverage);


            //return ORTALAMA;
            return totalDayList;
        }




        //public void ExportToExcel(List<ExcelDataModel> listRtg, List<ExcelDataModel> listShare, List<ExcelDataModel> listAvg)
        //{
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


        //    string dateString = listRtg[0].tableDate.ToString();

        //    DateTime date = DateTime.ParseExact(dateString, "dd.MM.yyyy HH:mm:ss", null);

        //    int monthNumber = date.Month;

        //    string monthString="";


        //    switch (monthNumber)
        //    {
        //        case 1:
        //            monthString = "OCAK";
        //            break;
        //        case 2:
        //            monthString = "SUBAT";
        //            break;
        //        case 3:
        //            monthString = "MART";
        //            break;
        //        case 4:
        //            monthString = "NISAN";
        //            break;
        //        case 5:
        //            monthString = "MAYIS";
        //            break;
        //        case 6:
        //            monthString = "HAZIRAN";
        //            break;
        //        case 7:
        //            monthString = "TEMMUZ";
        //            break;
        //        case 8:
        //            monthString = "AGUSTOS";
        //            break;
        //        case 9:
        //            monthString = "EYLUL";
        //            break;
        //        case 10:
        //            monthString = "EKIM";
        //            break;
        //        case 11:
        //            monthString = "KASIM";
        //            break;
        //        case 12:
        //            monthString = "ARALIK";
        //            break;
        //        default:
        //            break;
        //    }


        //    string sheetName=date.Day.ToString() + " " + monthString;

        //    string sheetName2 = sheetName + " ";

        //    //string sheetNameAvg = monthString + "'" + date.Year.ToString().Substring(2);

        //    //string directoryPath = Directory.GetCurrentDirectory() + @"\ExcelFiles\SUBAT_Rtg_11li.xlsx";

        //    string directoryPathRtg = Directory.GetCurrentDirectory() + @"\ExcelFiles\" + monthString +"_Rtg_11li.xlsx";


        //    if (File.Exists(directoryPathRtg))
        //    {
        //        using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(directoryPathRtg)))
        //        {
        //            //ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
        //            //ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.SingleOrDefault(sheet => sheet.Name == "19 SUBAT ");
        //            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.SingleOrDefault(sheet => sheet.Name.Contains(sheetName));

        //            if (worksheet==null)
        //            {
        //                worksheet = excelPackage.Workbook.Worksheets.SingleOrDefault(sheet => sheet.Name == sheetName2);

        //            }
        //            int startRow = 4; 

        //            foreach (var obj in listRtg)
        //            {
        //                if (startRow==101)
        //                {
        //                    startRow = 102;
        //                }
        //                worksheet.Cells[startRow, 5].Value = obj.TotalNTV;
        //                worksheet.Cells[startRow, 6].Value = obj.TotalHABERTURK;
        //                worksheet.Cells[startRow, 7].Value = obj.TotalCNNTURK;
        //                worksheet.Cells[startRow, 8].Value = obj.TotalHALKTV;
        //                worksheet.Cells[startRow, 9].Value = obj.TotalKRT;
        //                worksheet.Cells[startRow, 10].Value = obj.TotalAHABER;
        //                worksheet.Cells[startRow, 11].Value = obj.TotalTRTHABER;
        //                worksheet.Cells[startRow, 12].Value = obj.TotalTELE1;
        //                worksheet.Cells[startRow, 13].Value = obj.TotalTV100;
        //                worksheet.Cells[startRow, 14].Value = obj.TotalHABERGLOBAL;
        //                worksheet.Cells[startRow, 15].Value = obj.TotalSOZCUTV;

        //                worksheet.Cells[startRow, 17].Value = obj.ABSESNTV;
        //                worksheet.Cells[startRow, 18].Value = obj.ABSESHABERTURK;
        //                worksheet.Cells[startRow, 19].Value = obj.ABSESCNNTURK;
        //                worksheet.Cells[startRow, 20].Value = obj.ABSESHALKTV;
        //                worksheet.Cells[startRow, 21].Value = obj.ABSESKRT;
        //                worksheet.Cells[startRow, 22].Value = obj.ABSESAHABER;
        //                worksheet.Cells[startRow, 23].Value = obj.ABSESTRTHABER;
        //                worksheet.Cells[startRow, 24].Value = obj.ABSESTELE1;
        //                worksheet.Cells[startRow, 25].Value = obj.ABSESTV100;
        //                worksheet.Cells[startRow, 26].Value = obj.ABSESHABERGLOBAL;
        //                worksheet.Cells[startRow, 27].Value = obj.ABSESSOZCUTV;

                        

        //                startRow++;
        //            }
                    

        //            // Save changes to the Excel file
        //            excelPackage.Save();
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("The Excel file does not exist.");
        //    }


        //    string directoryPathShare = Directory.GetCurrentDirectory() + @"\ExcelFiles\" + monthString + "_Share_11li.xlsx";


        //    if (File.Exists(directoryPathShare))
        //    {
        //        using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(directoryPathShare)))
        //        {
        //            //ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
        //            //ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.SingleOrDefault(sheet => sheet.Name == "19 SUBAT ");
        //            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.SingleOrDefault(sheet => sheet.Name.Contains(sheetName));

        //            if (worksheet == null)
        //            {
        //                worksheet = excelPackage.Workbook.Worksheets.SingleOrDefault(sheet => sheet.Name == sheetName2);

        //            }
        //            int startRow = 4;

        //            foreach (var obj in listShare)
        //            {
        //                if (startRow == 101)
        //                {
        //                    startRow = 102;
        //                }
        //                worksheet.Cells[startRow, 5].Value = obj.TotalNTV;
        //                worksheet.Cells[startRow, 6].Value = obj.TotalHABERTURK;
        //                worksheet.Cells[startRow, 7].Value = obj.TotalCNNTURK;
        //                worksheet.Cells[startRow, 8].Value = obj.TotalHALKTV;
        //                worksheet.Cells[startRow, 9].Value = obj.TotalKRT;
        //                worksheet.Cells[startRow, 10].Value = obj.TotalAHABER;
        //                worksheet.Cells[startRow, 11].Value = obj.TotalTRTHABER;
        //                worksheet.Cells[startRow, 12].Value = obj.TotalTELE1;
        //                worksheet.Cells[startRow, 13].Value = obj.TotalTV100;
        //                worksheet.Cells[startRow, 14].Value = obj.TotalHABERGLOBAL;
        //                worksheet.Cells[startRow, 15].Value = obj.TotalSOZCUTV;
                            
        //                worksheet.Cells[startRow, 17].Value = obj.ABSESNTV;
        //                worksheet.Cells[startRow, 18].Value = obj.ABSESHABERTURK;
        //                worksheet.Cells[startRow, 19].Value = obj.ABSESCNNTURK;
        //                worksheet.Cells[startRow, 20].Value = obj.ABSESHALKTV;
        //                worksheet.Cells[startRow, 21].Value = obj.ABSESKRT;
        //                worksheet.Cells[startRow, 22].Value = obj.ABSESAHABER;
        //                worksheet.Cells[startRow, 23].Value = obj.ABSESTRTHABER;
        //                worksheet.Cells[startRow, 24].Value = obj.ABSESTELE1;
        //                worksheet.Cells[startRow, 25].Value = obj.ABSESTV100;
        //                worksheet.Cells[startRow, 26].Value = obj.ABSESHABERGLOBAL;
        //                worksheet.Cells[startRow, 27].Value = obj.ABSESSOZCUTV;



        //                startRow++;
        //            }


        //            // Save changes to the Excel file
        //            excelPackage.Save();
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("The Excel file does not exist.");
        //    }






        //    CultureInfo turkishCulture = new CultureInfo("tr-TR");

        //    DateTimeFormatInfo dateTimeFormat = turkishCulture.DateTimeFormat;

        //    string monthTurkishName = dateTimeFormat.GetMonthName(monthNumber);

        //    string sheetNameAvg = monthTurkishName.ToUpper() + "'" + date.Year.ToString().Substring(2);

        //    string sheetNameAvg2 = string.Concat(monthTurkishName, "'", date.Year.ToString().AsSpan(2));


        //    string directoryPathAvg = Directory.GetCurrentDirectory() + @"\ExcelFiles\AYLIK ORTALAMALAR_11li.xlsx";


        //    int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);


        //    if (File.Exists(directoryPathAvg))
        //    {
        //        using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(directoryPathAvg)))
        //        {
        //            //ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
        //            //ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.SingleOrDefault(sheet => sheet.Name == "19 SUBAT ");
        //            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.SingleOrDefault(sheet => sheet.Name.Contains(sheetNameAvg));

                   
        //            int startRow = 4;

        //            foreach (var obj in listAvg)
        //            {
        //                if (obj.timebands=="ORTALAMA")
        //                {
        //                    startRow = daysInMonth + 4;
        //                }
        //                if (startRow==(daysInMonth + 5))
        //                {
        //                    startRow++;
        //                }
        //                worksheet.Cells[startRow, 2].Value = obj.TotalNTV;
        //                worksheet.Cells[startRow, 3].Value = obj.TotalHABERTURK;
        //                worksheet.Cells[startRow, 4].Value = obj.TotalCNNTURK;
        //                worksheet.Cells[startRow, 5].Value = obj.TotalHALKTV;
        //                worksheet.Cells[startRow, 6].Value = obj.TotalKRT;
        //                worksheet.Cells[startRow, 7].Value = obj.TotalAHABER;
        //                worksheet.Cells[startRow, 8].Value = obj.TotalTRTHABER;
        //                worksheet.Cells[startRow, 9].Value = obj.TotalTELE1;
        //                worksheet.Cells[startRow, 10].Value = obj.TotalTV100;
        //                worksheet.Cells[startRow, 11].Value = obj.TotalHABERGLOBAL;
        //                worksheet.Cells[startRow, 12].Value = obj.TotalSOZCUTV;

        //                worksheet.Cells[startRow, 14].Value = obj.ABSESNTV;
        //                worksheet.Cells[startRow, 15].Value = obj.ABSESHABERTURK;
        //                worksheet.Cells[startRow, 16].Value = obj.ABSESCNNTURK;
        //                worksheet.Cells[startRow, 17].Value = obj.ABSESHALKTV;  
        //                worksheet.Cells[startRow, 18].Value = obj.ABSESKRT;
        //                worksheet.Cells[startRow, 19].Value = obj.ABSESAHABER;
        //                worksheet.Cells[startRow, 20].Value = obj.ABSESTRTHABER;
        //                worksheet.Cells[startRow, 21].Value = obj.ABSESTELE1;
        //                worksheet.Cells[startRow, 22].Value = obj.ABSESTV100;
        //                worksheet.Cells[startRow, 23].Value = obj.ABSESHABERGLOBAL;
        //                worksheet.Cells[startRow, 24].Value = obj.ABSESSOZCUTV;



        //                startRow++;
        //            }


        //            // Save changes to the Excel file
        //            excelPackage.Save();
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("The Excel file does not exist.");
        //    }

        //}


        public void SortExcelData(ExcelDataModel excelDataModel)
        {
            Dictionary<string, decimal> totalDictionary = new Dictionary<string, decimal>();
            Dictionary<string, decimal> absesDictionary = new Dictionary<string, decimal>();


            totalDictionary.Add("TotalNTV", excelDataModel.TotalNTV);
            totalDictionary.Add("TotalHABERTURK", excelDataModel.TotalHABERTURK);
            totalDictionary.Add("TotalCNNTURK", excelDataModel.TotalCNNTURK);
            totalDictionary.Add("TotalHALKTV", excelDataModel.TotalHALKTV);
            totalDictionary.Add("TotalKRT", excelDataModel.TotalKRT);
            totalDictionary.Add("TotalAHABER", excelDataModel.TotalAHABER);
            totalDictionary.Add("TotalTRTHABER", excelDataModel.TotalTRTHABER);
            totalDictionary.Add("TotalTELE1", excelDataModel.TotalTELE1);
            totalDictionary.Add("TotalTV100", excelDataModel.TotalTV100);
            totalDictionary.Add("TotalHABERGLOBAL", excelDataModel.TotalHABERGLOBAL);
            totalDictionary.Add("TotalSOZCUTV", excelDataModel.TotalSOZCUTV);

            absesDictionary.Add("ABSESNTV", excelDataModel.ABSESNTV);
            absesDictionary.Add("ABSESHABERTURK", excelDataModel.ABSESHABERTURK);
            absesDictionary.Add("ABSESCNNTURK", excelDataModel.ABSESCNNTURK);
            absesDictionary.Add("ABSESHALKTV", excelDataModel.ABSESHALKTV);
            absesDictionary.Add("ABSESKRT", excelDataModel.ABSESKRT);
            absesDictionary.Add("ABSESAHABER", excelDataModel.ABSESAHABER);
            absesDictionary.Add("ABSESTRTHABER", excelDataModel.ABSESTRTHABER);
            absesDictionary.Add("ABSESTELE1", excelDataModel.ABSESTELE1);
            absesDictionary.Add("ABSESTV100", excelDataModel.ABSESTV100);
            absesDictionary.Add("ABSESHABERGLOBAL", excelDataModel.ABSESHABERGLOBAL);
            absesDictionary.Add("ABSESSOZCUTV", excelDataModel.ABSESSOZCUTV);





            var sortedTotal = totalDictionary.OrderByDescending(kv => kv.Value);
            var sortedABSES = absesDictionary.OrderByDescending(kv => kv.Value);

            if (excelDataModel.timebands == "Sıralama")
            {
                sortedTotal = totalDictionary.OrderBy(kv => kv.Value);
                sortedABSES = absesDictionary.OrderBy(kv => kv.Value);
            }


            int rankTotal = 0;
            int rankABSES = 0;

            foreach (var item in sortedTotal)
            {

                string columnName = item.Key;

                switch (columnName)
                {
                    case "TotalNTV":
                        excelDataModel.ColorTotalNTV = colorArrayRGB[rankTotal];
                        break;
                    case "TotalHABERTURK":
                        excelDataModel.ColorTotalHABERTURK = colorArrayRGB[rankTotal];
                        break;
                    case "TotalCNNTURK":
                        excelDataModel.ColorTotalCNNTURK = colorArrayRGB[rankTotal];
                        break;
                    case "TotalHALKTV":
                        excelDataModel.ColorTotalHALKTV = colorArrayRGB[rankTotal];
                        break;
                    case "TotalKRT":
                        excelDataModel.ColorTotalKRT = colorArrayRGB[rankTotal];
                        break;
                    case "TotalAHABER":
                        excelDataModel.ColorTotalAHABER = colorArrayRGB[rankTotal];
                        break;
                    case "TotalTRTHABER":
                        excelDataModel.ColorTotalTRTHABER = colorArrayRGB[rankTotal];
                        break;
                    case "TotalTELE1":
                        excelDataModel.ColorTotalTELE1 = colorArrayRGB[rankTotal];
                        break;
                    case "TotalTV100":
                        excelDataModel.ColorTotalTV100 = colorArrayRGB[rankTotal];
                        break;
                    case "TotalHABERGLOBAL":
                        excelDataModel.ColorTotalHABERGLOBAL = colorArrayRGB[rankTotal];
                        break;
                    case "TotalSOZCUTV":
                        excelDataModel.ColorTotalSOZCUTV = colorArrayRGB[rankTotal];
                        break;
                }

                //Console.WriteLine($"Variable: {item.Key}, Value: {item.Value}, Color: {colorArrayRGB[rankTotal]}");

                rankTotal++;
            }



            //Console.WriteLine("\n");




            foreach (var item in sortedABSES)
            {

                string columnName = item.Key;

                switch (columnName)
                {
                    case "ABSESNTV":
                        excelDataModel.ColorABSESNTV = colorArrayRGB[rankABSES];
                        break;
                    case "ABSESHABERTURK":
                        excelDataModel.ColorABSESHABERTURK = colorArrayRGB[rankABSES];
                        break;
                    case "ABSESCNNTURK":
                        excelDataModel.ColorABSESCNNTURK = colorArrayRGB[rankABSES];
                        break;
                    case "ABSESHALKTV":
                        excelDataModel.ColorABSESHALKTV = colorArrayRGB[rankABSES];
                        break;
                    case "ABSESKRT":
                        excelDataModel.ColorABSESKRT = colorArrayRGB[rankABSES];
                        break;
                    case "ABSESAHABER":
                        excelDataModel.ColorABSESAHABER = colorArrayRGB[rankABSES];
                        break;
                    case "ABSESTRTHABER":
                        excelDataModel.ColorABSESTRTHABER = colorArrayRGB[rankABSES];
                        break;
                    case "ABSESTELE1":
                        excelDataModel.ColorABSESTELE1 = colorArrayRGB[rankABSES];
                        break;
                    case "ABSESTV100":
                        excelDataModel.ColorABSESTV100 = colorArrayRGB[rankABSES];
                        break;
                    case "ABSESHABERGLOBAL":
                        excelDataModel.ColorABSESHABERGLOBAL = colorArrayRGB[rankABSES];
                        break;
                    case "ABSESSOZCUTV":
                        excelDataModel.ColorABSESSOZCUTV = colorArrayRGB[rankABSES];
                        break;
                }


               // Console.WriteLine($"Variable: {item.Key}, Value: {item.Value}, Color: {colorArrayRGB[rankABSES]}");

                rankABSES++;
            }


        }



        public void AddUser(string username, string password)
        {
            using (IDbConnection db = new SqlConnection(ConfigModel.ConnectionString))
            {
                db.Execute(
                $"INSERT INTO LoginTable (LoginUsername, LoginPassword) VALUES (@Username, @Password)",
                new { Username = username, Password = password }
                );

            }
        }

        public void ChangePassword(string username, string newPassword)
        {

            using (IDbConnection db = new SqlConnection(ConfigModel.ConnectionString))
            {
                db.Execute(
                 $"UPDATE LoginTable SET LoginPassword = @Password WHERE LoginUsername = @Username",
                 new { Username = username, Password = newPassword }
                 );


            }
        }

        public void DeleteUser(string username)
        {

            using (IDbConnection db = new SqlConnection(ConfigModel.ConnectionString))
            {
                db.Execute(
                 $"DELETE FROM LoginTable WHERE LoginUsername = @Username",
                 new { Username = username }
                 );


            }
        }






        static JObject LoadAppSettings()
        {
            // Load settings from JSON file
            string json = File.ReadAllText("appsettings.json");
            return JObject.Parse(json);
        }


        static void SaveAppSettings(JObject appSettings)
        {
            // Save settings to JSON file
            string json = appSettings.ToString();
            File.WriteAllText("appsettings.json", json);
        }


        //public void SetConfig(string MailAdresi, string MailŞifresi, string connString, string KantarMediaMail, string IngestMail, string GönderilenMail, string APIURL, string ArayüzURL)
        public void SetConfig(string MailAdresi, string MailŞifresi,string KantarMediaMail, string IngestMail, string GonderilenMail)

        {
            JObject appSettings = LoadAppSettings();

            appSettings["MailSettings"]["MailAdress"] = MailAdresi;
            appSettings["MailSettings"]["MailPassword"] = MailŞifresi;

            //appSettings["ConnectionStrings"]["MyConnectionString"] = connString;

            appSettings["MailSettings"]["KantarMail"] = KantarMediaMail;
            appSettings["MailSettings"]["GönderilenMail"] = GonderilenMail ;
            appSettings["MailSettings"]["IngestMail"] = IngestMail;

            //appSettings["ApiUrl"]["MyApiUrl"] = APIURL;
            //appSettings["AngularUrl"]["MyAngularUrl"] = ArayüzURL;


            SaveAppSettings(appSettings);

        }

        public JObject GetConfig()
        {
            JObject appSettings = LoadAppSettings();

            return appSettings;
            
        }








    }
}
