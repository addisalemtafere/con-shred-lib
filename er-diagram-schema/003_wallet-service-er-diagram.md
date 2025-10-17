# 💰 **Wallet Service ER Diagram**

## 🎯 **Service Overview**
The Wallet Service handles all wallet management, balance tracking, and internal financial transactions for the betting platform. It manages user balances with multiple balance types and handles wallet-to-wallet transfers with complete multi-tenant isolation.

**Note: External payment processing is handled by the separate Payment Service.**

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
    
    WALLETS {
        uuid id PK
        uuid tenant_id FK
        varchar user_id FK
        decimal balance
        decimal payable_balance
        decimal deductable_balance
        decimal nonwithdrawable_balance
        decimal bonus_balance
        decimal locked_balance
        varchar currency
        boolean is_active
        timestamp last_updated
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    WALLET_TRANSACTIONS {
        uuid id PK
        uuid tenant_id FK
        uuid wallet_id FK
        varchar user_id FK
        varchar transaction_type
        decimal amount
        decimal balance_before
        decimal balance_after
        decimal payable_before
        decimal payable_after
        decimal deductable_before
        decimal deductable_after
        decimal nonwithdrawable_before
        decimal nonwithdrawable_after
        varchar reference_id
        varchar description
        varchar status
        jsonb metadata
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    WALLET_HISTORY {
        uuid id PK
        uuid tenant_id FK
        uuid wallet_id FK
        decimal balance
        decimal payable
        decimal deductable
        decimal nonwithdrawable
        decimal bonus
        decimal locked
        timestamp recorded_at
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    TRANSFER_REQUESTS {
        uuid id PK
        uuid tenant_id FK
        varchar from_user_id FK
        varchar to_user_id FK
        decimal amount
        varchar currency
        varchar status
        varchar reference_id
        text notes
        timestamp processed_at
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    WALLET_TRANSFERS {
        uuid id PK
        uuid tenant_id FK
        uuid from_wallet_id FK
        uuid to_wallet_id FK
        decimal amount
        decimal from_payable
        decimal to_payable
        varchar status
        varchar reference_id
        text notes
        timestamp processed_at
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    WALLET_TEMP_LOGS {
        uuid id PK
        uuid tenant_id FK
        uuid wallet_id FK
        decimal amount
        varchar transaction_type
        varchar status
        jsonb metadata
        timestamp expires_at
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    MEMBER_CLAIMS {
        uuid id PK
        uuid tenant_id FK
        varchar user_id FK
        decimal amount
        varchar claim_type
        varchar status
        varchar description
        timestamp claim_date
        timestamp processed_at
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    WALLET_BALANCE_PARTITIONS {
        uuid id PK
        uuid tenant_id FK
        uuid wallet_id FK
        varchar partition_type
        decimal amount
        decimal locked_amount
        varchar status
        timestamp expires_at
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    WALLET_ANALYTICS {
        uuid id PK
        uuid tenant_id FK
        varchar user_id FK
        varchar analytics_type
        decimal total_balance
        decimal total_payable
        decimal total_deductable
        decimal total_nonwithdrawable
        decimal total_bonus
        decimal total_locked
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
    TENANTS ||--o{ WALLETS : "has"
    TENANTS ||--o{ WALLET_TRANSACTIONS : "has"
    TENANTS ||--o{ WALLET_HISTORY : "has"
    TENANTS ||--o{ TRANSFER_REQUESTS : "has"
    TENANTS ||--o{ WALLET_TRANSFERS : "has"
    TENANTS ||--o{ WALLET_TEMP_LOGS : "has"
    TENANTS ||--o{ MEMBER_CLAIMS : "has"
    TENANTS ||--o{ WALLET_BALANCE_PARTITIONS : "has"
    TENANTS ||--o{ WALLET_ANALYTICS : "has"
    TENANTS ||--o{ AUDIT_LOGS : "has"
    
    %% Wallet Relationships
    WALLETS ||--o{ WALLET_TRANSACTIONS : "has"
    WALLETS ||--o{ WALLET_HISTORY : "has"
    WALLETS ||--o{ WALLET_TRANSFERS : "from_wallet"
    WALLETS ||--o{ WALLET_TRANSFERS : "to_wallet"
    WALLETS ||--o{ WALLET_TEMP_LOGS : "has"
    WALLETS ||--o{ WALLET_BALANCE_PARTITIONS : "has"
    
    %% Transfer Relationships
    TRANSFER_REQUESTS ||--o{ WALLET_TRANSFERS : "creates"
    WALLET_TRANSFERS ||--o{ WALLET_TRANSACTIONS : "creates"
```

## 🎯 **SRS Requirements Coverage**

### **FR-018: Wallet Management System** ✅
- **Wallet Creation** → `WALLETS` with balance partitions
- **Balance Management** → Multiple balance types (payable, deductable, nonwithdrawable)
- **Transaction History** → `WALLET_TRANSACTIONS` with complete audit trail
- **Transfer System** → `WALLET_TRANSFERS` for user-to-user transfers
- **Balance Analytics** → `WALLET_ANALYTICS` for reporting

## 🔒 **Security Features**

### **1. Multi-Tenant Isolation**
- **TenantId in every table** for complete data isolation
- **No cross-tenant data access** possible
- **Tenant-scoped queries** for performance

### **2. Wallet Security**
- **Balance validation** with transaction atomicity
- **Audit trail** for all wallet operations
- **Temporary logs** for pending transactions
- **Balance partitions** for different fund types

### **3. Data Integrity**
- **Transaction atomicity** with proper rollback
- **Balance consistency** with transaction validation
- **Historical tracking** with wallet history
- **Real-time monitoring** with analytics

## 🚀 **Performance Optimizations**

### **1. Indexing Strategy**
- **Primary indexes** on all ID columns
- **Composite indexes** on (tenant_id, user_id, created_at)
- **Performance indexes** on frequently queried columns
- **Balance indexes** for wallet operations

### **2. Query Optimization**
- **TenantId filtering** on all queries
- **Efficient joins** with proper foreign keys
- **Caching strategy** for wallet balances
- **Real-time updates** with transaction processing

## 📊 **Complete Table Organization & Structure**

### **🏢 1. TENANT MANAGEMENT (1 table)**
- `TENANTS` - Core tenant information

#### **💰 2. WALLET MANAGEMENT (2 tables)**
- `WALLETS` - User wallet with balance partitions
- `WALLET_TRANSACTIONS` - Complete transaction history

#### **📊 3. WALLET HISTORY (1 table)**
- `WALLET_HISTORY` - Historical balance snapshots

#### **🔄 4. TRANSFER SYSTEM (2 tables)**
- `TRANSFER_REQUESTS` - Transfer request management
- `WALLET_TRANSFERS` - Wallet-to-wallet transfers

#### **⏳ 5. TEMPORARY OPERATIONS (1 table)**
- `WALLET_TEMP_LOGS` - Temporary transaction logs

#### **🎁 6. CLAIMS SYSTEM (1 table)**
- `MEMBER_CLAIMS` - User claim management

#### **🔧 7. BALANCE PARTITIONS (1 table)**
- `WALLET_BALANCE_PARTITIONS` - Advanced balance management

#### **📈 8. ANALYTICS (1 table)**
- `WALLET_ANALYTICS` - Wallet analytics and reporting

#### **📋 9. AUDIT & LOGGING (1 table)**
- `AUDIT_LOGS` - Complete audit trail

## 🎯 **Total: 10 Tables**

### **✅ Complete Coverage:**
1. **Core Wallet Management** (2 tables)
2. **Wallet History** (1 table)
3. **Transfer System** (2 tables)
4. **Temporary Operations** (1 table)
5. **Claims System** (1 table)
6. **Balance Partitions** (1 table)
7. **Analytics** (1 table)
8. **Audit & Logging** (1 table)

### **✅ Migration Strategy:**
- **Preserve Business Logic** → Keep your current wallet and balance logic
- **Enhance with .NET** → Add modern microservices architecture
- **Multi-Tenant Support** → Add tenant_id to all existing patterns
- **Balance Management** → Enhance with modern balance partitioning

## 🚀 **Key Features:**

### **✅ 1. Multi-Balance Wallet System**
- **Main Balance** → Total available balance
- **Payable Balance** → Available for betting
- **Deductable Balance** → Available for withdrawals
- **Non-withdrawable Balance** → Bonus/restricted funds
- **Bonus Balance** → Promotional funds
- **Locked Balance** → Temporarily unavailable funds

### **✅ 2. Advanced Balance Management**
- **Balance Partitions** → Advanced fund categorization
- **Temporary Logs** → Pending transaction management
- **Historical Tracking** → Complete balance history
- **Claims System** → User claim management

### **✅ 3. Transfer System**
- **User-to-User Transfers** → Internal transfer system
- **Wallet-to-Wallet** → Direct wallet transfers
- **Transfer Requests** → Transfer request management
- **Transfer Analytics** → Transfer reporting

### **✅ 4. Security & Audit**
- **Transaction Atomicity** → All-or-nothing transactions
- **Balance Validation** → Consistent balance tracking
- **Audit Logging** → Complete transaction history
- **Temporary Operations** → Secure pending transactions

---

**This Wallet Service ER diagram provides complete wallet management, balance tracking, and internal financial transaction capabilities with multi-tenant support for your betting platform!** 🎯
