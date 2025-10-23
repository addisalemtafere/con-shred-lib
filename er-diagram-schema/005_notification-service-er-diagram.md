# 📱 **Notification Service ER Diagram**

## 🎯 **Service Overview**
The Notification Service handles all notification delivery systems including SMS, email, push notifications, and in-app notifications for the betting platform. It manages notification templates, delivery channels, and user preferences with complete multi-tenant isolation.

## 📊 **Entity Relationship Diagram**

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

#### **⚙️ 10. NOTIFICATION CONFIGURATIONS (3 tables)**
- `TENANT_SMS_CONFIGURATIONS` - SMS notification settings per tenant
- `SMS_TEMPLATES` - SMS message templates
- `OTP_VERIFICATIONS` - Phone verification codes

## 🎯 **Total: 21 Tables**

### **✅ Complete Coverage:**
1. **Notification Templates** (1 table)
2. **Notification Channels** (1 table)
3. **SMS System** (3 tables)
4. **Email System** (2 tables)
5. **Push Notification System** (2 tables)
6. **Notification Processing** (2 tables)
7. **User Preferences** (1 table)
8. **Analytics & Logging** (5 tables)
9. **Notification Configurations** (3 tables)

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

## 🚀 **gRPC Service Definition - Implementation Ready**

### **🔧 Notification Service (notification.proto)**

```protobuf
syntax = "proto3";

package notification.v1;

import "google/protobuf/empty.proto";
import "google/protobuf/timestamp.proto";

// Notification Service - Multi-channel notification delivery
service NotificationService {
  // Notification Management
  rpc SendNotification(SendNotificationRequest) returns (SendNotificationResponse);
  rpc SendBulkNotification(SendBulkNotificationRequest) returns (SendBulkNotificationResponse);
  rpc ScheduleNotification(ScheduleNotificationRequest) returns (ScheduleNotificationResponse);
  rpc CancelNotification(CancelNotificationRequest) returns (CancelNotificationResponse);
  
  // Template Management
  rpc CreateTemplate(CreateTemplateRequest) returns (CreateTemplateResponse);
  rpc UpdateTemplate(UpdateTemplateRequest) returns (UpdateTemplateResponse);
  rpc GetTemplate(GetTemplateRequest) returns (GetTemplateResponse);
  rpc ListTemplates(ListTemplatesRequest) returns (ListTemplatesResponse);
  rpc DeleteTemplate(DeleteTemplateRequest) returns (DeleteTemplateResponse);
  
  // Channel Management
  rpc ConfigureChannel(ConfigureChannelRequest) returns (ConfigureChannelResponse);
  rpc TestChannel(TestChannelRequest) returns (TestChannelResponse);
  rpc GetChannelStatus(GetChannelStatusRequest) returns (GetChannelStatusResponse);
  rpc ListChannels(ListChannelsRequest) returns (ListChannelsResponse);
  
  // User Preferences
  rpc SetUserPreferences(SetUserPreferencesRequest) returns (SetUserPreferencesResponse);
  rpc GetUserPreferences(GetUserPreferencesRequest) returns (GetUserPreferencesResponse);
  rpc UpdateUserPreferences(UpdateUserPreferencesRequest) returns (UpdateUserPreferencesResponse);
  rpc UnsubscribeUser(UnsubscribeUserRequest) returns (UnsubscribeUserResponse);
  
  // Delivery Tracking
  rpc GetDeliveryStatus(GetDeliveryStatusRequest) returns (GetDeliveryStatusResponse);
  rpc GetNotificationHistory(GetNotificationHistoryRequest) returns (GetNotificationHistoryResponse);
  rpc RetryFailedNotification(RetryFailedNotificationRequest) returns (RetryFailedNotificationResponse);
  
  // Analytics & Reporting
  rpc GetDeliveryAnalytics(GetDeliveryAnalyticsRequest) returns (GetDeliveryAnalyticsResponse);
  rpc GetChannelPerformance(GetChannelPerformanceRequest) returns (GetChannelPerformanceResponse);
  rpc GetUserEngagement(GetUserEngagementRequest) returns (GetUserEngagementResponse);
  
  // Health & Monitoring
  rpc HealthCheck(HealthCheckRequest) returns (HealthCheckResponse);
  rpc GetMetrics(GetMetricsRequest) returns (GetMetricsResponse);
}

// Request/Response Messages
message SendNotificationRequest {
  string tenant_id = 1;
  string user_id = 2;
  string channel = 3;
  string template_id = 4;
  map<string, string> variables = 5;
  string priority = 6;
  google.protobuf.Timestamp scheduled_at = 7;
}

message SendNotificationResponse {
  bool success = 1;
  string notification_id = 2;
  string status = 3;
  string error_message = 4;
}

message SendBulkNotificationRequest {
  string tenant_id = 1;
  repeated string user_ids = 2;
  string channel = 3;
  string template_id = 4;
  map<string, string> variables = 5;
  string priority = 6;
  google.protobuf.Timestamp scheduled_at = 7;
}

message SendBulkNotificationResponse {
  bool success = 1;
  repeated string notification_ids = 2;
  int32 total_sent = 3;
  int32 failed_count = 4;
  string error_message = 5;
}

message ScheduleNotificationRequest {
  string tenant_id = 1;
  string user_id = 2;
  string channel = 3;
  string template_id = 4;
  map<string, string> variables = 5;
  google.protobuf.Timestamp scheduled_at = 6;
}

message ScheduleNotificationResponse {
  bool success = 1;
  string notification_id = 2;
  string status = 3;
  string error_message = 4;
}

message CreateTemplateRequest {
  string tenant_id = 1;
  string name = 2;
  string channel = 3;
  string subject = 4;
  string content = 5;
  string content_type = 6;
  map<string, string> variables = 7;
  bool is_active = 8;
}

message CreateTemplateResponse {
  bool success = 1;
  string template_id = 2;
  string error_message = 3;
}

message GetTemplateRequest {
  string template_id = 1;
  string tenant_id = 2;
}

message GetTemplateResponse {
  bool success = 1;
  NotificationTemplate template = 2;
  string error_message = 3;
}

message ListTemplatesRequest {
  string tenant_id = 1;
  string channel = 2;
  int32 page = 3;
  int32 page_size = 4;
}

message ListTemplatesResponse {
  bool success = 1;
  repeated NotificationTemplate templates = 2;
  int32 total_count = 3;
  string error_message = 4;
}

message ConfigureChannelRequest {
  string tenant_id = 1;
  string channel = 2;
  map<string, string> configuration = 3;
  bool is_active = 4;
}

message ConfigureChannelResponse {
  bool success = 1;
  string channel_id = 2;
  string error_message = 3;
}

message TestChannelRequest {
  string tenant_id = 1;
  string channel = 2;
  string test_recipient = 3;
  string test_message = 4;
}

message TestChannelResponse {
  bool success = 1;
  string test_id = 2;
  string status = 3;
  string error_message = 4;
}

message SetUserPreferencesRequest {
  string tenant_id = 1;
  string user_id = 2;
  map<string, bool> channel_preferences = 3;
  map<string, string> notification_settings = 4;
}

message SetUserPreferencesResponse {
  bool success = 1;
  string error_message = 2;
}

message GetUserPreferencesRequest {
  string tenant_id = 1;
  string user_id = 2;
}

message GetUserPreferencesResponse {
  bool success = 1;
  UserNotificationPreferences preferences = 2;
  string error_message = 3;
}

message GetDeliveryStatusRequest {
  string notification_id = 1;
  string tenant_id = 2;
}

message GetDeliveryStatusResponse {
  bool success = 1;
  NotificationStatus status = 2;
  string error_message = 3;
}

message GetNotificationHistoryRequest {
  string tenant_id = 1;
  string user_id = 2;
  string channel = 3;
  google.protobuf.Timestamp from_date = 4;
  google.protobuf.Timestamp to_date = 5;
  int32 page = 6;
  int32 page_size = 7;
}

message GetNotificationHistoryResponse {
  bool success = 1;
  repeated NotificationHistoryItem history = 2;
  int32 total_count = 3;
  string error_message = 4;
}

message GetDeliveryAnalyticsRequest {
  string tenant_id = 1;
  string channel = 2;
  google.protobuf.Timestamp from_date = 3;
  google.protobuf.Timestamp to_date = 4;
}

message GetDeliveryAnalyticsResponse {
  bool success = 1;
  DeliveryAnalytics analytics = 2;
  string error_message = 3;
}

// Data Models
message NotificationTemplate {
  string id = 1;
  string tenant_id = 2;
  string name = 3;
  string channel = 4;
  string subject = 5;
  string content = 6;
  string content_type = 7;
  map<string, string> variables = 8;
  bool is_active = 9;
  google.protobuf.Timestamp created_at = 10;
  google.protobuf.Timestamp updated_at = 11;
}

message UserNotificationPreferences {
  string user_id = 1;
  string tenant_id = 2;
  map<string, bool> channel_preferences = 3;
  map<string, string> notification_settings = 4;
  bool is_global_opt_out = 5;
  google.protobuf.Timestamp updated_at = 6;
}

message NotificationStatus {
  string notification_id = 1;
  string status = 2;
  string channel = 3;
  string recipient = 4;
  google.protobuf.Timestamp sent_at = 5;
  google.protobuf.Timestamp delivered_at = 6;
  string error_message = 7;
  map<string, string> metadata = 8;
}

message NotificationHistoryItem {
  string notification_id = 1;
  string user_id = 2;
  string channel = 3;
  string template_id = 4;
  string status = 5;
  google.protobuf.Timestamp created_at = 6;
  google.protobuf.Timestamp sent_at = 7;
  google.protobuf.Timestamp delivered_at = 8;
  string error_message = 9;
}

message DeliveryAnalytics {
  string channel = 1;
  int32 total_sent = 2;
  int32 delivered = 3;
  int32 failed = 4;
  double delivery_rate = 5;
  double open_rate = 6;
  double click_rate = 7;
  google.protobuf.Timestamp period_start = 8;
  google.protobuf.Timestamp period_end = 9;
}
```

### **🔗 External Service Communication Patterns**

#### **Identity Service Integration:**
- **User Validation** → Validate user existence and status
- **User Preferences** → Sync notification preferences
- **Tenant Validation** → Verify tenant configuration

#### **All Services Integration:**
- **Event Notifications** → Send notifications for system events
- **User Alerts** → Notify users of important updates
- **System Notifications** → Send system-wide announcements

### **📊 Implementation Guidelines**

#### **Service Configuration:**
- Configure notification channels (SMS, Email, Push, In-App)
- Set up delivery providers and their configurations
- Define notification templates and variables
- Configure delivery schedules and retry policies

#### **Error Handling:**
- Handle provider failures gracefully with fallback channels
- Implement retry mechanisms for failed deliveries
- Log all delivery attempts and failures
- Provide detailed error messages for debugging

#### **Monitoring & Health Checks:**
- Monitor delivery rates and channel performance
- Track user engagement and notification effectiveness
- Alert on delivery failures and channel issues
- Generate analytics reports on notification performance

---

**This Notification Service ER diagram provides complete notification delivery system capabilities with multi-channel support, template management, and analytics for your betting platform!** 🎯