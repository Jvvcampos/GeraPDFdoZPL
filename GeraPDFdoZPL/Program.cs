using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

class Program
{
    static string filePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

    static async Task Main(string[] args)
    {
        string fileContent = "";
        string search = "*zpl*.txt";
        string[] files = Directory.GetFiles(filePath, search);

        if(files.Length == 0)
        {
            Console.WriteLine("Nenhum arquivo zpl encontrado na pasta!!");
            Console.WriteLine("Aperte qualquer tecla para continuar!!");
            Console.WriteLine(filePath);
            Console.ReadKey();
            return;
        }
        for (int i = 0; i < files.Length; i++) 
        {
            fileContent = await File.ReadAllTextAsync(files[i]);
            await BuildJson(fileContent);
        }

        Environment.Exit(1);
    }

    static async Task BuildJson(string fileContent)
    {
        string jsonTemplate = @"
        {
            ""info"": {
                ""_postman_id"": ""ce557b23-66f9-4b35-a199-bf63d4956cea"",
                ""name"": ""LabelZoom Samples"",
                ""schema"": ""https://schema.getpostman.com/json/collection/v2.1.0/collection.json"",
                ""_exporter_id"": ""12965179""
            },
            ""item"": [
                {
                    ""name"": ""Convert ZPL to PDF"",
                    ""protocolProfileBehavior"": {
                        ""disabledSystemHeaders"": {}
                    },
                    ""request"": {
                        ""method"": ""POST"",
                        ""header"": [
                            {
                                ""key"": ""Accept"",
                                ""value"": ""text/plain"",
                                ""type"": ""text"",
                                ""disabled"": true
                            }
                        ],
                        ""body"": {
                            ""mode"": ""raw"",
                            ""raw"": ""^XA\r\n^FT20,75^A0N,28^FH\\^FDSHIP FROM:^FS\r\n^FO406,47^GB0,376,4^FS\r\n^FT426,75^A0N,28^FH\\^FDCARRIER^FS\r\n^FT20,105^A0N,28^FH\\^FD~sf_adr_id_adrnam,20~^FS\r\n^FT20,134^A0N,28^FH\\^FD~sf_adr_id_adrln1,20~^FS\r\n^FT20,162^A0N,28^FH\\^FD~sf_adr_id_adrln2,20~^FS\r\n^FT20,190^A0N,28^FH\\^FD~sf_adr_id_adrln3,20~^FS\r\n^FT426,111^A0N,34^FH\\^FD~scacod,20~^FS\r\n^FT426,160^A0N,34^FH\\^FDPro No:^FS\r\n^FT538,160^A0N,34^FH\\^FD~pronum,20~^FS\r\n^FT426,211^A0N,34^FH\\^FDB/L No:^FS\r\n^FT538,211^A0N,34^FH\\^FD~doc_num,20~^FS\r\n^FO20,225^GB771,0,4^FS\r\n^FT20,274^A0N,28^FH\\^FDSHIP TO:^FS\r\n^FT426,274^A0N,28^FH\\^FDFOR:^FS\r\n^FT479,38^AAN,28^FH\\^FDLabelZo^FS\r\n^FT20,304^A0N,28^FH\\^FD~st_adr_id_adrnam,20~^FS\r\n^FT20,333^A0N,28^FH\\^FD~st_adr_id_adrln1,20~^FS\r\n^FT20,361^A0N,28^FH\\^FD~st_adr_id_adrln2,20~^FS\r\n^FT20,389^A0N,28^FH\\^FD~st_adr_id_adrln3,20~^FS\r\n^FT426,304^A0N,28^FH\\^FD~mf_adr_id_adrnam,20~^FS\r\n^FT426,333^A0N,28^FH\\^FD~mf_adr_id_adrln1,20~^FS\r\n^FT426,361^A0N,28^FH\\^FD~mf_adr_id_adrln2,20~^FS\r\n^FT426,389^A0N,28^FH\\^FD~mf_adr_id_adrln3,20~^FS\r\n^FT426,408^A0N,34^FH\\^FD~store_num,20~^FS\r\n^FO20,424^GB771,0,4^FS\r\n^FT467,465^A0N,31^FH\\^FDDept:^FS\r\n^FT548,465^A0N,31^FH\\^FD~deptno,20~^FS\r\n^FT20,473^A0N,28^FH\\^FDSHIP TO POST:^FS\r\n^FT51,512^A0N,39^FH\\^FD(420)^FS\r\n^FT152,512^A0N,39^FH\\^FD~st_adrpsz,20~^FS\r\n^FT607,38^AAN,28^FH\\^FDom^FS\r\n^FT467,524^A0N,31^FH\\^FDPO:^FS\r\n^FT528,524^A0N,31^FH\\^FD~cponum,20~^FS\r\n^FT51,621^BY3^BCN,97,N,N,N,N^FD~st_adrpsz_bc,20~^FS\r\n^FT467,583^A0N,31^FH\\^FDOrd Type:^FS\r\n^FT22,38^AAN,28^FH\\^FDDesign labels for ^FS\r\n^FT609,583^A0N,31^FH\\^FD~ordtyp,20~^FS\r\n^FT467,644^A0N,31^FH\\^FDFor:^FS\r\n^FT528,644^A0N,31^FH\\^FD~store_num,20~^FS\r\n^FT644,38^AAN,28^FH\\^FD.net^FS\r\n^FO20,662^GB771,0,4^FS\r\n^FT351,38^AAN,28^FH\\^FDfree @ ^FS\r\n^FT20,716^A0N,34^FH\\^FDItem Number:^FS\r\n^FT41,788^A0N,67^FH\\^FD~prtnum,20~^FS\r\n^FO20,851^GB771,0,4^FS\r\n^FT20,889^A0N,28^FH\\^FDSERIAL SHIP CODE^FS\r\n^FT20,1181^BY3^BCN,246,Y,Y,N,N^FD01234567890123456789^FS\r\n^XZ\r\n"",
                            ""options"": {
                                ""raw"": {
                                    ""language"": ""text""
                                }
                            }
                        },
                        ""url"": {
                            ""raw"": ""https://www.labelzoom.net/api/v2/convert/zpl/to/pdf"",
                            ""protocol"": ""https"",
                            ""host"": [
                                ""www"",
                                ""labelzoom"",
                                ""net""
                            ],
                            ""path"": [
                                ""api"",
                                ""v2"",
                                ""convert"",
                                ""zpl"",
                                ""to"",
                                ""pdf""
                            ]
                        }
                    },
                    ""response"": []
                }
            ]
        }";

        // Carregar o JSON existente em um JObject
        JObject jsonObject = JObject.Parse(jsonTemplate);

        // Inserir o conteúdo do arquivo TXT no campo desejado
        jsonObject["item"][0]["request"]["body"]["raw"] = fileContent;

        // Converter o JObject de volta para uma string JSON
        string jsonContent = jsonObject.ToString();

        string pdfDirectory = filePath;
        string pdfFileNamePattern = "zplPDF";
        string pdfFileExtension = ".pdf";
        int nextFileNumber = GetNextFileNumber(pdfDirectory, pdfFileNamePattern, pdfFileExtension);
        string pdfFilePath = Path.Combine(pdfDirectory, $"{pdfFileNamePattern}{nextFileNumber}{pdfFileExtension}");
        await SendJsonAndSavePdf(jsonContent, pdfFilePath);
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

    static async Task SendJsonAndSavePdf(string jsonContent, string pdfFilePath)
    {
        using (HttpClient client = new HttpClient())
        {
            var content = new StringContent(jsonContent, Encoding.UTF8, "text/plain");
            HttpResponseMessage response = await client.PostAsync("https://www.labelzoom.net/api/v2/convert/zpl/to/pdf", content);

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
    }
}