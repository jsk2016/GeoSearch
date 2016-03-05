using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FollowHealth.Maps.Entities;

namespace FollowHealth.Maps.DAL
{
	public class ImporLocations
	{
		Address _entityAddress = null;
		public void ImportExcelData(string excelSource)
		{
			string provider = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelSource + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES\";";
			OleDbConnection oconn = new OleDbConnection(provider);//OledbConnection and 

			try
			{
				//After connecting to the Excel sheet here we are selecting the data 
				//using select statement from the Excel sheet
				OleDbCommand ocmd = new OleDbCommand("select * from [Total VCs$]", oconn);
				oconn.Open();  //Here [Sheet1$] is the name of the sheet 
							   //in the Excel file where the data is present
				OleDbDataReader odr = ocmd.ExecuteReader();

				while (odr.Read())
				{
					_entityAddress = new Address();
					_entityAddress.District = valid(odr, 0).Trim();
					_entityAddress.Postal_Addres = valid(odr, 1).Trim();
					_entityAddress.City = valid(odr, 2).Trim();
					_entityAddress.Latitude = valid(odr,3);
					_entityAddress.Longitude = valid(odr, 4);
					_entityAddress.Latitude1 = valid(odr, 5);
					_entityAddress.Longitude1 = valid(odr, 6);

					using (FollowHealthEntities FollowHealthEntities = new FollowHealthEntities())
					{
						FollowHealthEntities.Addresses.Add(_entityAddress);
						FollowHealthEntities.SaveChanges();
					}

				}
				oconn.Close();
			}
			catch (DataException ee)
			{

			}
			finally
			{

			}
		}

		//This valid method is mainly used to check where the null values are 
		//contained in the Excel Sheet and replacing them with zero
		protected string valid(OleDbDataReader myreader, int stval)
		{
			object val = myreader[stval];
			if (val != DBNull.Value)
				return val.ToString();
			else
				return Convert.ToString(0);
		}


	}
}
