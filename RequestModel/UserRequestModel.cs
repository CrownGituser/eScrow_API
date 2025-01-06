namespace SMFG_API_New.RequestModel
{
    public class UserRequestModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;


    }

    public class UserRequestModelEny
    {
        public string DataRequest { get; set; } = string.Empty;
        public string AESKey { get; set; } = string.Empty;
        public string AESIv { get; set; } = string.Empty; 

    }

}
