using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FollowHealth.Maps.Entities;
using FollowHealth.Maps.Utilities;

namespace FollowHealth.Maps.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public void GetAllNearestFamousPlaces(double currentLatitude, double currentLongitude)
		{
			//var context = new FollowHealthEntities()
			//List<DistanceModel> Caldistance = new List<DistanceModel>();
			//var query = (from c in context.Addresses
			//			 select c).ToList();
			//foreach (var place in query)
			//{
			//	double distance = Distance(currentLatitude, currentLongitude, place.Latitude, place.Longitude);
			//	if (distance < 25)          //nearbyplaces which are within 25 kms 
			//	{
			//		DistanceModel dist = new DistanceModel();
			//		dist.Name = place.City;
			//		dist.Latitute = place.Latitude;
			//		dist.Longitude = place.Logitude;
			//		dist.PlaceId = place.PlaceId;
			//		Caldistance.Add(getDiff);
			//	}
			//}
		}

		private double Distance(double lat1, double lon1, double lat2, double lon2)
		{
			double theta = lon1 - lon2;
			double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
			dist = Math.Acos(dist);
			dist = rad2deg(dist);
			dist = (dist * 60 * 1.1515) / 0.6213711922;          //miles to kms
			return (dist);
		}

		private double deg2rad(double deg)
		{
			return (deg * Math.PI / 180.0);
		}

		private double rad2deg(double rad)
		{
			return (rad * 180.0 / Math.PI);
		}

		public JsonResult LoadAllLocationData(string isNew, string cityLat, string cityLng)
		{
			System.Web.HttpBrowserCapabilitiesBase brow = Request.Browser;
			string browser = brow.Type;
			
			var locations = new List<string>();
			var locationsData = new List<string>();
			var infoWindowContents = new List<string>();
			decimal addressLatitude = 21M, addressLongitude = 78M;
			decimal defaultLatitude = 21M, defaultLongitude = 78M;
			//showing no image for not reported bins
			string imgName = string.Empty;
			Address locModel = new Address();
			string defaultTitle = "LV Prasad Eye Hospital";
			FollowHealthEntities ent = new FollowHealthEntities();

			using (var context = new FollowHealthEntities())
			{
				List<Address> addressList = context.Addresses.ToList();

				foreach (Address locData in addressList)
				{
					//Address locData = addressList.FirstOrDefault();
					if (locData.Latitude != "0" && locData.Longitude != "0")
					{
						addressLatitude = Convert.ToDecimal(locData.Latitude);
						addressLongitude = Convert.ToDecimal(locData.Longitude);
					}
					if (addressLatitude != 0 && addressLongitude != 0 && locData.Latitude != "0" && locData.Longitude != "0")
					{
						defaultLatitude = addressLatitude;
						defaultLongitude = addressLongitude;
						locationsData.Add(string.Format(
						   @"{{ 
                                        title: ""<b>{0}</b><br/><span>{1} <br/> {2}</span>"",
										lat:{3}, 
										lon:{4}										
                                    }}",
										   defaultTitle,
										   locData.City.Trim(),
										   locData.District.Trim(),
										   addressLatitude,
										   addressLongitude										   
										)
									  );

						locations.Add(string.Format(
						   @"{{ 
				                                    title: ""{0}"", 
				                                    position: new google.maps.LatLng({1}, {2})
				                                }}",
										   defaultTitle,
										   addressLatitude,
										   addressLongitude
										)
									  );
						//Attetherate =
						infoWindowContents.Add(string.Format(
								@"{{ 
				                                content: ""<div class=\""infowindow\""><b>{0}</b><br/>City :{1} <br/> District :{2}</div>""
				                            }}",
										defaultTitle,
										locData.City.Trim(),
										locData.District.Trim()
										//locData.Postal_Addres.Trim()
										)
								   );
					}
				}

				
				if (defaultLatitude == 0 && defaultLongitude == 0)
				{
					defaultLatitude = 20.233333M;
					defaultLongitude = 85.833333M;
				}
				var locationsJson = "[" + string.Join(",", locations.ToArray()) + "]";
				var overlayContentsJson = "[" + string.Join(",", infoWindowContents.ToArray()) + "]";
				var locationsDataJson = "[" + string.Join(",", locationsData.ToArray()) + "]";
				return new LargeJsonResult() { Data = new { locationsJson, overlayContentsJson, defaultLatitude, defaultLongitude, locationsDataJson }, MaxJsonLength = int.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
			}
		}
	}

	public class DistanceModel
	{
		public int PlaceId { get; set; }

		public string Name { get; set; }

		public double Latitute { get; set; }

		public double Longitude { get; set; }

	}
}