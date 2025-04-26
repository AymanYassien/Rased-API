namespace Rased.Business.Dtos.Auths
{
    public class GeneralRespnose
    {
        public bool successed { get; set; } = false;
        public List<string> Errors { get; set; } = new List<string> ();
        public string Message { get; set; }
    }
}
