# Vietnam Company Compass API

A comprehensive RESTful API for managing and accessing company information in Vietnam. This API provides endpoints for managing companies, businesses, locations, users, and API keys.

## Features

- Company management (CRUD operations)
- Business type management
- Location management (Provinces, Districts, Wards)
- User management with authentication
- API key management for secure access
- Usage tracking for API calls
- Crawler job management for data collection
- Company status management
- Company-Business mapping

## API Endpoints

### Companies
- `GET /api/companies` - Get all companies (with pagination)
- `GET /api/companies/{id}` - Get company by ID
- `GET /api/companies/taxcode/{taxCode}` - Get company by tax code
- `GET /api/companies/province/{provinceId}` - Get companies by province
- `GET /api/companies/district/{districtId}` - Get companies by district
- `GET /api/companies/ward/{wardId}` - Get companies by ward
- `GET /api/companies/status/{statusId}` - Get companies by status
- `GET /api/companies/count` - Count companies (with filters)
- `POST /api/companies` - Create new company
- `PUT /api/companies/{id}` - Update company
- `DELETE /api/companies/{id}` - Delete company

### Businesses
- `GET /api/businesses` - Get all businesses (with pagination)
- `GET /api/businesses/{id}` - Get business by ID
- `GET /api/businesses/code/{code}` - Get business by code
- `GET /api/businesses/count` - Count businesses
- `POST /api/businesses` - Create new business
- `PUT /api/businesses/{id}` - Update business
- `DELETE /api/businesses/{id}` - Delete business

### Company-Business Mappings
- `GET /api/company-business-mappings` - Get all mappings
- `GET /api/company-business-mappings/company/{companyId}` - Get mappings by company
- `GET /api/company-business-mappings/business/{businessId}` - Get mappings by business
- `GET /api/company-business-mappings/company/{companyId}/business/{businessId}` - Get specific mapping
- `POST /api/company-business-mappings` - Create new mapping
- `DELETE /api/company-business-mappings/company/{companyId}/business/{businessId}` - Delete mapping

### Locations

#### Provinces
- `GET /api/provinces` - Get all provinces (with pagination)
- `GET /api/provinces/{id}` - Get province by ID
- `GET /api/provinces/code/{code}` - Get province by code
- `GET /api/provinces/count` - Count provinces
- `POST /api/provinces` - Create new province
- `PUT /api/provinces/{id}` - Update province
- `DELETE /api/provinces/{id}` - Delete province

#### Districts
- `GET /api/districts` - Get all districts (with pagination)
- `GET /api/districts/{id}` - Get district by ID
- `GET /api/districts/code/{code}` - Get district by code
- `GET /api/districts/province/{provinceId}` - Get districts by province
- `GET /api/districts/count` - Count districts
- `POST /api/districts` - Create new district
- `PUT /api/districts/{id}` - Update district
- `DELETE /api/districts/{id}` - Delete district

#### Wards
- `GET /api/wards` - Get all wards (with pagination)
- `GET /api/wards/{id}` - Get ward by ID
- `GET /api/wards/code/{code}` - Get ward by code
- `GET /api/wards/district/{districtId}` - Get wards by district
- `GET /api/wards/province/{provinceId}` - Get wards by province
- `GET /api/wards/count` - Count wards
- `POST /api/wards` - Create new ward
- `PUT /api/wards/{id}` - Update ward
- `DELETE /api/wards/{id}` - Delete ward

### Users
- `GET /api/users` - Get all users (with pagination)
- `GET /api/users/{id}` - Get user by ID
- `GET /api/users/email/{email}` - Get user by email
- `GET /api/users/count` - Count users
- `POST /api/users` - Create new user
- `PUT /api/users/{id}` - Update user
- `PATCH /api/users/{id}/password` - Update user password
- `DELETE /api/users/{id}` - Delete user

### API Keys
- `GET /api/api-keys` - Get all API keys (with pagination)
- `GET /api/api-keys/{id}` - Get API key by ID
- `GET /api/api-keys/key/{key}` - Get API key by key
- `GET /api/api-keys/user/{userId}` - Get API keys by user
- `POST /api/api-keys` - Create new API key
- `PUT /api/api-keys/{id}` - Update API key
- `DELETE /api/api-keys/{id}` - Delete API key
- `POST /api/api-keys/validate` - Validate API key

### API Usage Tracking
- `GET /api/api-usage-tracking` - Get all usage trackings (with pagination)
- `GET /api/api-usage-tracking/{id}` - Get usage tracking by ID
- `GET /api/api-usage-tracking/apikey/{apiKeyId}` - Get usage trackings by API key
- `GET /api/api-usage-tracking/count` - Count usage trackings
- `POST /api/api-usage-tracking` - Create new usage tracking

### Company Status
- `GET /api/company-statuses` - Get all company statuses
- `GET /api/company-statuses/{id}` - Get company status by ID
- `GET /api/company-statuses/code/{code}` - Get company status by code
- `POST /api/company-statuses` - Create new company status
- `PUT /api/company-statuses/{id}` - Update company status
- `DELETE /api/company-statuses/{id}` - Delete company status

### Crawler Jobs
- `GET /api/crawler-jobs` - Get all crawler jobs (with pagination)
- `GET /api/crawler-jobs/{id}` - Get crawler job by ID
- `GET /api/crawler-jobs/count` - Count crawler jobs
- `POST /api/crawler-jobs` - Create new crawler job
- `PUT /api/crawler-jobs/{id}` - Update crawler job
- `DELETE /api/crawler-jobs/{id}` - Delete crawler job

### Crawling Status
- `GET /api/crawling-statuses` - Get all crawling statuses
- `GET /api/crawling-statuses/{id}` - Get crawling status by ID
- `GET /api/crawling-statuses/code/{code}` - Get crawling status by code
- `POST /api/crawling-statuses` - Create new crawling status
- `PUT /api/crawling-statuses/{id}` - Update crawling status
- `DELETE /api/crawling-statuses/{id}` - Delete crawling status

## Authentication

The API uses API keys for authentication. Each request must include a valid API key in the header:

```
X-API-Key: your-api-key-here
```

## Pagination

Most list endpoints support pagination using the following query parameters:
- `skip`: Number of records to skip
- `take`: Number of records to take

Example: `GET /api/companies?skip=0&take=10`

## Error Handling

The API uses standard HTTP status codes:
- 200: Success
- 201: Created
- 400: Bad Request
- 401: Unauthorized
- 403: Forbidden
- 404: Not Found
- 409: Conflict
- 500: Internal Server Error

## Data Models

The API uses the following main data models:
- Company
- Business
- CompanyBusinessMapping
- Province
- District
- Ward
- User
- ApiKey
- ApiUsageTracking
- CompanyStatus
- CrawlerJob
- CrawlingStatus

## Development

### Prerequisites
- .NET 6.0 or later
- SQL Server or compatible database

### Setup
1. Clone the repository
2. Update the connection string in `appsettings.json`
3. Run database migrations
4. Start the application

### Running the Application
```bash
dotnet run
```

## License

[Add your license information here]
