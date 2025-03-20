using System.Net;

namespace Cloud5mins.ShortenerTools.Core.Services;

public class ShortenerToolException: Exception
{
	public HttpStatusCode StatusCode { get; set; }
	public ShortenerToolException(string message) : base(message)
	{
	}

	public ShortenerToolException(HttpStatusCode statusCode, string message) : base(message)
	{
		StatusCode = statusCode;
	}
}