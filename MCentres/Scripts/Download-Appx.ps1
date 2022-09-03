function Download-AppxPackage {
[CmdletBinding()]
param (
  [string]$Uri="www.microsoft.com/store/productId/9nblggh2jhxj",
  [string]$Path = "$env:userprofile\Downloads"
)
   
  process {
    $Path = (Resolve-Path $Path).Path
    #Get Urls to download
    $WebResponse = Invoke-WebRequest -UseBasicParsing -Method 'POST' -Uri 'https://store.rg-adguard.net/api/GetFiles' -Body "type=url&url=$Uri&ring=Retail" -ContentType 'application/x-www-form-urlencoded'
    $LinksMatch = $WebResponse.Links | where {$_ -like '*.appx*'} | where {$_ -like '*_neutral_*' -or $_ -like "*_"+$env:PROCESSOR_ARCHITECTURE.Replace("AMD","X").Replace("IA","X")+"_*"} | Select-String -Pattern '(?<=a href=").+(?=" r)'
    $DownloadLinks = $LinksMatch.matches.value 

    function Resolve-NameConflict{
    #Accepts Path to a FILE and changes it so there are no name conflicts
    param(
    [string]$Path
    )
        $newPath = $Path
        if(Test-Path $Path){
            $i = 0;
            $item = (Get-Item $Path)
            while(Test-Path $newPath){
                $i += 1;
                $newPath = Join-Path $item.DirectoryName ($item.BaseName+"($i)"+$item.Extension)
            }
        }
        return $newPath
    }
    #Download Urls
    foreach($url in $DownloadLinks){
        $FileRequest = Invoke-WebRequest -Uri $url -UseBasicParsing #-Method Head
        $FileName = ($FileRequest.Headers["Content-Disposition"] | Select-String -Pattern  '(?<=filename=).+').matches.value
        $FilePath = Join-Path $Path $FileName; $FilePath = Resolve-NameConflict($FilePath)
        [System.IO.File]::WriteAllBytes($FilePath, $FileRequest.content)
        echo $FilePath
    }
  }
}