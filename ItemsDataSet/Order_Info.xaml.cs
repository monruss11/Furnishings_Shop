using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace Furnishings_Shop
{
	/// <summary>
	/// Interaction logic for Order_Info.xaml
	/// </summary>
	public partial class Order_Info : Window
	{
		
		WriteReadExcel writereadExcel = new WriteReadExcel();
		DataSet ds = new DataSet(); 
		DataTable tbl_orderData = new DataTable();
		DataRow dr_Order ;
		List <string []> lst_ChoisedPositions = new List<string[]> { };
		public int index;

		public Order_Info()
		{
			InitializeComponent();
			Double width  = SystemParameters.FullPrimaryScreenWidth;
            Double height = SystemParameters.FullPrimaryScreenHeight;
			this.Top = (height - this.Height) / 2+50;
            this.Left = (width - this.Width) / 2+50;
			ds = writereadExcel.ReadFromFile();
		}

		public Order_Info(int index)
		{
			//Topmost = true;
			InitializeComponent(); 
			Double width  = SystemParameters.FullPrimaryScreenWidth;
            Double height = SystemParameters.FullPrimaryScreenHeight;
			this.Top = (height - this.Height) / 2+50;
            this.Left = (width - this.Width) / 2+50;
			//ds = writereadExcel.ReadFromFile();
			this.index = index; //this.Title = "Order #" + Convert.ToString(index) + " Info";
			tbl_orderData = MainWindow.dataSet.Tables[0]; dr_Order = tbl_orderData.Rows[index];
			this.Title =  "Order #" + Convert.ToString(dr_Order["id"]) ;
			FillTheFormOfOrder();
		}

		private void FillTheFormOfOrder()
		{
			DateTime dateOfOrder = (DateTime)dr_Order["Date_Order"];
			txt_Id.Text = dr_Order["id"].ToString();
			txt_Info.Text = dr_Order["Info"].ToString(); txt_Seller_Name.Text = dr_Order["Seller_Name"].ToString(); txt_Name.Text = dr_Order["Name"].ToString();
			txt_Adress.Text = dr_Order["adress"].ToString(); txt_Phone.Text = dr_Order["Telephone"].ToString(); txt_Cost.Text = dr_Order["cost"].ToString();
			txt_Balance.Text = dr_Order["balance"].ToString(); 
			txt_Time_Delivery.Text = dr_Order["time_delivery"].ToString();
			string date_order = dr_Order["date_order"].ToString();  txt_Date_Order.Text = date_order.Remove(10, 8);
			txt_Time_Left.Text=Convert.ToString( Convert.ToUInt32( dr_Order["Time_Delivery"]) - (DateTime.Now.Date - dateOfOrder.Date).TotalDays );
		}

		public void AcceptChangeItemDataSet(Item_Furnishings.ItemData itemData)
		{
			dr_Order["id"] = itemData.ID;
			 dr_Order["info"] = itemData.info; 
			 dr_Order["name"] = itemData.name_family;
			 dr_Order["adress"] = itemData.adress; 
			 dr_Order["telephone"] = itemData.telephone; 
			 dr_Order["seller_name"] = itemData.seller_name; 
			 dr_Order["cost"] = itemData.cost; 
			 dr_Order["balance"] = itemData.balance; 
			 dr_Order["time_delivery"] = itemData.time_delivery; 
			 dr_Order["date_order"] = itemData.date_order; 
			MainWindow.dataSet.AcceptChanges();
			
		}
		//private void  CheckInputQuery(TextBox[] positions, ComboBox[] conditions)
		//{
		//	for (int i = 0; i <= countPositions; i++)
		//	{
		//		if (positions[i].Text != "")
		//		{
		//			string[] valuesOfpositions = new string[3];
		//			switch (i)
		//			{
		//				case 0: valuesOfpositions[0]="cost" ; break;
		//				case 1: valuesOfpositions[0] = "balance"; break;
		//				case 2: valuesOfpositions[0] = "time_delivery"; break;
		//			}
		//			valuesOfpositions[2] = positions[i].Text.ToString();

		//			if (conditions[i].SelectedItem != null)
		//			{ ComboBoxItem selecteditem = (ComboBoxItem)conditions[i].SelectedItem; valuesOfpositions[1] = selecteditem.Content.ToString(); }
		//			else { MessageBox.Show("Choise Condition"); goto exit; }
		//			lst_ChoisedPositions.Add(valuesOfpositions);  //valuesOfpositions.c
		//		} //if
		//	} // for
		//	exit:;
		//}
#region Menu Button
		private void Search_MenuItem_Click(object sender, RoutedEventArgs ex)
		{
			//TextBox[] positions = { txt_CostQuery, txt_BalanceQuery, txt_TimeDeliveryQuery }; ComboBox[] conditions = { cmb_ConditionCost, cmb_ConditionBalance, cmb_ConditionTimeDelivery };
			//CheckInputQuery(positions, conditions);
			////if (lst_ChoisedPositions.Count > 0) SearchByOrderProperties(lst_ChoisedPositions);
			////else { MessageBox.Show("Check your query data"); goto exit; }
			//SearchByOrderProperties(lst_ChoisedPositions);
			//queryData.Clear();
			//queryDataAdpter.Fill(queryDataSet);
			//dgQuerySearchResult.ItemsSource = queryData.DefaultView; (dgQuerySearchResult.Columns[8] as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
exit:;
		} // Search MenuItem

		private void btn_AcceptChange_Click(object sender, RoutedEventArgs e)
		{
			try { Item_Furnishings.AcceptChangeItem(txt_Id, txt_Info, txt_Name, txt_Adress, txt_Phone, txt_Seller_Name, txt_Time_Delivery, txt_Cost, txt_Balance, txt_Date_Order); }
			catch (Exception ex) { MessageBox.Show(ex.Message.ToString() + "accept change 1"); }
			try { AcceptChangeItemDataSet(Item_Furnishings.str_ItemData); } catch (Exception ex) { MessageBox.Show(ex.Message.ToString() + "accept change 2");  }
			try { writereadExcel.writeToExcel(MainWindow.dataSet); } catch (Exception ex) { MessageBox.Show(ex.Message.ToString() + "accept change 3");  }
		}

		private void btn_PrintOrder_Click(object sender, RoutedEventArgs e)
		{
			PrintOrder printOrder = new PrintOrder(txt_Id.Text, txt_Date_Order.Text, txt_Name.Text, txt_Adress.Text , txt_Phone.Text,txt_Info.Text, txt_Time_Delivery.Text , txt_Cost.Text, txt_Balance.Text);
			printOrder.Show(); 
			//printOrder.Visibility = Visibility.Hidden;
			PrintDialog printDialog = new PrintDialog();  //Topmost = true;  //printDialog.ShowDialog(); 
			if (printDialog.ShowDialog() == true)
			{
				//printDialog.PrintDocument(printOrder, "My First Print Job");
				printDialog.PrintVisual(printOrder, "My First Print Job");
			}
		}
		//void SearchByOrderProperties(List<string[]> choisedpositions)
		//{
		//	string expression="Date_Order < #10/09/2019#";
		//	switch (choisedpositions.Count)
		//	{
		//		case 1:
		//			expression = choisedpositions[0][0] + " " + choisedpositions[0][1] + " " + choisedpositions[0][2];
		//		break;
		//		case 2:
		//			expression = choisedpositions[0][0] + " " + choisedpositions[0][1] + choisedpositions[0][2] + " and "
		//			+ choisedpositions[1][0] + " " + choisedpositions[1][1] + choisedpositions[1][2];
		//		break;
		//		case 3:
		//			expression = choisedpositions[0][0] + " " + choisedpositions[0][1] + choisedpositions[0][2] + " and "
		//			+ choisedpositions[1][0] + " " + choisedpositions[1][1] + choisedpositions[1][2] + " and "
		//			+ choisedpositions[2][0] + " " + choisedpositions[2][1] + choisedpositions[2][2];
		//		break;
		//	}
		//	DataRow[] selectedRows =  ds.Tables[0].Select(expression); // !!!!!! Query !!!!
		//		if (selectedRows.Any())
		//		{
		//			tbl_orderData= selectedRows.CopyToDataTable<DataRow>();
		//			// cmb_ConditionCost.SelectedItem=null; clear choise in comboBox
		//		}
		//}

#endregion Menu Button			
#region Events       
		private void DigitsOnly_Input(object sender, TextCompositionEventArgs e)
		{
			short value;
			if (!Int16.TryParse(e.Text, out value) ||  e.Text==" " ) e.Handled = true;
		}
		private void NoDigits_Input(object sender, TextCompositionEventArgs e)
		{
			short value;
			if (Int16.TryParse(e.Text, out value) ||  e.Text==" " ) e.Handled = true;
		}
		private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			////if (e.Key == Key.Space || e.Key == Key.RightShift || e.Key == Key.LeftShift) e.Handled = true; 
		}
		
		private void TelephoneNumber_Input (object sender,  KeyEventArgs e)
		{
			KeyConverter convert = new KeyConverter();
			//string number = "";  
			if (e.Key == Key.Space || e.Key < Key.D0  & e.Key!=Key.Delete || e.Key > Key.D9 ) { goto exit;  }
			if (txt_Phone.Text.Length < 13 ||  e.Key == Key.Delete)
			{
				int i = 0; char[] chararray = new char[13];
				switch (txt_Phone.Text.Length)
				{
					case 3: txt_Phone.Text += "-"; break;
					case 7: txt_Phone.Text += "-"; break;
					case 10: txt_Phone.Text += "-"; break;
				}
				if ( e.Key == Key.Back || e.Key == Key.Delete) { txt_Phone.Text = ""; goto exit; }
				if (e.Key >= Key.D0 || e.Key <= Key.D9  || txt_Phone.Text.Length!=3 || txt_Phone.Text.Length!=7 || txt_Phone.Text.Length!=10 )
				{ txt_Phone.Text += convert.ConvertToString(null, e.Key);  }
			}		
            exit:		
			e.Handled = true;
		}

#endregion Events
	} // class
} // namespace
