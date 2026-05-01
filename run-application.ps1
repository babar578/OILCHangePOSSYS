# PowerShell Script to Run POS Application
# This script helps run the ASP.NET MVC application

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  POS Application Launcher" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$projectPath = "D:\shahazadoil+Software\Software\POS.Web"
$solutionPath = "D:\shahazadoil+Software\Software\Dock27POS.sln"

# Check if solution exists
if (-not (Test-Path $solutionPath)) {
    Write-Host "ERROR: Solution file not found at $solutionPath" -ForegroundColor Red
    exit 1
}

Write-Host "Solution found: Dock27POS.sln" -ForegroundColor Green
Write-Host ""

# Check for Visual Studio
$vsPath = "${env:ProgramFiles}\Microsoft Visual Studio"
$vsPathx86 = "${env:ProgramFiles(x86)}\Microsoft Visual Studio"

if (Test-Path "$vsPathx86\2019\Community\Common7\IDE\devenv.exe") {
    $devenvPath = "$vsPathx86\2019\Community\Common7\IDE\devenv.exe"
    Write-Host "Found Visual Studio 2019 Community" -ForegroundColor Green
} elseif (Test-Path "$vsPathx86\2019\Professional\Common7\IDE\devenv.exe") {
    $devenvPath = "$vsPathx86\2019\Professional\Common7\IDE\devenv.exe"
    Write-Host "Found Visual Studio 2019 Professional" -ForegroundColor Green
} elseif (Test-Path "$vsPathx86\2022\Community\Common7\IDE\devenv.exe") {
    $devenvPath = "$vsPathx86\2022\Community\Common7\IDE\devenv.exe"
    Write-Host "Found Visual Studio 2022 Community" -ForegroundColor Green
} elseif (Test-Path "$vsPathx86\2022\Professional\Common7\IDE\devenv.exe") {
    $devenvPath = "$vsPathx86\2022\Professional\Common7\IDE\devenv.exe"
    Write-Host "Found Visual Studio 2022 Professional" -ForegroundColor Green
} else {
    Write-Host "Visual Studio not found in standard locations." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "OPTION 1: Open in Visual Studio" -ForegroundColor Cyan
    Write-Host "  1. Open Visual Studio" -ForegroundColor White
    Write-Host "  2. File > Open > Project/Solution" -ForegroundColor White
    Write-Host "  3. Select: $solutionPath" -ForegroundColor White
    Write-Host "  4. Press F5 to run" -ForegroundColor White
    Write-Host ""
    Write-Host "OPTION 2: Use IIS Express directly" -ForegroundColor Cyan
    Write-Host "  Application URL: https://localhost:44380/" -ForegroundColor White
    Write-Host ""
    exit 0
}

Write-Host ""
Write-Host "Opening solution in Visual Studio..." -ForegroundColor Yellow
Write-Host ""

# Open solution in Visual Studio
Start-Process $devenvPath -ArgumentList "`"$solutionPath`""

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Instructions:" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "1. Wait for Visual Studio to load the solution" -ForegroundColor White
Write-Host "2. Press F5 or click the 'IIS Express' button to run" -ForegroundColor White
Write-Host "3. The application will open at: https://localhost:44380/" -ForegroundColor White
Write-Host ""
Write-Host "Note: Make sure your database connection is configured correctly in Web.config" -ForegroundColor Yellow
Write-Host ""

