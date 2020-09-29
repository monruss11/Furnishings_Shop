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
//using ServertoDS = ToolsDataSQLServer.clsSQLServerToDataSet;

namespace Furnishings_Shop
{
	/// <summary>
	/// Interaction logic for QueryWindow.xaml
	/// </summary>
	public partial class QueryWindow : Window
	{
		WriteReadExcel writereadExcel = new WriteReadExcel();
		DataSet ds = new DataSet(); 
		DataTable queryData = new DataTable(); DataColumn dtColumn = new DataColumn();
		internal const int countPositions = 2; 
		List <string []> lst_ChoisedPositions = new List<string[]> { };
		private TextBox[] positions = new TextBox[3];
		ComboBox[] conditions = new ComboBox[2] ;
		public QueryWindow()
		{
			InitializeComponent();
			Double width  = SystemParameters.FullPrimaryScreenWidth;
            Double height = SystemParameters.FullPrimaryScreenHeight;
			this.Top = (height - this.Height) / 2+50;
            this.Left = (width - this.Width) / 2+50;
			ds = writereadExcel.ReadFromFile(); 
			dgQuerySearchResult.HorizontalContentAlignment = HorizontalAlignment.Left;
			positions[0] = txt_CostQuery; positions[1] = txt_BalanceQuery; positions[2]= txt_Telephone ;
			conditions[0] = cmb_ConditionCost; conditions[1]= cmb_ConditionBalance/*, cmb_ConditionTimeDelivery*/;
			//MainWindow.dataSet.Tables[0];
		}
		public void ColumnsProperty(DataTable table)
		{
			dgQuerySearchResult.HorizontalContentAlignment = HorizontalAlignment.Left;
			dgQuerySearchResult.ItemsSource = queryData.DefaultView;
			for (int i = 0; i < table.Columns.Count; i++)
			{
				switch (i)
				{
					case 1: dgQuerySearchResult.Columns[i].Header = "Order Info"; break;
					case 2: dgQuerySearchResult.Columns[i].Header = "Name"; break;
					case 3: dgQuerySearchResult.Columns[i].Header = "Adress"; break;
					case 4: dgQuerySearchResult.Columns[i].Header = "Telephone"; break;
					case 5: dgQuerySearchResult.Columns[i].Header = "Seller"; break;
					case 6: dgQuerySearchResult.Columns[i].Header = "Ammount"; break;
					case 7: dgQuerySearchResult.Columns[i].Header = "Ballance"; break;
					case 8: dgQuerySearchResult.Columns[i].Header = "Delivery Time"; break;
					case 9: dgQuerySearchResult.Columns[i].Header = "Date Order"; break;
				}
				//dgQuerySearchResult.Columns[i].Header = table.Columns[i].Caption;  }
				dgQuerySearchResult.Columns[0].IsReadOnly = true;
				//dgQuerySearchResult.Columns[8].
				//	????????  (dgQuerySearchResult.Columns[table.Columns.Count - 1] as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
			}
			// Format Date in Date Column
		}
		private void  CheckInputQuery(TextBox[] positions, ComboBox[] conditions)
		{
			for (int i = 0; i <= countPositions; i++)
			{
				if (positions[i].Text != "")
				{
					string[] valuesOfpositions = new string[3];
					switch (i)
					{
						case 0: valuesOfpositions[0]="cost" ; break;
						case 1: valuesOfpositions[0] = "balance"; break;
						case 2: valuesOfpositions[0] = "telephone"; break;
					}
					if (valuesOfpositions[0] == "telephone") { valuesOfpositions[2] = "'" + positions[i].Text + "'"; valuesOfpositions[1] = "="; }
					else
					{
						valuesOfpositions[2] = positions[i].Text.ToString();
						if (conditions[i].SelectedItem != null)
						{
							ComboBoxItem selecteditem = (ComboBoxItem)conditions[i].SelectedItem;
							valuesOfpositions[1] = selecteditem.Content.ToString();
						}
						else { MessageBox.Show("Choise Condition"); goto exit; }
					}
					
					
					
					//if (valuesOfpositions[0]!= "telephone")  valuesOfpositions[2] = positions[i].Text.ToString();
					//else valuesOfpositions[2] = "'"+positions[i].Text+"'";

					//if (conditions[i].SelectedItem != null)
					//{	ComboBoxItem selecteditem = (ComboBoxItem)conditions[i].SelectedItem;
					//	if (valuesOfpositions[0] != "telephone") valuesOfpositions[1] = "=";
					//	else valuesOfpositions[1] = selecteditem.Content.ToString();
					//}
					//else { MessageBox.Show("Choise Condition"); goto exit; }
					lst_ChoisedPositions.Add(valuesOfpositions);  //!!!!!  valuesOfpositions.c
				} //if
			} // for
			exit:;
		}
		void SearchByOrderProperties(List<string[]> choisedpositions)
		{
			string expression = "Date_Order < #10/09/2019#";
			switch (choisedpositions.Count)
			{
				case 1:
					expression = choisedpositions[0][0] + " " + choisedpositions[0][1] + " " + choisedpositions[0][2];
					break;
				case 2:
					expression = choisedpositions[0][0] + " " + choisedpositions[0][1] + choisedpositions[0][2] + " and "
					+ choisedpositions[1][0] + " " + choisedpositions[1][1] + choisedpositions[1][2];
					break;
				case 3:
					expression = choisedpositions[0][0] + " " + choisedpositions[0][1] + choisedpositions[0][2] + " and "
					+ choisedpositions[1][0] + " " + choisedpositions[1][1] + choisedpositions[1][2] + " and "
					+ choisedpositions[2][0] + " " + choisedpositions[2][1] + choisedpositions[2][2];
					break;
			}
			DataRow[] selectedRows = ds.Tables[0].Select(expression); // !!!!!! Query !!!!
																	  //DataRow[] selectedRows =  MainWindow.dataSet.Tables[0].Select(expression); // !!!!!! Query !!!!
			if (selectedRows.Any())
			{
				queryData = selectedRows.CopyToDataTable<DataRow>();
				// cmb_ConditionCost.SelectedItem=null; clear choise in comboBox
			}
		}

		#region Buttons Events       
		private void Search_MenuItem_Click(object sender, RoutedEventArgs ex)
		{
			//TextBox[] positions ={ txt_CostQuery, txt_BalanceQuery,  txt_Telephone};
			//ComboBox[] conditions = { cmb_ConditionCost, cmb_ConditionBalance/*, cmb_ConditionTimeDelivery*/ };
			CheckInputQuery(positions, conditions); 	SearchByOrderProperties(lst_ChoisedPositions); 	ColumnsProperty(queryData);
    	} // Search MenuItem
		private void Clear_MenuItem_Click(object sender, RoutedEventArgs e)
		{
			positions[0].Text = positions[1].Text = positions[2].Text = "";
			conditions[0].SelectedItem = conditions[1].SelectedItem = null;
			lst_ChoisedPositions.Clear(); queryData.Clear();
		}
#endregion Buttons Events
		
		private void DigitsOnly_Input(object sender, TextCompositionEventArgs e)
		{
			short value;
			if (!Int16.TryParse(e.Text, out value)) e.Handled = true;
		}
		
	} // class
} // namespace