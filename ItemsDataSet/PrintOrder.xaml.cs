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

namespace Furnishings_Shop
{
	/// <summary>
	/// Interaction logic for PrintOrder.xaml
	/// </summary>
	public partial class PrintOrder : Window
	{
		public PrintOrder(string id, string date_order, string name, string adress, string phone, string info, string time_delivery, string cost , string balance)
		{
			
			InitializeComponent();
			Double width  = SystemParameters.FullPrimaryScreenWidth;
            Double height = SystemParameters.FullPrimaryScreenHeight;
			this.Top = (height - this.Height) / 2+50;
            this.Left = (width - this.Width) / 2;
			txt_Id.Text = id; txt_Date_Order.Text = date_order; txt_Name.Text = name; txt_Adress.Text = adress; txt_Phone.Text = phone;
			txt_Info.Text = info; txt_Time_Delivery.Text = time_delivery; txt_Cost.Text = cost; txt_Balance.Text = balance;
		}
		//private void Print_Click(object sender, RoutedEventArgs e)
		//{
		//	PrintDialog printDialog = new PrintDialog();
		//	if (printDialog.ShowDialog() == true)
		//	{
		//		printDialog.PrintVisual ( this  , "My First Print Job");
		//          }
		//}
	}
} 
