namespace SMFG_API_New.RequestModel
{ 
    public class DataUploadRequestModelEny
    {

        public string DataRequest { get; set; }
       // public string DataRequest { get; set; } = string.Empty;
        public string AESKey { get; set; } = string.Empty;
        public string AESIv { get; set; } = string.Empty;

    }

    public class DURequestModelEntity
    {
        public List<DumpData> DataRequest { get; set; }

    }

    public class LANEntity
    {
        public List<LanDetails> DataRequest { get; set; }

    }

    public class LANEntityDetails
    {
        public string LAN { get; set; }

    }

    public class DataUploadRequestModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; 
    }
     
    public class DumpData
    {
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string CustomerID { get; set; }
        public string GroupID { get; set; }
        public string AppRefNo { get; set; }
        public string LAN { get; set; }
        public string CustomerName { get; set; }
        public string Disperseddate { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string FileStatus { get; set; }
        public decimal LoanAmount { get; set; }
    }

    public class LanDetails
    {
        public string Lan { get; set; }
        public string Status { get; set; }


    }


}
