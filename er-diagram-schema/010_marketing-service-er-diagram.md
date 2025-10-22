# 🎯 **Marketing Service ER Diagram**

## 🎯 **Service Overview**
The Marketing Service handles all marketing activities, bonus management, loyalty programs, and campaign management for the betting platform. It manages user bonuses, loyalty points, marketing campaigns, and promotional activities with complete multi-tenant isolation.

## 📊 **Entity Relationship Diagram**

## 🎯 **SRS Requirements Coverage**

### **FR-019: Deposit Bonus System** ✅
- **Bonus Configuration** → `DEPOSIT_BONUS_SETTINGS` with comprehensive settings
- **Wagering Policies** → `WAGERING_POLICIES` with rollover requirements
- **User Bonuses** → `USER_BONUSES` with tracking and management
- **Bonus Descriptions** → `BONUS_DESCRIPTIONS` with localization support

### **FR-020: Free Bet System** ✅
- **Free Bet Settings** → `FREE_BET_SETTINGS` with configuration
- **User Free Bets** → `USER_FREE_BETS` with tracking and usage
- **Free Bet Management** → Complete lifecycle management

### **FR-021: Wagering Requirements** ✅
- **Wagering Policies** → `WAGERING_POLICIES` with detailed requirements
- **Contribution Tracking** → Multiple tracking sources and types
- **Eligibility Criteria** → Comprehensive eligibility rules

### **FR-022: Loyalty Program** ✅
- **Loyalty Programs** → `LOYALTY_PROGRAMS` with point systems
- **User Loyalty Points** → `USER_LOYALTY_POINTS` with balance tracking
- **Loyalty Transactions** → `LOYALTY_TRANSACTIONS` with complete history

### **FR-023: Marketing Campaign Management** ✅
- **Marketing Campaigns** → `MARKETING_CAMPAIGNS` with campaign management
- **Campaign Participants** → `CAMPAIGN_PARTICIPANTS` with participation tracking
- **UTM Tracking** → `UTM_TRACKING` with comprehensive analytics

### **FR-024: UTM Tracking and Analytics** ✅
- **UTM Tracking** → Complete UTM parameter tracking
- **Marketing Analytics** → `MARKETING_ANALYTICS` with comprehensive metrics
- **Campaign Analytics** → Detailed campaign performance tracking

### **FR-025: Marketing Tools** ✅
- **Cashback System** → `CASHBACK_SETTINGS` and `USER_CASHBACKS`
- **Referral System** → `REFERRAL_PROGRAMS` and `USER_REFERRALS`
- **Promotion Management** → `PROMOTION_DESCRIPTIONS` with descriptions

### **FR-026: Commission Structure** ✅
- **Referral Commissions** → Built into referral system
- **Agent Commissions** → Handled by Identity Service
- **Marketing Commissions** → Tracked in analytics

### **FR-027: Marketing Analytics** ✅
- **Marketing Analytics** → `MARKETING_ANALYTICS` with comprehensive metrics
- **Campaign Performance** → Detailed campaign tracking
- **User Behavior** → UTM and participation tracking

### **FR-028: CRM Features** ✅
- **User Segmentation** → Campaign targeting
- **User Engagement** → Loyalty and bonus tracking
- **User Lifecycle** → Complete marketing journey tracking

### **FR-029: Cashback Management** ✅
- **Cashback Settings** → `CASHBACK_SETTINGS` with configuration
- **User Cashbacks** → `USER_CASHBACKS` with tracking
- **Cashback Analytics** → Performance tracking

## 🔒 **Security Features**

### **1. Multi-Tenant Isolation**
- **TenantId in every table** for complete data isolation
- **No cross-tenant data access** possible
- **Tenant-scoped queries** for performance

### **2. Marketing Security**
- **Bonus validation** with comprehensive rules
- **Fraud prevention** with wagering requirements
- **Audit trail** for all marketing activities
- **UTM validation** for campaign tracking

### **3. Data Integrity**
- **Bonus consistency** with proper validation
- **Loyalty point accuracy** with transaction tracking
- **Campaign integrity** with participant validation
- **Analytics accuracy** with comprehensive tracking

## 🚀 **Performance Optimizations**

### **1. Indexing Strategy**
- **Primary indexes** on all ID columns
- **Composite indexes** on (tenant_id, user_id, created_at)
- **Performance indexes** on frequently queried columns
- **Analytics indexes** for reporting queries

### **2. Query Optimization**
- **TenantId filtering** on all queries
- **Efficient joins** with proper foreign keys
- **Caching strategy** for bonus settings
- **Real-time updates** with campaign tracking

## 📊 **Complete Table Organization & Structure**

### **🏢 1. TENANT MANAGEMENT (1 table)**
- `TENANTS` - Core tenant information

#### **🎁 2. BONUS MANAGEMENT (5 tables)**
- `BONUS_DESCRIPTIONS` - Bonus description templates
- `BONUS_DESCRIPTION_LOCALES` - Localized bonus descriptions
- `WAGERING_POLICIES` - Wagering requirements and policies
- `DEPOSIT_BONUS_SETTINGS` - Deposit bonus configurations
- `USER_BONUSES` - User bonus tracking and management

#### **🎫 3. FREE BET SYSTEM (2 tables)**
- `FREE_BET_SETTINGS` - Free bet configurations
- `USER_FREE_BETS` - User free bet tracking

#### **⭐ 4. LOYALTY SYSTEM (3 tables)**
- `LOYALTY_PROGRAMS` - Loyalty program configurations
- `USER_LOYALTY_POINTS` - User loyalty point balances
- `LOYALTY_TRANSACTIONS` - Loyalty point transaction history

#### **📢 5. CAMPAIGN MANAGEMENT (3 tables)**
- `MARKETING_CAMPAIGNS` - Marketing campaign management
- `CAMPAIGN_PARTICIPANTS` - Campaign participation tracking
- `UTM_TRACKING` - UTM parameter tracking

#### **💰 6. CASHBACK SYSTEM (2 tables)**
- `CASHBACK_SETTINGS` - Cashback configurations
- `USER_CASHBACKS` - User cashback tracking

#### **🎫 7. CLAIMS MANAGEMENT (1 table)**
- `MEMBER_CLAIMS` - User claim management for bonuses, refunds, and compensation

#### **👥 8. REFERRAL SYSTEM (2 tables)**
- `REFERRAL_PROGRAMS` - Referral program configurations
- `USER_REFERRALS` - User referral tracking

#### **📝 9. PROMOTION MANAGEMENT (1 table)**
- `PROMOTION_DESCRIPTIONS` - Promotion descriptions

#### **📊 10. ANALYTICS (1 table)**
- `MARKETING_ANALYTICS` - Marketing analytics and reporting

#### **📋 11. AUDIT & LOGGING (1 table)**
- `AUDIT_LOGS` - Complete audit trail

#### **🔧 12. SYSTEM CONFIGURATIONS (3 tables)**
- `STREAK_SETTINGS` - User streak tracking configuration
- `CLIENT_SETTINGS` - Client-specific settings
- `APP_SETTINGS` - Application-level settings

#### **⚙️ 13. MARKETING CONFIGURATIONS (1 table)**
- `TENANT_REFERRAL_CONFIGURATIONS` - Referral system settings per tenant

## 🎯 **Total: 25 Tables**

### **✅ Complete Coverage:**
1. **Bonus Management** (5 tables)
2. **Free Bet System** (2 tables)
3. **Loyalty System** (3 tables)
4. **Campaign Management** (3 tables)
5. **Cashback System** (2 tables)
6. **Claims Management** (1 table)
7. **Referral System** (2 tables)
8. **Promotion Management** (1 table)
9. **Analytics** (1 table)
10. **Audit & Logging** (1 table)
11. **System Configurations** (3 tables)
12. **Marketing Configurations** (1 table)

### **✅ Migration Strategy:**
- **Preserve Business Logic** → Keep your current bonus and loyalty logic
- **Enhance with .NET** → Add modern microservices architecture
- **Multi-Tenant Support** → Add tenant_id to all existing patterns
- **Marketing Automation** → Enhance with modern marketing tools

## 🚀 **Key Features:**

### **✅ 1. Comprehensive Bonus System**
- **Deposit Bonuses** → First deposit, happy hour, sign-up bonuses
- **Wagering Requirements** → Flexible wagering policies
- **Bonus Tracking** → Complete bonus lifecycle management
- **Localization** → Multi-language bonus descriptions

### **✅ 2. Advanced Loyalty Program**
- **Points System** → Flexible points earning and redemption
- **Transaction Tracking** → Complete loyalty point history
- **Program Management** → Multiple loyalty programs per tenant
- **Analytics** → Loyalty program performance tracking

### **✅ 3. Marketing Campaign Management**
- **Campaign Creation** → Comprehensive campaign management
- **UTM Tracking** → Complete UTM parameter tracking
- **Participant Management** → Campaign participation tracking
- **Analytics** → Campaign performance metrics

### **✅ 4. Referral and Cashback Systems**
- **Referral Programs** → Flexible referral reward systems
- **Cashback Management** → Automated cashback processing
- **User Tracking** → Complete referral and cashback history
- **Reward Management** → Flexible reward distribution

---

## 📚 **Detailed Table Definitions**

### **MEMBER_CLAIMS** - User Claims Management

> **Purpose:** User claim management for bonuses, refunds, and compensation
> 
> **When to Use:**
> - **Bonus Claims** → Users request bonus credits from marketing service
> - **Refund Requests** → Users claim refunds for cancelled bets or technical issues
> - **Compensation Claims** → Users request compensation for service disruptions
> - **Rebate Claims** → Users claim cashback or rebate rewards
> - **Dispute Resolution** → Handle user complaints and compensation requests
> - **Approval Workflow** → Admin approval process for high-value claims
> 
> **Key Operations:**
> - Submit claim request from user
> - Review and approve claims by admin
> - Process approved claims to wallet
> - Track claim status and history
> - Generate claim reports
> - Handle claim disputes

| **Column** | **Type** | **Default** | **Constraints** | **Description** |
|------------|----------|-------------|-----------------|-----------------|
| **id** | `uuid` | `gen_random_uuid()` | `PRIMARY KEY` | Unique claim identifier |
| **tenant_id** | `uuid` | - | `NOT NULL, FK→TENANTS.id` | Multi-tenant isolation |
| **user_id** | `varchar(50)` | - | `NOT NULL, FK→ASPNET_USERS.id` | Claim owner |
| **wallet_id** | `uuid` | - | `NOT NULL, FK→WALLETS.id` | Target wallet (Wallet Service reference) |
| **claim_type** | `varchar(30)` | - | `NOT NULL` | Claim type (bonus/refund/compensation/rebate/cashback) |
| **amount** | `decimal(18,8)` | - | `NOT NULL, CHECK > 0` | Claim amount (8 decimals for crypto) |
| **status** | `varchar(20)` | `'pending'` | `NOT NULL` | Status (pending/approved/rejected/processed) |
| **description** | `text` | `null` | - | Claim description |
| **approved_by** | `varchar(50)` | `null` | `FK→ASPNET_USERS.id` | Approver |
| **created_at** | `timestamp` | `now()` | `NOT NULL` | Claim timestamp |
| **processed_at** | `timestamp` | `null` | - | Processing timestamp |
| **rowversion** | `bytea` | `gen_random_bytes(8)` | `NOT NULL` | Row version for optimistic concurrency |

**This Marketing Service ER diagram provides complete marketing automation, bonus management, loyalty programs, campaign management, and claims management capabilities with multi-tenant support for your betting platform!** 🎯