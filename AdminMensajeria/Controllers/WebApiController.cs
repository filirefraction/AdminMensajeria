using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminMensajeria.Controllers
{
    public class LoginModel
    {
        public string usuario;
        public string password;
    }

    public class WebApiController : Controller
    {
        // GET: WebApi
        public ActionResult Index()
        {
            LoginModel login = new LoginModel() { usuario = "etiquetas_rest", password = "untCLv05@*" };

            GetToken(login);
            return View();
        }

        public string GetToken(LoginModel login)
        {
            RestClient _client;
            string _url = "https://sigomx.carvajaltys.mx/";
            _client = new RestClient(_url);


            try
            {
                var request = new RestRequest("api/JWTController/Login", Method.POST) { RequestFormat = DataFormat.Json };
                request.AddBody(login);
                var response = _client.Execute<Object>(request);
                response.Data = JsonConvert.DeserializeObject<String>(response.Content);

                return response.Data.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}