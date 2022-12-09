using System.Text;
using TestCargowise;
using System.IO;
using System.Net;
using System.IO.Compression;

Console.WriteLine("Incio");
DateTime dateTime = new DateTime();
dateTime = DateTime.Now;
string strDate = Convert.ToDateTime(dateTime).ToString("yyMMdd");

string send = @"C:\Users\JUANCAMILONARVAEZCAI\source\repos\TestCargowise\TestCargowise\XMLs\Send\Request.xml";
string returned = @"C:\Users\JUANCAMILONARVAEZCAI\source\repos\TestCargowise\TestCargowise\XMLs\Return\Response" + strDate + ".xml";

    var uri = new Uri("https://c07tstservices.wisegrid.net/eAdaptor");
    var cliente = new HttpXmlClient(uri,false, "Robot", "robot");
string extract = File.ReadAllText(send);
var xmls = extract;
using (var sourseStream = new MemoryStream(Encoding.UTF8.GetBytes(xmls)))
{

	Console.WriteLine("Prueba envio CargoWise Xml");
	Thread.Sleep(2000);
	Console.WriteLine("Xml de envio:");
	Thread.Sleep(2000);
	Console.WriteLine(xmls);
	Thread.Sleep(2000);
	Console.WriteLine("Enviando a Web Service....");

	try
	{
		var response = cliente.Post(sourseStream);
		var responseStatus = response.StatusCode;

		Console.WriteLine((responseStatus == HttpStatusCode.OK ? "Codigo de respuesta " : "Error Recibido ") + ", Status:- " + (int)responseStatus + " - " + response.ReasonPhrase);

		if (response.Content != null)
		{
			var stream = response.Content.ReadAsStreamAsync().Result;

			if (response.Content.Headers.ContentEncoding.Contains("gzip", StringComparer.InvariantCultureIgnoreCase))
			{
				stream = new GZipStream(stream, CompressionMode.Decompress);
			}

			using (var reader = new StreamReader(stream))
			{
				string responsexml = reader.ReadToEnd();
				Console.WriteLine("XML de respuesta:");
				Thread.Sleep(2000);
				Console.WriteLine("<<<------------------------------------------------- Incio ------------------------------------------------->>>");
				Console.WriteLine(responsexml);
				Console.WriteLine("<<<-------------------------------------------------- Fin -------------------------------------------------->>>");
				using (StreamWriter genxml = File.AppendText(returned))
                {
					genxml.WriteLine(responsexml);
					genxml.Close();
                }
			}
		}
		Console.WriteLine("");
	}
	catch (Exception exception)
	{
		Console.WriteLine("Ha ocurrido un error");
		Console.WriteLine(exception.ToString());
		Console.WriteLine("");
	}
}
    



