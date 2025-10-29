---
applyTo: '**'
---

# Security Standards and Guidelines for Course Registration API

## Overview
This document outlines comprehensive security standards and guidelines for the Course Registration API application. These standards are based on analysis of the current codebase and industry best practices for .NET web applications.

## Current Security Assessment
Based on codebase analysis, the following critical security gaps have been identified:
- **No authentication or authorization mechanisms**
- **Missing input validation and sanitization**
- **No HTTPS enforcement in production**
- **Plaintext password storage in User model**
- **No rate limiting or API abuse protection**
- **Missing security headers**
- **No audit logging**
- **Overly permissive CORS configuration**

## 1. Authentication and Authorization

### 1.1 JWT Authentication Implementation
- Implement JWT Bearer authentication with proper token validation
- Configure token validation parameters including issuer, audience, lifetime, and signing key validation
- Use symmetric security keys with proper configuration from app settings
- Set ClockSkew to zero for precise token expiration

### 1.2 Authorization Policies
- Create role-based authorization policies:
  - AdminOnly: Requires Admin role
  - InstructorOrAdmin: Requires Instructor or Admin roles
  - StudentAccess: Requires Student, Instructor, or Admin roles

### 1.3 Controller Protection
- All controllers must include appropriate authorization attributes
- Require authentication for all endpoints
- Use specific authorization policies for different operations
- Only instructors/admins can create courses
- Only admins can delete courses

## 2. Input Validation and Sanitization

### 2.1 FluentValidation Implementation
- Install and configure FluentValidation.AspNetCore package (version 11.3.0 or later)
- Implement comprehensive input validation for all DTOs
- Register validators in dependency injection container

### 2.2 DTO Validators
Create validators for all DTOs with the following requirements:
- **Title validation**: Required, 5-200 characters, alphanumeric with spaces and hyphens only
- **Description validation**: Required, 10-1000 characters
- **Credits validation**: Range 1-6
- **MaxCapacity validation**: Range 1-500  
- **StartDate validation**: Required, must be at least 7 days in future
- **EndDate validation**: Required, must be after StartDate
- **Custom validation**: Implement business rule validation

### 2.3 Model Annotations
- Add data annotations to domain models for additional validation
- Use Required, StringLength, Range, and RegularExpression attributes
- Implement consistent validation rules across the application

## 3. Password Security

### 3.1 Password Hashing
- Never store plaintext passwords in the database
- Use BCrypt.Net for password hashing with work factor of 12
- Implement secure password verification methods
- Hash passwords before storing in database

### 3.2 Password Policy
Implement strong password requirements:
- Minimum 8 characters length
- At least one uppercase letter
- At least one lowercase letter  
- At least one number
- At least one special character
- Validate password strength on user registration and password changes

## 4. HTTPS and Transport Security

### 4.1 HTTPS Enforcement
- Always enforce HTTPS redirection in production environments
- Implement HSTS (HTTP Strict Transport Security) headers
- Use secure connection protocols only

### 4.2 Security Headers
Add comprehensive security headers to all responses:
- X-Content-Type-Options: nosniff
- X-Frame-Options: DENY
- X-XSS-Protection: 1; mode=block
- Referrer-Policy: strict-origin-when-cross-origin
- Content-Security-Policy with restrictive directives

## 5. Database Security

### 5.1 SQL Injection Prevention
- Always use parameterized queries (Entity Framework handles this)
- Never concatenate user input directly into SQL strings
- Use LINQ queries and Entity Framework methods for database access
- Validate and sanitize all user inputs before database operations

### 5.2 Database Connection Security
- Use encrypted database connections in production
- Configure connection strings with encryption enabled
- Set TrustServerCertificate to false in production
- Implement proper connection timeouts

## 6. API Security

### 6.1 Rate Limiting
- Implement rate limiting to prevent API abuse
- Configure memory cache for rate limit storage
- Set up IP-based rate limiting policies
- Define appropriate rate limits for different endpoints

### 6.2 CORS Configuration
- Configure CORS with restrictive policies
- Specify exact allowed origins (no wildcards in production)
- Limit allowed HTTP methods to only what's needed
- Restrict allowed headers to required ones only
- Enable credentials only when necessary

### 6.3 API Versioning
- Implement proper API versioning strategy
- Use version-specific routes
- Maintain backward compatibility where possible
- Document version changes and deprecation policies

## 7. Error Handling and Information Disclosure

### 7.1 Global Exception Handler
- Implement centralized exception handling middleware
- Log detailed errors for debugging while returning generic error messages to clients
- Include trace IDs for error correlation
- Never expose sensitive information in error responses
- Return appropriate HTTP status codes

### 7.2 Model State Validation
- Always validate ModelState in controller actions
- Return BadRequest with validation errors for invalid input
- Sanitize error messages before returning to client
- Implement consistent error response format

## 8. Logging and Monitoring

### 8.1 Security Event Logging
- Log all authentication failures with usernames and IP addresses
- Log unauthorized access attempts
- Log privilege escalation attempts
- Implement structured logging for better analysis
- Include correlation IDs for request tracking

### 8.2 Audit Trail
- Implement audit logging for all sensitive operations
- Track create, update, delete operations on critical entities
- Log user actions with timestamps and IP addresses
- Store audit logs securely with integrity protection
- Implement log retention policies

## 9. Configuration Security

### 9.1 Secrets Management
- Never store secrets in source code or configuration files
- Use Azure Key Vault or similar secure secret storage
- Implement proper secret rotation policies
- Use managed identities where possible
- Encrypt sensitive configuration data

### 9.2 Environment-Specific Configuration
- Use different security configurations for different environments
- Implement stricter security settings in production
- Disable detailed error messages in production
- Configure appropriate logging levels per environment
- Restrict allowed hosts in production

## 10. Dependency Security

### 10.1 Package Security
- Regularly update NuGet packages to latest secure versions
- Enable security analyzers in project files
- Scan dependencies for known vulnerabilities
- Implement automated dependency updates where appropriate
- Review security advisories for used packages

### 10.2 Security Scanning
- Implement automated security scanning in CI/CD pipeline
- Check for vulnerable packages during build process
- Enable .NET security analyzers
- Configure warnings as errors for security issues
- Regular penetration testing and security audits

## 11. Implementation Checklist

### Immediate Actions Required:
- [ ] Implement JWT authentication and authorization
- [ ] Add FluentValidation for input validation
- [ ] Replace plaintext password storage with proper hashing
- [ ] Add security headers middleware
- [ ] Implement global exception handling
- [ ] Configure CORS restrictively
- [ ] Add rate limiting
- [ ] Implement audit logging
- [ ] Add HTTPS enforcement for production
- [ ] Configure secure appsettings for different environments

### Medium Priority:
- [ ] Implement API versioning
- [ ] Add comprehensive unit tests for security features
- [ ] Set up security scanning in CI/CD
- [ ] Implement password policy validation
- [ ] Add request/response logging
- [ ] Configure Azure Key Vault integration

### Long Term:
- [ ] Implement OAuth2/OpenID Connect
- [ ] Add multi-factor authentication
- [ ] Implement advanced threat detection
- [ ] Set up security monitoring and alerting
- [ ] Regular security audits and penetration testing

## 12. Security Testing

### 12.1 Unit Tests for Security
- Test input validation with malicious payloads
- Verify authentication and authorization logic
- Test password hashing and verification
- Validate error handling behavior
- Test rate limiting functionality

### 12.2 Integration Tests
- Test authentication workflows end-to-end
- Verify authorization policies work correctly
- Test CORS configuration
- Validate security headers are present
- Test error handling in realistic scenarios

## Conclusion

This security standards document provides comprehensive guidelines for securing the Course Registration API. Implementation should be prioritized based on the immediate actions list, with regular security reviews and updates as the application evolves.

Remember: Security is not a one-time implementation but an ongoing process that requires continuous attention and improvement.