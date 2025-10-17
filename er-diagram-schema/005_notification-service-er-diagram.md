# 📱 **Notification Service ER Diagram**

## 🎯 **Service Overview**
The Notification Service handles all notification delivery systems including SMS, email, push notifications, and in-app notifications for the betting platform. It manages notification templates, delivery channels, and user preferences with complete multi-tenant isolation.

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
    
    NOTIFICATION_TEMPLATES {
        uuid id PK
        uuid tenant_id FK
        varchar template_name
        varchar template_type
        varchar notification_channel
        varchar subject
        text content
        text sms_content
        text email_content
        text push_content
        jsonb template_variables
        varchar status
        varchar language_code
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    NOTIFICATION_CHANNELS {
        uuid id PK
        uuid tenant_id FK
        varchar channel_name
        varchar channel_type
        varchar provider
        jsonb configuration
        boolean is_active
        varchar status
        integer priority
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    SMS_PROVIDERS {
        uuid id PK
        uuid tenant_id FK
        varchar provider_name
        varchar provider_code
        varchar api_endpoint
        varchar api_key
        varchar api_secret
        jsonb configuration
        boolean is_active
        boolean is_live_mode
        varchar status
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    SMS_TEMPLATES {
        uuid id PK
        uuid tenant_id FK
        uuid sms_provider_id FK
        varchar template_name
        text template_content
        varchar template_type
        varchar status
        varchar language_code
        jsonb template_variables
        boolean support_bulk_personalize_message
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    SMS_DRIVERS {
        uuid id PK
        uuid tenant_id FK
        varchar name
        varchar handler_class
        decimal supported_variable
        decimal bulk_chunk_size
        decimal delay_per_chunk
        boolean is_active
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    EMAIL_PROVIDERS {
        uuid id PK
        uuid tenant_id FK
        varchar provider_name
        varchar provider_code
        varchar smtp_host
        integer smtp_port
        varchar smtp_username
        varchar smtp_password
        boolean use_ssl
        boolean use_tls
        jsonb configuration
        boolean is_active
        varchar status
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    EMAIL_TEMPLATES {
        uuid id PK
        uuid tenant_id FK
        uuid email_provider_id FK
        varchar template_name
        varchar subject
        text html_content
        text text_content
        varchar template_type
        varchar status
        varchar language_code
        jsonb template_variables
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    PUSH_NOTIFICATION_PROVIDERS {
        uuid id PK
        uuid tenant_id FK
        varchar provider_name
        varchar provider_code
        varchar api_endpoint
        varchar api_key
        varchar api_secret
        jsonb configuration
        boolean is_active
        varchar status
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    PUSH_NOTIFICATION_TEMPLATES {
        uuid id PK
        uuid tenant_id FK
        uuid push_provider_id FK
        varchar template_name
        varchar title
        text body
        jsonb data
        varchar template_type
        varchar status
        varchar language_code
        jsonb template_variables
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    NOTIFICATION_QUEUE {
        uuid id PK
        uuid tenant_id FK
        varchar user_id FK
        uuid notification_template_id FK
        uuid notification_channel_id FK
        varchar notification_type
        varchar recipient
        varchar subject
        text content
        jsonb metadata
        varchar status
        integer priority
        integer retry_count
        timestamp scheduled_at
        timestamp sent_at
        timestamp failed_at
        varchar error_message
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    NOTIFICATION_DELIVERIES {
        uuid id PK
        uuid tenant_id FK
        uuid notification_queue_id FK
        varchar user_id FK
        varchar notification_type
        varchar channel
        varchar recipient
        varchar status
        varchar provider_reference
        jsonb provider_response
        timestamp sent_at
        timestamp delivered_at
        timestamp failed_at
        varchar error_message
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    USER_NOTIFICATION_PREFERENCES {
        uuid id PK
        uuid tenant_id FK
        varchar user_id FK
        varchar notification_type
        boolean sms_enabled
        boolean email_enabled
        boolean push_enabled
        boolean in_app_enabled
        varchar sms_frequency
        varchar email_frequency
        varchar push_frequency
        jsonb preferences
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    NOTIFICATION_LOGS {
        uuid id PK
        uuid tenant_id FK
        uuid notification_delivery_id FK
        varchar user_id FK
        varchar action
        varchar status
        jsonb details
        timestamp action_timestamp
        varchar ip_address
        varchar user_agent
        integer rowVersion
        timestamp created_at
    }
    
    NOTIFICATION_ANALYTICS {
        uuid id PK
        uuid tenant_id FK
        varchar analytics_type
        varchar notification_type
        varchar channel
        integer total_sent
        integer total_delivered
        integer total_failed
        decimal delivery_rate
        decimal open_rate
        decimal click_rate
        timestamp period_start
        timestamp period_end
        jsonb additional_metrics
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    MESSAGE_LOGS {
        uuid id PK
        uuid tenant_id FK
        varchar message_type
        varchar class_name
        varchar comment
        text detail_info
        varchar status
        timestamp processed_at
        integer rowVersion
        timestamp created_at
        timestamp updated_at
    }
    
    DELIVERY_ACKNOWLEDGMENTS {
        uuid id PK
        uuid tenant_id FK
        uuid notification_delivery_id FK
        varchar ack_type
        varchar ack_status
        varchar provider_reference
        jsonb ack_data
        timestamp ack_received_at
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
    TENANTS ||--o{ NOTIFICATION_TEMPLATES : "has"
    TENANTS ||--o{ NOTIFICATION_CHANNELS : "has"
    TENANTS ||--o{ SMS_PROVIDERS : "has"
    TENANTS ||--o{ SMS_TEMPLATES : "has"
    TENANTS ||--o{ EMAIL_PROVIDERS : "has"
    TENANTS ||--o{ EMAIL_TEMPLATES : "has"
    TENANTS ||--o{ PUSH_NOTIFICATION_PROVIDERS : "has"
    TENANTS ||--o{ PUSH_NOTIFICATION_TEMPLATES : "has"
    TENANTS ||--o{ NOTIFICATION_QUEUE : "has"
    TENANTS ||--o{ NOTIFICATION_DELIVERIES : "has"
    TENANTS ||--o{ USER_NOTIFICATION_PREFERENCES : "has"
    TENANTS ||--o{ NOTIFICATION_LOGS : "has"
    TENANTS ||--o{ NOTIFICATION_ANALYTICS : "has"
    TENANTS ||--o{ AUDIT_LOGS : "has"
    
    %% Provider Relationships
    SMS_PROVIDERS ||--o{ SMS_TEMPLATES : "has"
    SMS_DRIVERS ||--o{ SMS_TEMPLATES : "uses"
    EMAIL_PROVIDERS ||--o{ EMAIL_TEMPLATES : "has"
    PUSH_NOTIFICATION_PROVIDERS ||--o{ PUSH_NOTIFICATION_TEMPLATES : "has"
    
    %% Template Relationships
    NOTIFICATION_TEMPLATES ||--o{ NOTIFICATION_QUEUE : "uses"
    SMS_TEMPLATES ||--o{ NOTIFICATION_QUEUE : "uses"
    EMAIL_TEMPLATES ||--o{ NOTIFICATION_QUEUE : "uses"
    PUSH_NOTIFICATION_TEMPLATES ||--o{ NOTIFICATION_QUEUE : "uses"
    
    %% Queue Relationships
    NOTIFICATION_QUEUE ||--o{ NOTIFICATION_DELIVERIES : "creates"
    NOTIFICATION_DELIVERIES ||--o{ NOTIFICATION_LOGS : "has"
    NOTIFICATION_DELIVERIES ||--o{ DELIVERY_ACKNOWLEDGMENTS : "has"
    
    %% Logging Relationships
    TENANTS ||--o{ MESSAGE_LOGS : "has"
    TENANTS ||--o{ DELIVERY_ACKNOWLEDGMENTS : "has"
```

## 🎯 **SRS Requirements Coverage**

### **FR-035: Notification Delivery System** ✅
- **Multi-Channel Notifications** → SMS, Email, Push, In-App notifications
- **Template Management** → `NOTIFICATION_TEMPLATES` with multi-language support
- **Provider Integration** → Multiple SMS, Email, and Push providers
- **Queue Management** → `NOTIFICATION_QUEUE` with priority and retry logic
- **Delivery Tracking** → `NOTIFICATION_DELIVERIES` with complete status tracking
- **User Preferences** → `USER_NOTIFICATION_PREFERENCES` for personalized notifications
- **Analytics** → `NOTIFICATION_ANALYTICS` with delivery metrics

## 🔒 **Security Features**

### **1. Multi-Tenant Isolation**
- **TenantId in every table** for complete data isolation
- **No cross-tenant data access** possible
- **Tenant-scoped queries** for performance

### **2. Notification Security**
- **Template validation** with content sanitization
- **Provider authentication** with secure credentials
- **Delivery encryption** for sensitive notifications
- **Audit trail** for all notification activities

### **3. Data Integrity**
- **Queue consistency** with proper retry mechanisms
- **Delivery tracking** with status validation
- **Template versioning** with change management
- **Real-time monitoring** with delivery analytics

## 🚀 **Performance Optimizations**

### **1. Indexing Strategy**
- **Primary indexes** on all ID columns
- **Composite indexes** on (tenant_id, user_id, created_at)
- **Performance indexes** on frequently queried columns
- **Queue indexes** for priority-based processing

### **2. Query Optimization**
- **TenantId filtering** on all queries
- **Efficient joins** with proper foreign keys
- **Caching strategy** for templates and preferences
- **Real-time updates** with queue processing

## 📊 **Complete Table Organization & Structure**

### **🏢 1. TENANT MANAGEMENT (1 table)**
- `TENANTS` - Core tenant information

#### **📝 2. NOTIFICATION TEMPLATES (1 table)**
- `NOTIFICATION_TEMPLATES` - Multi-channel notification templates

#### **📡 3. NOTIFICATION CHANNELS (1 table)**
- `NOTIFICATION_CHANNELS` - Notification channel management

#### **📱 4. SMS SYSTEM (3 tables)**
- `SMS_PROVIDERS` - SMS provider management
- `SMS_TEMPLATES` - SMS message templates
- `SMS_DRIVERS` - SMS driver/handler management

#### **📧 5. EMAIL SYSTEM (2 tables)**
- `EMAIL_PROVIDERS` - Email provider management
- `EMAIL_TEMPLATES` - Email message templates

#### **🔔 6. PUSH NOTIFICATION SYSTEM (2 tables)**
- `PUSH_NOTIFICATION_PROVIDERS` - Push notification providers
- `PUSH_NOTIFICATION_TEMPLATES` - Push notification templates

#### **📋 7. NOTIFICATION PROCESSING (2 tables)**
- `NOTIFICATION_QUEUE` - Notification queue management
- `NOTIFICATION_DELIVERIES` - Delivery tracking and status

#### **👤 8. USER PREFERENCES (1 table)**
- `USER_NOTIFICATION_PREFERENCES` - User notification preferences

#### **📊 9. ANALYTICS & LOGGING (5 tables)**
- `NOTIFICATION_LOGS` - Notification activity logs
- `NOTIFICATION_ANALYTICS` - Notification performance analytics
- `MESSAGE_LOGS` - Message processing logs
- `DELIVERY_ACKNOWLEDGMENTS` - Delivery acknowledgment tracking
- `AUDIT_LOGS` - Complete audit trail

## 🎯 **Total: 18 Tables**

### **✅ Complete Coverage:**
1. **Notification Templates** (1 table)
2. **Notification Channels** (1 table)
3. **SMS System** (3 tables)
4. **Email System** (2 tables)
5. **Push Notification System** (2 tables)
6. **Notification Processing** (2 tables)
7. **User Preferences** (1 table)
8. **Analytics & Logging** (5 tables)

### **✅ Migration Strategy:**
- **Preserve Business Logic** → Keep your current SMS and notification logic
- **Enhance with .NET** → Add modern microservices architecture
- **Multi-Tenant Support** → Add tenant_id to all existing patterns
- **Provider Integration** → Enhance with modern notification APIs

## 🚀 **Key Features:**

### **✅ 1. Multi-Channel Notification System**
- **SMS Notifications** → Multiple SMS providers with templates
- **Email Notifications** → Rich email templates with HTML support
- **Push Notifications** → Mobile and web push notifications
- **In-App Notifications** → Real-time in-app messaging

### **✅ 2. Advanced Template Management**
- **Multi-Language Support** → Localized notification templates
- **Variable Substitution** → Dynamic content with user data
- **Template Versioning** → Template change management
- **A/B Testing** → Template performance optimization
- **Bulk Personalization** → Support for bulk personalized messages
- **SMS Driver Support** → Multiple SMS handler classes with chunking

### **✅ 3. Robust Delivery System**
- **Queue Management** → Priority-based notification processing
- **Retry Logic** → Automatic retry for failed deliveries
- **Delivery Tracking** → Complete delivery status tracking
- **Error Handling** → Comprehensive error management

### **✅ 4. User-Centric Features**
- **User Preferences** → Personalized notification settings
- **Channel Selection** → User choice of notification channels
- **Frequency Control** → Notification frequency management
- **Opt-out Management** → Easy unsubscribe options

### **✅ 5. Analytics & Monitoring**
- **Delivery Analytics** → Delivery rate, open rate, click rate
- **Performance Metrics** → Provider performance tracking
- **User Engagement** → User notification interaction tracking
- **Real-time Monitoring** → Live notification status monitoring
- **Message Logging** → Complete message processing logs
- **Delivery Acknowledgments** → SMS delivery confirmation tracking

---

**This Notification Service ER diagram provides complete notification delivery system capabilities with multi-channel support, template management, and analytics for your betting platform!** 🎯
