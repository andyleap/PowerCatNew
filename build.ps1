Add-Type @'
using System;
using System.IO.Compression;
using System.IO;

namespace PowerShellTools
{
    public class Encoding
    {
        public static string Compress(string file)
        {
            string code = File.ReadAllText(file);
            byte[] data = System.Text.Encoding.Unicode.GetBytes(code);
            MemoryStream compressed = new MemoryStream();
            GZipStream compress = new GZipStream(compressed, CompressionMode.Compress, true);
            compress.Write(data, 0, data.Length);
            compress.Flush();
            compress.Close();
            return "\"" + Convert.ToBase64String(compressed.ToArray()) + "\"";
        }
    }
}
'@

Clear-Content .\PowerCat.ps1

Get-ChildItem -Filter *.cs | `
Foreach-Object{
  $content = ("$" + $_.BaseName + " = " + [PowerShellTools.Encoding]::Compress($_.FullName))
  $content | Add-Content .\PowerCat.ps1
}

Get-Content .\PowerCat-Core.ps1 | Add-Content .\PowerCat.ps1