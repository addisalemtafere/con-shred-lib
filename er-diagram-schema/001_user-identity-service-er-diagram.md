# 🗄️ **User & Identity Service - Optimized ER Diagram**

## 📊 **Entity Relationship Diagram**

Based on your existing **ASP.NET Core Identity + OpenID Connect + OpenIddict** implementation, this optimized ER diagram shows the User & Identity Service database schema with multi-tenant support.

## 🎯 **Optimized Architecture Based on Your Current Implementation**
Caption: Summary of core identity and OAuth components used in this service.

### **✅ ASP.NET Core Identity Integration**
Caption: Core ASP.NET Identity features and associated tables.

| Feature | Tables / Notes |
|--------|-----------------|
| Standard Identity Tables | `ASPNET_USERS`, `ASPNET_ROLES`, `ASPNET_USER_ROLES` |
| Claims-Based Authorization | `ASPNET_USER_CLAIMS`, `ASPNET_ROLE_CLAIMS` |
| External Login Providers | `ASPNET_USER_LOGINS` |
| Token Management | `ASPNET_USER_TOKENS` |
| Multi-Tenant Support | `tenant_id` in all Identity tables |

Context notes (our sportsbook):
- `ASPNET_USER_LOGINS`: maps a bettor or staff account to an external IdP (e.g., Google/Microsoft) when SSO is enabled for backoffice.
- `ASPNET_USER_TOKENS`: app-managed, per-user tokens for flows like password reset, phone/email confirmation, and 2FA; not OAuth access tokens.

### **✅ OpenIddict Integration**
Caption: OAuth 2.0 / OpenID Connect components and tables.

| Feature | Tables / Notes |
|--------|-----------------|
| OAuth 2.0 / OpenID Connect | `OPENIDDICT_APPLICATIONS`, `OPENIDDICT_TOKENS` |
| Authorization Management | `OPENIDDICT_AUTHORIZATIONS` |
| Scope Management | `OPENIDDICT_SCOPES` |
| Multi-Tenant OAuth | `tenant_id` in all OpenIddict tables |

Context notes (our sportsbook):
- `OPENIDDICT_APPLICATIONS`: registered client apps such as the Public Web, Mobile App, Backoffice, and Machine-to-Machine clients per tenant.
- `OPENIDDICT_TOKENS`: OAuth/OIDC tokens (access/refresh/device/code) issued to clients; used by API Gateway and microservices to authenticate requests.

## 📊 **Complete Table Organization & Structure**
Caption: High-level index of tables by domain area.

### **🏢 1. TENANT MANAGEMENT TABLES**
Caption: Tables governing tenant configuration and policies.
| Table Name | Purpose | Key Fields |
|------------|---------|------------|
| `TENANTS` | Core tenant information | `id`, `tenant_code`, `tenant_name`, `is_active` |
| `TENANT_SETTINGS` | Cross-cutting tenant settings | `tenant_id`, `setting_key`, `setting_value` |
| `TENANT_CONFIGURATIONS` | Cross-cutting config (non-domain-specific) | `tenant_id`, `config_key`, `config_value` |
| `TENANT_GENERAL_CONFIGURATIONS` | General tenant settings | `tenant_id`, `country_code`, `currency`, `language_code` |

### **👤 2. ASP.NET CORE IDENTITY TABLES**
Caption: Identity user, role, and related mapping tables.
| Table Name | Purpose | Key Fields |
|------------|---------|------------|
| `ASPNET_USERS` | User accounts with tenant support | `id`, `tenant_id`, `username`, `email`, `password_hash` |
| `ASPNET_ROLES` | Role definitions per tenant | `id`, `tenant_id`, `name`, `description` |
| `ASPNET_USER_ROLES` | User-role assignments | `user_id`, `role_id`, `tenant_id` |
| `ASPNET_USER_CLAIMS` | User-specific permissions | `user_id`, `claim_type`, `claim_value`, `tenant_id` |
| `ASPNET_ROLE_CLAIMS` | Role-based permissions | `role_id`, `claim_type`, `claim_value`, `tenant_id` |
| `ASPNET_USER_LOGINS` | External login providers | `user_id`, `login_provider`, `provider_key`, `tenant_id` |
| `ASPNET_USER_TOKENS` | User authentication tokens | `user_id`, `login_provider`, `name`, `value`, `tenant_id` |

### **🔐 3. OAUTH 2.0 / OPENID CONNECT TABLES**
Caption: Summary list of OAuth-related tables.
| Table Name | Purpose | Key Fields |
|------------|---------|------------|
| `OPENIDDICT_APPLICATIONS` | OAuth client applications | `id`, `client_id`, `client_secret`, `tenant_id` |
| `OPENIDDICT_AUTHORIZATIONS` | OAuth authorizations | `id`, `application_id`, `subject`, `scopes`, `tenant_id` |
| `OPENIDDICT_SCOPES` | OAuth scopes/permissions | `id`, `name`, `description`, `tenant_id` |
| `OPENIDDICT_TOKENS` | OAuth access/refresh tokens | `id`, `application_id`, `subject`, `type`, `tenant_id` |

### **📋 4. AUDIT & LOGGING TABLES**
Caption: Summary list of auditing tables.
| Table Name | Purpose | Key Fields |
|------------|---------|------------|
| `AUDIT_LOGS` | Complete audit trail | `id`, `tenant_id`, `user_id`, `action`, `entity_type`, `old_values`, `new_values` |

## 📚 **Detailed Table Definitions**
Caption: Column-level specifications, defaults, relations, and brief remarks.

### 1) `TENANTS`
| No. | Column | Type | Default | Relation | Remarks |
|----|--------|------|---------|----------|---------|
| 1 | id | uuid | gen_random_uuid() | PK | Tenant ID |
| 2 | tenant_code | varchar | null | UK | Short unique code |
| 3 | tenant_name | varchar | null | - | Display name |
| 4 | description | text | null | - | Optional |
| 5 | is_active | boolean | true | - | Enable/disable tenant |
| 6 | rowVersion | integer | 1 | - | Concurrency token |
| 7 | created_at | timestamp | now() | - | - |
| 8 | updated_at | timestamp | now() | - | - |

### 2) `ASPNET_USERS`
| No. | Column | Type | Default | Relation | Remarks |
|----|--------|------|---------|----------|---------|
| 1 | id | varchar | uuid_generate_v7() | PK | Identity user ID |
| 2 | tenant_id | uuid | - | FK→TENANTS.id | Multi-tenant isolation |
| 3 | username | varchar | null | UK(tenant_id, normalized_username) | - |
| 4 | normalized_username | varchar | null | - | Upper-cased |
| 5 | email | varchar | null | - | - |
| 6 | normalized_email | varchar | null | - | Upper-cased |
| 7 | email_confirmed | boolean | false | - | - |
| 8 | password_hash | varchar | null | - | ASP.NET Core Identity |
| 9 | security_stamp | varchar | null | - | - |
| 10 | concurrency_stamp | varchar | null | - | - |
| 11 | phone_number | varchar | null | - | E.164 |
| 12 | phone_number_confirmed | boolean | false | - | - |
| 13 | two_factor_enabled | boolean | false | - | - |
| 14 | lockout_end | timestamp | null | - | - |
| 15 | lockout_enabled | boolean | true | - | - |
| 16 | access_failed_count | integer | 0 | - | - |
| 17 | friendly_name | varchar | null | - | - |
| 18 | job_title | varchar | null | - | - |
| 19 | full_name | varchar | null | - | - |
| 20 | configuration | varchar | null | - | JSON as string |
| 21 | is_enabled | boolean | true | - | Soft disable user |
| 22 | is_broker | boolean | false | - | For agent ecosystem |
| 23 | created_by | varchar | null | - | - |
| 24 | updated_by | varchar | null | - | - |
| 25 | created_date | timestamp | now() | - | - |
| 26 | updated_date | timestamp | now() | - | - |
| 27 | rowVersion | integer | 1 | - | Concurrency token |

### 3) `ASPNET_ROLES`
| No. | Column | Type | Default | Relation | Remarks |
|----|--------|------|---------|----------|---------|
| 1 | id | varchar | uuid_generate_v7() | PK | Role ID |
| 2 | tenant_id | uuid | - | FK→TENANTS.id | Tenant-scoped role |
| 3 | name | varchar | null | UK(tenant_id, normalized_name) | - |
| 4 | normalized_name | varchar | null | - | Upper-cased |
| 5 | concurrency_stamp | varchar | null | - | - |
| 6 | description | varchar | null | - | - |
| 7 | created_by | varchar | null | - | - |
| 8 | updated_by | varchar | null | - | - |
| 9 | created_date | timestamp | now() | - | - |
| 10 | updated_date | timestamp | now() | - | - |
| 11 | rowVersion | integer | 1 | - | Concurrency token |

### 4) `ASPNET_USER_ROLES`
| No. | Column | Type | Default | Relation | Remarks |
|----|--------|------|---------|----------|---------|
| 1 | user_id | varchar | - | FK→ASPNET_USERS.id | PK part |
| 2 | role_id | varchar | - | FK→ASPNET_ROLES.id | PK part |
| 3 | tenant_id | uuid | - | FK→TENANTS.id | Partition key |
| 4 | rowVersion | integer | 1 | - | Concurrency token |

### 5) `OPENIDDICT_APPLICATIONS`
| No. | Column | Type | Default | Relation | Remarks |
|----|--------|------|---------|----------|---------|
| 1 | id | varchar | uuid_generate_v7() | PK | Client record |
| 2 | client_id | varchar | null | UK | Public client ID |
| 3 | client_secret | varchar | null | - | Hashed if confidential |
| 4 | concurrency_stamp | varchar | null | - | - |
| 5 | consent_type | varchar | null | - | - |
| 6 | display_name | varchar | null | - | - |
| 7 | type | varchar | null | - | public/confidential |
| 8 | tenant_id | uuid | - | FK→TENANTS.id | Tenant-scoped client |
| 9 | rowVersion | integer | 1 | - | Concurrency token |

### 6) `OPENIDDICT_TOKENS`
| No. | Column | Type | Default | Relation | Remarks |
|----|--------|------|---------|----------|---------|
| 1 | id | varchar | uuid_generate_v7() | PK | Token record |
| 2 | application_id | varchar | - | FK→OPENIDDICT_APPLICATIONS.id | - |
| 3 | authorization_id | varchar | null | FK→OPENIDDICT_AUTHORIZATIONS.id | - |
| 4 | creation_date | timestamp | now() | - | - |
| 5 | expiration_date | timestamp | null | - | - |
| 6 | payload | varchar | null | - | Compact token data |
| 7 | properties | varchar | null | - | - |
| 8 | redemption_date | timestamp | null | - | - |
| 9 | reference_id | varchar | null | UK (optional) | Reference tokens |
| 10 | scopes | varchar | null | - | Space-separated |
| 11 | status | varchar | null | - | valid/revoked |
| 12 | subject | varchar | - | FK→ASPNET_USERS.id | Token owner |
| 13 | type | varchar | - | - | access/refresh/etc |
| 14 | tenant_id | uuid | - | FK→TENANTS.id | Tenant partition |
| 15 | rowVersion | integer | 1 | - | Concurrency token |

### 7) `TENANT_SETTINGS`
| No. | Column | Type | Default | Relation | Remarks |
|----|--------|------|---------|----------|---------|
| 1 | id | uuid | gen_random_uuid() | PK | - |
| 2 | tenant_id | uuid | - | FK→TENANTS.id | - |
| 3 | setting_key | varchar | - | UK(tenant_id, setting_key) | - |
| 4 | setting_value | text | null | - | Can be JSON |
| 5 | setting_type | varchar | 'string' | - | string/int/bool/json |
| 6 | description | text | null | - | - |
| 7 | is_encrypted | boolean | false | - | Secrets |
| 8 | rowVersion | integer | 1 | - | Concurrency token |
| 9 | created_at | timestamp | now() | - | - |
| 10 | updated_at | timestamp | now() | - | - |

### 8) `AUDIT_LOGS`
| No. | Column | Type | Default | Relation | Remarks |
|----|--------|------|---------|----------|---------|
| 1 | id | uuid | gen_random_uuid() | PK | - |
| 2 | tenant_id | uuid | - | FK→TENANTS.id | - |
| 3 | user_id | varchar | null | FK→ASPNET_USERS.id | Actor if any |
| 4 | action | varchar | - | - | e.g., CreateUser |
| 5 | entity_type | varchar | - | - | e.g., ASPNET_USERS |
| 6 | entity_id | varchar | null | - | Target entity key |
| 7 | old_values | jsonb | '{}' | - | Before state |
| 8 | new_values | jsonb | '{}' | - | After state |
| 9 | ip_address | inet | null | - | - |
| 10 | user_agent | text | null | - | - |
| 11 | request_id | varchar | null | - | Correlation ID |
| 12 | rowVersion | integer | 1 | - | Concurrency token |
| 13 | created_at | timestamp | now() | - | - |

### 9) `ASPNET_USER_CLAIMS`
Caption: User-specific claims
Description: Stores per-user claims for fine-grained authorization.
Purpose: Attach permissions or attributes directly to users.
| No. | Column | Type | Default | Relation | Remarks |
|----|--------|------|---------|----------|---------|
| 1 | id | int | identity | PK | - |
| 2 | user_id | varchar | - | FK→ASPNET_USERS.id | - |
| 3 | claim_type | varchar | - | - | e.g., permission |
| 4 | claim_value | varchar | - | - | e.g., Bet.Place |
| 5 | tenant_id | uuid | - | FK→TENANTS.id | Partition |
| 6 | rowVersion | integer | 1 | - | Concurrency token |

### 10) `ASPNET_ROLE_CLAIMS`
Caption: Role-based claims
Description: Claims granted to roles and inherited by members.
Purpose: Centralize permission sets per role.
| No. | Column | Type | Default | Relation | Remarks |
|----|--------|------|---------|----------|---------|
| 1 | id | int | identity | PK | - |
| 2 | role_id | varchar | - | FK→ASPNET_ROLES.id | - |
| 3 | claim_type | varchar | - | - | - |
| 4 | claim_value | varchar | - | - | - |
| 5 | tenant_id | uuid | - | FK→TENANTS.id | Partition |
| 6 | rowVersion | integer | 1 | - | Concurrency token |

### 11) `ASPNET_USER_LOGINS`
Caption: External login providers
Description: Links users to external identity providers.
Purpose: Support social logins and federated auth.
| No. | Column | Type | Default | Relation | Remarks |
|----|--------|------|---------|----------|---------|
| 1 | login_provider | varchar | - | PK part | e.g., google |
| 2 | provider_key | varchar | - | PK part | External user key |
| 3 | provider_display_name | varchar | null | - | - |
| 4 | user_id | varchar | - | FK→ASPNET_USERS.id | - |
| 5 | tenant_id | uuid | - | FK→TENANTS.id | Partition |
| 6 | rowVersion | integer | 1 | - | Concurrency token |

### 12) `ASPNET_USER_TOKENS`
Caption: User tokens
Description: Stores ancillary tokens (e.g., reset, 2FA) for users.
Purpose: Manage user-related token state.
| No. | Column | Type | Default | Relation | Remarks |
|----|--------|------|---------|----------|---------|
| 1 | user_id | varchar | - | PK part, FK→ASPNET_USERS.id | - |
| 2 | login_provider | varchar | - | PK part | - |
| 3 | name | varchar | - | PK part | e.g., ResetPassword |
| 4 | value | varchar | null | - | Token value/metadata |
| 5 | tenant_id | uuid | - | FK→TENANTS.id | Partition |
| 6 | rowVersion | integer | 1 | - | Concurrency token |

### 13) `OPENIDDICT_AUTHORIZATIONS`
Caption: OAuth authorizations
Description: User consent and authorization state for clients.
Purpose: Track grants and sessions per client/user.
| No. | Column | Type | Default | Relation | Remarks |
|----|--------|------|---------|----------|---------|
| 1 | id | varchar | uuid_generate_v7() | PK | - |
| 2 | application_id | varchar | - | FK→OPENIDDICT_APPLICATIONS.id | - |
| 3 | concurrency_stamp | varchar | null | - | - |
| 4 | scopes | varchar | null | - | Space-separated |
| 5 | status | varchar | null | - | active/revoked |
| 6 | subject | varchar | - | FK→ASPNET_USERS.id | - |
| 7 | type | varchar | - | - | permanent/ad-hoc |
| 8 | tenant_id | uuid | - | FK→TENANTS.id | Partition |
| 9 | rowVersion | integer | 1 | - | Concurrency token |

### 14) `OPENIDDICT_SCOPES`
Caption: OAuth scopes
Description: Named permissions exposed by the auth server.
Purpose: Define reusable permissions for clients.
| No. | Column | Type | Default | Relation | Remarks |
|----|--------|------|---------|----------|---------|
| 1 | id | varchar | uuid_generate_v7() | PK | - |
| 2 | name | varchar | - | UK | Scope name |
| 3 | description | varchar | null | - | - |
| 4 | descriptions | varchar | null | - | Localized JSON |
| 5 | display_name | varchar | null | - | - |
| 6 | display_names | varchar | null | - | Localized JSON |
| 7 | properties | varchar | null | - | JSON |
| 8 | resources | varchar | null | - | APIs covered |
| 9 | tenant_id | uuid | - | FK→TENANTS.id | Partition |
| 10 | rowVersion | integer | 1 | - | Concurrency token |

### 15) `TENANT_CONFIGURATIONS`
Caption: Advanced tenant configurations
Description: Arbitrary key/value tenant configs.
Purpose: Feature flags and custom settings per tenant.
| No. | Column | Type | Default | Relation | Remarks |
|----|--------|------|---------|----------|---------|
| 1 | id | uuid | gen_random_uuid() | PK | - |
| 2 | tenant_id | uuid | - | FK→TENANTS.id | - |
| 3 | config_key | varchar | - | UK(tenant_id, config_key) | - |
| 4 | config_value | text | null | - | Can be JSON |
| 5 | config_type | varchar | 'string' | - | string/int/bool/json |
| 6 | is_encrypted | boolean | false | - | Secrets |
| 7 | rowVersion | integer | 1 | - | Concurrency token |
| 8 | created_at | timestamp | now() | - | - |
| 9 | updated_at | timestamp | now() | - | - |

### 16) `TENANT_GENERAL_CONFIGURATIONS`
Caption: General tenant settings
Description: Country, currency, language, and general flags.
Purpose: Localize and govern tenant-wide behavior.
| No. | Column | Type | Default | Relation | Remarks |
|----|--------|------|---------|----------|---------|
| 1 | id | uuid | gen_random_uuid() | PK | - |
| 2 | tenant_id | uuid | - | FK→TENANTS.id | - |
| 3 | country_code | varchar | null | - | ISO-3166 |
| 4 | currency | varchar | null | - | ISO-4217 |
| 5 | language_code | varchar | null | - | BCP-47 |
| 6 | country | varchar | null | - | - |
| 7 | domain_name | varchar | null | - | - |
| 8 | contact_number | varchar | null | - | - |
| 9 | online_bet_support | boolean | true | - | - |
| 10 | offline_bet_support | boolean | true | - | - |
| 11 | offline_payout_enabled | boolean | false | - | - |
| 12 | language_supported | boolean | true | - | - |
| 13 | underage_limit | varchar | null | - | e.g., 18 |
| 14 | registration_flow | varchar | null | - | simple/advanced |
| 15 | rowVersion | integer | 1 | - | Concurrency token |
| 16 | created_at | timestamp | now() | - | - |
| 17 | updated_at | timestamp | now() | - | - |

## 🎯 **Table Relationships Summary**
Caption: High-level relationships among core entities.

### **🔗 Core Relationships:**
- **TENANTS** → Parent of all other tables (1:Many)
- **ASPNET_USERS** → Core user entity with tenant isolation
- **All tables** → Include `tenant_id` for multi-tenant isolation

### **🔗 Identity Relationships:**
- **ASPNET_USERS** ↔ **ASPNET_ROLES** (Many:Many via ASPNET_USER_ROLES)
- **ASPNET_USERS** → **ASPNET_USER_CLAIMS** (1:Many)
- **ASPNET_ROLES** → **ASPNET_ROLE_CLAIMS** (1:Many)

### **🔗 OAuth Relationships:**
- **OPENIDDICT_APPLICATIONS** → **OPENIDDICT_AUTHORIZATIONS** (1:Many)
- **OPENIDDICT_APPLICATIONS** → **OPENIDDICT_TOKENS** (1:Many)
- **OPENIDDICT_AUTHORIZATIONS** → **OPENIDDICT_TOKENS** (1:Many)

### **🔗 Tenant Relationships:**
- **TENANTS** → **TENANT_SETTINGS** (1:Many)
- **TENANTS** → **TENANT_CONFIGURATIONS** (1:Many)
- **TENANTS** → **TENANT_GENERAL_CONFIGURATIONS** (1:Many)