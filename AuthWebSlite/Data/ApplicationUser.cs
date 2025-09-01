using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthWebSlite.Data
{
	//ApplicationUser是Model
	public class ApplicationUser : IdentityUser
	{
		[MaxLength(3)] //限制Country的長度為3。string型態的屬性建議加上[MaxLength]Attrib
		public string? Country { get; set; } //加上?表示Country可以為空
	}
}
