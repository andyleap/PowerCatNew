function Decompress-PowerCat ($data)
{
  $dataarray = [System.Convert]::FromBase64String($data);
  $compressed = New-Object System.IO.MemoryStream @(,$dataarray);
  $decompress = New-Object System.IO.Compression.GZipStream($compressed, [System.IO.Compression.CompressionMode]::Decompress);
  $decompressed = New-Object System.IO.MemoryStream;
  $decompress.CopyTo($decompressed);
  return [System.Text.Encoding]::Unicode.GetString($decompressed.ToArray());
}

function New-Type ([string[]]$Types)
{
  ## Obtains an ICodeCompiler from a CodeDomProvider class.
  $provider = New-Object Microsoft.CSharp.CSharpCodeProvider
  ## Get the location for System.Management.Automation DLL
  $dllName = [PsObject].Assembly.Location
  ## Configure the compiler parameters
  $compilerParameters = New-Object System.CodeDom.Compiler.CompilerParameters

  $assemblies = @("System.dll", $dllName)
  $compilerParameters.ReferencedAssemblies.AddRange($assemblies)
  #   if($ReferencedAssemblies) { 
  #      $compilerParameters.ReferencedAssemblies.AddRange($ReferencedAssemblies) 
  #   }

  $compilerParameters.IncludeDebugInformation = $true
  $compilerParameters.GenerateInMemory = $true
  $compilerResults = $provider.CompileAssemblyFromSource($compilerParameters, $Types)
  if($compilerResults.Errors.Count -gt 0) {
    $compilerResults.Errors | % { Write-Error ("Error: {0}({1}):`t{2}" -f $_.FileName,$_.Line,$_.ErrorText) }
  }
  return $compilerResults.CompiledAssembly
}

$Assembly = @()
$Assembly += (Decompress-PowerCat $ProcessStream)
$Assembly += (Decompress-PowerCat $TCPStream)
$Assembly += (Decompress-PowerCat $StreamCombiner)
$Assembly += (Decompress-PowerCat $StreamConnector)
$Assembly += (Decompress-PowerCat $Core)
$Assembly | Set-Content debug.cs

New-Type $Assembly | Import-Module
