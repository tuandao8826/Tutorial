using AutoMapper;

namespace HttpClientTutorial.API.HttpClients
{
    public class HttpResultProfile : Profile
    {
        public HttpResultProfile()
        {
            CreateMap<HttpResponseMessage, HttpResult>();
        }
    }
}
