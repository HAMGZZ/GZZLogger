cd "C:\Users\Lewis\OneDrive - UTS\Projects\2020_start\GZZLogger"

dotnet clean
dotnet publish --configuration Release --self-contained true --runtime linux-x64 -v n   
dotnet publish --configuration Release --self-contained true --runtime osx-x64 -v n
dotnet publish --configuration Release --self-contained true --runtime win-x64 -v n  
dotnet publish --configuration Release --self-contained true --runtime win-x86 -v n  

Copy-Item .\Search\callsignLocationLookup.csv -Destination .\bin\Release\netcoreapp3.0\linux-x64\publish
Copy-Item .\Search\callsignLocationLookup.csv -Destination .\bin\Release\netcoreapp3.0\osx-x64\publish
Copy-Item .\Search\callsignLocationLookup.csv -Destination .\bin\Release\netcoreapp3.0\win-x64\publish
Copy-Item .\Search\callsignLocationLookup.csv -Destination .\bin\Release\netcoreapp3.0\win-x86\publish


Compress-Archive -Path .\bin\Release\netcoreapp3.0\linux-x64\publish\* -DestinationPath "C:\Users\Lewis\OneDrive - UTS\Projects\NET\CZGZZ_NET\CZ-GZZ\bin\Release\netcoreapp3.1\linux-x64\publish\wwwroot\GZZLogger-Linux-x64.zip" -Force

Compress-Archive -Path .\bin\Release\netcoreapp3.0\osx-x64\publish\* -DestinationPath "C:\Users\Lewis\OneDrive - UTS\Projects\NET\CZGZZ_NET\CZ-GZZ\bin\Release\netcoreapp3.1\linux-x64\publish\wwwroot\GZZLogger-osx-x64.zip" -Force

Compress-Archive -Path .\bin\Release\netcoreapp3.0\win-x64\publish\* -DestinationPath "C:\Users\Lewis\OneDrive - UTS\Projects\NET\CZGZZ_NET\CZ-GZZ\bin\Release\netcoreapp3.1\linux-x64\publish\wwwroot\GZZLogger-win-x64.zip" -Force

Compress-Archive -Path .\bin\Release\netcoreapp3.0\win-x86\publish\* -DestinationPath "C:\Users\Lewis\OneDrive - UTS\Projects\NET\CZGZZ_NET\CZ-GZZ\bin\Release\netcoreapp3.1\linux-x64\publish\wwwroot\GZZLogger-win-x86.zip" -Force

scp  "C:\Users\Lewis\OneDrive - UTS\Projects\NET\CZGZZ_NET\CZ-GZZ\bin\Release\netcoreapp3.1\linux-x64\publish\wwwroot\*.zip" lewis@czgzz.space:/var/www/czgzz/wwwroot/
