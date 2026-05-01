# Multi-Tenant Testing & Validation Checklist

## Pre-Deployment Testing

### Database Setup
- [ ] ControlDB created successfully
- [ ] Tenants table has correct schema
- [ ] ControlUsers table has correct schema
- [ ] First tenant record exists (TENANT001)
- [ ] All existing users mapped to Tenant 1
- [ ] Foreign key constraints working

### Build & Compilation
- [ ] Solution builds without errors
- [ ] No missing references
- [ ] All service files updated correctly
- [ ] MultiTenant folder added to POS.Utilities project
- [ ] Filters folder added to POS.Web project
- [ ] All using statements added correctly

### Configuration
- [ ] ControlDB connection string in Web.config
- [ ] Connection string credentials correct
- [ ] POSEntities connection string updated
- [ ] FilterConfig registers TenantAuthorizationFilter
- [ ] Global.asax.cs Hangfire uses ControlDB

## Functional Testing

### Test Case 1: Basic Login Flow
**Objective**: Verify tenant resolution during login

**Steps**:
1. Navigate to `/Account/Login`
2. Enter valid username and password
3. Click Login

**Expected Results**:
- [ ] Login successful
- [ ] Redirected to dashboard
- [ ] No errors in browser console
- [ ] Session contains TenantId
- [ ] Session contains TenantName

**SQL Verification**:
```sql
-- Check tenant was resolved
SELECT * FROM ControlDB.dbo.Tenants WHERE TenantId = 1;
SELECT * FROM ControlDB.dbo.ControlUsers WHERE UserName = '<your-username>';
```

### Test Case 2: Data Access with Tenant Context
**Objective**: Verify all data operations use tenant database

**Steps**:
1. Login successfully
2. Navigate to dashboard
3. View orders, items, customers
4. Create new order
5. Update existing record
6. Delete a test record

**Expected Results**:
- [ ] All data loads correctly
- [ ] No "object not found" errors
- [ ] Data matches existing database
- [ ] CRUD operations work
- [ ] Reports generate correctly

**Code Verification**:
- Set breakpoint in any service method
- Verify `TenantContext.CurrentTenant` is not null
- Verify connection string contains correct database

### Test Case 3: Logout and Context Clearing
**Objective**: Verify tenant context is cleared on logout

**Steps**:
1. Login successfully
2. Navigate to any page
3. Click Logout
4. Verify redirect to login page

**Expected Results**:
- [ ] Redirected to login
- [ ] Session cleared
- [ ] Tenant context cleared
- [ ] Cannot access protected pages

### Test Case 4: Session Persistence
**Objective**: Verify tenant context persists across requests

**Steps**:
1. Login successfully
2. Navigate to different pages
3. Perform various operations
4. Check session state

**Expected Results**:
- [ ] Tenant context available on all pages
- [ ] No re-authentication needed
- [ ] Session timeout works correctly
- [ ] Tenant restored from session

### Test Case 5: Concurrent Users
**Objective**: Verify tenant isolation with multiple users

**Steps**:
1. Open two different browsers
2. Login with different users (same tenant)
3. Perform operations in both sessions

**Expected Results**:
- [ ] Both users can work simultaneously
- [ ] No cross-contamination of sessions
- [ ] Each has independent tenant context
- [ ] Data changes reflected for both

### Test Case 6: Password Encryption
**Objective**: Verify database passwords are encrypted

**Steps**:
1. Check Tenants table
2. Verify DBPassword column

**SQL Verification**:
```sql
USE ControlDB;
SELECT TenantId, TenantName, DBPassword FROM Tenants;
```

**Expected Results**:
- [ ] DBPassword is Base64 encoded string
- [ ] Not plain text
- [ ] Login still works (decryption working)
- [ ] Connection successful

### Test Case 7: Cache Performance
**Objective**: Verify tenant caching improves performance

**Steps**:
1. Clear cache (app restart)
2. First login - measure time
3. Second login (same user) - measure time
4. Check cache statistics

**Expected Results**:
- [ ] Second login faster than first
- [ ] Cache contains tenant info
- [ ] Cache expires after 1 hour
- [ ] Invalidation works correctly

### Test Case 8: Error Handling
**Objective**: Verify graceful error handling

**Test Scenarios**:
1. Invalid username (not in ControlUsers)
2. Inactive tenant
3. Wrong database credentials
4. Missing tenant database
5. ControlDB unavailable

**Expected Results**:
- [ ] Appropriate error messages
- [ ] No stack traces to user
- [ ] Logs contain details
- [ ] Application doesn't crash
- [ ] User redirected to login

### Test Case 9: Authorization Filter
**Objective**: Verify TenantAuthorizationFilter works

**Steps**:
1. Logout
2. Try to access `/Home/Index` directly
3. Login
4. Access protected pages

**Expected Results**:
- [ ] Redirected to login when not authenticated
- [ ] Can access pages after login
- [ ] Tenant context checked on every request
- [ ] Login action bypasses filter

### Test Case 10: Background Jobs (Hangfire)
**Objective**: Verify Hangfire works with multi-tenancy

**Steps**:
1. Check Hangfire dashboard (`/hangfire`)
2. Verify recurring jobs
3. Manually trigger job
4. Check execution logs

**Expected Results**:
- [ ] Hangfire uses ControlDB
- [ ] Jobs receive tenantId parameter
- [ ] Tenant context set correctly
- [ ] Context cleared after execution

## Multi-Tenant Isolation Testing

### Test Case 11: Create Second Tenant (Optional)
**Objective**: Verify complete tenant isolation

**Setup**:
```sql
USE ControlDB;

-- Create second tenant
INSERT INTO Tenants (TenantName, TenantCode, DBServer, DBName, DBUser, DBPassword, IsActive)
VALUES ('Test Tenant 2', 'TENANT002', 'localhost', 'TestDB2', 'sa', 'Entrum786@', 1);

-- Create test user
INSERT INTO ControlUsers (UserName, TenantId, IsActive)
VALUES ('testuser2', 2, 1);

-- Create test database
CREATE DATABASE TestDB2;
-- Copy schema from main database to TestDB2
```

**Steps**:
1. Login as Tenant 1 user
2. Create some test data
3. Logout
4. Login as Tenant 2 user (testuser2)
5. Verify cannot see Tenant 1 data
6. Create data in Tenant 2
7. Logout and login back as Tenant 1
8. Verify Tenant 2 data not visible

**Expected Results**:
- [ ] Complete data isolation
- [ ] Each tenant sees only their data
- [ ] No cross-tenant data leakage
- [ ] Connection switches correctly
- [ ] Both tenants can work independently

## Security Testing

### Test Case 12: SQL Injection Protection
**Objective**: Verify parameterized queries prevent injection

**Test Inputs**:
- Username: `admin' OR '1'='1`
- Password: `' OR '1'='1`

**Expected Results**:
- [ ] Login fails (invalid credentials)
- [ ] No SQL errors
- [ ] Queries use parameters
- [ ] No security breach

### Test Case 13: Session Hijacking Prevention
**Objective**: Verify session security

**Steps**:
1. Login successfully
2. Copy session cookie
3. Clear browser and paste cookie
4. Try to access application

**Expected Results**:
- [ ] Session validation works
- [ ] Tenant context validated
- [ ] HttpOnly cookies enabled
- [ ] Session timeout enforced

### Test Case 14: Cross-Site Request Forgery (CSRF)
**Objective**: Verify CSRF protection

**Expected Results**:
- [ ] Anti-forgery tokens on forms
- [ ] ValidateAntiForgeryToken on POST actions
- [ ] CSRF attacks prevented

## Performance Testing

### Test Case 15: Load Testing
**Objective**: Verify performance under load

**Tool**: Use JMeter, LoadRunner, or Visual Studio Load Test

**Scenarios**:
1. 10 concurrent users
2. 50 concurrent users  
3. 100 concurrent users

**Metrics to Monitor**:
- [ ] Average response time < 2 seconds
- [ ] No timeout errors
- [ ] Connection pool not exhausted
- [ ] Memory usage stable
- [ ] CPU usage acceptable
- [ ] Cache hit rate > 80%

### Test Case 16: Database Connection Pooling
**Objective**: Verify connection pooling works

**Monitoring**:
```sql
-- Check active connections
SELECT 
    DB_NAME(dbid) as DatabaseName,
    COUNT(dbid) as NumberOfConnections,
    loginame as LoginName
FROM sys.sysprocesses
WHERE dbid > 0
GROUP BY dbid, loginame;
```

**Expected Results**:
- [ ] Connections reused
- [ ] Max pool size not exceeded
- [ ] No connection leaks
- [ ] Proper connection disposal

## Reporting & Analytics

### Test Case 17: Reports with Tenant Context
**Objective**: Verify reports use correct tenant database

**Steps**:
1. Login as Tenant 1
2. Generate various reports
3. Verify data matches tenant database

**Expected Results**:
- [ ] Reports show correct data
- [ ] No data from other tenants
- [ ] Crystal Reports work correctly
- [ ] Export functions work

## Backup & Recovery

### Test Case 18: Backup Procedures
**Objective**: Verify backup strategy

**Steps**:
1. Backup ControlDB
2. Backup Tenant database
3. Simulate data loss
4. Restore from backup
5. Verify application works

**Expected Results**:
- [ ] Backups complete successfully
- [ ] Restore works correctly
- [ ] No data loss
- [ ] Application functional after restore

## Deployment Testing

### Test Case 19: Staging Deployment
**Objective**: Verify deployment to staging environment

**Steps**:
1. Deploy to staging server
2. Configure connection strings
3. Run full test suite
4. Monitor for 24 hours

**Expected Results**:
- [ ] Deployment successful
- [ ] All tests pass in staging
- [ ] No production data accessed
- [ ] Configuration correct

### Test Case 20: Production Readiness
**Objective**: Final pre-production checks

**Checklist**:
- [ ] All tests passed
- [ ] Performance acceptable
- [ ] Security validated
- [ ] Backups configured
- [ ] Monitoring in place
- [ ] Rollback plan ready
- [ ] Team trained
- [ ] Documentation complete

## Post-Deployment Validation

### Test Case 21: Production Smoke Tests
**Objective**: Verify production deployment

**Steps** (within 1 hour of deployment):
1. Login test
2. Data access test
3. CRUD operations test
4. Report generation test
5. Background job test

**Expected Results**:
- [ ] All smoke tests pass
- [ ] No errors in logs
- [ ] Performance acceptable
- [ ] Users can login

### Test Case 22: Production Monitoring (First 24 Hours)
**Metrics to Track**:
- [ ] Error rate < 0.1%
- [ ] Average response time < 2 sec
- [ ] Successful login rate > 99%
- [ ] No data leakage incidents
- [ ] Cache hit rate > 80%
- [ ] Database connection health
- [ ] Memory/CPU usage normal

## Sign-Off

### Test Summary
- Total Test Cases: 22
- Passed: ___
- Failed: ___
- Blocked: ___
- Not Executed: ___

### Critical Issues Found
1. _____________________
2. _____________________
3. _____________________

### Tested By
- Name: _____________________
- Date: _____________________
- Signature: _____________________

### Approved By
- Name: _____________________
- Date: _____________________
- Signature: _____________________

---

**Status**: Ready for Production ☐ | Needs Fixes ☐
**Deployment Date**: _____________________

