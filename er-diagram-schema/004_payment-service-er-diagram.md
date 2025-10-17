# 💳 **Payment Service ER Diagram**

## 🎯 **Service Overview**
The Payment Service handles all payment processing and external payment provider integrations for the betting platform. It manages payment methods, processes deposits/withdrawals, and integrates with multiple payment providers with complete multi-tenant isolation.

**Note: Wallet management is handled by the separate Wallet Service.**

## 📊 **Entity Relationship Diagram**

```mermaid
erDiagram
    %% Core Tables
    TENANTS {
        uuid id PK
        varchar tenant_code UK
        varchar tenant_name
        boolean is_active
        varchar country_code
        varchar currency
        varchar language_code
        timestamp created_at
        timestamp updated_at
        integer rowVersion
    }
    
    
    PAYMENT_METHODS {
        uuid id PK
        uuid tenant_id FK
        varchar user_id FK
        varchar method_type
        varchar provider
        varchar account_number
        varchar account_name
        varchar bank_code
        varchar phone_number
        boolean is_verified
        boolean is_default
        jsonb provider_data
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    PAYMENT_TRANSACTIONS {
        uuid id PK
        uuid tenant_id FK
        uuid payment_method_id FK
        varchar user_id FK
        varchar transaction_id UK
        varchar transaction_type
        decimal amount
        varchar currency
        varchar status
        varchar provider_reference
        varchar provider_response
        jsonb provider_data
        timestamp processed_at
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    DEPOSITS {
        uuid id PK
        uuid tenant_id FK
        uuid payment_method_id FK
        varchar user_id FK
        decimal amount
        varchar currency
        varchar status
        varchar provider_reference
        varchar provider_response
        jsonb provider_data
        timestamp processed_at
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    WITHDRAWALS {
        uuid id PK
        uuid tenant_id FK
        uuid payment_method_id FK
        varchar user_id FK
        decimal amount
        varchar currency
        varchar status
        varchar provider_reference
        varchar provider_response
        jsonb provider_data
        timestamp processed_at
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    PAYMENT_PROVIDERS {
        uuid id PK
        uuid tenant_id FK
        varchar provider_name
        varchar provider_type
        varchar api_endpoint
        varchar api_key
        varchar api_secret
        jsonb configuration
        boolean is_active
        boolean is_live_mode
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    PAYMENT_PROVIDER_CONFIGURATIONS {
        uuid id PK
        uuid tenant_id FK
        uuid payment_provider_id FK
        varchar configuration_key
        text configuration_value
        boolean is_encrypted
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    TENANT_PAYMENT_CONFIGURATIONS {
        uuid id PK
        uuid tenant_id FK
        varchar payment_method
        boolean is_enabled
        varchar enable_option
        boolean deposit_enabled
        boolean withdraw_enabled
        decimal min_amount
        decimal max_amount
        boolean otp_required
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    
    
    REFUNDS {
        uuid id PK
        uuid tenant_id FK
        uuid payment_transaction_id FK
        varchar user_id FK
        decimal amount
        varchar currency
        varchar reason
        varchar status
        varchar provider_reference
        jsonb provider_data
        timestamp processed_at
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    PAYMENT_WEBHOOKS {
        uuid id PK
        uuid tenant_id FK
        uuid payment_provider_id FK
        varchar webhook_type
        jsonb payload
        varchar signature
        varchar status
        integer retry_count
        timestamp processed_at
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    PAYMENT_ANALYTICS {
        uuid id PK
        uuid tenant_id FK
        varchar user_id FK
        varchar analytics_type
        decimal total_deposits
        decimal total_withdrawals
        decimal total_transactions
        integer transaction_count
        decimal average_transaction
        timestamp period_start
        timestamp period_end
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    AUDIT_LOGS {
        uuid id PK
        uuid tenant_id FK
        varchar user_id FK
        varchar action
        varchar entity_type
        uuid entity_id FK
        jsonb old_values
        jsonb new_values
        varchar ip_address
        varchar user_agent
        integer rowVersion
        timestamp created_at
    }
    
    %% Core Relationships
    TENANTS ||--o{ PAYMENT_METHODS : "has"
    TENANTS ||--o{ PAYMENT_TRANSACTIONS : "has"
    TENANTS ||--o{ DEPOSITS : "has"
    TENANTS ||--o{ WITHDRAWALS : "has"
    TENANTS ||--o{ PAYMENT_PROVIDERS : "has"
    TENANTS ||--o{ PAYMENT_PROVIDER_CONFIGURATIONS : "has"
    TENANTS ||--o{ TENANT_PAYMENT_CONFIGURATIONS : "has"
    TENANTS ||--o{ REFUNDS : "has"
    TENANTS ||--o{ PAYMENT_WEBHOOKS : "has"
    TENANTS ||--o{ PAYMENT_ANALYTICS : "has"
    TENANTS ||--o{ AUDIT_LOGS : "has"
    
    %% Payment Method Relationships
    PAYMENT_METHODS ||--o{ PAYMENT_TRANSACTIONS : "uses"
    PAYMENT_METHODS ||--o{ DEPOSITS : "uses"
    PAYMENT_METHODS ||--o{ WITHDRAWALS : "uses"
    
    %% Payment Provider Relationships
    PAYMENT_PROVIDERS ||--o{ PAYMENT_PROVIDER_CONFIGURATIONS : "has"
    PAYMENT_PROVIDERS ||--o{ PAYMENT_TRANSACTIONS : "processes"
    PAYMENT_PROVIDERS ||--o{ PAYMENT_WEBHOOKS : "receives"
    
    %% Transaction Relationships
    PAYMENT_TRANSACTIONS ||--o{ REFUNDS : "can_have"
    
```

## 🎯 **SRS Requirements Coverage**

### **FR-017: Payment Integration and Processing** ✅
- **Payment Provider Integration** → `PAYMENT_PROVIDERS` with multiple provider support
- **Payment Method Management** → `PAYMENT_METHODS` with user payment options
- **Transaction Processing** → `PAYMENT_TRANSACTIONS` with complete lifecycle
- **Webhook Handling** → `PAYMENT_WEBHOOKS` for real-time updates
- **Provider Configuration** → `PAYMENT_PROVIDER_CONFIGURATIONS` for settings


## 🔒 **Security Features**

### **1. Multi-Tenant Isolation**
- **TenantId in every table** for complete data isolation
- **No cross-tenant data access** possible
- **Tenant-scoped queries** for performance

### **2. Payment Security**
- **Encrypted payment data** with provider-specific encryption
- **Webhook signature validation** for security
- **Audit trail** for all financial transactions
- **OTP verification** for sensitive operations

### **3. Data Integrity**
- **Transaction atomicity** with proper rollback
- **Balance consistency** with transaction validation
- **Provider integration** with retry mechanisms
- **Real-time monitoring** with webhook processing

## 🚀 **Performance Optimizations**

### **1. Indexing Strategy**
- **Primary indexes** on all ID columns
- **Composite indexes** on (tenant_id, user_id, created_at)
- **Performance indexes** on frequently queried columns
- **Transaction indexes** for payment processing

### **2. Query Optimization**
- **TenantId filtering** on all queries
- **Efficient joins** with proper foreign keys
- **Caching strategy** for payment providers
- **Real-time updates** with webhook processing

## 📊 **Complete Table Organization & Structure**

### **🏢 1. TENANT MANAGEMENT (1 table)**
- `TENANTS` - Core tenant information

#### **💳 2. PAYMENT METHODS (1 table)**
- `PAYMENT_METHODS` - User payment options

#### **🔄 3. PAYMENT PROCESSING (4 tables)**
- `PAYMENT_TRANSACTIONS` - Payment transaction tracking
- `DEPOSITS` - Deposit transaction management
- `WITHDRAWALS` - Withdrawal transaction management
- `REFUNDS` - Refund transaction management

#### **🏦 4. PAYMENT PROVIDERS (2 tables)**
- `PAYMENT_PROVIDERS` - Payment provider management
- `PAYMENT_PROVIDER_CONFIGURATIONS` - Provider-specific settings

#### **⚙️ 5. TENANT CONFIGURATIONS (1 table)**
- `TENANT_PAYMENT_CONFIGURATIONS` - Payment method settings per tenant

#### **🔗 6. INTEGRATION (2 tables)**
- `PAYMENT_WEBHOOKS` - Webhook processing
- `PAYMENT_ANALYTICS` - Payment analytics and reporting

#### **📋 7. AUDIT & LOGGING (1 table)**
- `AUDIT_LOGS` - Complete audit trail

## 🎯 **Total: 12 Tables**

### **✅ Complete Coverage:**
1. **Payment Methods** (1 table)
2. **Payment Processing** (4 tables)
3. **Payment Providers** (2 tables)
4. **Tenant Configurations** (1 table)
5. **Integration** (2 tables)
6. **Analytics** (1 table)
7. **Audit & Logging** (1 table)

### **✅ Migration Strategy:**
- **Preserve Business Logic** → Keep your current payment processing logic
- **Enhance with .NET** → Add modern microservices architecture
- **Multi-Tenant Support** → Add tenant_id to all existing patterns
- **Payment Provider Integration** → Enhance with modern payment APIs

## 🚀 **Key Features:**

### **✅ 1. Payment Method Management**
- **Multiple Payment Methods** → Bank transfer, mobile money, cards
- **User Payment Options** → Saved payment methods
- **Payment Method Verification** → Secure payment method validation
- **Payment Method Analytics** → Usage tracking and reporting

### **✅ 2. Comprehensive Payment Processing**
- **Multiple Providers** → M-Pesa, Chapa, ArifPay, etc.
- **Real-time Processing** → Webhook-based updates
- **Transaction History** → Complete audit trail
- **Provider Integration** → Seamless provider switching

### **✅ 3. Advanced Security**
- **Encrypted Data** → Sensitive payment information
- **Webhook Validation** → Signature verification
- **OTP Verification** → Two-factor authentication
- **Audit Logging** → Complete transaction tracking

### **✅ 4. Tenant-Specific Configuration**
- **Payment Method Control** → Enable/disable per tenant
- **Limit Management** → Min/max amounts per tenant
- **Provider Selection** → Available providers per tenant
- **Currency Support** → Multi-currency per tenant

---

**This Payment Service ER diagram provides complete payment processing and external payment provider integration capabilities with multi-tenant support for your betting platform!** 🎯
