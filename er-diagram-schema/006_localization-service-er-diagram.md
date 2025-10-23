# 🌍 **Localization Service ER Diagram**

## 🎯 **Service Overview**
The Localization Service handles multi-language support, regional settings, currency management, and timezone handling for the betting platform. It manages translations, language configurations, regional settings, and cultural adaptations with complete multi-tenant isolation.

## 📊 **Entity Relationship Diagram**

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

## 🚀 **gRPC Service Definition - Implementation Ready**

### **🔧 Localization Service (localization.proto)**

```protobuf
syntax = "proto3";

package localization.v1;

import "google/protobuf/empty.proto";
import "google/protobuf/timestamp.proto";

// Localization Service - Multi-language and regional support
service LocalizationService {
  // Translation Management
  rpc GetTranslation(GetTranslationRequest) returns (GetTranslationResponse);
  rpc GetTranslations(GetTranslationsRequest) returns (GetTranslationsResponse);
  rpc CreateTranslation(CreateTranslationRequest) returns (CreateTranslationResponse);
  rpc UpdateTranslation(UpdateTranslationRequest) returns (UpdateTranslationResponse);
  rpc DeleteTranslation(DeleteTranslationRequest) returns (DeleteTranslationResponse);
  
  // Language Management
  rpc GetSupportedLanguages(GetSupportedLanguagesRequest) returns (GetSupportedLanguagesResponse);
  rpc AddLanguage(AddLanguageRequest) returns (AddLanguageResponse);
  rpc UpdateLanguage(UpdateLanguageRequest) returns (UpdateLanguageResponse);
  rpc RemoveLanguage(RemoveLanguageRequest) returns (RemoveLanguageResponse);
  
  // Regional Settings
  rpc GetRegionalSettings(GetRegionalSettingsRequest) returns (GetRegionalSettingsResponse);
  rpc UpdateRegionalSettings(UpdateRegionalSettingsRequest) returns (UpdateRegionalSettingsResponse);
  rpc GetCurrencySettings(GetCurrencySettingsRequest) returns (GetCurrencySettingsResponse);
  rpc UpdateCurrencySettings(UpdateCurrencySettingsRequest) returns (UpdateCurrencySettingsResponse);
  
  // Timezone Management
  rpc GetTimezoneSettings(GetTimezoneSettingsRequest) returns (GetTimezoneSettingsResponse);
  rpc UpdateTimezoneSettings(UpdateTimezoneSettingsRequest) returns (UpdateTimezoneSettingsResponse);
  rpc GetAvailableTimezones(GetAvailableTimezonesRequest) returns (GetAvailableTimezonesResponse);
  
  // User Preferences
  rpc GetUserLanguagePreferences(GetUserLanguagePreferencesRequest) returns (GetUserLanguagePreferencesResponse);
  rpc SetUserLanguagePreferences(SetUserLanguagePreferencesRequest) returns (SetUserLanguagePreferencesResponse);
  rpc GetUserRegionalPreferences(GetUserRegionalPreferencesRequest) returns (GetUserRegionalPreferencesResponse);
  rpc SetUserRegionalPreferences(SetUserRegionalPreferencesRequest) returns (SetUserRegionalPreferencesResponse);
  
  // Content Localization
  rpc LocalizeContent(LocalizeContentRequest) returns (LocalizeContentResponse);
  rpc BatchLocalizeContent(BatchLocalizeContentRequest) returns (BatchLocalizeContentResponse);
  rpc GetLocalizedContent(GetLocalizedContentRequest) returns (GetLocalizedContentResponse);
  
  // Analytics & Reporting
  rpc GetLanguageUsage(GetLanguageUsageRequest) returns (GetLanguageUsageResponse);
  rpc GetRegionalAnalytics(GetRegionalAnalyticsRequest) returns (GetRegionalAnalyticsResponse);
  rpc GetTranslationMetrics(GetTranslationMetricsRequest) returns (GetTranslationMetricsResponse);
  
  // Health & Monitoring
  rpc HealthCheck(HealthCheckRequest) returns (HealthCheckResponse);
  rpc GetMetrics(GetMetricsRequest) returns (GetMetricsResponse);
}

// Request/Response Messages
message GetTranslationRequest {
  string tenant_id = 1;
  string key = 2;
  string language_code = 3;
  string context = 4;
}

message GetTranslationResponse {
  bool success = 1;
  string translation = 2;
  string language_code = 3;
  string context = 4;
  bool is_fallback = 5;
  string error_message = 6;
}

message GetTranslationsRequest {
  string tenant_id = 1;
  repeated string keys = 2;
  string language_code = 3;
  string context = 4;
}

message GetTranslationsResponse {
  bool success = 1;
  map<string, string> translations = 2;
  string language_code = 3;
  string error_message = 4;
}

message CreateTranslationRequest {
  string tenant_id = 1;
  string key = 2;
  string language_code = 3;
  string translation = 4;
  string context = 5;
  string description = 6;
}

message CreateTranslationResponse {
  bool success = 1;
  string translation_id = 2;
  string error_message = 3;
}

message GetSupportedLanguagesRequest {
  string tenant_id = 1;
}

message GetSupportedLanguagesResponse {
  bool success = 1;
  repeated Language languages = 2;
  string error_message = 3;
}

message AddLanguageRequest {
  string tenant_id = 1;
  string language_code = 2;
  string language_name = 3;
  string native_name = 4;
  string flag_emoji = 5;
  bool is_rtl = 6;
  bool is_active = 7;
}

message AddLanguageResponse {
  bool success = 1;
  string language_id = 2;
  string error_message = 3;
}

message GetRegionalSettingsRequest {
  string tenant_id = 1;
  string region_code = 2;
}

message GetRegionalSettingsResponse {
  bool success = 1;
  RegionalSettings settings = 2;
  string error_message = 3;
}

message UpdateRegionalSettingsRequest {
  string tenant_id = 1;
  string region_code = 2;
  RegionalSettings settings = 3;
}

message UpdateRegionalSettingsResponse {
  bool success = 1;
  string error_message = 2;
}

message GetCurrencySettingsRequest {
  string tenant_id = 1;
  string currency_code = 2;
}

message GetCurrencySettingsResponse {
  bool success = 1;
  CurrencySettings settings = 2;
  string error_message = 3;
}

message GetTimezoneSettingsRequest {
  string tenant_id = 1;
  string timezone_id = 2;
}

message GetTimezoneSettingsResponse {
  bool success = 1;
  TimezoneSettings settings = 2;
  string error_message = 3;
}

message GetUserLanguagePreferencesRequest {
  string tenant_id = 1;
  string user_id = 2;
}

message GetUserLanguagePreferencesResponse {
  bool success = 1;
  UserLanguagePreferences preferences = 2;
  string error_message = 3;
}

message SetUserLanguagePreferencesRequest {
  string tenant_id = 1;
  string user_id = 2;
  UserLanguagePreferences preferences = 3;
}

message SetUserLanguagePreferencesResponse {
  bool success = 1;
  string error_message = 2;
}

message LocalizeContentRequest {
  string tenant_id = 1;
  string content = 2;
  string source_language = 3;
  string target_language = 4;
  string context = 5;
}

message LocalizeContentResponse {
  bool success = 1;
  string localized_content = 2;
  string target_language = 3;
  double confidence_score = 4;
  string error_message = 5;
}

message BatchLocalizeContentRequest {
  string tenant_id = 1;
  repeated string content_items = 2;
  string source_language = 3;
  string target_language = 4;
  string context = 5;
}

message BatchLocalizeContentResponse {
  bool success = 1;
  repeated string localized_content = 2;
  string target_language = 3;
  double average_confidence_score = 4;
  string error_message = 5;
}

message GetLanguageUsageRequest {
  string tenant_id = 1;
  google.protobuf.Timestamp from_date = 2;
  google.protobuf.Timestamp to_date = 3;
}

message GetLanguageUsageResponse {
  bool success = 1;
  repeated LanguageUsage usage = 2;
  string error_message = 3;
}

message GetRegionalAnalyticsRequest {
  string tenant_id = 1;
  google.protobuf.Timestamp from_date = 2;
  google.protobuf.Timestamp to_date = 3;
}

message GetRegionalAnalyticsResponse {
  bool success = 1;
  repeated RegionalAnalytics analytics = 2;
  string error_message = 3;
}

// Data Models
message Language {
  string id = 1;
  string tenant_id = 2;
  string language_code = 3;
  string language_name = 4;
  string native_name = 5;
  string flag_emoji = 6;
  bool is_rtl = 7;
  bool is_active = 8;
  int32 sort_order = 9;
  google.protobuf.Timestamp created_at = 10;
  google.protobuf.Timestamp updated_at = 11;
}

message RegionalSettings {
  string region_code = 1;
  string region_name = 2;
  string currency_code = 3;
  string timezone_id = 4;
  string date_format = 5;
  string time_format = 6;
  string number_format = 7;
  string phone_format = 8;
  string address_format = 9;
  bool is_active = 10;
}

message CurrencySettings {
  string currency_code = 1;
  string currency_name = 2;
  string symbol = 3;
  int32 decimal_places = 4;
  string decimal_separator = 5;
  string thousands_separator = 6;
  bool is_active = 7;
}

message TimezoneSettings {
  string timezone_id = 1;
  string timezone_name = 2;
  string utc_offset = 3;
  bool observes_dst = 4;
  string dst_start = 5;
  string dst_end = 6;
  bool is_active = 7;
}

message UserLanguagePreferences {
  string user_id = 1;
  string tenant_id = 2;
  string primary_language = 3;
  repeated string secondary_languages = 4;
  string interface_language = 5;
  string content_language = 6;
  bool auto_translate = 7;
  google.protobuf.Timestamp updated_at = 8;
}

message UserRegionalPreferences {
  string user_id = 1;
  string tenant_id = 2;
  string region_code = 3;
  string currency_code = 4;
  string timezone_id = 5;
  string date_format = 6;
  string time_format = 7;
  google.protobuf.Timestamp updated_at = 8;
}

message LanguageUsage {
  string language_code = 1;
  string language_name = 2;
  int32 usage_count = 3;
  double usage_percentage = 4;
  google.protobuf.Timestamp period_start = 5;
  google.protobuf.Timestamp period_end = 6;
}

message RegionalAnalytics {
  string region_code = 1;
  string region_name = 2;
  int32 user_count = 3;
  double usage_percentage = 4;
  repeated string top_languages = 5;
  google.protobuf.Timestamp period_start = 6;
  google.protobuf.Timestamp period_end = 7;
}
```

### **🔗 External Service Communication Patterns**

#### **Identity Service Integration:**
- **User Preferences** → Sync user language and regional preferences
- **Tenant Settings** → Validate tenant localization configuration
- **User Validation** → Verify user existence for preference updates

#### **All Services Integration:**
- **Content Localization** → Provide translations for all service content
- **Regional Settings** → Supply regional configurations to all services
- **Language Detection** → Help services determine user language preferences

### **📊 Implementation Guidelines**

#### **Service Configuration:**
- Configure supported languages and their properties
- Set up regional settings for different markets
- Define currency and timezone configurations
- Configure translation providers and fallback languages

#### **Error Handling:**
- Handle missing translations gracefully with fallback languages
- Implement translation caching for performance
- Log translation requests and failures
- Provide meaningful error messages in appropriate languages

#### **Monitoring & Health Checks:**
- Monitor translation service performance
- Track language usage patterns and trends
- Alert on translation service failures
- Generate reports on localization effectiveness

---

**This Localization Service ER diagram provides complete multi-language and regional support capabilities for your betting platform!** 🎯