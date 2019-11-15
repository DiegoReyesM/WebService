using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebServiceMDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]  
    public class LoginController : ControllerBase
    {


        [HttpGet]
        public ActionResult<IEnumerable<string>> Get([FromBody] Login login)
        {
            return new string[] { "value1", "value2" };
        }
 

        [HttpPost]
        [AllowAnonymous]
        public void Post([FromBody] Login login)
        {
            Console.WriteLine("");
  

        }




    }


    public class Login
    {

        public string email { get; set; }
        public string token { get; set; }
        public string usuario { get; set; }

    }


}