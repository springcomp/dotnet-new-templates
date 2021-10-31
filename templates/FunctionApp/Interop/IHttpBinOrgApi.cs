using Refit;
using System.Net.Http;
using System.Threading.Tasks;

namespace FunctionApp.Interop
{
    /// <summary>
    /// This interface representas access to the HttpBin.org API.
    /// </summary>
    public interface IHttpBinOrgApi
    {
        [Get("/status/{code}")]
        Task<HttpContent> StatusCodes(int code);

        [Get("/headers")]
        Task<HttpContent> GetRequestHeaders();
    }
}
