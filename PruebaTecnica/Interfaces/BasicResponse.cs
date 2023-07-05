using PruebaTecnica.Models;
using System;
namespace PruebaTecnica.Interfaces
{
	public interface IBasicResponse<T>
	{
		public string Message { get; set; }
		public DateTime Date { get; set; }
        public T Data { get; set; }
    }

	public class BasicResponse<T> : IBasicResponse<T>
    {
		public string Message { get; set; }
		public DateTime Date { get; set; }
        public T Data { get; set; }

    }

}

