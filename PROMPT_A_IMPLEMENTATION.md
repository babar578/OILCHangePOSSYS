# PROMPT A - Single Page Website + Lead Query System

## ✅ Implementation Complete

This document describes the implementation of the public website with lead capture functionality.

## 📋 What Was Implemented

### 1. Database Table
- **File**: `ControlDB_WebsiteLeads_Table.sql`
- **Table**: `WebsiteLeads` in ControlDB
- **Fields**: 
  - Id (UNIQUEIDENTIFIER, Primary Key)
  - FullName, Company, Email, Phone, Message
  - InterestedPlan, Source, Status
  - AssignedTo, Notes, FollowUpDate
  - Country, Language
  - CreatedAt, LastUpdated, IsActive
- **Indexes**: Created for Email, CreatedAt, Status, Source

### 2. Single-Page Website
- **File**: `POS.Web/Views/Website/Index.cshtml`
- **Route**: `/Website/Index` or `/Website`
- **Sections**:
  - Header with navigation
  - Hero section
  - Features section (6 feature cards)
  - Pricing section (Plan A: $25/month, Plan B: $150/month)
  - Demo section with credentials
  - Lead query form
  - Footer

### 3. API Endpoint
- **Controller**: `POS.Web/Controllers/WebsiteController.cs`
- **Endpoints**:
  - `POST /api/leads` - With anti-forgery token
  - `POST /api/leads/public` - Without anti-forgery token (for external forms)
  - `GET /Website/Index` - Public website page
- **Validation**: 
  - Required fields: FullName, Email, Message
  - Email format validation
  - Returns JSON response with success/error messages

### 4. Service Layer
- **File**: `POS.Utilities/Services/LeadService.cs`
- **Methods**:
  - `CreateLead(WebsiteLeadViewModel)` - Inserts new lead into ControlDB
  - `GetAllLeads()` - Retrieves all active leads (for admin portal)

### 5. ViewModel
- **File**: `POS.Utilities/ViewModel/WebsiteLeadViewModel.cs`
- **Properties**: All fields matching the database table

## 🚀 How to Use

### Step 1: Create Database Table

Execute the SQL script:
```sql
-- Run this script in SQL Server Management Studio
-- Or use sqlcmd:
sqlcmd -S localhost -U sa -P Entrum786@ -i ControlDB_WebsiteLeads_Table.sql
```

### Step 2: Access the Website

Navigate to:
```
http://localhost:44380/Website/Index
```

Or if using default routing:
```
http://localhost:44380/Website
```

### Step 3: Test Lead Submission

1. Fill out the lead form on the website
2. Submit the form
3. Check ControlDB for the new lead:
```sql
SELECT * FROM ControlDB.dbo.WebsiteLeads ORDER BY CreatedAt DESC;
```

## 📝 Demo Credentials

The demo section shows:
- **Demo URL**: `https://demo.torrosell.com` (update this in the view)
- **Username**: `demo@torrosell.com`
- **Password**: `Demo@123`

**Note**: Update the demo URL in `POS.Web/Views/Website/Index.cshtml` line with the actual demo link.

## 🔧 Configuration

### Update Demo URL

Edit `POS.Web/Views/Website/Index.cshtml` and change:
```html
<a href="https://demo.torrosell.com" target="_blank" class="btn btn-primary">
```

### API Endpoint

The API accepts POST requests to:
- `/api/leads` (requires anti-forgery token)
- `/api/leads/public` (no token required)

**Request Format**:
```json
{
  "FullName": "John Doe",
  "Company": "Acme Corp",
  "Email": "john@example.com",
  "Phone": "+1 (555) 123-4567",
  "InterestedPlan": "$150",
  "Message": "I'm interested in Plan B",
  "Source": "Web"
}
```

**Response Format**:
```json
{
  "success": true,
  "message": "Thank you! We'll contact you soon."
}
```

## 🎨 Features

### Responsive Design
- Mobile-friendly layout
- Smooth scrolling navigation
- Modern UI with gradient backgrounds
- Toastr notifications for form submissions

### Form Validation
- Client-side HTML5 validation
- Server-side validation
- Email format checking
- Required field enforcement

### Lead Tracking
- Automatic timestamp (CreatedAt)
- Source tracking (Web, Facebook, Instagram, etc.)
- Status management (New, Contacted, Qualified, etc.)
- Support for future features (AssignedTo, FollowUpDate, Notes)

## 📊 Database Schema

```sql
CREATE TABLE WebsiteLeads (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FullName NVARCHAR(250) NOT NULL,
    Company NVARCHAR(250) NULL,
    Email NVARCHAR(320) NOT NULL,
    Phone NVARCHAR(50) NULL,
    Message NVARCHAR(MAX) NOT NULL,
    InterestedPlan NVARCHAR(50) NULL,
    Source NVARCHAR(250) NULL,
    Status NVARCHAR(50) NULL DEFAULT 'New',
    AssignedTo INT NULL,
    Notes NVARCHAR(MAX) NULL,
    FollowUpDate DATETIME2 NULL,
    Country NVARCHAR(100) NULL,
    Language NVARCHAR(50) NULL DEFAULT 'en',
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    LastUpdated DATETIME2 NULL,
    IsActive BIT NOT NULL DEFAULT 1
);
```

## 🔐 Security

- Anti-forgery token support (optional for public endpoint)
- SQL parameterized queries (prevents SQL injection)
- Email validation
- Input sanitization

## 🧪 Testing

### Test Lead Submission

1. Open browser developer tools (F12)
2. Navigate to `/Website/Index`
3. Fill out the form
4. Submit and check Network tab for API response
5. Verify in database:
```sql
SELECT TOP 10 * FROM ControlDB.dbo.WebsiteLeads 
ORDER BY CreatedAt DESC;
```

### Test API Directly

```javascript
// Using jQuery
$.ajax({
    url: '/api/leads/public',
    type: 'POST',
    contentType: 'application/json',
    data: JSON.stringify({
        FullName: 'Test User',
        Email: 'test@example.com',
        Message: 'Test message',
        InterestedPlan: '$150',
        Source: 'Web'
    }),
    success: function(response) {
        console.log('Success:', response);
    }
});
```

## 📁 Files Created

1. `ControlDB_WebsiteLeads_Table.sql` - Database table script
2. `POS.Utilities/ViewModel/WebsiteLeadViewModel.cs` - ViewModel
3. `POS.Utilities/Services/LeadService.cs` - Service layer
4. `POS.Web/Controllers/WebsiteController.cs` - Controller
5. `POS.Web/Views/Website/Index.cshtml` - Single-page website

## 📁 Files Modified

1. `POS.Web/App_Start/RouteConfig.cs` - Added API route
2. `POS.Web/POS.Web.csproj` - Added WebsiteController reference

## 🎯 Next Steps (For PROMPT B)

The lead data is now ready to be displayed in the Admin Portal (PROMPT B):
- All leads stored in `ControlDB.WebsiteLeads`
- `LeadService.GetAllLeads()` method available
- Status, assignment, and follow-up fields ready for admin features

## ✅ Checklist

- [x] Database table created
- [x] Single-page website with all sections
- [x] API endpoint for lead submission
- [x] Form validation (client & server)
- [x] Responsive design
- [x] Demo section with credentials
- [x] Pricing section (Plan A & Plan B)
- [x] Lead form with all required fields
- [x] Error handling
- [x] Toastr notifications
- [x] Documentation

---

**Status**: ✅ **PROMPT A - COMPLETE**

Ready for PROMPT B (Admin Portal) implementation.

