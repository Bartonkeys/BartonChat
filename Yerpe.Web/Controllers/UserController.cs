using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Yerpe.Web.Models;
using Yerpe.Web.Extensions;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Mail;

namespace Yerpe.Web.Controllers
{
     [Authorize(Roles="Admin")]
    public class UserController : BaseApiController
    {
         private ApplicationUserManager _userManager;

         public HttpResponseMessage Get()
         {
             var users = YerpeContext.AspNetUsers.Select(u => new UserViewModel { Id = u.Id, UserName = u.UserName }).ToList();
             return Request.CreateResponse(HttpStatusCode.OK, users);
         }

         public async Task<HttpResponseMessage> Post([FromBody] UserViewModel model)
         {
             try
             {
                 var user = new ApplicationUser { UserName = model.UserName, Email = model.UserName };
                 var result = await UserManager.CreateAsync(user, model.Password);

                 if (result.Succeeded)
                 {
                     YerpeContext.AspNetUsers.Single(x => x.UserName == user.UserName).Rooms.Add(YerpeContext.Rooms.Single(r => r.Id == model.RoomId));
                     YerpeContext.SaveChanges();

                     var message = new MailMessage("graham.mcvea@gmail.com", model.UserName)
                     {
                         Body = String.Format(
                             "The legend that is Graham McVea has signed you up for Yerpe \n\n Your username is {0}  \n Your password is {1} \n\n http://www.yerpe.com",
                             model.UserName, model.Password)
                     };

                     var client = new SmtpClient();
                     client.Send(message);

                     return Request.CreateResponse(HttpStatusCode.Created, model);
                 }
                 return Request.CreateResponse(HttpStatusCode.NotImplemented);
             }
             catch (Exception ex)
             {
                 return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
             }
         }

         public ApplicationUserManager UserManager
         {
             get
             {
                 return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
             }
             private set
             {
                 _userManager = value;
             }
         }
    }
}
