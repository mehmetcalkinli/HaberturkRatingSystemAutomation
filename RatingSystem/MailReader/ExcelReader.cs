using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using System.Data.SqlClient;
using Dapper;
using System.Threading.Channels;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using static Dapper.SqlMapper;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Data.Common;

using RestSharp;
using Newtonsoft.Json;
using MailReader.Model;
using System.ComponentModel;
using OfficeOpenXml;
//using System.Net.Mail;
using System.Net;
using System.Xml.Linq;
using Aspose.Email.Clients;
using Aspose.Email;
using Aspose.Email.Mime;
using Aspose.Email.Clients.Smtp;
using OpenPop.Mime;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace MailReader
{
    internal class ExcelReader
    {
        //private string connectionString = "Server=DESKTOP-G9FEQCM;Database=RatingSystem;Integrated Security=true;";
        private string[] channelsArray = { "NTV", "HABER TURK", "CNN TURK", "HALK TV", "KRT TV", "AHABER", "TRT HABER", "TELE1", "TV100", "HABER GLOBAL", "SOZCU TV" };

       






        public void getFilesFromMail()
        {
            string savePath = Directory.GetCurrentDirectory() + @"\Income\";

            DateTime yesterday = DateTime.Today;
            DateTime today = yesterday.AddDays(-1);


            DateTime startDate = new DateTime(2024, 7, 1);
            DateTime endDate = new DateTime(2024, 7, 30);



            int monthNumber = today.Month;
            string monthString = "";

            switch (monthNumber)
            {
                case 1:
                    monthString = "OCAK";
                    break;
                case 2:
                    monthString = "SUBAT";
                    break;
                case 3:
                    monthString = "MART";
                    break;
                case 4:
                    monthString = "NISAN";
                    break;
                case 5:
                    monthString = "MAYIS";
                    break;
                case 6:
                    monthString = "HAZIRAN";
                    break;
                case 7:
                    monthString = "TEMMUZ";
                    break;
                case 8:
                    monthString = "AGUSTOS";
                    break;
                case 9:
                    monthString = "EYLUL";
                    break;
                case 10:
                    monthString = "EKIM";
                    break;
                case 11:
                    monthString = "KASIM";
                    break;
                case 12:
                    monthString = "ARALIK";
                    break;
                default:
                    break;
            }


            //string fileInputData = "15 Min Report (" + today.ToString().Substring(0, 10) + ").xlsx";
            string fileInputData = "15 Min Report (" + today.ToString("dd.MM.yyyy") + ").xlsx";

            string fileRtgData = monthString + "_Rtg_11li.xlsx";
            string fileShareData = monthString + "_Share_11li.xlsx";

            //string fileInputData = "15 Min Report (19.02.2024).xlsx";
            //string fileRtgData = "SUBAT_Rtg_11li.xlsx";
            //string fileShareData =  "SUBAT_Share_11li.xlsx";

            string[] fileNames = new string[] { fileInputData, fileRtgData, fileShareData };


            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (OpenPop.Pop3.Pop3Client client = new OpenPop.Pop3.Pop3Client())
            {
                client.Connect("mail.cyh.com.tr", 995, true);
                client.Authenticate(ConfigModel.MailAdress, ConfigModel.MailPassword, OpenPop.Pop3.AuthenticationMethod.UsernameAndPassword);



                if (client.Connected)
                {
                    int mailCount = client.GetMessageCount();

                    List<Message> todayMails = new List<Message>(mailCount);
                    for (int i = mailCount; i > 0; i--)
                    {
                        todayMails.Add(client.GetMessage(i));
                        //today = client.GetMessage(i).Headers.DateSent.Date;
                        if (client.GetMessage(i).Headers.DateSent.Date != yesterday)
                        {
                            break;
                        }

                        

                        if (client.GetMessage(i).Headers.DateSent.Date >= startDate && client.GetMessage(i).Headers.DateSent.Date <= endDate)
                        {
                        }
                        else
                        {
                            return;
                        }
                    }
                    foreach (Message mail in todayMails)
                    {

                        if (mail.Headers.From.Address == ConfigModel.KantarMail || mail.Headers.From.Address == ConfigModel.IngestMail)
                        {
                            // Check if the email has attachments
                            if (mail.FindAllAttachments().Any())
                            {
                                foreach (var attachment in mail.FindAllAttachments())
                                {
                                    //string filenamee = "15 Min Report (" + today.ToString().Substring(0, 10) + ").xlsx";
                                    //string filenameeee = "15 Min Report (" +/*today.ToString().Substring(0,10)*/"19.02.2024" + ").xlsx";



                                    //if (attachment.FileName == "15 Min Report (19.02.2024).xlsx"
                                    //    || attachment.FileName == "SUBAT_Rtg_11li.xlsx"
                                    //    || attachment.FileName == "SUBAT_Share_11li.xlsx")
                                    if (attachment.FileName == fileInputData
                                    || attachment.FileName == fileRtgData
                                    || attachment.FileName == fileShareData)
                                    {

                                        File.WriteAllBytes(savePath+ attachment.FileName, attachment.Body); // Use default UTF-8 encoding
                                    }
                                }
                            }
                        }

                    }
                }
            }


            if (Directory.Exists(savePath))
            {
                int foundCount = 0;



                foreach (string fileName in fileNames)
                {
                    string[] files = Directory.GetFiles(savePath, fileName);

                    if (files.Length > 0)
                    {
                        foundCount++;
                    }

                }
                if (foundCount == 3)
                {
                   processExcel();
                }

            }
            
        }



        public void processExcel()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string directoryPath = Directory.GetCurrentDirectory() + @"\Income";


            if (Directory.Exists(directoryPath))
            {
                // Create a DirectoryInfo object
                DirectoryInfo directory = new DirectoryInfo(directoryPath);

                // Get all files in the directory
                FileInfo[] files = directory.GetFiles("*.xlsx");

                // Display each file's name
                Console.WriteLine("Files in the directory:");



                foreach (FileInfo file in files)
                {
                    Console.WriteLine(file.Name);

                    if (file.Name.Contains("15 Min Report"))
                    {
                        readMainExcel(directoryPath + @"\" + file.Name, file.Name);

                    }
                    else if (file.Name.Contains("Rtg_11li"))
                    {
                        readRtgExcel(directoryPath + @"\" + file.Name, file.Name);
                    }
                }

                //executeJoinQuery();

                

            }
            



        }

        private void readMainExcel(string fileName, string fileName2)
        {




            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {


                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = false // Use the first row as column names
                        }
                    });





                    string formattedDate = "";



                    var RtgTotalNTV = "";
                    var RtgTotalHABERTURK = "";
                    var RtgTotalCNNTURK = "";
                    var RtgTotalHALKTV = "";
                    var RtgTotalKRT = "";
                    var RtgTotalAHABER = "";
                    var RtgTotalTRTHABER = "";
                    var RtgTotalTELE1 = "";
                    var RtgTotalTV100 = "";
                    var RtgTotalHABERGLOBAL = "";
                    var RtgTotalSOZCUTV = "";

                    var RtgABSESNTV = "";
                    var RtgABSESHABERTURK = "";
                    var RtgABSESCNNTURK = "";
                    var RtgABSESHALKTV = "";
                    var RtgABSESKRT = "";
                    var RtgABSESAHABER = "";
                    var RtgABSESTRTHABER = "";
                    var RtgABSESTELE1 = "";
                    var RtgABSESTV100 = "";
                    var RtgABSESHABERGLOBAL = "";
                    var RtgABSESSOZCUTV = "";

                    var ShareTotalNTV = "";
                    var ShareTotalHABERTURK = "";
                    var ShareTotalCNNTURK = "";
                    var ShareTotalHALKTV = "";
                    var ShareTotalKRT = "";
                    var ShareTotalAHABER = "";
                    var ShareTotalTRTHABER = "";
                    var ShareTotalTELE1 = "";
                    var ShareTotalTV100 = "";
                    var ShareTotalHABERGLOBAL = "";
                    var ShareTotalSOZCUTV = "";

                    var ShareABSESNTV = "";
                    var ShareABSESHABERTURK = "";
                    var ShareABSESCNNTURK = "";
                    var ShareABSESHALKTV = "";
                    var ShareABSESKRT = "";
                    var ShareABSESAHABER = "";
                    var ShareABSESTRTHABER = "";
                    var ShareABSESTELE1 = "";
                    var ShareABSESTV100 = "";
                    var ShareABSESHABERGLOBAL = "";
                    var ShareABSESSOZCUTV = "";




                    int RankRtgTotalNTV = -1;
                    int RankRtgTotalHABERTURK = -1;
                    int RankRtgTotalCNNTURK = -1;
                    int RankRtgTotalHALKTV = -1;
                    int RankRtgTotalKRT = -1;
                    int RankRtgTotalAHABER = -1;
                    int RankRtgTotalTRTHABER = -1;
                    int RankRtgTotalTELE1 = -1;
                    int RankRtgTotalTV100 = -1;
                    int RankRtgTotalHABERGLOBAL = -1;
                    int RankRtgTotalSOZCUTV = -1;

                    int RankRtgABSESNTV = -1;
                    int RankRtgABSESHABERTURK = -1;
                    int RankRtgABSESCNNTURK = -1;
                    int RankRtgABSESHALKTV = -1;
                    int RankRtgABSESKRT = -1;
                    int RankRtgABSESAHABER = -1;
                    int RankRtgABSESTRTHABER = -1;
                    int RankRtgABSESTELE1 = -1;
                    int RankRtgABSESTV100 = -1;
                    int RankRtgABSESHABERGLOBAL = -1;
                    int RankRtgABSESSOZCUTV = -1;

                    int RankShareTotalNTV = -1;
                    int RankShareTotalHABERTURK = -1;
                    int RankShareTotalCNNTURK = -1;
                    int RankShareTotalHALKTV = -1;
                    int RankShareTotalKRT = -1;
                    int RankShareTotalAHABER = -1;
                    int RankShareTotalTRTHABER = -1;
                    int RankShareTotalTELE1 = -1;
                    int RankShareTotalTV100 = -1;
                    int RankShareTotalHABERGLOBAL = -1;
                    int RankShareTotalSOZCUTV = -1;

                    int RankShareABSESNTV = -1;
                    int RankShareABSESHABERTURK = -1;
                    int RankShareABSESCNNTURK = -1;
                    int RankShareABSESHALKTV = -1;
                    int RankShareABSESKRT = -1;
                    int RankShareABSESAHABER = -1;
                    int RankShareABSESTRTHABER = -1;
                    int RankShareABSESTELE1 = -1;
                    int RankShareABSESTV100 = -1;
                    int RankShareABSESHABERGLOBAL = -1;
                    int RankShareABSESSOZCUTV = -1;









                    List<decimal> RtgTotalArray = new List<decimal>();
                    List<decimal> RtgABSESArray = new List<decimal>();
                    List<decimal> ShareTotalArray = new List<decimal>();
                    List<decimal> ShareABSESArray = new List<decimal>();













                    foreach (DataTable table in result.Tables)// SHEET LOOP
                    {
                        Console.WriteLine($"Found DataTable: {table.TableName}");

                        string channelName = "";

                        Match match = Regex.Match(table.TableName, @"\((.*?)\)");

                        if (match.Success)
                        {
                            channelName = match.Groups[1].Value;
                           // Console.WriteLine(channelName);

                            string queryChannelInsert = @$"INSERT INTO Channels (channelName) Values ('{channelName}');";

                            executeQuery(queryChannelInsert);

                        }


                        // Print column names
                        //foreach (DataColumn column in table.Columns)
                        //{
                        //   // Console.Write($"{column.ColumnName}\t");
                        //}






                        string queryGetChannelId = $"SELECT id FROM Channels WHERE channelName = '{channelName}'";

                        int channelId = 0;

                        using (var connection = new SqlConnection(ConfigModel.ConnectionString))
                        {
                            try
                            {
                                connection.Open();

                                channelId = connection.QueryFirstOrDefault<int>(queryGetChannelId);

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("CONFIRM: " + ex.Message);
                            }
                            connection.Close();
                        }






                        int rowIndex = 1;
                        DateOnly date = new DateOnly();
                        var culture = System.Globalization.CultureInfo.InvariantCulture;

                        var Timebands = "";

                        var RtgIndividuals5Plus = "";
                        var RtgMale20Plus = "";
                        var RtgFemale20Plus = "";
                        var RtgIndividualsSESAB = "";
                        var RtgIndividuals20PlusABC1 = "";
                        var RtgIndividuals511 = "";

                        var ShareIndividuals5Plus = "";
                        var ShareMale20Plus = "";
                        var ShareFemale20Plus = "";
                        var ShareIndividualsSESAB = "";
                        var ShareIndividuals20PlusABC1 = "";
                        var ShareIndividuals511 = "";


                        var Rtg0Individuals5Plus = "";
                        var Rtg0Male20Plus = "";
                        var Rtg0Female20Plus = "";
                        var Rtg0IndividualsSESAB = "";
                        var Rtg0Individuals20PlusABC1 = "";
                        var Rtg0Individuals511 = "";



                        foreach (DataRow row in table.Rows) // ROW LOOP
                        {

                            if (rowIndex == 3)
                            {
                                string periodDateExcel = row.ItemArray[0].ToString();


                                DateTime dateTime;


                                string pattern = @"\d{2}\.\d{2}\.\d{4}";
                                Regex regex = new Regex(pattern);

                                Match match2 = regex.Match(periodDateExcel);

                                if (match2.Success)
                                {
                                    string periodDate = match2.Value;
                                    DateTime.TryParseExact(periodDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);


                                    date = DateOnly.FromDateTime(dateTime);
                                    formattedDate = date.ToString("yyyy-MM-dd");
                                }

                            }
                            else if (rowIndex >= 7 && rowIndex <= 103)
                            {

                                if (channelName == "TTV")
                                {
                                    ShareIndividuals5Plus = "null";
                                    ShareMale20Plus = "null";
                                    ShareFemale20Plus = "null";
                                    ShareIndividualsSESAB = "null";
                                    ShareIndividuals20PlusABC1 = "null";
                                    ShareIndividuals511 = "null";

                                    Rtg0Individuals5Plus = Convert.ToDouble(row.ItemArray[7]).ToString("#,0.#########", culture);
                                    Rtg0Male20Plus = Convert.ToDouble(row.ItemArray[8]).ToString("#,0.#########", culture);
                                    Rtg0Female20Plus = Convert.ToDouble(row.ItemArray[9]).ToString("#,0.#########", culture);
                                    Rtg0IndividualsSESAB = Convert.ToDouble(row.ItemArray[10]).ToString("#,0.#########", culture);
                                    Rtg0Individuals20PlusABC1 = Convert.ToDouble(row.ItemArray[11]).ToString("#,0.#########", culture);
                                    Rtg0Individuals511 = Convert.ToDouble(row.ItemArray[12]).ToString("#,0.#########", culture);
                                }
                                else
                                {
                                    ShareIndividuals5Plus = Convert.ToDouble(row.ItemArray[7]).ToString("#,0.#########", culture);
                                    ShareMale20Plus = Convert.ToDouble(row.ItemArray[8]).ToString("#,0.#########", culture);
                                    ShareFemale20Plus = Convert.ToDouble(row.ItemArray[9]).ToString("#,0.#########", culture);
                                    ShareIndividualsSESAB = Convert.ToDouble(row.ItemArray[10]).ToString("#,0.#########", culture);
                                    ShareIndividuals20PlusABC1 = Convert.ToDouble(row.ItemArray[11]).ToString("#,0.#########", culture);
                                    ShareIndividuals511 = Convert.ToDouble(row.ItemArray[12]).ToString("#,0.#########", culture);

                                    Rtg0Individuals5Plus = Convert.ToDouble(row.ItemArray[13]).ToString("#,0.#########", culture);
                                    Rtg0Male20Plus = Convert.ToDouble(row.ItemArray[14]).ToString("#,0.#########", culture);
                                    Rtg0Female20Plus = Convert.ToDouble(row.ItemArray[15]).ToString("#,0.#########", culture);
                                    Rtg0IndividualsSESAB = Convert.ToDouble(row.ItemArray[16]).ToString("#,0.#########", culture);
                                    Rtg0Individuals20PlusABC1 = Convert.ToDouble(row.ItemArray[17]).ToString("#,0.#########", culture);
                                    Rtg0Individuals511 = Convert.ToDouble(row.ItemArray[18]).ToString("#,0.#########", culture);

                                    ShareIndividuals5Plus = ShareIndividuals5Plus.Replace(",", "");
                                    ShareMale20Plus = ShareMale20Plus.Replace(",", "");
                                    ShareFemale20Plus = ShareFemale20Plus.Replace(",", "");
                                    ShareIndividualsSESAB = ShareIndividualsSESAB.Replace(",", "");
                                    ShareIndividuals20PlusABC1 = ShareIndividuals20PlusABC1.Replace(",", "");
                                    ShareIndividuals511 = ShareIndividuals511.Replace(",", "");
                                }


                                //RtgIndividuals5Plus         = Convert.ToDouble(row.ItemArray[1]).ToString("#,0.#########", culture);
                                //RtgMale20Plus               = Convert.ToDouble(row.ItemArray[2]).ToString("#,0.#########", culture);
                                //RtgFemale20Plus             = Convert.ToDouble(row.ItemArray[3]).ToString("#,0.#########", culture);
                                //RtgIndividualsSESAB         = Convert.ToDouble(row.ItemArray[4]).ToString("#,0.#########", culture);
                                //RtgIndividuals20PlusABC1    = Convert.ToDouble(row.ItemArray[5]).ToString("#,0.#########", culture);
                                //RtgIndividuals511           = Convert.ToDouble(row.ItemArray[6]).ToString("#,0.#########", culture);

                                //Rtg0Individuals5Plus        = Convert.ToDouble(row.ItemArray[7]).ToString("#,0.#########", culture);
                                //Rtg0Male20Plus              = Convert.ToDouble(row.ItemArray[8]).ToString("#,0.#########", culture);
                                //Rtg0Female20Plus            = Convert.ToDouble(row.ItemArray[9]).ToString("#,0.#########", culture);
                                //Rtg0IndividualsSESAB        = Convert.ToDouble(row.ItemArray[10]).ToString("#,0.#########", culture);
                                //Rtg0Individuals20PlusABC1   = Convert.ToDouble(row.ItemArray[11]).ToString("#,0.#########", culture);
                                //Rtg0Individuals511          = Convert.ToDouble(row.ItemArray[12]).ToString("#,0.#########", culture);

                                string timebandsExcel = row.ItemArray[0].ToString();

                                int indexOfSpace = timebandsExcel.IndexOf(' ');

                                string extractedValue = timebandsExcel.Substring(0, indexOfSpace);

                                Timebands = extractedValue;



                                switch (Timebands)
                                {
                                    case "24:00":
                                        Timebands = "00:00";
                                        break;

                                    case "24:15":
                                        Timebands = "00:15";
                                        break;

                                    case "24:30":
                                        Timebands = "00:30";
                                        break;

                                    case "24:45":
                                        Timebands = "00:45";
                                        break;

                                    case "25:00":
                                        Timebands = "01:00";
                                        break;

                                    case "25:15":
                                        Timebands = "01:15";
                                        break;

                                    case "25:30":
                                        Timebands = "01:30";
                                        break;

                                    case "25:45":
                                        Timebands = "01:45";
                                        break;


                                    default:
                                        break;
                                }

                                RtgIndividuals5Plus = Convert.ToDouble(row.ItemArray[1]).ToString("#,0.#########", culture);
                                RtgMale20Plus = Convert.ToDouble(row.ItemArray[2]).ToString("#,0.#########", culture);
                                RtgFemale20Plus = Convert.ToDouble(row.ItemArray[3]).ToString("#,0.#########", culture);
                                RtgIndividualsSESAB = Convert.ToDouble(row.ItemArray[4]).ToString("#,0.#########", culture);
                                RtgIndividuals20PlusABC1 = Convert.ToDouble(row.ItemArray[5]).ToString("#,0.#########", culture);
                                RtgIndividuals511 = Convert.ToDouble(row.ItemArray[6]).ToString("#,0.#########", culture);


                                RtgIndividuals5Plus = RtgIndividuals5Plus.Replace(",", "");
                                RtgMale20Plus = RtgMale20Plus.Replace(",", "");
                                RtgFemale20Plus = RtgFemale20Plus.Replace(",", "");
                                RtgIndividualsSESAB = RtgIndividualsSESAB.Replace(",", "");
                                RtgIndividuals20PlusABC1 = RtgIndividuals20PlusABC1.Replace(",", "");
                                RtgIndividuals511 = RtgIndividuals511.Replace(",", "");

                                Rtg0Individuals5Plus = Rtg0Individuals5Plus.Replace(",", "");
                                Rtg0Male20Plus = Rtg0Male20Plus.Replace(",", "");
                                Rtg0Female20Plus = Rtg0Female20Plus.Replace(",", "");
                                Rtg0IndividualsSESAB = Rtg0IndividualsSESAB.Replace(",", "");
                                Rtg0Individuals20PlusABC1 = Rtg0Individuals20PlusABC1.Replace(",", "");
                                Rtg0Individuals511 = Rtg0Individuals511.Replace(",", "");




                                string queryInsert = @$"INSERT INTO ChannelPeriods ( 
channelid
,periodDate
,Timebands
,RtgIndividuals5Plus
,RtgMale20Plus
,RtgFemale20Plus
,RtgIndividualsSESAB
,RtgIndividuals20PlusABC1
,RtgIndividuals511
,ShareIndividuals5Plus
,ShareMale20Plus
,ShareFemale20Plus
,ShareIndividualsSESAB
,ShareIndividuals20PlusABC1
,ShareIndividuals511
,Rtg0Individuals5Plus
,Rtg0Male20Plus
,Rtg0Female20Plus
,Rtg0IndividualsSESAB
,Rtg0Individuals20PlusABC1
,Rtg0Individuals511) 
VALUES
(
{channelId}
,'{formattedDate}'
,'{Timebands}'
,{RtgIndividuals5Plus}
,{RtgMale20Plus}
,{RtgFemale20Plus}
,{RtgIndividualsSESAB}
,{RtgIndividuals20PlusABC1}
,{RtgIndividuals511}
,{ShareIndividuals5Plus}
,{ShareMale20Plus}
,{ShareFemale20Plus}
,{ShareIndividualsSESAB}
,{ShareIndividuals20PlusABC1}
,{RtgIndividuals511}
,{Rtg0Individuals5Plus}
,{Rtg0Male20Plus}
,{Rtg0Female20Plus}
,{Rtg0IndividualsSESAB}
,{Rtg0Individuals20PlusABC1}
,{Rtg0Individuals511}

)";
                                 executeQuery(queryInsert);




                                if (Timebands == "Total" && Array.Exists(channelsArray, element => element == channelName))
                                {


                                    switch (channelName)
                                    {

                                        case "NTV":
                                            RtgTotalNTV = RtgIndividuals5Plus;
                                            RtgABSESNTV = RtgIndividualsSESAB;
                                            ShareTotalNTV = ShareIndividuals5Plus;
                                            ShareABSESNTV = ShareIndividualsSESAB;

                                            break;

                                        case "HABER TURK":
                                            RtgTotalHABERTURK = RtgIndividuals5Plus;
                                            RtgABSESHABERTURK = RtgIndividualsSESAB;
                                            ShareTotalHABERTURK = ShareIndividuals5Plus;
                                            ShareABSESHABERTURK = ShareIndividualsSESAB;

                                            break;

                                        case "CNN TURK":
                                            RtgTotalCNNTURK = RtgIndividuals5Plus;
                                            RtgABSESCNNTURK = RtgIndividualsSESAB;
                                            ShareTotalCNNTURK = ShareIndividuals5Plus;
                                            ShareABSESCNNTURK = ShareIndividualsSESAB;
                                            break;

                                        case "HALK TV":
                                            RtgTotalHALKTV = RtgIndividuals5Plus;
                                            RtgABSESHALKTV = RtgIndividualsSESAB;
                                            ShareTotalHALKTV = ShareIndividuals5Plus;
                                            ShareABSESHALKTV = ShareIndividualsSESAB;
                                            break;

                                        case "KRT TV":
                                            RtgTotalKRT = RtgIndividuals5Plus;
                                            RtgABSESKRT = RtgIndividualsSESAB;
                                            ShareTotalKRT = ShareIndividuals5Plus;
                                            ShareABSESKRT = ShareIndividualsSESAB;

                                            break;

                                        case "AHABER":
                                            RtgTotalAHABER = RtgIndividuals5Plus;
                                            RtgABSESAHABER = RtgIndividualsSESAB;
                                            ShareTotalAHABER = ShareIndividuals5Plus;
                                            ShareABSESAHABER = ShareIndividualsSESAB;
                                            break;

                                        case "TRT HABER":
                                            RtgTotalTRTHABER = RtgIndividuals5Plus;
                                            RtgABSESTRTHABER = RtgIndividualsSESAB;
                                            ShareTotalTRTHABER = ShareIndividuals5Plus;
                                            ShareABSESTRTHABER = ShareIndividualsSESAB;
                                            break;

                                        case "TELE1":
                                            RtgTotalTELE1 = RtgIndividuals5Plus;
                                            RtgABSESTELE1 = RtgIndividualsSESAB;
                                            ShareTotalTELE1 = ShareIndividuals5Plus;
                                            ShareABSESTELE1 = ShareIndividualsSESAB;
                                            break;

                                        case "TV100":
                                            RtgTotalTV100 = RtgIndividuals5Plus;
                                            RtgABSESTV100 = RtgIndividualsSESAB;
                                            ShareTotalTV100 = ShareIndividuals5Plus;
                                            ShareABSESTV100 = ShareIndividualsSESAB;
                                            break;

                                        case "HABER GLOBAL":
                                            RtgTotalHABERGLOBAL = RtgIndividuals5Plus;
                                            RtgABSESHABERGLOBAL = RtgIndividualsSESAB;
                                            ShareTotalHABERGLOBAL = ShareIndividuals5Plus;
                                            ShareABSESHABERGLOBAL = ShareIndividualsSESAB;
                                            break;

                                        case "SOZCU TV":
                                            RtgTotalSOZCUTV = RtgIndividuals5Plus;
                                            RtgABSESSOZCUTV = RtgIndividualsSESAB;
                                            ShareTotalSOZCUTV = ShareIndividuals5Plus;
                                            ShareABSESSOZCUTV = ShareIndividualsSESAB;
                                            break;

                                        default:
                                            break;
                                    }



                                    decimal decimalRtgTotal = decimal.Parse(RtgIndividuals5Plus, CultureInfo.InvariantCulture);
                                    decimal decimalRtgABSES = decimal.Parse(RtgIndividualsSESAB, CultureInfo.InvariantCulture);
                                    decimal decimalShareTotal = decimal.Parse(ShareIndividuals5Plus, CultureInfo.InvariantCulture);
                                    decimal decimalShareABSES = decimal.Parse(ShareIndividualsSESAB, CultureInfo.InvariantCulture);


                                    RtgTotalArray.Add(decimalRtgTotal);
                                    RtgABSESArray.Add(decimalRtgABSES);
                                    ShareTotalArray.Add(decimalShareTotal);
                                    ShareABSESArray.Add(decimalShareABSES);


                                }





                            }


                            rowIndex++;


                        }

                    }




                    //foreach (var item in tuples1)
                    //{
                    //    Console.Write(item.Value + " ");

                    //    Console.Write(item.NewIndex + "\n");
                    //}


                    // Create an array of tuples to store each decimal value with its corresponding rank
                    var tuples1 = RtgTotalArray.Select((x, i) => (Value: x, OldIndex: i, NewIndex: -1))
                                         .OrderByDescending(x => x.Value)
                                         .Select((x, i) => (x.Value, x.OldIndex, NewIndex: i + 1))
                                         .OrderBy(x => x.OldIndex)
                                         .ToArray();

                    var tuples2 = RtgABSESArray.Select((x, i) => (Value: x, OldIndex: i, NewIndex: -1))
                                         .OrderByDescending(x => x.Value)
                                         .Select((x, i) => (x.Value, x.OldIndex, NewIndex: i + 1))
                                         .OrderBy(x => x.OldIndex)
                                         .ToArray();

                    var tuples3 = ShareTotalArray.Select((x, i) => (Value: x, OldIndex: i, NewIndex: -1))
                                         .OrderByDescending(x => x.Value)
                                         .Select((x, i) => (x.Value, x.OldIndex, NewIndex: i + 1))
                                         .OrderBy(x => x.OldIndex)
                                         .ToArray();

                    var tuples4 = ShareABSESArray.Select((x, i) => (Value: x, OldIndex: i, NewIndex: -1))
                                         .OrderByDescending(x => x.Value)
                                         .Select((x, i) => (x.Value, x.OldIndex, NewIndex: i + 1))
                                         .OrderBy(x => x.OldIndex)
                                         .ToArray();


                    RankRtgTotalNTV           = tuples1[0].NewIndex;
                    RankRtgTotalHABERTURK     = tuples1[1].NewIndex;
                    RankRtgTotalCNNTURK       = tuples1[2].NewIndex;
                    RankRtgTotalHALKTV        = tuples1[3].NewIndex;
                    RankRtgTotalKRT           = tuples1[8].NewIndex;
                    RankRtgTotalAHABER        = tuples1[4].NewIndex;
                    RankRtgTotalTRTHABER      = tuples1[5].NewIndex;
                    RankRtgTotalTELE1         = tuples1[6].NewIndex;
                    RankRtgTotalTV100         = tuples1[7].NewIndex;
                    RankRtgTotalHABERGLOBAL   = tuples1[9].NewIndex;
                    RankRtgTotalSOZCUTV       = tuples1[10].NewIndex;

                    RankRtgABSESNTV           = tuples2[0].NewIndex;
                    RankRtgABSESHABERTURK     = tuples2[1].NewIndex;
                    RankRtgABSESCNNTURK       = tuples2[2].NewIndex;
                    RankRtgABSESHALKTV        = tuples2[3].NewIndex;
                    RankRtgABSESKRT           = tuples2[8].NewIndex;
                    RankRtgABSESAHABER        = tuples2[4].NewIndex;
                    RankRtgABSESTRTHABER      = tuples2[5].NewIndex;
                    RankRtgABSESTELE1         = tuples2[6].NewIndex;
                    RankRtgABSESTV100         = tuples2[7].NewIndex;
                    RankRtgABSESHABERGLOBAL   = tuples2[9].NewIndex;
                    RankRtgABSESSOZCUTV       = tuples2[10].NewIndex;

                    RankShareTotalNTV         = tuples3[0].NewIndex;
                    RankShareTotalHABERTURK   = tuples3[1].NewIndex;
                    RankShareTotalCNNTURK     = tuples3[2].NewIndex;
                    RankShareTotalHALKTV      = tuples3[3].NewIndex;
                    RankShareTotalKRT         = tuples3[8].NewIndex;
                    RankShareTotalAHABER      = tuples3[4].NewIndex;
                    RankShareTotalTRTHABER    = tuples3[5].NewIndex;
                    RankShareTotalTELE1       = tuples3[6].NewIndex;
                    RankShareTotalTV100       = tuples3[7].NewIndex;
                    RankShareTotalHABERGLOBAL = tuples3[9].NewIndex;
                    RankShareTotalSOZCUTV     = tuples3[10].NewIndex;

                    RankShareABSESNTV         = tuples4[0].NewIndex;
                    RankShareABSESHABERTURK   = tuples4[1].NewIndex;
                    RankShareABSESCNNTURK     = tuples4[2].NewIndex;
                    RankShareABSESHALKTV      = tuples4[3].NewIndex;
                    RankShareABSESKRT         = tuples4[8].NewIndex;
                    RankShareABSESAHABER      = tuples4[4].NewIndex;
                    RankShareABSESTRTHABER    = tuples4[5].NewIndex;
                    RankShareABSESTELE1       = tuples4[6].NewIndex;
                    RankShareABSESTV100       = tuples4[7].NewIndex;
                    RankShareABSESHABERGLOBAL = tuples4[9].NewIndex;
                    RankShareABSESSOZCUTV     = tuples4[10].NewIndex;




                    string queryTotal = @$"INSERT INTO TotalDay ( 
tableDate 
,tableType 
,TotalNTV  
,TotalHABERTURK  	
,TotalCNNTURK  	
,TotalHALKTV  	
,TotalKRT      	
,TotalAHABER   	
,TotalTRTHABER     	
,TotalTELE1    	
,TotalTV100    	
,TotalHABERGLOBAL  	
,TotalSOZCUTV  	
,ABSESNTV  
,ABSESHABERTURK  	
,ABSESCNNTURK  	
,ABSESHALKTV  	
,ABSESKRT      	
,ABSESAHABER   	
,ABSESTRTHABER     	
,ABSESTELE1    	
,ABSESTV100    	
,ABSESHABERGLOBAL  	
,ABSESSOZCUTV  	
,RankTotalNTV   
,RankTotalHABERTURK   
,RankTotalCNNTURK   
,RankTotalHALKTV   
,RankTotalKRT       
,RankTotalAHABER    
,RankTotalTRTHABER      
,RankTotalTELE1     
,RankTotalTV100     
,RankTotalHABERGLOBAL   
,RankTotalSOZCUTV   
,RankABSESNTV   
,RankABSESHABERTURK   
,RankABSESCNNTURK   
,RankABSESHALKTV   
,RankABSESKRT       
,RankABSESAHABER    
,RankABSESTRTHABER      
,RankABSESTELE1     
,RankABSESTV100     
,RankABSESHABERGLOBAL   
,RankABSESSOZCUTV  
) 
VALUES
(
@ValuetableDate 
,@ValuetableType 
,@ValueTotalNTV  
,@ValueTotalHABERTURK  	
,@ValueTotalCNNTURK  	
,@ValueTotalHALKTV  	
,@ValueTotalKRT      	
,@ValueTotalAHABER   	
,@ValueTotalTRTHABER     	
,@ValueTotalTELE1    	
,@ValueTotalTV100    	
,@ValueTotalHABERGLOBAL  	
,@ValueTotalSOZCUTV  
,@ValueABSESNTV  
,@ValueABSESHABERTURK  	
,@ValueABSESCNNTURK  	
,@ValueABSESHALKTV  	
,@ValueABSESKRT      	
,@ValueABSESAHABER   	
,@ValueABSESTRTHABER     	
,@ValueABSESTELE1    	
,@ValueABSESTV100    	
,@ValueABSESHABERGLOBAL  	
,@ValueABSESSOZCUTV  	
,@ValueRankTotalNTV   
,@ValueRankTotalHABERTURK   
,@ValueRankTotalCNNTURK   
,@ValueRankTotalHALKTV   
,@ValueRankTotalKRT       
,@ValueRankTotalAHABER    
,@ValueRankTotalTRTHABER      
,@ValueRankTotalTELE1     
,@ValueRankTotalTV100     
,@ValueRankTotalHABERGLOBAL   
,@ValueRankTotalSOZCUTV   
,@ValueRankABSESNTV   
,@ValueRankABSESHABERTURK   
,@ValueRankABSESCNNTURK   
,@ValueRankABSESHALKTV   
,@ValueRankABSESKRT       
,@ValueRankABSESAHABER    
,@ValueRankABSESTRTHABER      
,@ValueRankABSESTELE1     
,@ValueRankABSESTV100     
,@ValueRankABSESHABERGLOBAL   
,@ValueRankABSESSOZCUTV  
)";

                    using (var connection = new SqlConnection(ConfigModel.ConnectionString))
                    {
                        try
                        {
                            connection.Open();

                            SqlCommand commandTotal = new SqlCommand(queryTotal, connection);


                            commandTotal.Parameters.AddWithValue("@ValuetableDate", formattedDate);


                            commandTotal.Parameters.AddWithValue("@ValuetableType", "RTG");

                            commandTotal.Parameters.AddWithValue("@ValueTotalNTV", RtgTotalNTV);
                            commandTotal.Parameters.AddWithValue("@ValueABSESNTV", RtgABSESNTV);
                            commandTotal.Parameters.AddWithValue("@ValueTotalHABERTURK", RtgTotalHABERTURK);
                            commandTotal.Parameters.AddWithValue("@ValueABSESHABERTURK", RtgABSESHABERTURK);
                            commandTotal.Parameters.AddWithValue("@ValueTotalCNNTURK", RtgTotalCNNTURK);
                            commandTotal.Parameters.AddWithValue("@ValueABSESCNNTURK", RtgABSESCNNTURK);
                            commandTotal.Parameters.AddWithValue("@ValueTotalHALKTV", RtgTotalHALKTV);
                            commandTotal.Parameters.AddWithValue("@ValueABSESHALKTV", RtgABSESHALKTV);
                            commandTotal.Parameters.AddWithValue("@ValueTotalKRT", RtgTotalKRT);
                            commandTotal.Parameters.AddWithValue("@ValueABSESKRT", RtgABSESKRT);
                            commandTotal.Parameters.AddWithValue("@ValueTotalAHABER", RtgTotalAHABER);
                            commandTotal.Parameters.AddWithValue("@ValueABSESAHABER", RtgABSESAHABER);
                            commandTotal.Parameters.AddWithValue("@ValueTotalTRTHABER", RtgTotalTRTHABER);
                            commandTotal.Parameters.AddWithValue("@ValueABSESTRTHABER", RtgABSESTRTHABER);
                            commandTotal.Parameters.AddWithValue("@ValueTotalTELE1", RtgTotalTELE1);
                            commandTotal.Parameters.AddWithValue("@ValueABSESTELE1", RtgABSESTELE1);
                            commandTotal.Parameters.AddWithValue("@ValueTotalTV100", RtgTotalTV100);
                            commandTotal.Parameters.AddWithValue("@ValueABSESTV100", RtgABSESTV100);
                            commandTotal.Parameters.AddWithValue("@ValueTotalHABERGLOBAL", RtgTotalHABERGLOBAL);
                            commandTotal.Parameters.AddWithValue("@ValueABSESHABERGLOBAL", RtgABSESHABERGLOBAL);
                            commandTotal.Parameters.AddWithValue("@ValueTotalSOZCUTV", RtgTotalSOZCUTV);
                            commandTotal.Parameters.AddWithValue("@ValueABSESSOZCUTV", RtgABSESSOZCUTV);

                            commandTotal.Parameters.AddWithValue("@ValueRankTotalNTV", RankRtgTotalNTV);
                            commandTotal.Parameters.AddWithValue("@ValueRankABSESNTV", RankRtgABSESNTV);
                            commandTotal.Parameters.AddWithValue("@ValueRankTotalHABERTURK", RankRtgTotalHABERTURK);
                            commandTotal.Parameters.AddWithValue("@ValueRankABSESHABERTURK", RankRtgABSESHABERTURK);
                            commandTotal.Parameters.AddWithValue("@ValueRankTotalCNNTURK", RankRtgTotalCNNTURK);
                            commandTotal.Parameters.AddWithValue("@ValueRankABSESCNNTURK", RankRtgABSESCNNTURK);
                            commandTotal.Parameters.AddWithValue("@ValueRankTotalHALKTV", RankRtgTotalHALKTV);
                            commandTotal.Parameters.AddWithValue("@ValueRankABSESHALKTV", RankRtgABSESHALKTV);
                            commandTotal.Parameters.AddWithValue("@ValueRankTotalKRT", RankRtgTotalKRT);
                            commandTotal.Parameters.AddWithValue("@ValueRankABSESKRT", RankRtgABSESKRT);
                            commandTotal.Parameters.AddWithValue("@ValueRankTotalAHABER", RankRtgTotalAHABER);
                            commandTotal.Parameters.AddWithValue("@ValueRankABSESAHABER", RankRtgABSESAHABER);
                            commandTotal.Parameters.AddWithValue("@ValueRankTotalTRTHABER", RankRtgTotalTRTHABER);
                            commandTotal.Parameters.AddWithValue("@ValueRankABSESTRTHABER", RankRtgABSESTRTHABER);
                            commandTotal.Parameters.AddWithValue("@ValueRankTotalTELE1", RankRtgTotalTELE1);
                            commandTotal.Parameters.AddWithValue("@ValueRankABSESTELE1", RankRtgABSESTELE1);
                            commandTotal.Parameters.AddWithValue("@ValueRankTotalTV100", RankRtgTotalTV100);
                            commandTotal.Parameters.AddWithValue("@ValueRankABSESTV100", RankRtgABSESTV100);
                            commandTotal.Parameters.AddWithValue("@ValueRankTotalHABERGLOBAL", RankRtgTotalHABERGLOBAL);
                            commandTotal.Parameters.AddWithValue("@ValueRankABSESHABERGLOBAL", RankRtgABSESHABERGLOBAL);
                            commandTotal.Parameters.AddWithValue("@ValueRankTotalSOZCUTV", RankRtgTotalSOZCUTV);
                            commandTotal.Parameters.AddWithValue("@ValueRankABSESSOZCUTV", RankRtgABSESSOZCUTV);



                            commandTotal.ExecuteNonQuery();



                            commandTotal.Parameters["@ValuetableType"].Value = "SHARE";


                            commandTotal.Parameters["@ValueTotalNTV"].Value                = ShareTotalNTV;
                            commandTotal.Parameters["@ValueABSESNTV"].Value                = ShareABSESNTV;
                            commandTotal.Parameters["@ValueTotalHABERTURK"].Value          = ShareTotalHABERTURK;
                            commandTotal.Parameters["@ValueABSESHABERTURK"].Value          = ShareABSESHABERTURK;
                            commandTotal.Parameters["@ValueTotalCNNTURK"].Value            = ShareTotalCNNTURK;
                            commandTotal.Parameters["@ValueABSESCNNTURK"].Value            = ShareABSESCNNTURK;
                            commandTotal.Parameters["@ValueTotalHALKTV"].Value             = ShareTotalHALKTV;
                            commandTotal.Parameters["@ValueABSESHALKTV"].Value             = ShareABSESHALKTV;
                            commandTotal.Parameters["@ValueTotalKRT"].Value                = ShareTotalKRT;
                            commandTotal.Parameters["@ValueABSESKRT"].Value                = ShareABSESKRT;
                            commandTotal.Parameters["@ValueTotalAHABER"].Value             = ShareTotalAHABER;
                            commandTotal.Parameters["@ValueABSESAHABER"].Value             = ShareABSESAHABER;
                            commandTotal.Parameters["@ValueTotalTRTHABER"].Value           = ShareTotalTRTHABER;
                            commandTotal.Parameters["@ValueABSESTRTHABER"].Value           = ShareABSESTRTHABER;
                            commandTotal.Parameters["@ValueTotalTELE1"].Value              = ShareTotalTELE1;
                            commandTotal.Parameters["@ValueABSESTELE1"].Value              = ShareABSESTELE1;
                            commandTotal.Parameters["@ValueTotalTV100"].Value              = ShareTotalTV100;
                            commandTotal.Parameters["@ValueABSESTV100"].Value              = ShareABSESTV100;
                            commandTotal.Parameters["@ValueTotalHABERGLOBAL"].Value        = ShareTotalHABERGLOBAL;
                            commandTotal.Parameters["@ValueABSESHABERGLOBAL"].Value        = ShareABSESHABERGLOBAL;
                            commandTotal.Parameters["@ValueTotalSOZCUTV"].Value            = ShareTotalSOZCUTV;
                            commandTotal.Parameters["@ValueABSESSOZCUTV"].Value            = ShareABSESSOZCUTV;
                                                   
                            commandTotal.Parameters["@ValueRankTotalNTV"].Value            = RankShareTotalNTV;
                            commandTotal.Parameters["@ValueRankABSESNTV"].Value            = RankShareABSESNTV;
                            commandTotal.Parameters["@ValueRankTotalHABERTURK"].Value      = RankShareTotalHABERTURK;
                            commandTotal.Parameters["@ValueRankABSESHABERTURK"].Value      = RankShareABSESHABERTURK;
                            commandTotal.Parameters["@ValueRankTotalCNNTURK"].Value        = RankShareTotalCNNTURK;
                            commandTotal.Parameters["@ValueRankABSESCNNTURK"].Value        = RankShareABSESCNNTURK;
                            commandTotal.Parameters["@ValueRankTotalHALKTV"].Value         = RankShareTotalHALKTV;
                            commandTotal.Parameters["@ValueRankABSESHALKTV"].Value         = RankShareABSESHALKTV;
                            commandTotal.Parameters["@ValueRankTotalKRT"].Value            = RankShareTotalKRT;
                            commandTotal.Parameters["@ValueRankABSESKRT"].Value            = RankShareABSESKRT;
                            commandTotal.Parameters["@ValueRankTotalAHABER"].Value         = RankShareTotalAHABER;
                            commandTotal.Parameters["@ValueRankABSESAHABER"].Value         = RankShareABSESAHABER;
                            commandTotal.Parameters["@ValueRankTotalTRTHABER"].Value       = RankShareTotalTRTHABER;
                            commandTotal.Parameters["@ValueRankABSESTRTHABER"].Value       = RankShareABSESTRTHABER;
                            commandTotal.Parameters["@ValueRankTotalTELE1"].Value          = RankShareTotalTELE1;
                            commandTotal.Parameters["@ValueRankABSESTELE1"].Value          = RankShareABSESTELE1;
                            commandTotal.Parameters["@ValueRankTotalTV100"].Value          = RankShareTotalTV100;
                            commandTotal.Parameters["@ValueRankABSESTV100"].Value          = RankShareABSESTV100;
                            commandTotal.Parameters["@ValueRankTotalHABERGLOBAL"].Value    = RankShareTotalHABERGLOBAL;
                            commandTotal.Parameters["@ValueRankABSESHABERGLOBAL"].Value    = RankShareABSESHABERGLOBAL;
                            commandTotal.Parameters["@ValueRankTotalSOZCUTV"].Value        = RankShareTotalSOZCUTV;
                            commandTotal.Parameters["@ValueRankABSESSOZCUTV"].Value        = RankShareABSESSOZCUTV;


                            commandTotal.ExecuteNonQuery();


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                        connection.Close();




                    }
                    //using (IDbConnection db = new SqlConnection(ConfigModel.ConnectionString))
                    //{
                    //    db.Execute($"INSERT INTO CheckInsert (insertedDate,insertValue) VALUES ('{formattedDate}',1)");

                    //}
                }
            }

            string destinationFile = Directory.GetCurrentDirectory() + @"\Completed\" + fileName2;

            try
            {
                File.Move(fileName, destinationFile);

            }
            catch (Exception)
            {
                File.Delete(fileName);
            }

        }


        



        private void readRtgExcel(string fileName, string fileName2)
        {


            string formattedDate = "";



            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {


                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = false // Use the first row as column names
                        }
                    });




                    int rowIndex = 1;
                    DateOnly date = new DateOnly();
                    var culture = System.Globalization.CultureInfo.InvariantCulture;





                    foreach (DataTable table in result.Tables)// Sheet loop
                    {
                        Console.WriteLine($"Found DataTable: {table.TableName}");

                        int queryResult = 0;

                        var Timebands = "";

                        var programNTV = "";
                        var programHABERTURK = "";
                        var programCNNTURK = "";





                        foreach (DataRow row in table.Rows)
                        {
                            if (rowIndex == 1)
                            {
                                string tableDate = row.ItemArray[0].ToString();

                                DateTime dateTime = DateTime.ParseExact(tableDate, "d.M.yyyy HH:mm:ss", null);

                                DateOnly dateOnly = DateOnly.FromDateTime(dateTime);

                                string datePart = dateOnly.ToString("dd.MM.yyyy");

                                formattedDate = dateOnly.ToString("yyyy-MM-dd");





                                string queryToCheckDatabase = $"SELECT id FROM OutputTable WHERE tableDate = '{formattedDate}'";



                                using (var connection = new SqlConnection(ConfigModel.ConnectionString))
                                {
                                    try
                                    {
                                        connection.Open();

                                        queryResult = connection.QueryFirstOrDefault<int>(queryToCheckDatabase);

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error: " + ex.Message);
                                    }
                                    connection.Close();
                                }
                            }




                            if (queryResult != 0)
                            {
                                break;
                            }
                            else if (queryResult == 0 && rowIndex >= 4 && rowIndex < 100)
                            {




                                var timeBandsExcel = row.ItemArray[0].ToString();


                                if (row.ItemArray[1].ToString() != "")
                                {
                                    programNTV = row.ItemArray[1].ToString();

                                }

                                if (row.ItemArray[2].ToString() != "")
                                {
                                    programHABERTURK = row.ItemArray[2].ToString();

                                }

                                if (row.ItemArray[3].ToString() != "")
                                {
                                    programCNNTURK = row.ItemArray[3].ToString();

                                }



                                int spaceIndex = timeBandsExcel.IndexOf(' ');

                                Timebands = timeBandsExcel.Substring(spaceIndex + 1, 5); // 5 characters from the spaceIndex



                                string queryRtg = @$"INSERT INTO OutputTable (
tableDate
,tableType
,timebands
,programNTV
,programHABERTURK
,programCNNTURK) 
VALUES 
(@Value1
,@Value2
,@Value3
,@Value4
,@Value5
,@Value6)";

                                string queryShare = @$"INSERT INTO OutputTable (
tableDate
,tableType
,timebands
,programNTV
,programHABERTURK
,programCNNTURK) 
VALUES 
(@Value1
,@Value2
,@Value3
,@Value4
,@Value5
,@Value6)";








                                using (var connection = new SqlConnection(ConfigModel.ConnectionString))
                                {
                                    try
                                    {
                                        connection.Open();

                                        SqlCommand commandRtg = new SqlCommand(queryRtg, connection);
                                        SqlCommand commandShare = new SqlCommand(queryShare, connection);


                                        commandRtg.Parameters.AddWithValue("@Value1", formattedDate);
                                        commandRtg.Parameters.AddWithValue("@Value2", "RTG");
                                        commandRtg.Parameters.AddWithValue("@Value3", Timebands);
                                        commandRtg.Parameters.AddWithValue("@Value4", programNTV);
                                        commandRtg.Parameters.AddWithValue("@Value5", programHABERTURK);
                                        commandRtg.Parameters.AddWithValue("@Value6", programCNNTURK);

                                        commandShare.Parameters.AddWithValue("@Value1", formattedDate);
                                        commandShare.Parameters.AddWithValue("@Value2", "SHARE");
                                        commandShare.Parameters.AddWithValue("@Value3", Timebands);
                                        commandShare.Parameters.AddWithValue("@Value4", programNTV);
                                        commandShare.Parameters.AddWithValue("@Value5", programHABERTURK);
                                        commandShare.Parameters.AddWithValue("@Value6", programCNNTURK);



                                        commandRtg.ExecuteNonQuery();
                                        commandShare.ExecuteNonQuery();


                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error: " + ex.Message);
                                    }
                                    connection.Close();
                                }






                            }








                            rowIndex++;





                        }
                        break;

                    }
                }
            }

            //string destinationFile = Directory.GetCurrentDirectory() + @"\Completed\" + fileName2;

            //File.Move(fileName, destinationFile);

            using (var connection = new SqlConnection(ConfigModel.ConnectionString))
            {
                try
                {
                    connection.Open();

                    connection.Execute("JoinChannelPeriodsOutputTable", new { date = formattedDate }, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                connection.Close();
            }


            exportToExcel(formattedDate);
        }

        



        public void executeQuery(string queryStr)
        {


            using (var connection = new SqlConnection(ConfigModel.ConnectionString))
            {
                try
                {
                    connection.Open();

                    connection.Execute(queryStr);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                connection.Close();
            }
        }

        //public Color convertToRgb(ExcelDataModel obj,string channel)
        //{
        //    string[] rgbParts = obj.ColorTotalNTV.Replace("rgb(", "").Replace(")", "").Split(',');
        //    int r = int.Parse(rgbParts[0]);
        //    int g = int.Parse(rgbParts[1]);
        //    int b = int.Parse(rgbParts[2]);

        //    return
        //}
        public Color convertToRgb(ExcelDataModel obj, string channel)
        {
            string colorValue = typeof(ExcelDataModel).GetProperty(channel).GetValue(obj, null).ToString();
            string[] rgbParts = colorValue.Replace("rgb(", "").Replace(")", "").Split(',');
            int r = int.Parse(rgbParts[0]);
            int g = int.Parse(rgbParts[1]);
            int b = int.Parse(rgbParts[2]);

            return Color.FromArgb(r, g, b);
        }


        public void exportToExcel(string dateString)
        {
            //string ApiBaseUrl = "https://localhost:7238/RatingSystem/";

            DateTime date5 = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string dateStringConverted = date5.ToString("dd-MM-yyyy");

            string endpointLogin = "Login";
            string apiUrlLogin = $"{ConfigModel.ApiUrl}{endpointLogin}";


            var clientLogin = new RestClient(apiUrlLogin);
            var requestLogin = new RestRequest(apiUrlLogin, Method.Get);

            requestLogin.AddParameter("username", "consoleApp"); // Assuming date is the value you want to pass
            requestLogin.AddParameter("password", "ksR27,iQ3?8OhcgFt5342/Y&mB6c"); // Assuming date is the value you want to pass


            RestResponse responseLogin = clientLogin.Execute(requestLogin);

            //var jwtToken = JsonConvert.DeserializeObject(responseLogin.Content);
            var jwtToken = JsonConvert.DeserializeObject<string>(JsonConvert.DeserializeObject<string>(responseLogin.Content));







            string endpoint = "GetExcelData";
            string apiUrl = $"{ConfigModel.ApiUrl}{endpoint}";

            var client = new RestClient(apiUrl);
            var request = new RestRequest(apiUrl, Method.Get);

            request.AddParameter("inputDate", dateStringConverted); // Assuming date is the value you want to pass

            request.AddHeader("Authorization", "Bearer " + jwtToken);

            RestResponse responseProject = client.Execute(request);

            var responseList = JsonConvert.DeserializeObject<ResponseModel>(responseProject.Content);



            List<ExcelDataModel> listRtg = responseList.propertyName1;
            List<ExcelDataModel> listShare = responseList.propertyName2;
            List<ExcelDataModel> listAvg = responseList.propertyName3;


            
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;



           // DateTime date = DateTime.ParseExact(date5, "dd-MM-yyyy", null);

            int monthNumber = date5.Month;

            string onlyDayString = dateStringConverted.Substring(0, 2);
            string onlyMonthString = dateStringConverted.Substring(3, 2);
            string onlyYearString = dateStringConverted.Substring(6, 4);

            string monthString = "";


            switch (monthNumber)
            {
                case 1:
                    monthString = "OCAK";
                    break;
                case 2:
                    monthString = "SUBAT";
                    break;
                case 3:
                    monthString = "MART";
                    break;
                case 4:
                    monthString = "NISAN";
                    break;
                case 5:
                    monthString = "MAYIS";
                    break;
                case 6:
                    monthString = "HAZIRAN";
                    break;
                case 7:
                    monthString = "TEMMUZ";
                    break;
                case 8:
                    monthString = "AGUSTOS";
                    break;
                case 9:
                    monthString = "EYLUL";
                    break;
                case 10:
                    monthString = "EKIM";
                    break;
                case 11:
                    monthString = "KASIM";
                    break;
                case 12:
                    monthString = "ARALIK";
                    break;
                default:
                    break;
            }


            string sheetName = date5.Day.ToString() + " " + monthString;

            string sheetName2 = sheetName + " ";

            //string sheetNameAvg = monthString + "'" + date.Year.ToString().Substring(2);

            //string directoryPath = Directory.GetCurrentDirectory() + @"\Income\SUBAT_Rtg_11li.xlsx";

            string directoryPathRtg = Directory.GetCurrentDirectory() + @"\Income\" + monthString + "_Rtg_11li.xlsx";


            if (File.Exists(directoryPathRtg))
            {
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(directoryPathRtg)))
                {
                    //ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                    //ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.SingleOrDefault(sheet => sheet.Name == "19 SUBAT ");
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.SingleOrDefault(sheet => sheet.Name.Contains(sheetName));

                    if (worksheet == null)
                    {
                        worksheet = excelPackage.Workbook.Worksheets.SingleOrDefault(sheet => sheet.Name == sheetName2);

                    }
                    int startRow = 4;

                    foreach (var obj in listRtg)
                    {
                        if (startRow == 101)
                        {
                            startRow = 102;
                        }
                        worksheet.Cells[startRow, 5].Value = obj.TotalNTV;
                        worksheet.Cells[startRow, 6].Value = obj.TotalHABERTURK;
                        worksheet.Cells[startRow, 7].Value = obj.TotalCNNTURK;
                        worksheet.Cells[startRow, 8].Value = obj.TotalHALKTV;
                        worksheet.Cells[startRow, 9].Value = obj.TotalKRT;
                        worksheet.Cells[startRow, 10].Value = obj.TotalAHABER;
                        worksheet.Cells[startRow, 11].Value = obj.TotalTRTHABER;
                        worksheet.Cells[startRow, 12].Value = obj.TotalTELE1;
                        worksheet.Cells[startRow, 13].Value = obj.TotalTV100;
                        worksheet.Cells[startRow, 14].Value = obj.TotalHABERGLOBAL;
                        worksheet.Cells[startRow, 15].Value = obj.TotalSOZCUTV;

                        worksheet.Cells[startRow, 17].Value = obj.ABSESNTV;
                        worksheet.Cells[startRow, 18].Value = obj.ABSESHABERTURK;
                        worksheet.Cells[startRow, 19].Value = obj.ABSESCNNTURK;
                        worksheet.Cells[startRow, 20].Value = obj.ABSESHALKTV;
                        worksheet.Cells[startRow, 21].Value = obj.ABSESKRT;
                        worksheet.Cells[startRow, 22].Value = obj.ABSESAHABER;
                        worksheet.Cells[startRow, 23].Value = obj.ABSESTRTHABER;
                        worksheet.Cells[startRow, 24].Value = obj.ABSESTELE1;
                        worksheet.Cells[startRow, 25].Value = obj.ABSESTV100;
                        worksheet.Cells[startRow, 26].Value = obj.ABSESHABERGLOBAL;
                        worksheet.Cells[startRow, 27].Value = obj.ABSESSOZCUTV;




                        Color ColorTotalNTV         = convertToRgb(obj, "ColorTotalNTV");
                        Color ColorTotalHABERTURK   = convertToRgb(obj, "ColorTotalHABERTURK");
                        Color ColorTotalCNNTURK     = convertToRgb(obj, "ColorTotalCNNTURK");
                        Color ColorTotalHALKTV      = convertToRgb(obj, "ColorTotalHALKTV");
                        Color ColorTotalKRT         = convertToRgb(obj, "ColorTotalKRT");
                        Color ColorTotalAHABER      = convertToRgb(obj, "ColorTotalAHABER");
                        Color ColorTotalTRTHABER    = convertToRgb(obj, "ColorTotalTRTHABER");
                        Color ColorTotalTELE1       = convertToRgb(obj, "ColorTotalTELE1");
                        Color ColorTotalTV100       = convertToRgb(obj, "ColorTotalTV100");
                        Color ColorTotalHABERGLOBAL = convertToRgb(obj, "ColorTotalHABERGLOBAL");
                        Color ColorTotalSOZCUTV     = convertToRgb(obj, "ColorTotalSOZCUTV");

                        Color ColorABSESNTV         = convertToRgb(obj, "ColorABSESNTV")          ;
                        Color ColorABSESHABERTURK   = convertToRgb(obj, "ColorABSESHABERTURK")    ;
                        Color ColorABSESCNNTURK     = convertToRgb(obj, "ColorABSESCNNTURK")      ;
                        Color ColorABSESHALKTV      = convertToRgb(obj, "ColorABSESHALKTV")       ;
                        Color ColorABSESKRT         = convertToRgb(obj, "ColorABSESKRT")          ;
                        Color ColorABSESAHABER      = convertToRgb(obj, "ColorABSESAHABER")       ;
                        Color ColorABSESTRTHABER    = convertToRgb(obj, "ColorABSESTRTHABER")     ;
                        Color ColorABSESTELE1       = convertToRgb(obj, "ColorABSESTELE1")        ;
                        Color ColorABSESTV100       = convertToRgb(obj, "ColorABSESTV100")        ;
                        Color ColorABSESHABERGLOBAL = convertToRgb(obj, "ColorABSESHABERGLOBAL")  ;
                        Color ColorABSESSOZCUTV     = convertToRgb(obj, "ColorABSESSOZCUTV");





                        worksheet.Cells[startRow, 5].Style.Font.Color.SetColor(ColorTotalNTV);
                        worksheet.Cells[startRow, 6].Style.Font.Color.SetColor(ColorTotalHABERTURK);
                        worksheet.Cells[startRow, 7].Style.Font.Color.SetColor(ColorTotalCNNTURK  );
                        worksheet.Cells[startRow, 8].Style.Font.Color.SetColor(ColorTotalHALKTV   );
                        worksheet.Cells[startRow, 9].Style.Font.Color.SetColor(ColorTotalKRT);
                        worksheet.Cells[startRow, 10].Style.Font.Color.SetColor(ColorTotalAHABER     );
                        worksheet.Cells[startRow, 11].Style.Font.Color.SetColor(ColorTotalTRTHABER   );
                        worksheet.Cells[startRow, 12].Style.Font.Color.SetColor(ColorTotalTELE1      );
                        worksheet.Cells[startRow, 13].Style.Font.Color.SetColor(ColorTotalTV100      );
                        worksheet.Cells[startRow, 14].Style.Font.Color.SetColor(ColorTotalHABERGLOBAL);
                        worksheet.Cells[startRow, 15].Style.Font.Color.SetColor(ColorTotalSOZCUTV);

                        worksheet.Cells[startRow, 17].Style.Font.Color.SetColor(ColorABSESNTV        );
                        worksheet.Cells[startRow, 18].Style.Font.Color.SetColor(ColorABSESHABERTURK  );
                        worksheet.Cells[startRow, 19].Style.Font.Color.SetColor(ColorABSESCNNTURK    );
                        worksheet.Cells[startRow, 20].Style.Font.Color.SetColor(ColorABSESHALKTV     );
                        worksheet.Cells[startRow, 21].Style.Font.Color.SetColor(ColorABSESKRT        );
                        worksheet.Cells[startRow, 22].Style.Font.Color.SetColor(ColorABSESAHABER     );
                        worksheet.Cells[startRow, 23].Style.Font.Color.SetColor(ColorABSESTRTHABER   );
                        worksheet.Cells[startRow, 24].Style.Font.Color.SetColor(ColorABSESTELE1      );
                        worksheet.Cells[startRow, 25].Style.Font.Color.SetColor(ColorABSESTV100      );
                        worksheet.Cells[startRow, 26].Style.Font.Color.SetColor(ColorABSESHABERGLOBAL);
                        worksheet.Cells[startRow, 27].Style.Font.Color.SetColor(ColorABSESSOZCUTV);


                        startRow++;
                    }


                    // Save changes to the Excel file
                    excelPackage.Save();
                }
            }
            else
            {
                Console.WriteLine("The Excel file does not exist.");
            }


            string directoryPathShare = Directory.GetCurrentDirectory() + @"\Income\" + monthString + "_Share_11li.xlsx";


            if (File.Exists(directoryPathShare))
            {
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(directoryPathShare)))
                {
                    //ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                    //ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.SingleOrDefault(sheet => sheet.Name == "19 SUBAT ");
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.SingleOrDefault(sheet => sheet.Name.Contains(sheetName));

                    if (worksheet == null)
                    {
                        worksheet = excelPackage.Workbook.Worksheets.SingleOrDefault(sheet => sheet.Name == sheetName2);

                    }
                    int startRow = 4;

                    foreach (var obj in listShare)
                    {
                        if (startRow == 101)
                        {
                            startRow = 102;
                        }
                        worksheet.Cells[startRow, 5].Value = obj.TotalNTV;
                        worksheet.Cells[startRow, 6].Value = obj.TotalHABERTURK;
                        worksheet.Cells[startRow, 7].Value = obj.TotalCNNTURK;
                        worksheet.Cells[startRow, 8].Value = obj.TotalHALKTV;
                        worksheet.Cells[startRow, 9].Value = obj.TotalKRT;
                        worksheet.Cells[startRow, 10].Value = obj.TotalAHABER;
                        worksheet.Cells[startRow, 11].Value = obj.TotalTRTHABER;
                        worksheet.Cells[startRow, 12].Value = obj.TotalTELE1;
                        worksheet.Cells[startRow, 13].Value = obj.TotalTV100;
                        worksheet.Cells[startRow, 14].Value = obj.TotalHABERGLOBAL;
                        worksheet.Cells[startRow, 15].Value = obj.TotalSOZCUTV;

                        worksheet.Cells[startRow, 17].Value = obj.ABSESNTV;
                        worksheet.Cells[startRow, 18].Value = obj.ABSESHABERTURK;
                        worksheet.Cells[startRow, 19].Value = obj.ABSESCNNTURK;
                        worksheet.Cells[startRow, 20].Value = obj.ABSESHALKTV;
                        worksheet.Cells[startRow, 21].Value = obj.ABSESKRT;
                        worksheet.Cells[startRow, 22].Value = obj.ABSESAHABER;
                        worksheet.Cells[startRow, 23].Value = obj.ABSESTRTHABER;
                        worksheet.Cells[startRow, 24].Value = obj.ABSESTELE1;
                        worksheet.Cells[startRow, 25].Value = obj.ABSESTV100;
                        worksheet.Cells[startRow, 26].Value = obj.ABSESHABERGLOBAL;
                        worksheet.Cells[startRow, 27].Value = obj.ABSESSOZCUTV;



                        Color ColorTotalNTV = convertToRgb(obj, "ColorTotalNTV");
                        Color ColorTotalHABERTURK = convertToRgb(obj, "ColorTotalHABERTURK");
                        Color ColorTotalCNNTURK = convertToRgb(obj, "ColorTotalCNNTURK");
                        Color ColorTotalHALKTV = convertToRgb(obj, "ColorTotalHALKTV");
                        Color ColorTotalKRT = convertToRgb(obj, "ColorTotalKRT");
                        Color ColorTotalAHABER = convertToRgb(obj, "ColorTotalAHABER");
                        Color ColorTotalTRTHABER = convertToRgb(obj, "ColorTotalTRTHABER");
                        Color ColorTotalTELE1 = convertToRgb(obj, "ColorTotalTELE1");
                        Color ColorTotalTV100 = convertToRgb(obj, "ColorTotalTV100");
                        Color ColorTotalHABERGLOBAL = convertToRgb(obj, "ColorTotalHABERGLOBAL");
                        Color ColorTotalSOZCUTV = convertToRgb(obj, "ColorTotalSOZCUTV");

                        Color ColorABSESNTV = convertToRgb(obj, "ColorABSESNTV");
                        Color ColorABSESHABERTURK = convertToRgb(obj, "ColorABSESHABERTURK");
                        Color ColorABSESCNNTURK = convertToRgb(obj, "ColorABSESCNNTURK");
                        Color ColorABSESHALKTV = convertToRgb(obj, "ColorABSESHALKTV");
                        Color ColorABSESKRT = convertToRgb(obj, "ColorABSESKRT");
                        Color ColorABSESAHABER = convertToRgb(obj, "ColorABSESAHABER");
                        Color ColorABSESTRTHABER = convertToRgb(obj, "ColorABSESTRTHABER");
                        Color ColorABSESTELE1 = convertToRgb(obj, "ColorABSESTELE1");
                        Color ColorABSESTV100 = convertToRgb(obj, "ColorABSESTV100");
                        Color ColorABSESHABERGLOBAL = convertToRgb(obj, "ColorABSESHABERGLOBAL");
                        Color ColorABSESSOZCUTV = convertToRgb(obj, "ColorABSESSOZCUTV");





                        worksheet.Cells[startRow, 5].Style.Font.Color.SetColor(ColorTotalNTV);
                        worksheet.Cells[startRow, 6].Style.Font.Color.SetColor(ColorTotalHABERTURK);
                        worksheet.Cells[startRow, 7].Style.Font.Color.SetColor(ColorTotalCNNTURK);
                        worksheet.Cells[startRow, 8].Style.Font.Color.SetColor(ColorTotalHALKTV);
                        worksheet.Cells[startRow, 9].Style.Font.Color.SetColor(ColorTotalKRT);
                        worksheet.Cells[startRow, 10].Style.Font.Color.SetColor(ColorTotalAHABER);
                        worksheet.Cells[startRow, 11].Style.Font.Color.SetColor(ColorTotalTRTHABER);
                        worksheet.Cells[startRow, 12].Style.Font.Color.SetColor(ColorTotalTELE1);
                        worksheet.Cells[startRow, 13].Style.Font.Color.SetColor(ColorTotalTV100);
                        worksheet.Cells[startRow, 14].Style.Font.Color.SetColor(ColorTotalHABERGLOBAL);
                        worksheet.Cells[startRow, 15].Style.Font.Color.SetColor(ColorTotalSOZCUTV);

                        worksheet.Cells[startRow, 17].Style.Font.Color.SetColor(ColorABSESNTV);
                        worksheet.Cells[startRow, 18].Style.Font.Color.SetColor(ColorABSESHABERTURK);
                        worksheet.Cells[startRow, 19].Style.Font.Color.SetColor(ColorABSESCNNTURK);
                        worksheet.Cells[startRow, 20].Style.Font.Color.SetColor(ColorABSESHALKTV);
                        worksheet.Cells[startRow, 21].Style.Font.Color.SetColor(ColorABSESKRT);
                        worksheet.Cells[startRow, 22].Style.Font.Color.SetColor(ColorABSESAHABER);
                        worksheet.Cells[startRow, 23].Style.Font.Color.SetColor(ColorABSESTRTHABER);
                        worksheet.Cells[startRow, 24].Style.Font.Color.SetColor(ColorABSESTELE1);
                        worksheet.Cells[startRow, 25].Style.Font.Color.SetColor(ColorABSESTV100);
                        worksheet.Cells[startRow, 26].Style.Font.Color.SetColor(ColorABSESHABERGLOBAL);
                        worksheet.Cells[startRow, 27].Style.Font.Color.SetColor(ColorABSESSOZCUTV);

                        startRow++;
                    }


                    // Save changes to the Excel file
                    excelPackage.Save();
                }
            }
            else
            {
                Console.WriteLine("The Excel file does not exist.");
            }






            CultureInfo turkishCulture = new CultureInfo("tr-TR");

            DateTimeFormatInfo dateTimeFormat = turkishCulture.DateTimeFormat;

            string monthTurkishName = dateTimeFormat.GetMonthName(monthNumber);

            string sheetNameAvg = monthTurkishName.ToUpper() + "'" + date5.Year.ToString().Substring(2);

            string sheetNameAvg2 = string.Concat(monthTurkishName, "'", date5.Year.ToString().AsSpan(2));


            string directoryPathAvg = Directory.GetCurrentDirectory() + @"\Income\AYLIK ORTALAMALAR_11li.xlsx";


            int daysInMonth = DateTime.DaysInMonth(date5.Year, date5.Month);


            if (File.Exists(directoryPathAvg))
            {
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(directoryPathAvg)))
                {
                    //ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                    //ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.SingleOrDefault(sheet => sheet.Name == "19 SUBAT ");
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.SingleOrDefault(sheet => sheet.Name.Contains(sheetNameAvg));


                    int startRow = 4;

                    foreach (var obj in listAvg)
                    {

                        startRow = obj.tableDate.Day+3;

                        if (obj.timebands == "ORTALAMA")
                        {
                            startRow = daysInMonth + 4;
                        }
                        if (obj.timebands == "Sıralama")
                        {
                            startRow = (daysInMonth + 6);
                        }
                        //if (startRow == (daysInMonth + 5))
                        //{
                        //    startRow++;
                        //}
                        worksheet.Cells[startRow, 2].Value = obj.TotalNTV;
                        worksheet.Cells[startRow, 3].Value = obj.TotalHABERTURK;
                        worksheet.Cells[startRow, 4].Value = obj.TotalCNNTURK;
                        worksheet.Cells[startRow, 5].Value = obj.TotalHALKTV;
                        worksheet.Cells[startRow, 6].Value = obj.TotalKRT;
                        worksheet.Cells[startRow, 7].Value = obj.TotalAHABER;
                        worksheet.Cells[startRow, 8].Value = obj.TotalTRTHABER;
                        worksheet.Cells[startRow, 9].Value = obj.TotalTELE1;
                        worksheet.Cells[startRow, 10].Value = obj.TotalTV100;
                        worksheet.Cells[startRow, 11].Value = obj.TotalHABERGLOBAL;
                        worksheet.Cells[startRow, 12].Value = obj.TotalSOZCUTV;

                        worksheet.Cells[startRow, 14].Value = obj.ABSESNTV;
                        worksheet.Cells[startRow, 15].Value = obj.ABSESHABERTURK;
                        worksheet.Cells[startRow, 16].Value = obj.ABSESCNNTURK;
                        worksheet.Cells[startRow, 17].Value = obj.ABSESHALKTV;
                        worksheet.Cells[startRow, 18].Value = obj.ABSESKRT;
                        worksheet.Cells[startRow, 19].Value = obj.ABSESAHABER;
                        worksheet.Cells[startRow, 20].Value = obj.ABSESTRTHABER;
                        worksheet.Cells[startRow, 21].Value = obj.ABSESTELE1;
                        worksheet.Cells[startRow, 22].Value = obj.ABSESTV100;
                        worksheet.Cells[startRow, 23].Value = obj.ABSESHABERGLOBAL;
                        worksheet.Cells[startRow, 24].Value = obj.ABSESSOZCUTV;




                        Color ColorTotalNTV = convertToRgb(obj, "ColorTotalNTV");
                        Color ColorTotalHABERTURK = convertToRgb(obj, "ColorTotalHABERTURK");
                        Color ColorTotalCNNTURK = convertToRgb(obj, "ColorTotalCNNTURK");
                        Color ColorTotalHALKTV = convertToRgb(obj, "ColorTotalHALKTV");
                        Color ColorTotalKRT = convertToRgb(obj, "ColorTotalKRT");
                        Color ColorTotalAHABER = convertToRgb(obj, "ColorTotalAHABER");
                        Color ColorTotalTRTHABER = convertToRgb(obj, "ColorTotalTRTHABER");
                        Color ColorTotalTELE1 = convertToRgb(obj, "ColorTotalTELE1");
                        Color ColorTotalTV100 = convertToRgb(obj, "ColorTotalTV100");
                        Color ColorTotalHABERGLOBAL = convertToRgb(obj, "ColorTotalHABERGLOBAL");
                        Color ColorTotalSOZCUTV = convertToRgb(obj, "ColorTotalSOZCUTV");

                        Color ColorABSESNTV = convertToRgb(obj, "ColorABSESNTV");
                        Color ColorABSESHABERTURK = convertToRgb(obj, "ColorABSESHABERTURK");
                        Color ColorABSESCNNTURK = convertToRgb(obj, "ColorABSESCNNTURK");
                        Color ColorABSESHALKTV = convertToRgb(obj, "ColorABSESHALKTV");
                        Color ColorABSESKRT = convertToRgb(obj, "ColorABSESKRT");
                        Color ColorABSESAHABER = convertToRgb(obj, "ColorABSESAHABER");
                        Color ColorABSESTRTHABER = convertToRgb(obj, "ColorABSESTRTHABER");
                        Color ColorABSESTELE1 = convertToRgb(obj, "ColorABSESTELE1");
                        Color ColorABSESTV100 = convertToRgb(obj, "ColorABSESTV100");
                        Color ColorABSESHABERGLOBAL = convertToRgb(obj, "ColorABSESHABERGLOBAL");
                        Color ColorABSESSOZCUTV = convertToRgb(obj, "ColorABSESSOZCUTV");





                       worksheet.Cells[startRow, 2].Style.Font.Color.SetColor(ColorTotalNTV);
                       worksheet.Cells[startRow, 3].Style.Font.Color.SetColor(ColorTotalHABERTURK);
                       worksheet.Cells[startRow, 4].Style.Font.Color.SetColor(ColorTotalCNNTURK);
                       worksheet.Cells[startRow, 5].Style.Font.Color.SetColor(ColorTotalHALKTV);
                       worksheet.Cells[startRow, 6].Style.Font.Color.SetColor(ColorTotalKRT);
                       worksheet.Cells[startRow, 7].Style.Font.Color.SetColor(ColorTotalAHABER);
                       worksheet.Cells[startRow, 8].Style.Font.Color.SetColor(ColorTotalTRTHABER);
                       worksheet.Cells[startRow, 9].Style.Font.Color.SetColor(ColorTotalTELE1);
                       worksheet.Cells[startRow, 10].Style.Font.Color.SetColor(ColorTotalTV100);
                       worksheet.Cells[startRow, 11].Style.Font.Color.SetColor(ColorTotalHABERGLOBAL);
                        worksheet.Cells[startRow, 12].Style.Font.Color.SetColor(ColorTotalSOZCUTV);

                        worksheet.Cells[startRow, 14].Style.Font.Color.SetColor(ColorABSESNTV);
                        worksheet.Cells[startRow, 15].Style.Font.Color.SetColor(ColorABSESHABERTURK);
                        worksheet.Cells[startRow, 16].Style.Font.Color.SetColor(ColorABSESCNNTURK);
                        worksheet.Cells[startRow, 17].Style.Font.Color.SetColor(ColorABSESHALKTV);
                        worksheet.Cells[startRow, 18].Style.Font.Color.SetColor(ColorABSESKRT);
                        worksheet.Cells[startRow, 19].Style.Font.Color.SetColor(ColorABSESAHABER);
                        worksheet.Cells[startRow, 20].Style.Font.Color.SetColor(ColorABSESTRTHABER);
                        worksheet.Cells[startRow, 21].Style.Font.Color.SetColor(ColorABSESTELE1);
                        worksheet.Cells[startRow, 22].Style.Font.Color.SetColor(ColorABSESTV100);
                        worksheet.Cells[startRow, 23].Style.Font.Color.SetColor(ColorABSESHABERGLOBAL);
                        worksheet.Cells[startRow, 24].Style.Font.Color.SetColor(ColorABSESSOZCUTV);
                        startRow++;
                    }


                    // Save changes to the Excel file
                    excelPackage.Save();
                }
            }
            else
            {
                Console.WriteLine("The Excel file does not exist.");
            }






            //string sourceFileRtg = Directory.GetCurrentDirectory() + @"\Income\" + monthString + "_Rtg_11li.xlsx";
            string sourceFileRtg = Path.Combine(Directory.GetCurrentDirectory(), @"Income\", monthString + "_Rtg_11li.xlsx");

            string destinationFileRtg = Directory.GetCurrentDirectory() + @"\Completed\" + "RTG_" + onlyMonthString +"_"+ onlyYearString+".xlsx";

            string sourceFileShare = Directory.GetCurrentDirectory() + @"\Income\" + monthString + "_Share_11li.xlsx";
            string destinationFileShare = Directory.GetCurrentDirectory() + @"\Completed\" + "SHARE_" + onlyMonthString + "_" + onlyYearString + ".xlsx";

            string sourceFileAvg = Directory.GetCurrentDirectory() + @"\Income\"  + "AYLIK ORTALAMALAR_11li.xlsx";
            string destinationFileAvg = Directory.GetCurrentDirectory() + @"\Completed\" + "ORTALAMA_2024.xlsx";



            //sendMail(sourceFileRtg, sourceFileShare, sourceFileAvg);
            sendMail(Directory.GetParent(sourceFileRtg).ToString(),onlyDayString, monthTurkishName);


            using (IDbConnection db = new SqlConnection(ConfigModel.ConnectionString))
            {
                db.Execute($"INSERT INTO CheckInsert (insertedDate,insertValue) VALUES ('{dateString}',1)");

            }


            //File.Move(sourceFileRtg, destinationFileRtg);
            //File.Move(sourceFileShare, destinationFileShare);


            try
            {
                // Move the first file, overwriting the destination if it exists
                MoveFileWithOverwrite(sourceFileRtg, destinationFileRtg);

                // Move the second file, overwriting the destination if it exists
                MoveFileWithOverwrite(sourceFileShare, destinationFileShare);

                File.Copy(sourceFileAvg, destinationFileAvg, true);


                Console.WriteLine("Files moved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }



            //File.Move(sourceFileAvg, destinationFileAvg);


            //using (IDbConnection db = new SqlConnection(ConfigModel.ConnectionString))
            //{
            //    db.Execute($"INSERT INTO CheckInsert (insertedDate,insertValue) VALUES ('{dateString}',1)");

            //}
        }

        public void MoveFileWithOverwrite(string sourceFilePath, string destinationFilePath)
        {
            // Delete the destination file if it exists
            if (File.Exists(destinationFilePath))
            {
                File.Delete(destinationFilePath);
            }

            // Move the source file to the destination
            File.Move(sourceFilePath, destinationFilePath);
        }


        public void sendMail(string sourceFileDirectory, string onlyDayString, string monthTurkishName) 
        {
            // Create a new instance of MailMessage class
            MailMessage message = new MailMessage();

            // Set subject of the message, body and sender information
            message.Subject = onlyDayString + " " +monthTurkishName +" ve Aylık Ortalamalar            ";
            //message.Body = "This is the body of the email.";
            message.From = new MailAddress(ConfigModel.MailAdress, "Habertürk Rating", false);

            // Add To recipients and CC recipients
            message.To.Add(new MailAddress(ConfigModel.GonderilenMail, "Recipient 1", false));
           // message.CC.Add(new MailAddress("rating@cyh.com.tr", "Recipient 3", false));

            // Add attachments
            //message.Attachments.Add(new Attachment("C:\\Users\\mehme\\source\\repos\\C# repos\\RatingSystem\\MailReader\\bin\\Debug\\net8.0\\Income\\SUBAT_Rtg_11li.xlsx", System.Net.Mime.MediaTypeNames.Application.Octet));

            foreach (string filePath in Directory.GetFiles(sourceFileDirectory))
            {

                message.Attachments.Add(new Attachment(filePath, System.Net.Mime.MediaTypeNames.Application.Octet));

            }

            // Save message in EML, EMLX, MSG and MHTML formats
            message.Save("EmailMessage.eml", Aspose.Email.SaveOptions.DefaultEml);
            message.Save("EmailMessage.emlx", Aspose.Email.SaveOptions.CreateSaveOptions(MailMessageSaveType.EmlxFormat));
            message.Save("EmailMessage.msg", Aspose.Email.SaveOptions.DefaultMsgUnicode);
            message.Save("EmailMessage.mhtml", Aspose.Email.SaveOptions.DefaultMhtml);




            //MailMessage msg = MailMessage.Load("message.msg");

            // Create an instance of SmtpClient class
            SmtpClient client = new SmtpClient();

            // Specify your mailing Host, Username, Password, Port # and Security option
            client.Host = "mail.cyh.com.tr";
            client.Username = ConfigModel.MailAdress;
            client.Password = ConfigModel.MailPassword;
            client.Port = 587;

            client.SecurityOptions = SecurityOptions.Auto;
            try
            {
                // Send this email
                client.Send(message);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }


        //public void sendMail(string sourceFileRtg, string sourceFileShare, string sourceFileAvg) 
        //public void sendMail(string sourceFileDirectory)
        //{

        //    string senderEmail = "mehmetcalkinli@gmail.com";
        //    string password = "Qatternity.971";

        //    // Recipient's email address
        //    string recipientEmail = "mhmtclknl@gmail.com";

        //    // Email subject and body
        //    string subject = "Email with attachments";
        //    string body = "Please find the attached files.";

        //    // Directory containing the files to be attached

        //    // Create an instance of MailMessage
        //    MailMessage mail = new MailMessage(senderEmail, recipientEmail, subject, body);

            
        //    // Attach files from the directory
        //    foreach (string filePath in Directory.GetFiles(sourceFileDirectory))
        //    {
                
        //        Attachment attachment = new Attachment(filePath);
        //        mail.Attachments.Add(attachment);
        //    }

        //    // Configure SMTP client
        //    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
        //    {
        //        Port = 587,
        //        Credentials = new NetworkCredential(senderEmail, password),
        //        EnableSsl = true,
        //        UseDefaultCredentials = true
        //    };

        //    try
        //    {
        //        // Send the email
        //        smtpClient.Send(mail);
        //        Console.WriteLine("Email sent successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Failed to send email. Error message: " + ex.Message);
        //    }
        //    finally
        //    {
        //        // Dispose of objects
        //        mail.Dispose();
        //        smtpClient.Dispose();
        //    }

        //}
    }

    

}

