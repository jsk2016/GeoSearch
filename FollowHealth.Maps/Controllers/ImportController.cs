using FollowHealth.Maps.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FollowHealth.Maps.Controllers
{
	public class ImportController : Controller
	{
		// GET: Import

		public ActionResult Import()
		{
			return View();
		}

		public ActionResult ImportExcelData()
        {
			FollowHealth.Maps.DAL.ImporLocations import = new DAL.ImporLocations();
			import.ImportExcelData(System.Web.HttpContext.Current.Server.MapPath("LVPEI Vision Centres.xlsx"));
			return Content("Success");
        }

	}
}