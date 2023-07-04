using System;
namespace PruebaTecnica.Interfaces
{
	public interface IBasicResponse
	{
		public string Message { get; set; }
		public DateTime Date { get; set; }
	}

	public class BasicResponse : IBasicResponse
	{
		public string Message { get; set; }
		public DateTime Date { get; set; }
	}
}

