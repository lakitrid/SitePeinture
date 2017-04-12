using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SitePeinture.Models;
using SitePeinture.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SitePeinture.Controllers
{
    [Route("service/[controller]")]
    public class ContactController : Controller
    {
        public MailService MailService { get; set; }

        public ContactController(MailService mailService)
        {
            this.MailService = mailService;
        }

        // POST api/values
        [HttpPost]
        public void SendMessage([FromBody]Contact contact)
        {
            MailService.SendMail(contact);
        }
    }
}
