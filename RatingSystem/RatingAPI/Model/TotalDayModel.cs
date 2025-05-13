namespace RatingAPI.Model
{
    public class TotalDayModel
    {

        
        public int id { get; set; }
        public DateTime avgDate { get; set; }
        public string avgType { get; set; }
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
                ColorTotalNTV = "darkgreen";
            if (TotalHABERTURK == 0)
                ColorTotalHABERTURK = "darkgreen";
            if (TotalCNNTURK == 0)
                ColorTotalCNNTURK = "darkgreen";
            if (TotalHALKTV == 0)
                ColorTotalHALKTV = "darkgreen";
            if (TotalKRT == 0)
                ColorTotalKRT = "darkgreen";
            if (TotalAHABER == 0)
                ColorTotalAHABER = "darkgreen";
            if (TotalTRTHABER == 0)
                ColorTotalTRTHABER = "darkgreen";
            if (TotalTELE1 == 0)
                ColorTotalTELE1 = "darkgreen";
            if (TotalTV100 == 0)
                ColorTotalTV100 = "darkgreen";
            if (TotalHABERGLOBAL == 0)
                ColorTotalHABERGLOBAL = "darkgreen";
            if (TotalSOZCUTV == 0)
                ColorTotalSOZCUTV = "darkgreen";

            if (ABSESNTV == 0)
                ColorABSESNTV = "darkgreen";
            if (ABSESHABERTURK == 0)
                ColorABSESHABERTURK = "darkgreen";
            if (ABSESCNNTURK == 0)
                ColorABSESCNNTURK = "darkgreen";
            if (ABSESHALKTV == 0)
                ColorABSESHALKTV = "darkgreen";
            if (ABSESKRT == 0)
                ColorABSESKRT = "darkgreen";
            if (ABSESAHABER == 0)
                ColorABSESAHABER = "darkgreen";
            if (ABSESTRTHABER == 0)
                ColorABSESTRTHABER = "darkgreen";
            if (ABSESTELE1 == 0)
                ColorABSESTELE1 = "darkgreen";
            if (ABSESTV100 == 0)
                ColorABSESTV100 = "darkgreen";
            if (ABSESHABERGLOBAL == 0)
                ColorABSESHABERGLOBAL = "darkgreen";
            if (ABSESSOZCUTV == 0)
                ColorABSESSOZCUTV = "darkgreen";
        }
    }
}
