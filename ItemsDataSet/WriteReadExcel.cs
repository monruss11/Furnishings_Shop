using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Utils;
using OfficeOpenXml.Style;
using System.Xml;
using System.Drawing;
using System.Windows;
using System.Data;
using System.Windows.Controls;
using System.IO.Packaging;
using System.Security.Cryptography;
using DataTable = System.Data.DataTable;
using System.Globalization;
using System.Threading;
//using dsTable=MainWindow.dataSet.Tables[0];

namespace Furnishings_Shop
{
	public class WriteReadExcel
	{
		public static string filename="d:\\First.xlsx";
		public FileInfo existingFile = new FileInfo(filename);
		
		public void CheckFileExcel()
		{
			if (System.IO.File.Exists(filename) == false)
			{
				MessageBox.Show("File not exist " + /*filename.ToString()+*/ " \n\r Create file ");
				using (var package = new ExcelPackage())
				{
					// Add a new worksheet to the empty workbook
					ExcelWorksheet ws = package.Workbook.Worksheets.Add("Orders");
					//Add the headers
					ws.Cells[1, 1].Value = "ID";
					ws.Cells[1, 2].Value = "Info"; 
					ws.Cells[1, 3].Value = "Name"; 
					ws.Cells[1, 4].Value = "Adress";
					ws.Cells[1, 5].Value = "Telephone";
					ws.Cells[1, 6].Value = "Cost";
					ws.Cells[1, 7].Value = "Balance";
					ws.Cells[1, 8].Value = "Time Delivery";
					ws.Cells[1, 9].Value = "Date Order";
					var xlFile = new FileInfo(filename);
                  // save our new workbook in the output directory and we are done!
                    package.SaveAs(xlFile); package.Dispose();
					MessageBox.Show("File created automatically \n\r" + xlFile.FullName.ToString());
				}
				OpenFile();
			}
			
		}
		internal string OpenFile()
		{
			Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
			ofd.Filter = "Excel Files (*.xlsx)|*.xlsx";
			Nullable<bool> result = false; ofd.ShowDialog(); result = ofd.ValidateNames;
			// Get the selected file name and display in a TextBox.
			// Load content of file in a TextBlock
			try
			{
				if (result == true & ofd.FileName != "")  	{ WriteReadExcel.filename = ofd.FileName; 	}
				/*ReadDataFromFile();*/
				else { MessageBox.Show("File not choised !"); WriteReadExcel.filename = "d:\\First_5.xlsx"; CheckFileExcel(); }
			}
			catch (Exception ex)
			{ MessageBox.Show("File not exist \n\r Create file " + ex.Message); }
			return WriteReadExcel.filename;
		} // OpenFileExcel
		public DataSet ReadFromFile()
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
			DataSet dataSet=new DataSet(); 
			DataTable dataTable = new DataTable();  
			DataColumn dtColumn;  DataRow dtRow;
#region Create Columns
				dtColumn = new DataColumn();  dtColumn.DataType = Type.GetType("System.Int32");  dtColumn.ColumnName = "Id"; 
				dtColumn.ReadOnly = false;  
			    dtColumn.Unique = true;  dtColumn.AutoIncrement = true;  dtColumn.AutoIncrementSeed = 5; // начальное значение  
				dtColumn.AutoIncrementStep = 3;
				dataTable.Columns.Add(dtColumn);
				dtColumn = new DataColumn();  dtColumn.DataType = Type.GetType("System.String");  dtColumn.ColumnName = "Info";  dtColumn.Caption = "Order Info";  
				dataTable.Columns.Add(dtColumn);  
				dtColumn = new DataColumn();  dtColumn.DataType = Type.GetType("System.String");  dtColumn.ColumnName = "Name";  dtColumn.Caption = "Name Customer"; 
				dataTable.Columns.Add(dtColumn);  
				dtColumn = new DataColumn();  dtColumn.DataType = Type.GetType("System.String");  dtColumn.ColumnName = "Adress";  dtColumn.Caption = "Adress"; 
				dataTable.Columns.Add(dtColumn);
				dtColumn = new DataColumn();  dtColumn.DataType = Type.GetType("System.String");  dtColumn.ColumnName = "Telephone";  dtColumn.Caption = "Telephone";  
				dataTable.Columns.Add(dtColumn);
				dtColumn = new DataColumn();  dtColumn.DataType = Type.GetType("System.String");  dtColumn.ColumnName = "Seller_Name";  dtColumn.Caption = "Seller";
				dataTable.Columns.Add(dtColumn);
				dtColumn = new DataColumn();  dtColumn.DataType = Type.GetType("System.Int32");  dtColumn.ColumnName = "Cost";  dtColumn.Caption = "Cost"; 
				dataTable.Columns.Add(dtColumn);
				dtColumn = new DataColumn();  dtColumn.DataType = Type.GetType("System.Int32");  dtColumn.ColumnName = "Balance";  dtColumn.Caption = "Balance"; 
				dataTable.Columns.Add(dtColumn);
				dtColumn = new DataColumn();  dtColumn.DataType = Type.GetType("System.Int32");  dtColumn.ColumnName = "Time_Delivery";  dtColumn.Caption = "Time"; 
				dataTable.Columns.Add(dtColumn);
				dtColumn = new DataColumn();  dtColumn.DataType = Type.GetType("System.DateTime");  dtColumn.ColumnName = "Date_Order"; dtColumn.Caption = "Date Order"; 
				dataTable.Columns.Add(dtColumn);
#endregion  Create Columns
			var xlFile = new FileInfo(filename);
			ExcelPackage package = new ExcelPackage( xlFile); ExcelWorksheet ws =  package.Workbook.Worksheets.FirstOrDefault();
			// get number of rows and columns in the sheet
			int rows = ws.Dimension.Rows;  int columns = ws.Dimension.Columns; 
			// loop through the worksheet rows and columns
			for (int i = 1; i < rows; i++) {
				dtRow = dataTable.NewRow();  
				for (int j = 0; j < columns; j++) 
					{   if (j != 9) dtRow[j] = ws.Cells[i + 1, j + 1].Value;
						else
						{
							long dateNum = long.Parse(ws.Cells[i + 1, j + 1].Value.ToString());
							dtRow[j] = DateTime.FromOADate(dateNum);
							//DateTime.ParseExact(ws.Cells[i + 1, j + 1].Value.ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
						}	
					}
				dataTable.Rows.Add(dtRow);
			}
			dataSet.Tables.Add(dataTable);
			return dataSet;
		}
		public void writeToExcel(DataSet ds)
		{
			try
			{
				var xlFile = new FileInfo(filename);

				using ( var package = new ExcelPackage(xlFile)) 
				{ //Add some items...
					ExcelWorksheet ws = package.Workbook.Worksheets[1];
					// Clear excel file 
					for (int index = 2; index <= ws.Dimension.Rows; index++) ws.DeleteRow(index);
					// ==Write to Excel file DataSet.DataTable
					foreach (DataTable table in ds.Tables)
					{
						for (int j = 0; j < table.Rows.Count; j++)
						{
							for (int k = 0; k < table.Columns.Count; k++)
							{
								//j + 2 becouse index==1 this header of column
								ws.Cells[j + 2, k + 1].Value = table.Rows[j].ItemArray[k];
							}
						}
					}
					try { package.SaveAs(xlFile); } catch ( Exception e ) { MessageBox.Show(e.Message.ToString() + "all file in package"); }
				}   // package
			}
			catch ( Exception e ) { MessageBox.Show(e.Message.ToString() + "all file" ); }
		} // writeToExcel
		public void writeItemToExcel(Item_Furnishings.ItemData itemData)
		{
			var xlFile = new FileInfo(filename);
			//string i=readIDfromExcel().ToString();
			using (var package = new ExcelPackage(xlFile))
			{ //Add some items...
				ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
				//worksheet.Cells["A" + i].Value = itemData.ID;
				int i=worksheet.Dimension.End.Row;
				worksheet.Cells["a" + i].Value = itemData.ID;
				worksheet.Cells["B"+i].Value = itemData.info;
                worksheet.Cells["C"+i].Value = itemData.name_family;
                worksheet.Cells["D"+i].Value = itemData.adress;
				worksheet.Cells["e" + i].Value = itemData.telephone;
				worksheet.Cells["f" + i].Value = itemData.seller_name;
				worksheet.Cells["g" + i].Value = itemData.cost;
				worksheet.Cells["h" + i].Value = itemData.balance;
				worksheet.Cells["i" + i].Value = itemData.time_delivery;
				worksheet.Cells["j" + i].Style.Numberformat.Format = "dd.MM.yyyy";
				worksheet.Cells["j" + i].Value = itemData.date_order;
				// save our new workbook in the output directory and we are done!
               try { package.SaveAs(xlFile); } catch ( Exception e ) { MessageBox.Show(e.Message.ToString() + "add item"  ); }
			}
			
		}
		public void AddItemToDataSet(Item_Furnishings.ItemData itemData)
		{
			//string id=readIDfromExcel().ToString();
			DataRow dtRow = MainWindow.dataSet.Tables[0].NewRow();
			//int rowsCount = MainWindow.dataSet.Tables[0].Rows.Count;  int columnsCount = MainWindow.dataSet.Tables[0].Columns.Count;
			//for ( int i=0; i<rowsCount; i++ )
			//{
			//	 if(  Convert.ToUInt32(MainWindow.dataSet.Tables[0].Rows[i].ItemArray[0])==itemData.ID) 
			//		{ MessageBox.Show("ID already existing"); goto end; }
			//}
			
			 dtRow["Id"] = itemData.ID;
			 dtRow["info"] = itemData.info; 
			 dtRow["name"] = itemData.name_family;
			 dtRow["adress"] = itemData.adress; 
			 dtRow["telephone"] = itemData.telephone; 
			dtRow["seller_name"] = itemData.seller_name; 
			 dtRow["cost"] = itemData.cost; 
			 dtRow["balance"] = itemData.balance; 
			 dtRow["time_delivery"] = itemData.time_delivery; 
			 dtRow["date_order"] = itemData.date_order; 

			MainWindow.dataSet.Tables[0].Rows.Add(dtRow);
			MainWindow.dataSet.AcceptChanges();
           end:;
		}
	////	public int readIDfromExcel()
	//	{
	//		int rowCount; int id;
	//		var xlFile = new FileInfo(filename);
	//		//FileStream fs = File.Open(WriteReadExcel.filename, FileMode.Open, FileAccess.Read);
	//		//IExcelDataReader reader = ExcelReaderFactory.CreateReader(fs);
	//		using (ExcelPackage  package = new ExcelPackage(xlFile))
	//		{ //Add some items...
	//			ExcelWorksheet ws = package.Workbook.Worksheets[1];
	//			rowCount = ws.Dimension.End.Row;
	//			if (rowCount > 1) id = Convert.ToInt32(ws.Cells[rowCount - 1, 1].Value);
	//			else id = 1;
	//			package.Save();
	//		}
	//		return id+1;
	//	} // readIDfromExcel

		
	} // class
}//namespace
