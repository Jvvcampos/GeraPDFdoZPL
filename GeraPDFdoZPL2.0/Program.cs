using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;

class Program
{
    static string filePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

    static async Task Main(string[] args)
    {
        byte[] fileContent;
        string search = "*zpl*.txt";
        string[] files = Directory.GetFiles(filePath, search);

        if (files.Length == 0)
        {
            Console.WriteLine("Nenhum arquivo zpl encontrado na pasta!!");
            Console.WriteLine("Aperte qualquer tecla para continuar!!");
            Console.WriteLine(filePath);
            Console.ReadKey();
            return;
        }
        for (int i = 0; i < files.Length; i++)
        {
            string fileText = await File.ReadAllTextAsync(files[i]);
            fileContent = Encoding.UTF8.GetBytes(fileText);
            await BuildRequest(fileContent);
        }

        Environment.Exit(1);
    }

    static async Task BuildRequest(byte[] fileContent)
    {
        var request = (HttpWebRequest)WebRequest.Create("http://api.labelary.com/v1/printers/8dpmm/labels/4x6/");
        request.Method = "POST";
        request.Accept = "application/pdf"; // omit this line to get PNG images back
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = fileContent.Length;

        var requestStream = request.GetRequestStream();
        requestStream.Write(fileContent, 0, fileContent.Length);
        requestStream.Close();

        try
        {
            var response = (HttpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            string pdfDirectory = filePath;
            string pdfFileNamePattern = "zplPDF";
            string pdfFileExtension = ".pdf";
            int nextFileNumber = GetNextFileNumber(pdfDirectory, pdfFileNamePattern, pdfFileExtension);
            string pdfFilePath = Path.Combine(pdfDirectory, $"{pdfFileNamePattern}{nextFileNumber}{pdfFileExtension}");

            var fileStream = File.Create(pdfFilePath); // change file name for PNG images
            //var fileStream = File.Create("teste"); // change file name for PNG images
            responseStream.CopyTo(fileStream);
            responseStream.Close();
            fileStream.Close();
        }
        catch (WebException e)
        {
            Console.WriteLine("Error: {0}", e.Status);
        }



        //await SendJsonAndSavePdf(pdfFilePath);
    }

    static int GetNextFileNumber(string directory, string fileNamePattern, string fileExtension)
    {
        var existingFiles = Directory.GetFiles(directory, $"{fileNamePattern}*{fileExtension}")
                                     .Select(Path.GetFileNameWithoutExtension)
                                     .Where(name => name.StartsWith(fileNamePattern))
                                     .Select(name => name.Substring(fileNamePattern.Length))
                                     .Select(number => int.TryParse(number, out int n) ? n : 0)
                                     .ToList();

        return existingFiles.Count > 0 ? existingFiles.Max() + 1 : 1;
    }
    /*
    static async Task SendJsonAndSavePdf(string pdfFilePath)
    {

        using (HttpClient client = new HttpClient())
        {
            var content = new StringContent(jsonContent, Encoding.UTF8, "text/plain");
            HttpResponseMessage response = await client.PostAsync("https://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/", content);

            if (response.IsSuccessStatusCode)
            {
                byte[] pdfBytes = await response.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync(pdfFilePath, pdfBytes);
                Console.WriteLine("PDF salvo com sucesso!");
            }
            else
            {
                Console.WriteLine("Erro ao enviar o JSON: " + response.ReasonPhrase);
                Console.WriteLine("Tente novamente ou tente outro arquivo. Lembre-se de teclar qualquer tecla para encerar");
                Console.ReadKey();
            }
        }
    }*/
}
