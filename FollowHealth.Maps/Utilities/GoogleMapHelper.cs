using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FollowHealth.Maps.Utilities
{
	public static class GoogleMapHelper
	{
		// Convert from Degrees to Radians
		private static double ToRad(this double num)
		{
			return num * Math.PI / 180;
		}

		// Convert from Radians to Degrees
		private static double ToDeg(this double num)
		{
			return num * 180 / Math.PI;
		}

		public static double GetDistance(double lat1, double lon1, double lat2, double lon2)
		{
			const int r = 6371; // radius of earth in km

			// Convert to Radians
			lat1 = lat1.ToRad();
			lon1 = lon1.ToRad();
			lat2 = lat2.ToRad();
			lon2 = lon2.ToRad();

			// Spherical Law of Cosines
			var resultCos =
				Math.Acos(
					Math.Sin(lat1) * Math.Sin(lat2) +
					Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(lon2 - lon1)
					) * r;

			return resultCos;
		}

		public static ILocation GetClosestPoint(double originLat, double originLong, IEnumerable<ILocation> points)
		{
			// Build a List<Distance> that contains the calculated distance for each point
			var list = points.Select(p => new Distance(p, GetDistance(originLat, originLong, p.Latitude, p.Longitude))).ToList();

			if (!list.Any())
				return null;

			// Sort the List using the custom IComparable implementation to sort by Distance.Km
			list.Sort();

			return list.First();
		}
	}

	public interface ILocation
	{
		int Id { get; set; }
		double Latitude { get; set; }
		double Longitude { get; set; }
	}

	

	public class Distance : IComparable, ILocation
	{
		public int Id { get; set; }
		public double Km { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }

		public Distance(ILocation iLocation, double km)
		{
			Id = iLocation.Id;
			Latitude = iLocation.Latitude;
			Longitude = iLocation.Longitude;
			Km = km;
		}

		// Compare Km for sorting
		public int CompareTo(object obj)
		{
			var d = (Distance)obj;
			return Km.CompareTo(d.Km);
		}
	}
}