# PowerShell Script to Add Multi-Tenant Context Fix to ALL Report Pages
# This script adds tenant context validation at the beginning of Page_Load for each report

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Multi-Tenant Fix for ALL Report Pages" -ForegroundColor Cyan  
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

$reportsPath = "POS.Web\Reports\"

if (!(Test-Path $reportsPath)) {
    Write-Host "Error: Reports folder not found at: $reportsPath" -ForegroundColor Red
    exit
}

# The tenant context code to inject
$tenantContextFix = @'
            try
            {
                // === MULTI-TENANT: Ensure tenant context is set ===
                if (!TenantContext.HasTenant)
                {
                    var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
                    if (user == null)
                    {
                        Response.Redirect("~/Account/Login");
                        return;
                    }

                    var tenantId = Session["TenantId"] as int?;
                    if (tenantId.HasValue)
                    {
                        var tenant = TenantCache.GetTenant(tenantId.Value);
                        if (tenant != null && tenant.IsActive)
                        {
                            TenantContext.CurrentTenant = tenant;
                        }
                        else
                        {
                            Response.Redirect("~/Account/Login");
                            return;
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Account/Login");
                        return;
                    }
                }
                // === END MULTI-TENANT FIX ===

'@

$errorHandling = @'
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[REPORT_NAME] Error: {ex.Message}");
                Response.Write($"<div style='padding:20px;background:#ffebee;border:1px solid #f44336;margin:20px;'>");
                Response.Write($"<h3 style='color:#c62828;'>Report Error</h3>");
                Response.Write($"<p><strong>Message:</strong> {ex.Message}</p>");
                Response.Write($"<p><a href='/Home/Index'>Return to Dashboard</a></p>");
                Response.Write($"</div>");
            }
'@

$files = Get-ChildItem -Path $reportsPath -Filter "*.aspx.cs" -File | Where-Object { $_.Name -ne "ReportBasePage.cs" }
$updatedCount = 0
$skippedCount = 0

foreach ($file in $files) {
    try {
        Write-Host "Processing: $($file.Name)..." -NoNewline
        
        $content = Get-Content $file.FullName -Raw
        
        # Skip if already has multi-tenant fix
        if ($content -match "MULTI-TENANT.*Ensure tenant context") {
            Write-Host " [SKIPPED - Already fixed]" -ForegroundColor Green
            $skippedCount++
            continue
        }
        
        # Skip commented out files
        if ($content -match "^//using Microsoft\.Reporting") {
            Write-Host " [SKIPPED - Commented out]" -ForegroundColor Gray
            $skippedCount++
            continue
        }
        
        $reportName = $file.BaseName
        $customErrorHandling = $errorHandling -replace "REPORT_NAME", $reportName
        
        # Add using statements if missing
        if ($content -notmatch "using POS\.Utilities\.MultiTenant") {
            $content = $content -replace "(using Microsoft\.Reporting\.WebForms;)", "`$1`r`nusing POS.Utilities.MultiTenant;"
        }
        if ($content -notmatch "using POS\.Utilities\.Utilities" -and $content -notmatch "WebUtil") {
            $content = $content -replace "(using Microsoft\.Reporting\.WebForms;)", "`$1`r`nusing POS.Utilities.Utilities;"
        }
        
        # Find and replace Page_Load method start
        # Pattern: protected void Page_Load(...) { if (!IsPostBack) {
        $pattern = '(protected void Page_Load\(object sender, EventArgs e\)\s*\{\s*)(if \(!IsPostBack\))'
        $replacement = "`$1$tenantContextFix`$2"
        
        if ($content -match $pattern) {
            $content = $content -replace $pattern, $replacement
            
            # Find the closing braces and add error handling
            # Look for the last closing brace of the Page_Load method
            # This is tricky - we'll find the second-to-last } before the class closing }
            $lastBracePattern = '(\s*}\s*}\s*}\s*$)'
            $content = $content -replace $lastBracePattern, "$customErrorHandling`r`n        }`r`n    }`r`n}"
            
            # Backup
            Copy-Item -Path $file.FullName -Destination ($file.FullName + ".backup") -Force
            
            # Save
            Set-Content -Path $file.FullName -Value $content -NoNewline
            
            Write-Host " [UPDATED]" -ForegroundColor Yellow
            $updatedCount++
        }
        else {
            Write-Host " [SKIP - Pattern not found]" -ForegroundColor Gray
            $skippedCount++
        }
    }
    catch {
        Write-Host " [ERROR: $($_.Exception.Message)]" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Summary:" -ForegroundColor Cyan
Write-Host "  Files Updated: $updatedCount" -ForegroundColor Yellow
Write-Host "  Files Skipped: $skippedCount" -ForegroundColor Green
Write-Host "==========================================" -ForegroundColor Cyan

if ($updatedCount -gt 0) {
    Write-Host ""
    Write-Host "NEXT STEPS:" -ForegroundColor Cyan
    Write-Host "1. Open Visual Studio" -ForegroundColor White
    Write-Host "2. Build -> Rebuild Solution" -ForegroundColor White
    Write-Host "3. Test your reports!" -ForegroundColor White
}

Write-Host ""

