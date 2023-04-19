using System.Net;

namespace MagicVillage_API.Model
{
    public class APIResponse
    {

        public HttpStatusCode StatusCode {  get; set; }
        public bool IsSucess { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }

    }
}
