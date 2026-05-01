# PowerShell Script to Update All Report Pages to Use ReportBasePage
# This script updates all .aspx.cs files in the Reports folder to inherit from ReportBasePage

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Report Pages Multi-Tenant Update Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$reportsPath = "POS.Web\Reports\"

if (!(Test-Path $reportsPath)) {
    Write-Host "Error: Reports folder not found at: $reportsPath" -ForegroundColor Red
    Write-Host "Please run this script from the solution root directory." -ForegroundColor Red
    exit
}

Write-Host "Scanning for report files in: $reportsPath" -ForegroundColor Yellow
Write-Host ""

$files = Get-ChildItem -Path $reportsPath -Filter "*.aspx.cs" -Recurse
$updatedCount = 0
$skippedCount = 0
$errorCount = 0

foreach ($file in $files) {
    try {
        Write-Host "Processing: $($file.Name)..." -NoNewline
        
        $content = Get-Content $file.FullName -Raw -ErrorAction Stop
        
        # Skip if already using ReportBasePage
        if ($content -match ": ReportBasePage") {
            Write-Host " [SKIPPED - Already updated]" -ForegroundColor Green
            $skippedCount++
            continue
        }
        
        # Skip if it's the ReportBasePage itself
        if ($file.Name -eq "ReportBasePage.cs") {
            Write-Host " [SKIPPED - Base class]" -ForegroundColor Gray
            $skippedCount++
            continue
        }
        
        # Replace System.Web.UI.Page with ReportBasePage
        $originalContent = $content
        $content = $content -replace ": System\.Web\.UI\.Page\s*\r?\n", ": ReportBasePage`r`n"
        
        # Check if any changes were made
        if ($content -ne $originalContent) {
            # Backup original file
            $backupPath = $file.FullName + ".backup"
            Copy-Item -Path $file.FullName -Destination $backupPath -Force
            
            # Write updated content
            Set-Content -Path $file.FullName -Value $content -NoNewline
            
            Write-Host " [UPDATED]" -ForegroundColor Yellow
            $updatedCount++
        }
        else {
            Write-Host " [NO CHANGES NEEDED]" -ForegroundColor Gray
            $skippedCount++
        }
    }
    catch {
        Write-Host " [ERROR: $($_.Exception.Message)]" -ForegroundColor Red
        $errorCount++
    }
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Summary:" -ForegroundColor Cyan
Write-Host "  Files Updated: $updatedCount" -ForegroundColor Yellow
Write-Host "  Files Skipped: $skippedCount" -ForegroundColor Green
Write-Host "  Errors: $errorCount" -ForegroundColor $(if ($errorCount -gt 0) { "Red" } else { "Green" })
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

if ($updatedCount -gt 0) {
    Write-Host "IMPORTANT: Backup files created with .backup extension" -ForegroundColor Magenta
    Write-Host ""
    Write-Host "Next Steps:" -ForegroundColor Cyan
    Write-Host "  1. Open Visual Studio" -ForegroundColor White
    Write-Host "  2. Reload any modified files" -ForegroundColor White
    Write-Host "  3. Build -> Rebuild Solution" -ForegroundColor White
    Write-Host "  4. Test your reports" -ForegroundColor White
    Write-Host ""
    Write-Host "If something goes wrong, you can restore from .backup files" -ForegroundColor Yellow
}
else {
    Write-Host "No files needed updating. All reports already use ReportBasePage or no reports found." -ForegroundColor Green
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

