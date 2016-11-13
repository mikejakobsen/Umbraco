using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AarhusWebDev.ViewModels;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace AarhusWebDev.Controllers
{
    public class MessageBoardSurfaceController : SurfaceController
    {
        // GET: MessageBoardSurface
        [HttpGet]
        public ActionResult CreateMessage()
        {
            return PartialView("MessageForm", new MessageBoard());
        }

        // Post the form
        [HttpPost]
        public ActionResult CreateMessage(MessageBoard messageBoard)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();
            var msg = Services.ContentService.CreateContent(messageBoard.Name, CurrentPage.Id, "messageBoard");

            msg.SetValue("msgbName", messageBoard.Name);
            msg.SetValue("msgbMessage", messageBoard.Message);
            try
            {
                //Saves but doesnt publish IContent
                //Services.ContentService.Save(msg);

                //Saves and publish IContent
                Services.ContentService.SaveAndPublishWithStatus(msg);
            }
            catch (Exception e)
            {
                TempData["TempMsgBoard"] = "<div class='alert alert-danger'><p>Message was not created</p>" +
                                           "<p>Something went wrong. Try again later or contact your administrator.</p></div>";
                ModelState.AddModelError(string.Empty, "" + e.InnerException);
            }
            TempData["TempMsgBoard"] = "<div class='alert alert-success'><p>Message was created</p>" +
                                       "<p>You have succesfully created a message.</p></div>";
            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult DeleteMessage(int contentId, int redirectId)
        {
            var cs = Services.ContentService;
            var msg = cs.GetById(contentId);
            if (msg != null)
                try
                {
                    cs.Delete(msg);
                }
                catch (Exception e)
                {
                    TempData["TempMsgBoard"] = "<div class='alert alert-danger'><p>Message was not deleted</p>" +
                                               "<p>Something went wrong. Try again later or contact your administrator.</p></div>";
                    ModelState.AddModelError(string.Empty, "" + e.InnerException);
                }
            TempData["TempMsgBoard"] = "<div class='alert alert-success'><p>Message was deleted</p>" +
                                       "<p>You have succesfully deleted a message.</p></div>";
            return RedirectToUmbracoPage(redirectId);
            //return RedirectToCurrentUmbracoPage();
        }
    }
}