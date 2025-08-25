using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;

namespace CustomerWebSite.ViewModels
{
<<<<<<< HEAD
	public class ContactViewModel : IValidatableObject
	{

		[Display(Name = "姓名")]
		[Required(ErrorMessage ="姓名欄位不可空白")]//可以做多語系處理
		[StringLength(maximumLength:8,MinimumLength =3,ErrorMessage ="姓名至少需要三個字元" )]
		public string Name { get; set; }      //聯絡人姓名

		[Display(Name = "電子郵件")]
		[EmailAddress(ErrorMessage = "電子郵件格式錯誤")]
		public string? Email { get; set; }     //聯絡人電子郵件

		[Display(Name = "連絡電話")]
		public string? Phone { get; set; }   //聯絡電話


		//Validate方法用來實作自訂的驗證邏輯
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			//如果Email欄位有值，Phone欄位就必須有值
			if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Phone))
			{
				yield return new ValidationResult("電子郵件與聯絡電話須擇一填寫!");

				//yield return new ValidationResult("電子郵件與聯絡電話須擇一填寫!", new string[] { "Email", "Phone" });
			}
		}
=======
	public class ContactViewModel
	{

		[Display(Name="姓名")]

		public string Name { get; set; }      //聯絡人姓名

		[Display(Name = "電子信箱")]
		public string Email { get; set; }     //聯絡人電子郵件

		[Display(Name = "連絡電話")]
		public string Phone { get; set; }   //聯絡電話
>>>>>>> origin/main
	}
}