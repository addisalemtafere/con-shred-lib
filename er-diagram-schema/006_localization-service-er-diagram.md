# 🌍 **Localization Service ER Diagram**

## 🎯 **Service Overview**
The Localization Service handles multi-language support, regional settings, currency management, and timezone handling for the betting platform. It manages translations, language configurations, regional settings, and cultural adaptations with complete multi-tenant isolation.

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
    
    LANGUAGES {
        uuid id PK
        uuid tenant_id FK
        varchar code UK
        varchar name
        varchar native_name
        varchar language_culture
        varchar flag_image_filename
        integer display_order
        varchar description
        boolean is_active
        boolean is_default
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    TRANSLATIONS {
        uuid id PK
        uuid tenant_id FK
        uuid language_id FK
        varchar entity_type
        varchar entity_id
        varchar language_code FK
        varchar display_name
        text description
        boolean is_active
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    CURRENCIES {
        uuid id PK
        uuid tenant_id FK
        varchar currency_code UK
        varchar currency_name
        varchar currency_symbol
        varchar country_code
        decimal exchange_rate
        boolean is_active
        boolean is_default
        integer decimal_places
        varchar number_format
        varchar date_format
        varchar time_format
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    REGIONS {
        uuid id PK
        uuid tenant_id FK
        varchar source_id
        varchar name
        varchar name_localized
        varchar country_code
        varchar sport_type_source_id
        integer order_index
        integer result_id
        timestamp activated_on
        boolean is_active
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    TIMEZONES {
        uuid id PK
        uuid tenant_id FK
        varchar timezone_id UK
        varchar timezone_name
        varchar timezone_abbreviation
        varchar utc_offset
        boolean supports_dst
        varchar dst_start_rule
        varchar dst_end_rule
        boolean is_active
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    CULTURAL_SETTINGS {
        uuid id PK
        uuid tenant_id FK
        varchar country_code
        varchar language_code
        varchar date_format
        varchar time_format
        varchar number_format
        varchar currency_format
        varchar phone_format
        varchar address_format
        boolean is_active
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    TRANSLATION_TEMPLATES {
        uuid id PK
        uuid tenant_id FK
        varchar template_name
        varchar template_type
        varchar template_category
        text template_content
        jsonb template_variables
        boolean is_active
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    TRANSLATION_BATCHES {
        uuid id PK
        uuid tenant_id FK
        varchar batch_name
        varchar batch_type
        varchar entity_type
        jsonb entity_ids
        varchar status
        integer total_items
        integer processed_items
        integer failed_items
        timestamp started_at
        timestamp completed_at
        text error_message
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    TRANSLATION_LOGS {
        uuid id PK
        uuid tenant_id FK
        uuid translation_id FK
        varchar action
        varchar old_value
        varchar new_value
        varchar user_id
        varchar ip_address
        timestamp action_timestamp
        integer rowVersion
        timestamp created_at
    }
    
    LANGUAGE_ANALYTICS {
        uuid id PK
        uuid tenant_id FK
        varchar language_code
        varchar analytics_type
        integer usage_count
        integer translation_count
        decimal popularity_score
        timestamp period_start
        timestamp period_end
        jsonb additional_metrics
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    REGIONAL_CONFIGURATIONS {
        uuid id PK
        uuid tenant_id FK
        varchar region_code
        varchar configuration_type
        jsonb configuration_data
        boolean is_active
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
    TENANTS ||--o{ LANGUAGES : "has"
    TENANTS ||--o{ TRANSLATIONS : "has"
    TENANTS ||--o{ CURRENCIES : "has"
    TENANTS ||--o{ REGIONS : "has"
    TENANTS ||--o{ TIMEZONES : "has"
    TENANTS ||--o{ CULTURAL_SETTINGS : "has"
    TENANTS ||--o{ TRANSLATION_TEMPLATES : "has"
    TENANTS ||--o{ TRANSLATION_BATCHES : "has"
    TENANTS ||--o{ TRANSLATION_LOGS : "has"
    TENANTS ||--o{ LANGUAGE_ANALYTICS : "has"
    TENANTS ||--o{ REGIONAL_CONFIGURATIONS : "has"
    TENANTS ||--o{ AUDIT_LOGS : "has"
    
    %% Language Relationships
    LANGUAGES ||--o{ TRANSLATIONS : "has"
    LANGUAGES ||--o{ LANGUAGE_ANALYTICS : "tracks"
    
    %% Translation Relationships
    TRANSLATIONS ||--o{ TRANSLATION_LOGS : "has"
    TRANSLATION_BATCHES ||--o{ TRANSLATIONS : "processes"
    
    %% Regional Relationships
    REGIONS ||--o{ REGIONAL_CONFIGURATIONS : "has"
    CULTURAL_SETTINGS ||--o{ REGIONAL_CONFIGURATIONS : "configures"
```

## 🎯 **SRS Requirements Coverage**

### **FR-037: Regional Support** ✅
- **Multi-Language Support** → `LANGUAGES` and `TRANSLATIONS` for complete localization
- **Currency Management** → `CURRENCIES` with exchange rates and formatting
- **Regional Settings** → `REGIONS` and `CULTURAL_SETTINGS` for country-specific configurations
- **Timezone Support** → `TIMEZONES` with DST handling
- **Translation Management** → `TRANSLATION_TEMPLATES` and `TRANSLATION_BATCHES` for bulk operations
- **Analytics** → `LANGUAGE_ANALYTICS` for usage tracking and optimization

## 🔒 **Security Features**

### **1. Multi-Tenant Isolation**
- **TenantId in every table** for complete data isolation
- **No cross-tenant access** possible
- **Tenant-scoped queries** for performance

### **2. Translation Security**
- **Translation validation** with content sanitization
- **Batch processing** with error handling
- **Audit trail** for all translation activities
- **Version control** with change tracking

### **3. Data Integrity**
- **Language consistency** with proper validation
- **Currency accuracy** with exchange rate management
- **Regional compliance** with local regulations
- **Real-time updates** with caching support

## 🚀 **Performance Optimizations**

### **1. Indexing Strategy**
- **Primary indexes** on all ID columns
- **Composite indexes** on (tenant_id, language_code, entity_type)
- **Performance indexes** on frequently queried columns
- **Translation indexes** for fast lookups

### **2. Query Optimization**
- **TenantId filtering** on all queries
- **Efficient joins** with proper foreign keys
- **Caching strategy** for translations and languages
- **Batch processing** for bulk operations

## 📊 **Complete Table Organization & Structure**

### **🏢 1. TENANT MANAGEMENT (1 table)**
- `TENANTS` - Core tenant information

#### **🌍 2. LANGUAGE MANAGEMENT (1 table)**
- `LANGUAGES` - Language configuration and settings

#### **📝 3. TRANSLATION SYSTEM (3 tables)**
- `TRANSLATIONS` - Entity translations
- `TRANSLATION_TEMPLATES` - Translation templates
- `TRANSLATION_BATCHES` - Bulk translation processing

#### **💰 4. CURRENCY SYSTEM (1 table)**
- `CURRENCIES` - Currency management and exchange rates

#### **🗺️ 5. REGIONAL SYSTEM (2 tables)**
- `REGIONS` - Geographical regions
- `CULTURAL_SETTINGS` - Cultural and regional settings

#### **⏰ 6. TIMEZONE SYSTEM (1 table)**
- `TIMEZONES` - Timezone management with DST support

#### **📊 7. ANALYTICS & LOGGING (3 tables)**
- `TRANSLATION_LOGS` - Translation activity logs
- `LANGUAGE_ANALYTICS` - Language usage analytics
- `REGIONAL_CONFIGURATIONS` - Regional configuration management

#### **🔍 8. AUDIT TRAIL (1 table)**
- `AUDIT_LOGS` - Complete audit trail

## 🎯 **Total: 12 Tables**

### **✅ Complete Coverage:**
1. **Language Management** (1 table)
2. **Translation System** (3 tables)
3. **Currency System** (1 table)
4. **Regional System** (2 tables)
5. **Timezone System** (1 table)
6. **Analytics & Logging** (3 tables)
7. **Audit Trail** (1 table)

### **✅ Migration Strategy:**
- **Preserve Business Logic** → Keep your current translation logic
- **Enhance with .NET** → Add modern microservices architecture
- **Multi-Tenant Support** → Add tenant_id to all existing patterns
- **Advanced Features** → Add batch processing and analytics

## 🚀 **Key Features:**

### **✅ 1. Multi-Language Support**
- **Language Management** → Complete language configuration
- **Translation System** → Entity-based translations
- **Template Support** → Reusable translation templates
- **Batch Processing** → Bulk translation operations

### **✅ 2. Regional Configuration**
- **Currency Support** → Multi-currency with exchange rates
- **Regional Settings** → Country-specific configurations
- **Cultural Adaptation** → Date, time, and number formatting
- **Timezone Management** → DST-aware timezone handling

### **✅ 3. Advanced Translation Features**
- **Entity-Based Translations** → Translate any entity type
- **Template System** → Reusable translation templates
- **Batch Operations** → Bulk translation processing
- **Version Control** → Translation change tracking

### **✅ 4. Analytics & Monitoring**
- **Usage Analytics** → Language usage tracking
- **Performance Metrics** → Translation performance monitoring
- **Audit Trail** → Complete translation activity logging
- **Regional Analytics** → Regional usage patterns

### **✅ 5. Enterprise Features**
- **Multi-Tenant Support** → Complete tenant isolation
- **Configuration Management** → Regional and cultural settings
- **Caching Support** → High-performance translation lookups
- **API Integration** → RESTful translation services

---

**This Localization Service ER diagram provides complete multi-language and regional support capabilities for your betting platform!** 🎯
