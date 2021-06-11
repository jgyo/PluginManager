@echo off

echo Plugin Manager %2>ReadMe.txt
echo Author: Gil Yoder>>ReadMe.txt
echo Copyright: %date:~-4% by Gil Yoder>>ReadMe.txt
echo Support: https://github.com/jgyo/PluginManager>>ReadMe.txt
echo.>>ReadMe.txt
echo Installation Instructions:>>ReadMe.txt
echo.>>ReadMe.txt
echo 1. If a previous version of the software is installed, search for "Add or Remove Programs" to find and uninstall the program.>>ReadMe.txt
echo 2. Extract PluginManager%2.zip to a directory of your choice.>>ReadMe.txt
echo 3. From that directory, execute setup.exe.>>ReadMe.txt
echo 4. Follow its directions to install the program. You may receive warnings because the setup program is unsigned. Please ignore those warnings to continue the installation.>>ReadMe.txt
echo.>>ReadMe.txt
echo %date% %time% CT>>ReadMe.txt

"C:\Program Files\7-Zip\7z.exe" a PluginManager%2.zip *.*
explorer .
