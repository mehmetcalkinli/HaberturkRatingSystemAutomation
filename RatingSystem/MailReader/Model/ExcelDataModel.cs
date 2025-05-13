namespace MailReader.Model
{

    public class ExcelDataModel
    {
        public int id { get; set; }
        public DateTime tableDate { get; set; }
        public string tableType { get; set; }
        public string timebands { get; set; }
        public string programNTV { get; set; }
        public string programHABERTURK { get; set; }
        public string programCNNTURK { get; set; }

        // private CellModel TotalNTV = new CellModel();

        public decimal TotalNTV { get; set; }
        public decimal TotalHABERTURK { get; set; }
        public decimal TotalCNNTURK { get; set; }
        public decimal TotalHALKTV { get; set; }
        public decimal TotalKRT { get; set; }
        public decimal TotalAHABER { get; set; }
        public decimal TotalTRTHABER { get; set; }
        public decimal TotalTELE1 { get; set; }
        public decimal TotalTV100 { get; set; }
        public decimal TotalHABERGLOBAL { get; set; }
        public decimal TotalSOZCUTV { get; set; }
        public decimal ABSESNTV { get; set; }
        public decimal ABSESHABERTURK { get; set; }
        public decimal ABSESCNNTURK { get; set; }
        public decimal ABSESHALKTV { get; set; }
        public decimal ABSESKRT { get; set; }
        public decimal ABSESAHABER { get; set; }
        public decimal ABSESTRTHABER { get; set; }
        public decimal ABSESTELE1 { get; set; }
        public decimal ABSESTV100 { get; set; }
        public decimal ABSESHABERGLOBAL { get; set; }
        public decimal ABSESSOZCUTV { get; set; }


        public string ColorTotalNTV { get; set; }
        public string ColorTotalHABERTURK { get; set; }
        public string ColorTotalCNNTURK { get; set; }
        public string ColorTotalHALKTV { get; set; }
        public string ColorTotalKRT { get; set; }
        public string ColorTotalAHABER { get; set; }
        public string ColorTotalTRTHABER { get; set; }
        public string ColorTotalTELE1 { get; set; }
        public string ColorTotalTV100 { get; set; }
        public string ColorTotalHABERGLOBAL { get; set; }
        public string ColorTotalSOZCUTV { get; set; }
        public string ColorABSESNTV { get; set; }
        public string ColorABSESHABERTURK { get; set; }
        public string ColorABSESCNNTURK { get; set; }
        public string ColorABSESHALKTV { get; set; }
        public string ColorABSESKRT { get; set; }
        public string ColorABSESAHABER { get; set; }
        public string ColorABSESTRTHABER { get; set; }
        public string ColorABSESTELE1 { get; set; }
        public string ColorABSESTV100 { get; set; }
        public string ColorABSESHABERGLOBAL { get; set; }
        public string ColorABSESSOZCUTV { get; set; }


        public void CheckZeroValues()
        {
            if (TotalNTV == 0)
                ColorTotalNTV = "rgb(42, 124, 38)";
            if (TotalHABERTURK == 0)
                ColorTotalHABERTURK = "rgb(42, 124, 38)";
            if (TotalCNNTURK == 0)
                ColorTotalCNNTURK = "rgb(42, 124, 38)";
            if (TotalHALKTV == 0)
                ColorTotalHALKTV = "rgb(42, 124, 38)";
            if (TotalKRT == 0)
                ColorTotalKRT = "rgb(42, 124, 38)";
            if (TotalAHABER == 0)
                ColorTotalAHABER = "rgb(42, 124, 38)";
            if (TotalTRTHABER == 0)
                ColorTotalTRTHABER = "rgb(42, 124, 38)";
            if (TotalTELE1 == 0)
                ColorTotalTELE1 = "rgb(42, 124, 38)";
            if (TotalTV100 == 0)
                ColorTotalTV100 = "rgb(42, 124, 38)";
            if (TotalHABERGLOBAL == 0)
                ColorTotalHABERGLOBAL = "rgb(42, 124, 38)";
            if (TotalSOZCUTV == 0)
                ColorTotalSOZCUTV = "rgb(42, 124, 38)";

            if (ABSESNTV == 0)
                ColorABSESNTV = "rgb(42, 124, 38)";
            if (ABSESHABERTURK == 0)
                ColorABSESHABERTURK = "rgb(42, 124, 38)";
            if (ABSESCNNTURK == 0)
                ColorABSESCNNTURK = "rgb(42, 124, 38)";
            if (ABSESHALKTV == 0)
                ColorABSESHALKTV = "rgb(42, 124, 38)";
            if (ABSESKRT == 0)
                ColorABSESKRT = "rgb(42, 124, 38)";
            if (ABSESAHABER == 0)
                ColorABSESAHABER = "rgb(42, 124, 38)";
            if (ABSESTRTHABER == 0)
                ColorABSESTRTHABER = "rgb(42, 124, 38)";
            if (ABSESTELE1 == 0)
                ColorABSESTELE1 = "rgb(42, 124, 38)";
            if (ABSESTV100 == 0)
                ColorABSESTV100 = "rgb(42, 124, 38)";
            if (ABSESHABERGLOBAL == 0)
                ColorABSESHABERGLOBAL = "rgb(42, 124, 38)";
            if (ABSESSOZCUTV == 0)
                ColorABSESSOZCUTV = "rgb(42, 124, 38)";
        }


    }
}
