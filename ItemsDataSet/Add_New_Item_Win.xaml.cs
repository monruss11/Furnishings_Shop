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
using System.Windows.Shapes;
using System.Threading;
using System.Globalization;

namespace Furnishings_Shop
{
	/// <summary>
	/// Interaction logic for Add_New_Tool_Win.xaml
	/// </summary>
	public partial class Add_New_Item_Win : Window
	{
		public WriteReadExcel writeExcel = new WriteReadExcel();
		public int indexPressedKey = 0;
		public Add_New_Item_Win( )
		{
			InitializeComponent(); 
			Double width  = SystemParameters.FullPrimaryScreenWidth;
            Double height = SystemParameters.FullPrimaryScreenHeight;
			this.Top = (height - this.Height) / 2+70;
            this.Left = (width - this.Width) / 2+70;
			//Thread.CurrentThread.CurrentCulture = new CultureInfo("he");
			
			txtInfo.Focus();
		}

#region Button Events
		private void Close_ToolWindow_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		public  void Add_Item_Click(object sender, RoutedEventArgs e )
		{		
				if (txtId.Text=="" || txtInfo.Text == "" ||  txtName_Family.Text =="" || txtAdress.Text =="" || txtTelephone.Text =="" ||  txtTime_Delivery.Text ==""|| 
					txtCost.Text=="" || txtBalance.Text=="" )  
				{	MessageBox.Show("Input empty"); txtInfo.Focus();	}
			else //
			{ 
				DateTime date_order; 
				if (clndr_Date_Order.SelectedDate != null)
					date_order = clndr_Date_Order.SelectedDate.Value.Date; //Calendar.SelectedDate.Value.Date.ToString();
				//date_order = Calendar.SelectedDate.Value.Day.ToString() + "/" + Calendar.SelectedDate.Value.Month.ToString() + "/" + Calendar.SelectedDate.Value.Year.ToString();
				else date_order =DateTime.Now ;
					Item_Furnishings.AddItem(txtId, txtInfo ,txtName_Family , txtAdress , txtTelephone , cmbSeller_Name, txtTime_Delivery, txtCost ,txtBalance, date_order);
					writeExcel.AddItemToDataSet(Item_Furnishings.str_ItemData); 
					//writeExcel.writeItemToExcel(Item_Furnishings.str_ItemData);
					txtId.Text= txtInfo.Text = txtName_Family.Text = txtAdress.Text = txtTelephone.Text = txtTime_Delivery.Text = txtCost.Text = txtBalance.Text="";
					cmbSeller_Name.SelectedItem = null;
			}
		}
#endregion Button Events
//==================================================================
#region Events
		private void DigitsOnly_Input(object sender, TextCompositionEventArgs e)
		{
			short value;
			//while (indexPressedKey < 5)
			{
				if (!Int16.TryParse(e.Text, out value) /*& e.Text != " "*/ ) e.Handled = true;
			}		
		}
		private void TelephoneNumber_Input (object sender,  KeyEventArgs e)
		{
			KeyConverter convert = new KeyConverter();
			//string number = "";  
			if (e.Key == Key.Space || e.Key < Key.D0  & e.Key!=Key.Delete || e.Key > Key.D9 ) { goto exit;  }
			if (txtTelephone.Text.Length < 13 ||  e.Key == Key.Delete)
			{
				int i = 0; char[] chararray = new char[13];
				switch (txtTelephone.Text.Length)
				{
					case 3: txtTelephone.Text += "-"; break;
					case 7: txtTelephone.Text += "-"; break;
					case 10: txtTelephone.Text += "-"; break;
				}
				if ( e.Key == Key.Back || e.Key == Key.Delete) { txtTelephone.Text = ""; goto exit; }
				if (e.Key >= Key.D0 || e.Key <= Key.D9  || txtTelephone.Text.Length!=3 || txtTelephone.Text.Length!=7 || txtTelephone.Text.Length!=10 )
				{ txtTelephone.Text += convert.ConvertToString(null, e.Key);  }
			}		
            exit:		
			e.Handled = true;
		}
		private void NoDigits_Input(object sender, TextCompositionEventArgs e)
		{
			short value;
			if (Int16.TryParse(e.Text, out value) &  e.Text!="," )  e.Handled = true;
		}
		private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (/*e.Key == Key.Space ||*/ e.Key == Key.RightShift || e.Key == Key.LeftShift) e.Handled = true; 
		}
		private void Calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			//Calendar.Text = "ttt"; 
			//ToolsDataBase.toolData.date_receiving = Calendar.SelectedDate.Value.Day.ToString()+"/"+Calendar.SelectedDate.Value.Month.ToString()+"/"+Calendar.SelectedDate.Value.Year.ToString();
		}
		private void TxtId_LostFocus(object sender, RoutedEventArgs e)
		{
			int rowsCount = MainWindow.dataSet.Tables[0].Rows.Count;  int columnsCount = MainWindow.dataSet.Tables[0].Columns.Count;
			for (int i = 0; i < rowsCount; i++)
			{
				if (Convert.ToUInt32(MainWindow.dataSet.Tables[0].Rows[i].ItemArray[0]) == Convert.ToUInt32(txtId.Text))
				{ MessageBox.Show("ID already existing"); /*txtId.Focus();*/
					txtId.Text = ""; goto exit; }
			}
			exit:;
		}
		#endregion Events
		//private void TxtType_LostFocus(object sender, RoutedEventArgs e)
		//{
		//	if (txtType.Text == "")
		//	{
		//		e.Handled = true;
		//		txtType.Focus();
		//	}
		//	e.Handled = true;
		//	ToolsDataBase.CheckField(txtType);
		//
		//}// lost focus
	} //partial class
} //namespace

