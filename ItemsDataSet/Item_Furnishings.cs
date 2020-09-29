using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Furnishings_Shop
{
	public static class Item_Furnishings
	{
		public struct  ItemData
		{
			public int ID; public string info; public string adress; public string seller_name; public string telephone; public string name_family;  public DateTime date_order;
			 public int time_delivery; public int cost; public int balance; public int moving_assembly;
		} // struct
		public static ItemData str_ItemData = new ItemData();
		public static List<ItemData> lst_ItemData  = new List<ItemData> { }; //internal int i = 0;
		//public static WriteReadXML XML = new WriteReadXML();
		//(string type, int diametr, int length, int cut_lenght, float corner_radius, int quantity_receiving, int quantity_issuing)
		public static void AddItem( TextBox id, TextBox  info, TextBox name_family,TextBox adress, TextBox telephone, ComboBox seller_name ,TextBox time_delivery, TextBox cost,TextBox balance , DateTime date_order )
		{
			str_ItemData.ID =Int32.Parse( id.Text); str_ItemData.info = info.Text; str_ItemData.adress = adress.Text; str_ItemData.telephone = telephone.Text; str_ItemData.name_family = name_family.Text;
			ComboBoxItem item = (ComboBoxItem) seller_name.SelectedItem; 	str_ItemData.seller_name =  item.Content.ToString();  
			str_ItemData.time_delivery =Int32.Parse(time_delivery.Text); str_ItemData.cost =Int32.Parse(cost.Text); str_ItemData.balance =Int32.Parse(balance.Text);
			str_ItemData.date_order = date_order;
		}
		public static void AcceptChangeItem(  TextBox id,TextBox  info, TextBox name_family,TextBox adress, TextBox telephone, TextBox seller_name ,TextBox time_delivery, TextBox cost,TextBox balance , TextBox date_order )
		{
			str_ItemData.ID =Int32.Parse( id.Text);  str_ItemData.info = info.Text; str_ItemData.adress = adress.Text; str_ItemData.telephone = telephone.Text; 
			str_ItemData.name_family = name_family.Text;
			//ComboBoxItem item = (ComboBoxItem) seller_name.SelectedItem; 	str_ItemData.seller_name =  item.Content.ToString();  
			str_ItemData.seller_name = seller_name.Text;
			str_ItemData.time_delivery =Int32.Parse(time_delivery.Text); str_ItemData.cost =Int32.Parse(cost.Text); str_ItemData.balance =Int32.Parse(balance.Text);
			str_ItemData.date_order = Convert.ToDateTime( date_order.Text);
		}

	}// class Item_Furnishings
}
