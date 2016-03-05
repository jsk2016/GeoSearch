using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FollowHealth.Maps.Utilities
{
	public static class Extensions
	{
		public static string Image(this UrlHelper helper, string fileName)
		{
			return helper.Content("~/Content/Images/" + fileName);
		}

		public static string Stylesheet(this UrlHelper helper, string fileName)
		{
			return helper.Content("~/Content/" + fileName);
		}

		public static string Script(this UrlHelper helper, string fileName)
		{
			return helper.Content("~/Scripts/" + fileName);
		}

		public static bool Like(this string s, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
		{
			return Regex.IsMatch(s, pattern, options);
		}
	}
}
