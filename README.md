Esse .exe irá converter um .txt que tenha o código ZPL gerado pela Shopee em um PDF com a etiqueta no formato A4. A conversão ocorre pelo uso de uma API do site labelzoom, ou seja, é um executável que consome essa API.

Para que ele funcione basta que coloque os arquivos a serem lidos em algum diretório que o executável esteja presente, esses arquivos precisam ter “zpl” em seu nome, ao executar ele irá ler todos e ir gerando os PDF’s com o padrão de nome “zplPDF(n)”. Por exemplo, se colocarmos dois arquivos a serem lidos na pasta, irá gerar “zplPDF1” e 'zplPDF2” em diante.
