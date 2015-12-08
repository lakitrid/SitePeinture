using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SitePeinture.Models;
using MimeKit;
using MailKit.Net.Smtp;
using SitePeinture.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SitePeinture.Controllers
{
    [Route("service/[controller]")]
    public class ContactController : Controller
    {
        [FromServices]
        public MailService MailService { get; set; }

        // POST api/values
        [HttpPost]
        public void SendMessage([FromBody]Contact contact)
        {
            MailService.SendMail(contact);
        }
    }
}
