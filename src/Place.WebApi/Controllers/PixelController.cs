using Orleans;
using Place.Interfaces;
using Place.Interfaces.Commands;
using Place.Interfaces.Grains;
using Place.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Place.WebApi.Controllers
{
    public class PixelController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/values
        public async Task<IHttpActionResult> Post([FromBody]PixelViewModel model)
        {
            IHttpActionResult result;
            if(ModelState.IsValid)
            {
                var command = model.ToCommand(GetClientIpString(Request));

                var author = GrainClient.GrainFactory.GetGrain<IUserGrain>(command.Author);
                await author.WritePixel(command);

                result = Ok();
            }
            else
            {
                result = BadRequest(ModelState);
            }

            return result;
        }

        public static string GetClientIpString(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }

            return null;
        }
    }

    public class PixelViewModel
    {
        [Required]
        [Range(0, Constants.Width)]
        public short? X { get; set; }
        [Required]
        [Range(0, Constants.Height)]
        public short? Y { get; set; }
        [Required]
        public Colors? Color { get; set; }

        public WritePixelCommand ToCommand(string ipAddress)
        {
            return new WritePixelCommand()
            {
                X = X.GetValueOrDefault(),
                Y = Y.GetValueOrDefault(),
                Color = Color.GetValueOrDefault(),
                Timestamp = DateTime.UtcNow,
                Author = ipAddress
            };
        }
    }
}
