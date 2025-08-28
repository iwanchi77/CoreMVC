using System.ComponentModel.DataAnnotations;

namespace CategoryProducts.Metadata
{
	internal class ProductMetadata
	{
		[Required(ErrorMessage ="商品名稱未填寫!")]
		[Display(Name ="商品名稱")]
		[StringLength(maximumLength:40,MinimumLength =3,ErrorMessage ="商品名稱至少3個字")]
		public string ProductName { get; set; } = null!;

		[DisplayFormat(DataFormatString ="{0:C}")]
		[Display(Name ="商品單價")]
		public decimal? UnitPrice { get; set; }

		[Display(Name ="訂購數量")]
		[Range(1,100,ErrorMessage ="{0}必須介於{1}~{2}之間")]
		public short? UnitsOnOrder { get; set; }
	}
}