using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
        }else	
        {
            Console.WriteLine($"Foram encontrados {files.Length} arquivos zpl na pasta!");
        }
        foreach (string file in files)
        {
            string FileText = await File.ReadAllTextAsync(file);
            string[] etiquetas = FileText.Split(new[] { "~DGR:DEMO"}, StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine($"\nTotal de entiquetas encontradas: {etiquetas.Length}");
            int totalGrupos = (int)Math.Ceiling(etiquetas.Length / 15.0);
            Console.WriteLine($"Serão processados {totalGrupos} grupos");

            for(int i=0; i<etiquetas.Length; i+=15)
            {
                int grupoAtual = (i/15) + 1;
                int etiquetasNesteGrupo = Math.Min(15, etiquetas.Length - i);

                Console.WriteLine($"\nProcessando grupo {grupoAtual} de {totalGrupos}");
                Console.WriteLine($"Processando etiquetas {i + 1} até {i + etiquetasNesteGrupo}");

                var grupoEtiquetas = etiquetas.Skip(i).Take(15).ToList();
                string zplGrupo = string.Join("~DGR:DEMO", grupoEtiquetas);
                if(!string.IsNullOrWhiteSpace(zplGrupo))
                {
                    zplGrupo = "~DGR:DEMO" + zplGrupo;
                }
                fileContent = Encoding.UTF8.GetBytes(zplGrupo);
                await BuildRequest(fileContent);
                // Delay entre os grupos
                await Task.Delay(3000);
            }
        }
        Console.WriteLine("\nProcessamento concluído!");
        Environment.Exit(1);
    }

    static async Task BuildRequest(byte[] fileContent)
    {
        //Montando a requisição da API
        var request = (HttpWebRequest)WebRequest.Create("http://api.labelary.com/v1/printers/8dpmm/labels/4x6/");
        request.Method = "POST";
        request.Accept = "application/pdf";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = fileContent.Length;

        var requestStream = request.GetRequestStream();
        requestStream.Write(fileContent, 0, fileContent.Length);
        requestStream.Close();

        try
        {
            var response = (HttpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            //Ajusto o nome do PDF a ser salvo e caminho
            string pdfDirectory = filePath;
            string pdfFileNamePattern = "zplPDF";
            string pdfFileExtension = ".pdf";
            int nextFileNumber = GetNextFileNumber(pdfDirectory, pdfFileNamePattern, pdfFileExtension);
            string pdfFilePath = Path.Combine(pdfDirectory, $"{pdfFileNamePattern}{nextFileNumber}{pdfFileExtension}");
            //Copio a resposta da API para o conteúdo do PDF e gravo
            var fileStream = File.Create(pdfFilePath);
            responseStream.CopyTo(fileStream);
            responseStream.Close();
            fileStream.Close();
        }
        catch (WebException e)
        {
            Console.WriteLine("Error: {0}", e.Response);
        }
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
}
