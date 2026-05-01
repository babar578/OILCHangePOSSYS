# Debug: Reports "No Tenant Context" Issue

## Current Error
When accessing: `https://localhost:44380/Reports/CurrentStock.aspx`

Error: `No tenant context available. Ensure tenant is resolved before accessing database.`

## Root Cause Analysis

The HTTP Module (`TenantContextHttpModule`) should be setting tenant context for ALL requests, including WebForms reports (.aspx files). If you're still getting this error, one of these is happening:

### Possible Causes:

1. **HTTP Module Not Registered** - Module not in Web.config
2. **Session Not Available** - Session state issue
3. **TenantId Not in Session** - Login didn't set it properly
4. **Module Not Running** - IIS/ASP.NET pipeline issue

## Debugging Steps

### Step 1: Check if Module is Registered

Open `Web.config` and verify you see:

```xml
<system.webServer>
  <modules runAllManagedModulesForAllRequests="true">
    <add name="TenantContextHttpModule" type="POS.Web.Infrastructure.TenantContextHttpModule" />
  </modules>
</system.webServer>
```

✅ **This should already be there** from my previous fix.

### Step 2: Enable Debug Output

The module now has extensive debug logging. To see it:

**In Visual Studio**:
1. Run application in **Debug mode** (F5, not Ctrl+F5)
2. Go to **Debug → Windows → Output**
3. Look for messages starting with `[TenantModule]`

**What to Look For**:
```
[TenantModule] Tenant context set for Shahzad Oil Store on path: /reports/currentstock.aspx
```

If you see:
```
[TenantModule] WARNING: User authenticated but NO TenantId in session
```
→ The login didn't set TenantId properly

### Step 3: Test Login Sets TenantId

After logging in successfully, check session in debugger:

**Add breakpoint** in `HomeController.Index`:
```csharp
public ActionResult Index()
{
    var tenantId = Session["TenantId"]; // <- BREAKPOINT HERE
    // What is tenantId? Should be an integer (1, 2, etc.)
    
    var user = Session[WebUtil.CURRENT_USER];
    // User should not be null
    
    return View();
}
```

**Expected Values**:
- `tenantId` = 1 (or another number)
- `user` = UserViewModel object

If `tenantId` is null → Login controller has a bug

### Step 4: Add Breakpoint in HTTP Module

**Add breakpoint** in `TenantContextHttpModule.cs` line 22 (OnBeginRequest method)

**When accessing a report**:
1. Breakpoint should hit
2. Step through the code
3. Check values:
   - `user` (line 43) - should not be null
   - `tenantId` (line 63) - should have a value
   - `tenant` (line 68) - should be loaded

**If breakpoint doesn't hit** → Module not registered properly

## Quick Fix Options

### Option 1: Force Tenant Context in Reports Base Page

If you have a base page for reports, add this in `Page_Init`:

```csharp
protected override void OnInit(EventArgs e)
{
    base.OnInit(e);
    
    // Ensure tenant context
    if (!TenantContext.HasTenant)
    {
        var tenantId = Session["TenantId"] as int?;
        if (tenantId.HasValue)
        {
            var tenant = TenantCache.GetTenant(tenantId.Value);
            if (tenant != null)
            {
                TenantContext.CurrentTenant = tenant;
            }
        }
    }
}
```

### Option 2: Check Session State Configuration

In `Web.config`, ensure session is configured:

```xml
<system.web>
  <sessionState mode="InProc" timeout="900" cookieless="false" />
</system.web>
```

### Option 3: Verify Module Runs for .aspx Files

Add this to `Web.config` if module isn't running:

```xml
<system.webServer>
  <modules>
    <remove name="TenantContextHttpModule" />
    <add name="TenantContextHttpModule" type="POS.Web.Infrastructure.TenantContextHttpModule" preCondition="managedHandler" />
  </modules>
</system.webServer>
```

## Manual Test: Direct Database

To verify it's a tenant context issue (not a data issue), temporarily modify the report:

**In `CurrentStock.aspx.cs`**, add at the top of `Page_Load`:

```csharp
protected void Page_Load(object sender, EventArgs e)
{
    try
    {
        // DEBUG: Check session and tenant
        var user = Session[WebUtil.CURRENT_USER];
        var tenantId = Session["TenantId"];
        
        Response.Write($"<p>User: {user != null}</p>");
        Response.Write($"<p>TenantId: {tenantId}</p>");
        Response.Write($"<p>HasTenant: {TenantContext.HasTenant}</p>");
        
        if (!TenantContext.HasTenant && tenantId != null)
        {
            // Manually set tenant context
            var tenant = TenantCache.GetTenant((int)tenantId);
            if (tenant != null)
            {
                TenantContext.CurrentTenant = tenant;
                Response.Write($"<p>Manually set tenant: {tenant.TenantName}</p>");
            }
        }
        
        // ... rest of your code
    }
    catch (Exception ex)
    {
        Response.Write($"<p style='color:red'>Error: {ex.Message}</p>");
        Response.Write($"<p style='color:red'>Stack: {ex.StackTrace}</p>");
        throw;
    }
}
```

This will show you exactly what's happening.

## Expected Output After Fix

When you access a report, you should see in Output window:

```
[TenantModule] Tenant context set for Shahzad Oil Store on path: /reports/currentstock.aspx
```

And the report should load without errors.

## If Nothing Works

### Nuclear Option: Bypass HTTP Module for Reports

If the HTTP Module continues to fail, you can add tenant resolution directly in each report's code-behind:

**Create a base class for reports**:

```csharp
// POS.Web/Reports/ReportBasePage.cs
public class ReportBasePage : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        EnsureTenantContext();
    }
    
    private void EnsureTenantContext()
    {
        if (!TenantContext.HasTenant)
        {
            // Check if user is logged in
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
            {
                Response.Redirect("~/Account/Login");
                return;
            }
            
            // Get tenant from session
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
                }
            }
            else
            {
                Response.Redirect("~/Account/Login");
            }
        }
    }
}
```

Then make all report pages inherit from it:

```csharp
// CurrentStock.aspx.cs
public partial class CurrentStock : ReportBasePage // Changed from: Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Your existing code - tenant context is now available
    }
}
```

---

**Status**: Investigating  
**Next Step**: Run with debugger and check Output window for [TenantModule] messages

