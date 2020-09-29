using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using Microsoft.Win32;
using System.Xml;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Utils;
using System.Drawing;
using OfficeOpenXml.Style;
using System.Threading;
using System.Globalization;


namespace Furnishings_Shop
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>

	#region Class Body
	public partial class MainWindow : Window
	{
		public WriteReadExcel writereadExcel = new WriteReadExcel();
		public static DataSet dataSet = new DataSet();

		internal int rowIndex, columnIndex;
		public MainWindow()
		{
			InitializeComponent();
			Double width = SystemParameters.FullPrimaryScreenWidth;
			Double height = SystemParameters.FullPrimaryScreenHeight;
			this.Top = (height - this.Height) / 2;
			this.Left = (width - this.Width) / 2;
			Thread.CurrentThread.CurrentCulture = new CultureInfo("he");

		}
		public void ColumnsProperty(DataTable table)
		{
			dataGrid_ItemsDataSet.HorizontalContentAlignment = HorizontalAlignment.Right; 
			for (int i = 0; i < table.Columns.Count; i++) { dataGrid_ItemsDataSet.Columns[i].Header = table.Columns[i].Caption;  }
			dataGrid_ItemsDataSet.Columns[0].IsReadOnly = false; 
			(dataGrid_ItemsDataSet.Columns[table.Columns.Count - 1] as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
			// Format Date in Date Column
		}
		public DataTable Time_Delivery_Left(string choise)
		{
			DataTable dtTimeLeft = new DataTable();
			DataRow dataRow;  DataColumn dtColumn = new DataColumn(); 
			dtColumn.DataType = Type.GetType("System.Int32"); dtColumn.ColumnName = "ID"; dtColumn.ReadOnly = true;
			dtTimeLeft.Columns.Add(dtColumn);
			dtColumn = new DataColumn(); dtColumn.DataType = Type.GetType("System.Int32"); dtColumn.ColumnName = "Time Left";
			dtTimeLeft.Columns.Add(dtColumn);
			int rowcount = dataSet.Tables[0].Rows.Count;
			for (int i = 0; i < rowcount; i++)
			{
				DateTime dateOrder = (DateTime)dataSet.Tables[0].Rows[i].ItemArray[dataSet.Tables[0].Columns.Count - 1];
				dataRow = dtTimeLeft.NewRow();
				dataRow[0] = dataSet.Tables[0].Rows[i].ItemArray[0];
				dataRow[1] = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[8]) - (DateTime.Now.Date - dateOrder.Date).TotalDays;
				if (Convert.ToInt32(dataRow[1])<=Convert.ToInt32(choise)) 	dtTimeLeft.Rows.Add(dataRow);
			}
			return dtTimeLeft;
		}
#endregion Class Body

#region Button Events
		public void btnNewItem_Click(object sender, RoutedEventArgs e)
		{
			Add_New_Item_Win newtoolwindow = new Add_New_Item_Win();
			newtoolwindow.ShowDialog();
		}
		private void BtnConnectShowData_Click(object sender, RoutedEventArgs e)
		{
			dataGrid_ItemsDataSet.Visibility = Visibility.Visible;
			dataGrid_Time_Delivery_Left.Visibility = Visibility.Hidden;
			dataSet = writereadExcel.ReadFromFile(); 
			dataGrid_ItemsDataSet.ItemsSource = dataSet.Tables[0].DefaultView;
			ColumnsProperty(dataSet.Tables[0]);
			btnNewItem.Visibility = Visibility.Visible; btnQueryWindow.Visibility = Visibility.Visible; btnRemoveItem.Visibility = Visibility.Visible;
			btnSaveChange.Visibility = Visibility.Visible;
			btnTimeDeliveryLeft.Visibility = Visibility.Visible; txt_TimeDeliveryLeft.Visibility = Visibility.Visible;
			btn_ChoiseByID.Visibility = Visibility.Visible; txt_ChoiseByID.Visibility = Visibility.Visible;
			btn_ShowBalance.Visibility = Visibility.Visible;
		}
		private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
		{
			int index = dataGrid_ItemsDataSet.SelectedIndex;
			if (index != -1) { dataSet.Tables[0].Rows.RemoveAt(index); dataSet.AcceptChanges(); }
		}
		//======== WORK !!!
		private void btnSaveChange_Click(object sender, RoutedEventArgs e)
		{
			WriteReadExcel writereadExcel = new WriteReadExcel();
			dataSet.AcceptChanges(); 
			writereadExcel.writeToExcel(dataSet);
		}
		private void btnOpenFile_Click(object sender, RoutedEventArgs e)
		{
			writereadExcel.OpenFile();
		}
		private void btnQueryWindow_Click(object sender, RoutedEventArgs e)
		{
			QueryWindow queryWindow = new QueryWindow();
			queryWindow.ShowDialog();
		}
		private void btnTimeDeliveryLeft_Click(object sender, RoutedEventArgs e)
		{
			 if (dataSet.Tables.Count > 0 & txt_TimeDeliveryLeft.Text!="")
			 {
				dataGrid_Time_Delivery_Left.Visibility = Visibility.Visible;
				dataGrid_ItemsDataSet.Visibility = Visibility.Hidden;
				dataGrid_Time_Delivery_Left.ItemsSource = Time_Delivery_Left(txt_TimeDeliveryLeft.Text).DefaultView;
			 }
			 else
			 {
				 MessageBox.Show("Data not connected or Input Empty \n\r Open file "); e.Handled = true;
			 }
		}
		// !!!!!!!!!!!!!!
		private void Btn_ChoiseByID_Click(object sender, RoutedEventArgs e)
		{
			if (dataSet.Tables.Count > 0 & txt_ChoiseByID.Text != "")
			{
				int id_textBox = Int32.Parse(txt_ChoiseByID.Text.ToString());
				for (int index = 0; index < dataSet.Tables[0].Rows.Count; index++)
				{
					if (Convert.ToUInt32(dataSet.Tables[0].Rows[index].ItemArray[0]) == id_textBox)
					{
						dataGrid_ItemsDataSet.SelectedIndex = index; object item = dataGrid_ItemsDataSet.Items[index]; dataGrid_ItemsDataSet.SelectedItem = item;
						dataGrid_ItemsDataSet.ScrollIntoView(item);
						dataGrid_ItemsDataSet.Focus();
						Order_Info wnd_order_Info = new Order_Info(index); //wnd_order_Info(index)
						wnd_order_Info.ShowDialog();
					}
				} //for
			}    //if
			else
			{
				MessageBox.Show("Data not connected or Input Empty \n\r Open file "); e.Handled = true;
			}
		}
		private void Btn_ShowBalance_Click(object sender, RoutedEventArgs e)
		{
			if (dataSet.Tables.Count > 0)
			{
				int bigBalance = 0;
				for (int index = 0; index < dataSet.Tables[0].Rows.Count; index++)
				{
					bigBalance += Int32.Parse(((DataRowView)dataGrid_ItemsDataSet.Items[index]).Row["balance"].ToString());
				}
				txt_ShowBalance.Text = bigBalance.ToString();
			}
			else
			{
				MessageBox.Show("Data not connected"); e.Handled = true;
			}
		}
	
#endregion Button 

# region Events
		private void DigitsOnly_Input(object sender, TextCompositionEventArgs e)
		{
			short value;
			if (!Int16.TryParse(e.Text, out value) || e.Text == " ") e.Handled = true;
		}
		private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space || e.Key == Key.RightShift || e.Key == Key.LeftShift) e.Handled = true;
		}
		private void txt_ChoiseByID_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			txt_ChoiseByID.Text = "";
		}
		private void txt_TimeDeliveryLeft_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			txt_TimeDeliveryLeft.Text = "";
		}

		#endregion Events

		#region DataGtid Events
		private void DataGrid_ItemsDataSet_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
		{
			rowIndex = e.Row.GetIndex();
			columnIndex = e.Column.DisplayIndex;
		}

		
		private void DataGrid_ItemsDataSet_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			//columnIndex
			short value;
			//switch
			if (columnIndex == 6 ||  columnIndex == 7 || columnIndex == 8 ) { if (!Int16.TryParse(e.Text, out value)) e.Handled = true; }
			if (columnIndex == 2  ||  columnIndex == 5 ) { if (Int16.TryParse(e.Text, out value)) e.Handled = true; } // only letters
			if (columnIndex == 4) { if (!Int16.TryParse(e.Text, out value) & e.Text != "-") e.Handled = true; }
		}
		//private void DataGrid_ToolsDataBase_SelectionChanged(object sender, SelectionChangedEventArgs e)
		//{
		//	object item = dataGrid_ToolsDataBase.SelectedItem;
		//	int rowID = Convert.ToInt32((dataGrid_ToolsDataBase.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
		//	string strID = (dataGrid_ToolsDataBase.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;

		//	int selectedCellValue = Convert.ToInt32(((DataRowView)dataGrid_ToolsDataBase.SelectedItem).Row[6]);
		//	string ID = ((DataRowView)dataGrid_ToolsDataBase.SelectedItem).Row["type"].ToString();
		//}
#endregion DataGtid Events
	}//MainWindow
} // NameSpace