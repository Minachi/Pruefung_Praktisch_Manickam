using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Web.Mvc;
using System.Linq;

namespace Pruefung_Praktisch_Musterloesung.Controllers
{
    public class Lab1Controller : Controller
    {
        /**
         * Frage 1: Bei der Detailansicht kann durch Ändern der URL Zugang zu Daten verschafft werden, welche man nicht unbedingt sehen sollte.
         *          Bei der Overview kann der User das Directory und auch noch das File welches er will eingeben. Directory Traversal Attack
         * Frage 2: Overview: ~/Lab1/index?type=bears
         *          Detail: ~/Lab1/Detail?file=bear1.jpg&type=bears
         * Frage 3: Bei der Overview wird damit erreich, dass Bilder von Bären (welche normalerweise nicht zugänglich sind) angezeigt werden.
         *          Bei der Detailansicht wird damit erreicht, dass man die Detailansicht des Bärenbildes bear1.jpg sieht (welche auch nicht zugänglich sein sollten).
         * Frage 4: 
         * 
         * */


        public ActionResult Index()
        {
            List<string> accessTypes = new List<string> { "elephants", "lions", "bears" };
            var type = Request.QueryString["type"];

            if (string.IsNullOrEmpty(type))
            {
                type = "lions";                
            }

            if (!accessTypes.Contains(type))
            {
                throw new ApplicationException("Der angegebene Typ ist nicht korrekt.");
            }

            var path = "~/Content/images/" + type;

            List<List<string>> fileUriList = new List<List<string>>();

            if (Directory.Exists(Server.MapPath(path)))
            {
                var scheme = Request.Url.Scheme; 
                var host = Request.Url.Host; 
                var port = Request.Url.Port;
                
                string[] fileEntries = Directory.GetFiles(Server.MapPath(path));
                foreach (var filepath in fileEntries)
                {
                    var filename = Path.GetFileName(filepath);
                    var imageuri = scheme + "://" + host + ":" + port + path.Replace("~", "") + "/" + filename;

                    var urilistelement = new List<string>();
                    urilistelement.Add(filename);
                    urilistelement.Add(imageuri);
                    urilistelement.Add(type);

                    fileUriList.Add(urilistelement);
                }
            }
            
            return View(fileUriList);
        }

        public ActionResult Detail()
        {
            List<string> accessTypes = new List<string> { "elephants", "lions", "bears" };

            var file = Request.QueryString["file"];
            var type = Request.QueryString["type"];

            if (string.IsNullOrEmpty(file))
            {
                file = "Lion1.jpg";
            }
            if (string.IsNullOrEmpty(type))
            {
                file = "lions";
            }

            if (!accessTypes.Contains(type))
            {
                throw new ApplicationException("Der angegebene Typ ist nicht korrekt.");
            }

            var accessPath = "/Content/images/" + type;

            if (!Directory.GetFiles(accessPath).Contains(file))
            {
                throw new ApplicationException("Falsches File angegeben.");
            }

            var relpath = "~/Content/images/" + type + "/" + file;


            List<List<string>> fileUriItem = new List<List<string>>();
            var path = Server.MapPath(relpath);

            if (System.IO.File.Exists(path))
            {
                var scheme = Request.Url.Scheme;
                var host = Request.Url.Host;
                var port = Request.Url.Port;
                var absolutepath = Request.Url.AbsolutePath;

                var filename = Path.GetFileName(file);
                var imageuri = scheme + "://" + host + ":" + port + "/Content/images/" + type + "/" + filename;

                var urilistelement = new List<string>();
                urilistelement.Add(filename);
                urilistelement.Add(imageuri);
                urilistelement.Add(type);

                fileUriItem.Add(urilistelement);
            }
            
            return View(fileUriItem);
        }
    }
}