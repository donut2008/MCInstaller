Download-AppxPackage
$build = [System.Environment]::OSVersion.Version.Build
$WindowsBuild = (-join($build,".",(Get-ItemProperty "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion").UBR))
$requestURI64 = (-join("https://github.com/donut2008/MCInstaller/tree/dlls/DLLs/",$WindowsBuild,"/x64/Windows.ApplicationModel.Store.dll"))
$requestURI86 = (-join("https://github.com/donut2008/MCInstaller/tree/dlls/DLLs/",$WindowsBuild,"/x86/Windows.ApplicationModel.Store.dll"))
$dest64 = (-join($env:ProgramData,"\MCInstaller\DLLs\",$WindowsBuild,"\x64\Windows.ApplicationModel.Store.dll"))
$dest86 = (-join($env:ProgramData,"\MCInstaller\DLLs\",$WindowsBuild,"\x86\Windows.ApplicationModel.Store.dll"))
Start-BitsTransfer -Source $requestURI64,$requestURI86 -Destination $dest64,$dest86
Copy-Item $dest64 -Destination "C:\Windows\System32\" -Recurse -Force
Copy-Item $dest86 -Destination "C:\Windows\SysWOW64\" -Recurse -Force
timeout /t 5
exit