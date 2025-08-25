using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;

namespace CustomerWebSite.ViewModels
{
	public class ContactViewModel
	{

		[Display(Name="姓名")]

		public string Name { get; set; }      //聯絡人姓名

		[Display(Name = "電子信箱")]
		public string Email { get; set; }     //聯絡人電子郵件

		[Display(Name = "連絡電話")]
		public string Phone { get; set; }   //聯絡電話
	}
}