# Mymarket Backend Documentation

This document describes the current backend implementation in this repository. It documents the REST API, request/response models, domain entities, enums, authentication, roles, permissions, validation behavior, and project setup.

Chat and SignalR are intentionally excluded, per request.

## Project Overview

Mymarket is an ASP.NET Core Web API for a marketplace application. The solution follows a layered/Clean Architecture style:

- `Mymarket.WebApi`: HTTP controllers, middleware, current-user service, CORS, OpenAPI/Scalar setup.
- `Mymarket.Application`: CQRS commands/queries, DTOs, validators, interfaces, localization resources.
- `Mymarket.Domain`: entities, enums, constants, common models.
- `Mymarket.Infrastructure`: EF Core PostgreSQL data access, migrations, authentication, authorization policies, email/image services, caching, Supabase integration.

Main backend capabilities:

- User registration, email verification, login, refresh token, password recovery.
- User profile read/update.
- Category, brand, attribute, attribute option, unit, home-category management.
- Category-brand and category-attribute linking.
- Post creation, search, detail, favorites, views, enable/disable/delete.
- Role and permission authorization support.

## Technology Stack

- .NET / ASP.NET Core Web API
- MediatR for commands and queries
- FluentValidation for request validation
- Entity Framework Core with PostgreSQL provider
- JWT bearer authentication
- Custom permission-based authorization policies
- Supabase client for storage/infrastructure integration
- EFCoreSecondLevelCacheInterceptor memory cache
- Scalar/OpenAPI in development

## Runtime Configuration

`src/Mymarket.WebApi/appsettings.json` only contains logging and `AllowedHosts`. The infrastructure layer expects additional configuration at runtime:

```json
{
  "ConnectionStrings": {
    "Supabase": "postgres connection string"
  },
  "JwtOptions": {
    "Issuer": "issuer",
    "Audience": "audience",
    "Secret": "long signing secret",
    "AccessTokenTtl": 15,
    "RefreshTokenTtl": 43200
  },
  "Supabase": {
    "Url": "https://...",
    "AnonKey": "..."
  }
}
```

The exact values are environment-specific and should be provided via user secrets, environment variables, or deployment configuration.

## Middleware Pipeline

Configured in `MiddlewareConfiguration`:

1. Request localization using `Accept-Language`.
2. Development-only OpenAPI and Scalar API reference.
3. Global exception handling.
4. Anonymous session cookie middleware.
5. HTTPS redirection.
6. CORS policy.
7. Authentication.
8. Authorization.
9. Controllers.

Supported UI cultures:

- `en-US`
- `ru-RU`
- `ka-GE`

Default culture: `en-US`.

## CORS

Policy name: `CorsPolicy`

Allowed origins:

- `http://localhost:4200`
- `http://localhost:5173`

Allowed:

- Any HTTP method
- Any header
- Credentials

## Authentication

Authentication uses JWT bearer tokens.

Protected endpoints require:

```http
Authorization: Bearer <accessToken>
```

Access tokens include these custom claims:

| Claim | Meaning |
| --- | --- |
| `id` | User id |
| `un` | User first name |
| `em` | User email |
| `al` | Access level numeric value |
| `prm` | Permission id; one claim per permission |

Refresh tokens are stored on the user record with an expiry timestamp.

## Authorization

Some endpoints use `[Authorize]`; others use the custom `[HasPermission]` attribute.

`HasPermission(permission, accessLevel)` requires:

1. The JWT has an `al` access-level claim.
2. User access level is greater than or equal to the required access level.
3. If a permission is specified, the JWT contains a `prm` claim matching the permission id.

Default `HasPermission` access level is `Admin`.

When permission is `default` and access level is provided, only access level is checked. This is used by role endpoints for `SuperAdmin`.

## Access Levels

| Value | Name |
| --- | --- |
| 1 | `User` |
| 2 | `Admin` |
| 3 | `SuperAdmin` |

## Permissions

Permissions are seeded from the `Permissions` enum into the `Permissions` table. Permission ids are stable because EF config uses `ValueGeneratedNever`.

| Id | Permission |
| --- | --- |
| 1 | `CategoriesView` |
| 2 | `CategoriesAdd` |
| 3 | `CategoriesEdit` |
| 4 | `CategoriesDelete` |
| 21 | `HomeCategoriesView` |
| 22 | `HomeCategoriesAdd` |
| 23 | `HomeCategoriesEdit` |
| 24 | `HomeCategoriesDelete` |
| 25 | `HomeCategoriesReorder` |
| 41 | `BrandsView` |
| 42 | `BrandsAdd` |
| 43 | `BrandsEdit` |
| 44 | `BrandsDelete` |
| 61 | `UnitsView` |
| 62 | `UnitsAdd` |
| 63 | `UnitsEdit` |
| 64 | `UnitsDelete` |
| 81 | `AttributesView` |
| 82 | `AttributeAdd` |
| 83 | `AttributeEdit` |
| 84 | `AttributeDelete` |
| 101 | `AttributeOptionsView` |
| 102 | `AttributeOptionsAdd` |
| 103 | `AttributeOptionsEdit` |
| 104 | `AttributeOptionsDelete` |

## Enums

### `GenderType`

| Value | Name |
| --- | --- |
| 1 | `Male` |
| 2 | `Female` |

### `AccessLevelType`

| Value | Name |
| --- | --- |
| 1 | `User` |
| 2 | `Admin` |
| 3 | `SuperAdmin` |

### `AttributeType`

| Value | Name |
| --- | --- |
| 1 | `Text` |
| 2 | `Number` |
| 3 | `Select` |
| 4 | `Bool` |

### `CategoryPostType`

| Value | Name |
| --- | --- |
| 1 | `SellBuy` |
| 2 | `Rent` |
| 3 | `Service` |

### `CodeType`

| Value | Name |
| --- | --- |
| 1 | `EmailVerification` |
| 2 | `PasswordRecovery` |

### `ConditionType`

| Value | Name |
| --- | --- |
| 1 | `Used` |
| 2 | `New` |
| 3 | `LikeNew` |
| 4 | `ForParts` |

### `CurrencyType`

| Value | Name |
| --- | --- |
| 1 | `GEL` |
| 2 | `USD` |

### `ImageTargetType`

| Value | Name |
| --- | --- |
| 1 | `Post` |

### `PostStatus`

| Value | Name |
| --- | --- |
| 0 | `Active` |
| 1 | `Inactive` |
| 2 | `Blocked` |

### `PostType`

| Value | Name |
| --- | --- |
| 1 | `Sell` |
| 2 | `Buy` |
| 3 | `Rent` |
| 4 | `Service` |

### `PromoType`

| Value | Name |
| --- | --- |
| 0 | `None` |
| 1 | `VIP` |
| 2 | `VIP_PLUS` |
| 3 | `SUPER_VIP` |

### `SortType`

| Value | Name |
| --- | --- |
| 1 | `DateDesc` |
| 2 | `DateAsc` |
| 3 | `PriceDesc` |
| 4 | `PriceAsc` |
| 5 | `ViewsDesc` |
| 6 | `ViewsAsc` |
| 7 | `WithDiscount` |

## Common Response Models

### `PaginatedResult<T>`

```json
{
  "items": [],
  "page": 1,
  "pageSize": 20,
  "totalCount": 0,
  "totalPages": 0,
  "hasNextPage": false,
  "hasPreviousPage": false
}
```

### Problem Details

Validation errors return HTTP 400 with `application/problem+json`.

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "Validation Failed",
  "status": 400,
  "instance": "POST /api/example",
  "errors": {
    "field": ["message"]
  },
  "code": "ValidationError"
}
```

Unauthorized errors return HTTP 401:

```json
{
  "title": "Unauthorized",
  "status": 401,
  "detail": "You are not authorized to access this resource.",
  "code": "UnauthorizedAccessError"
}
```

Unverified-email errors return HTTP 403:

```json
{
  "title": "Email not verified",
  "status": 403,
  "email": "user@example.com",
  "code": "EmailNotVerified"
}
```

Unhandled exceptions return HTTP 500 with code `UnexpectedError`.

## Domain Entities

### `UserEntity`

Represents an application user.

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `Firstname` | `string` | Required, max 255 |
| `LastName` | `string` | Required, max 255 |
| `Email` | `string` | Required, max 256, unique |
| `Gender` | `GenderType` | Required |
| `BirthYear` | `int` | Required |
| `PhoneNumber` | `string` | Required, unique |
| `PasswordHash` | `string` | Required |
| `EmailVerified` | `bool` | Required |
| `RefreshToken` | `string?` | Current refresh token |
| `RefreshTokenExpiry` | `DateTime` | Refresh token expiry |
| `AccessLevel` | `AccessLevelType` | Defaults to `User` |
| `Roles` | `ICollection<RoleEntity>` | Many-to-many via `UserRoles` |
| `Posts` | `ICollection<PostEntity>` | User posts |
| `PostViews` | `ICollection<PostViewEntity>` | User views |

### `RoleEntity`

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `Name` | `string` | Required, max 100, unique |
| `Permissions` | `ICollection<PermissionEntity>` | Many-to-many via `RolePermissions` |
| `Users` | `ICollection<UserEntity>` | Many-to-many via `UserRoles` |

### `PermissionEntity`

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Enum value, not generated |
| `Name` | `string` | Required, max 100, unique |
| `Roles` | `ICollection<RoleEntity>` | Roles with this permission |

### `VerificationCodeEntity`

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `UserId` | `int` | Required |
| `CodeHash` | `string` | Required |
| `ExpiresAt` | `DateTime` | Required |
| `CodeType` | `CodeType` | Email verification or password recovery |
| `IsVerified` | `bool` | Required |
| `User` | `UserEntity?` | FK, cascade delete |

### `CategoryEntity`

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `ParentId` | `int?` | Parent category id |
| `Name` | `string` | Required, max 255 |
| `NameEn` | `string?` | Max 255 |
| `NameRu` | `string?` | Max 255 |
| `BrandRequired` | `bool` | Defaults false |
| `BrandVisible` | `bool` | Defaults false |
| `LogoId` | `int?` | Optional image id |
| `Logo` | `ImageEntity?` | Optional image |
| `CategoryPostType` | `CategoryPostType` | Required |
| `Parent` | `CategoryEntity?` | Parent navigation |
| `Children` | `ICollection<CategoryEntity>` | Child categories |

### `BrandEntity`

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `Name` | `string` | Required, max 255 |
| `LogoId` | `int` | Required image id |
| `Logo` | `ImageEntity` | Required image, restrict delete |

### `AttributeEntity`

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `Code` | `string` | Required, max 255, unique |
| `Name` | `string` | Required, max 255 |
| `NameEn` | `string?` | Max 255 |
| `NameRu` | `string?` | Max 255 |
| `AttributeType` | `AttributeType` | Required |
| `UnitId` | `int?` | Optional unit id |
| `Unit` | `AttributeUnitEntity?` | Restrict delete |
| `PostAttributes` | `ICollection<PostAttributesEntity>` | Post values |

### `AttributeUnitEntity`

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `Name` | `string` | Required, max 255 |
| `NameEn` | `string` | Required, max 255 |
| `NameRu` | `string` | Required, max 255 |
| `Attributes` | `ICollection<AttributeEntity>` | Linked attributes |

### `AttributeOptionsEntity`

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `AttributeId` | `int` | Required |
| `Attribute` | `AttributeEntity?` | Parent attribute |
| `Name` | `string` | Required, max 255 |
| `NameEn` | `string?` | Max 255 |
| `NameRu` | `string?` | Max 255 |
| `Order` | `int` | Required |

### `CategoryAttributesEntity`

Links categories to allowed attributes.

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `CategoryId` | `int` | Required |
| `Category` | `CategoryEntity?` | Category |
| `AttributeId` | `int` | Required |
| `Attribute` | `AttributeEntity?` | Attribute |
| `IsRequired` | `bool` | Required |
| `Order` | `int` | Required |

### `CategoryBrandsEntity`

Links categories to allowed brands.

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `CategoryId` | `int` | Required |
| `BrandId` | `int` | Required |

There is a unique index on `(CategoryId, BrandId)`.

### `PostEntity`

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `PostType` | `PostType` | Sell/buy/rent/service |
| `CategoryId` | `int` | Required |
| `YoutubeLink` | `string?` | Optional |
| `ConditionType` | `ConditionType` | Required |
| `Title` | `string` | Required, max 255 |
| `Description` | `string?` | Max 4000 |
| `TitleEn` | `string?` | Max 255 |
| `DescriptionEn` | `string?` | Max 4000 |
| `TitleRu` | `string?` | Max 255 |
| `DescriptionRu` | `string?` | Max 4000 |
| `ForDisabledPerson` | `bool` | Defaults false |
| `Price` | `double?` | Optional in entity, required by add command |
| `CurrencyType` | `CurrencyType` | Required |
| `SalePercentage` | `byte` | Discount percentage |
| `CanOfferPrice` | `bool` | Defaults false |
| `IsNegotiable` | `bool` | Defaults false |
| `CityId` | `int` | Required |
| `Name` | `string` | Contact name, required |
| `PhoneNumber` | `string` | Required |
| `UserId` | `int` | Owner id |
| `PromoType` | `PromoType?` | Optional |
| `PromoDays` | `int?` | Optional |
| `IsColored` | `bool` | Defaults false |
| `ColorDays` | `int?` | Optional |
| `AutoRenewal` | `bool` | Defaults false |
| `AutoRenewalOnceIn` | `int?` | Optional |
| `AutoRenewalAtTime` | `int?` | Optional |
| `BrandId` | `int?` | Optional |
| `Status` | `PostStatus` | Defaults `Active` |
| `PostsImages` | `ICollection<PostsImagesEntity>` | Images |
| `Favorites` | `ICollection<FavoritesEntity>` | Favorites |
| `PostAttributes` | `ICollection<PostAttributesEntity>` | Attribute values |
| `PostViews` | `ICollection<PostViewEntity>` | View records |

### `PostAttributesEntity`

Stores submitted post attribute values.

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `PostId` | `int` | Required |
| `AttributeId` | `int` | Required |
| `Value` | `string` | Stored normalized value, max 255 |
| `ValueType` | `AttributeType` | Attribute type at write time |

### `PostsImagesEntity`

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `PostId` | `int` | Required |
| `ImageId` | `int` | Required |
| `Order` | `int` | Required |

### `ImageEntity`

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `Url` | `string` | Required, max 500 |
| `UniqueId` | `Guid` | Required |

### `CityEntity`

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `Name` | `string` | Required, max 255 |

### `HomeCategoriesEntity`

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `CategoryId` | `int` | Required |
| `Order` | `int` | Display order |

### `FavoritesEntity`

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `UserId` | `int` | User id |
| `PostId` | `int` | Post id |

### `PostViewEntity`

Tracks unique daily post views by authenticated user or anonymous session cookie.

| Property | Type | Notes |
| --- | --- | --- |
| `Id` | `int` | Primary key |
| `PostId` | `int` | Required |
| `UserId` | `int?` | Authenticated user id |
| `SessionId` | `Guid?` | Anonymous session id |
| `ViewDate` | `DateOnly` | Daily uniqueness key |
| `ViewedAt` | `DateTime` | Timestamp |

Unique indexes:

- `(PostId, UserId, ViewDate)` where `UserId IS NOT NULL`
- `(PostId, SessionId, ViewDate)` where `SessionId IS NOT NULL`

## REST API

Base route prefix is `/api`.

Request body binding:

- JSON body unless the endpoint says `multipart/form-data`.
- `FromForm` endpoints are used for commands that include images/files.
- Query parameters use standard ASP.NET Core model binding.

### Auth Endpoints

#### `POST /api/auth/register-user`

Registers a user. The user is created with `EmailVerified = false` and `AccessLevel = User`.

Auth: Public

Request body:

```json
{
  "firstname": "John",
  "lastname": "Doe",
  "email": "john@example.com",
  "gender": 1,
  "birthYear": 1995,
  "phoneNumber": "+995555000000",
  "password": "Password1!",
  "passwordConfirm": "Password1!"
}
```

Response:

- `201 Created`

Validation:

- `Firstname`: required, max 72, letters only.
- `Lastname`: required, max 72, letters only.
- `Email`: required, valid email, max 256, unique.
- `Gender`: required.
- `BirthYear`: required.
- `PhoneNumber`: required, unique.
- `Password`: required, 8-64 chars, no spaces, at least one uppercase letter, one number, one special character.
- `PasswordConfirm`: required and should match password.

#### `POST /api/auth/send-email-verification-code`

Sends a 3-minute email verification code.

Auth: Public

Request body:

```json
{
  "email": "john@example.com"
}
```

Response:

- `204 No Content`

#### `POST /api/auth/verify-email-code`

Verifies email and returns auth tokens.

Auth: Public

Request body:

```json
{
  "email": "john@example.com",
  "code": "123456"
}
```

Response `200 OK`:

```json
{
  "accessToken": "...",
  "refreshToken": "...",
  "expiresAt": "2026-05-16T10:00:00Z",
  "user": {
    "id": 1,
    "name": "John",
    "lastname": "Doe",
    "email": "john@example.com",
    "emailVerified": true,
    "favoritesCount": 0,
    "number": "+995555000000",
    "genderType": 1,
    "birthYear": 1995
  }
}
```

#### `POST /api/auth/login-user`

Logs in using email or phone number.

Auth: Public

Request body:

```json
{
  "emailOrPhone": "john@example.com",
  "password": "Password1!"
}
```

Response:

- `200 OK` with `AuthDto`.
- `403 Forbidden` with `EmailNotVerified` if email is not verified.

#### `POST /api/auth/send-password-recovery`

Sends a 3-minute password recovery code to a verified user.

Auth: Public

Request body:

```json
{
  "email": "john@example.com"
}
```

Response:

- `204 No Content`

#### `POST /api/auth/verify-password-code`

Marks a password recovery code as verified.

Auth: Public

Request body:

```json
{
  "email": "john@example.com",
  "code": "123456"
}
```

Response:

- `204 No Content`

#### `POST /api/auth/password-recovery`

Updates password by recovery code.

Auth: Public

Request body:

```json
{
  "code": "123456",
  "password": "NewPassword1!",
  "passwordConfirm": "NewPassword1!"
}
```

Response:

- `204 No Content`

#### `POST /api/auth/refresh-user`

Refreshes access and refresh tokens.

Auth: Public

Request body:

```json
{
  "accessToken": "expired-or-current-token",
  "refreshToken": "refresh-token"
}
```

Response:

- `200 OK` with `AuthDto`.

#### `GET /api/auth/user-exists`

Checks whether a user exists.

Auth: Public

Query model:

```http
GET /api/auth/user-exists?email=john@example.com&phoneNumber=+995555000000
```

Request fields are defined by `UserExistsQuery`.

Response:

- `204 No Content` if user exists.
- `404 Not Found` if user does not exist.

### User Endpoints

#### `GET /api/users/{id}`

Returns public user information.

Auth: Public

Response `200 OK`:

```json
{
  "id": 1,
  "firstName": "John",
  "lastname": "Doe",
  "email": "john@example.com",
  "genderType": 1,
  "birthYear": 1995,
  "phoneNumber": "+995555000000",
  "emailVerified": true,
  "postsCount": 4
}
```

#### `GET /api/users/{id}/phone-number`

Returns the user's phone number.

Auth: Public

Response:

- `200 OK` with a string.
- `404 Not Found` if not found.

#### `GET /api/users/current`

Returns the authenticated current user.

Auth: Required

Response:

- `200 OK` with `UserDto`.
- `404 Not Found` if current user cannot be loaded.

#### `PUT /api/users`

Updates the authenticated user's account.

Auth: Required

Request body:

```json
{
  "firstname": "John",
  "lastname": "Doe",
  "email": "john@example.com",
  "gender": 1,
  "birthYear": 1995,
  "phoneNumber": "+995555000000"
}
```

Response:

- `204 No Content`

### Post Endpoints

#### `POST /api/posts`

Creates a post.

Auth: Required

Content type: `multipart/form-data`

Form fields:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| `postType` | `PostType` | Yes | 1 sell, 2 buy, 3 rent, 4 service |
| `categoryId` | `int` | Yes | Must be positive |
| `youtubeLink` | `string?` | No | Optional |
| `conditionType` | `ConditionType` | Yes | Used/new/etc. |
| `images` | `List<IFormFile>` | Conditional | Additional images |
| `mainImage` | `IFormFile` | Conditional | Used if `images` is empty |
| `brandId` | `int?` | Conditional | Required when category requires brand |
| `title` | `string` | Yes | Max 200 by validator, 255 in DB |
| `description` | `string?` | No | Max 4000 |
| `titleEn` | `string?` | No | Max 255 |
| `descriptionEn` | `string?` | No | Max 4000 |
| `titleRu` | `string?` | No | Max 255 |
| `descriptionRu` | `string?` | No | Max 4000 |
| `forDisabledPerson` | `bool` | Yes | Accessibility flag |
| `price` | `double` | Yes | Must be >= 0 |
| `currencyType` | `CurrencyType` | Yes | 1 GEL, 2 USD |
| `salePercentage` | `byte` | Yes | Discount percentage |
| `canOfferPrice` | `bool` | Yes | Offer allowed |
| `isNegotiable` | `bool` | Yes | Price negotiable |
| `cityId` | `int` | Yes | Must be positive |
| `name` | `string` | Yes | Contact name, max 72 by validator |
| `phoneNumber` | `string` | Yes | Contact phone |
| `promoType` | `PromoType?` | No | Promotion level |
| `promoDays` | `int?` | Conditional | Required when promo selected, must be > 0 |
| `isColored` | `bool` | Yes | Highlight flag |
| `colorDays` | `int?` | Conditional | Required when colored, must be > 0 |
| `autoRenewal` | `bool` | Yes | Auto-renew flag |
| `autoRenewalOnceIn` | `int?` | Conditional | Required when auto-renewal, must be > 0 |
| `autoRenewalAtTime` | `int?` | No | Renewal time value |
| `attributesJson` | `string?` | No | JSON array of attribute values |

`attributesJson` format:

```json
[
  { "id": 10, "value": "red" },
  { "id": 11, "value": 42 },
  { "id": 12, "value": 5 },
  { "id": 13, "value": 1 }
]
```

Attribute value validation:

- Attribute must be linked to the selected category.
- Required category attributes must have a value.
- `Text`: JSON string, not blank.
- `Number`: JSON number or numeric string.
- `Select`: valid option id for that attribute.
- `Bool`: integer `1` or `2`.

Response:

- `201 Created`

#### `GET /api/posts`

Searches posts.

Auth: Public

Query parameters:

| Parameter | Type | Default | Notes |
| --- | --- | --- | --- |
| `priceFrom` | `int?` | null | Minimum price |
| `priceTo` | `int?` | null | Maximum price |
| `offerPrice` | `bool?` | null | Can offer price |
| `discount` | `bool?` | null | Discounted posts |
| `locId` | `int?` | null | City/location id |
| `condType` | `List<ConditionType>?` | null | Repeated query param |
| `postType` | `List<PostType>?` | null | Repeated query param |
| `sortType` | `SortType?` | null | Sorting |
| `forPsn` | `bool?` | null | For disabled person |
| `catId` | `int?` | null | Category id |
| `brandId` | `int?` | null | Brand id |
| `sortBy` | `SortType?` | null | Additional sort parameter |
| `keyword` | `string?` | null | Text search |
| `page` | `int` | 1 | Page number |
| `pageSize` | `int` | 20 | Page size |

Response `200 OK`:

```json
{
  "result": {
    "items": [
      {
        "id": 1,
        "autoRenewal": false,
        "canOfferPrice": true,
        "categoryId": 5,
        "conditionType": 2,
        "currencyType": 1,
        "description": "Description",
        "forDisabledPerson": false,
        "isColored": false,
        "isNegotiable": true,
        "name": "John",
        "phoneNumber": "+995555000000",
        "postType": 1,
        "price": 100,
        "promoType": 0,
        "salePercentage": 0,
        "priceAfterDiscount": 100,
        "title": "Title",
        "brandId": 3,
        "images": ["https://..."],
        "isFavorite": false
      }
    ],
    "page": 1,
    "pageSize": 20,
    "totalCount": 1,
    "totalPages": 1,
    "hasNextPage": false,
    "hasPreviousPage": false
  },
  "breadcrumb": [],
  "categories": []
}
```

#### `GET /api/posts/{id}`

Returns post details.

Auth: Public

Response `200 OK`:

```json
{
  "id": 1,
  "autoRenewal": false,
  "canOfferPrice": true,
  "categoryId": 5,
  "conditionType": 2,
  "currencyType": 1,
  "description": "Description",
  "forDisabledPerson": false,
  "isColored": false,
  "isNegotiable": true,
  "name": "John",
  "phoneNumber": "+995555000000",
  "postType": 1,
  "price": 100,
  "priceAfterDiscount": 100,
  "promoType": 0,
  "salePercentage": 0,
  "city": "Tbilisi",
  "title": "Title",
  "isFavorite": false,
  "viewsCount": 10,
  "createdAt": "2026-05-16T10:00:00Z",
  "chatExists": false,
  "breadcrumb": [],
  "attributes": [],
  "images": [],
  "user": {
    "id": 1,
    "firstName": "John",
    "lastname": "Doe",
    "email": "john@example.com",
    "genderType": 1,
    "birthYear": 1995,
    "phoneNumber": "+995555000000",
    "emailVerified": true,
    "postsCount": 4
  }
}
```

#### `GET /api/posts/{id}/view`

Records a view for a post.

Auth: Public

Response:

- `204 No Content`

Anonymous users are tracked by the `anon_sid` cookie generated by `SessionMiddleware`.

#### `GET /api/posts/lite`

Returns promoted post lists.

Auth: Public

Response `200 OK`:

```json
{
  "superVip": [],
  "vipPlus": [],
  "vip": []
}
```

#### `GET /api/posts/my`

Returns the authenticated user's posts.

Auth: Required

Query parameters:

| Parameter | Type | Default |
| --- | --- | --- |
| `page` | `int` | 1 |
| `pageSize` | `int` | 10 |
| `postStatus` | `PostStatus` | `Active` |

Response `200 OK`:

```json
{
  "result": {
    "items": [],
    "page": 1,
    "pageSize": 10,
    "totalCount": 0,
    "totalPages": 0,
    "hasNextPage": false,
    "hasPreviousPage": false
  },
  "activeCount": 0,
  "inactiveCount": 0,
  "blockedCount": 0,
  "totalViews": 0
}
```

#### `GET /api/posts/favorite`

Returns authenticated user's favorite posts.

Auth: Required

Query parameters:

| Parameter | Type | Default |
| --- | --- | --- |
| `page` | `int` | 1 |
| `pageSize` | `int` | 20 |

Response:

- `200 OK` with paginated post data.

#### `POST /api/posts/{id}/favorite`

Adds a post to favorites.

Auth: Required

Response:

- `204 No Content`

#### `DELETE /api/posts/{id}/favorite`

Removes a post from favorites.

Auth: Required

Response:

- `204 No Content`

#### `DELETE /api/posts/{id}`

Deletes a post.

Auth: Required

Response:

- `204 No Content`

#### `POST /api/posts/{id}/disable`

Disables a post.

Auth: Required

Response:

- `204 No Content`

#### `POST /api/posts/{id}/enable`

Enables a post.

Auth: Required

Response:

- `204 No Content`

### Category Endpoints

#### `GET /api/categories`

Returns flat localized category list.

Auth: Public

Response item:

```json
{
  "id": 1,
  "parentId": null,
  "name": "Cars",
  "categoryPostType": 1,
  "brandRequired": true,
  "logoUrl": "https://..."
}
```

#### `GET /api/categories/get-localized`

Returns localized category DTOs.

Auth: Public

Response item:

```json
{
  "id": 1,
  "parentId": null,
  "name": "Cars",
  "nameEn": "Cars",
  "nameRu": "Cars",
  "hasChildren": true,
  "brandRequired": true,
  "brandVisible": true,
  "categoryPostType": 1,
  "logoUrl": "https://..."
}
```

#### `GET /api/categories/{id}`

Returns category by id.

Auth: Public

Response:

- `200 OK` with `CategoryDto`.
- `404 Not Found`.

#### `GET /api/categories/{id}/attributes`

Returns attributes configured for a category.

Auth: Public

Response item:

```json
{
  "id": 1,
  "categoryId": 5,
  "attributeId": 10,
  "attributeName": "Color",
  "attributeType": 3,
  "unitName": null,
  "isRequired": true,
  "order": 1,
  "options": [
    {
      "id": 1,
      "name": "Red",
      "order": 1
    }
  ]
}
```

#### `GET /api/categories/{id}/brands`

Returns brands allowed for a category.

Auth: Public

Response:

- `200 OK` with list of `BrandDto`.

#### `POST /api/categories`

Creates category.

Auth: Required, `CategoriesAdd`, minimum `Admin`.

Content type: `multipart/form-data`

Form fields:

| Field | Type | Required |
| --- | --- | --- |
| `parentId` | `int?` | No |
| `name` | `string` | Yes |
| `nameEn` | `string?` | Yes by validator |
| `nameRu` | `string?` | Yes by validator |
| `brandRequired` | `bool` | Yes |
| `brandVisible` | `bool` | Yes |
| `logo` | `IFormFile?` | No |
| `categoryPostType` | `CategoryPostType` | Yes |

Response:

- `200 OK` with `CategoryDto`.

#### `PUT /api/categories/{id}`

Updates category.

Auth: Required, `CategoriesEdit`, minimum `Admin`.

Content type: `multipart/form-data`

Fields: same as create plus route `id`.

Response:

- `200 OK` with `CategoryDto`.

#### `DELETE /api/categories/{id}`

Deletes category.

Auth: Required, `CategoriesDelete`, minimum `Admin`.

Response:

- `204 No Content`

### Brand Endpoints

#### `GET /api/brands`

Returns all brands.

Auth: Public

Response item:

```json
{
  "id": 1,
  "name": "Apple",
  "logoId": 10,
  "logoUrl": "https://..."
}
```

#### `GET /api/brands/{id}`

Returns brand by id.

Auth: Public

Response:

- `200 OK` with `BrandDto`.
- `404 Not Found`.

#### `POST /api/brands`

Creates brand.

Auth: Required, `BrandsAdd`, minimum `Admin`.

Content type: `multipart/form-data`

Form fields:

| Field | Type | Required |
| --- | --- | --- |
| `name` | `string` | Yes, max 255 |
| `logo` | `IFormFile` | Yes |

Response:

- `200 OK` with `BrandDto`.

#### `PUT /api/brands/{id}`

Updates brand.

Auth: Required, `BrandsEdit`, minimum `Admin`.

Content type: `multipart/form-data`

Form fields:

| Field | Type | Required |
| --- | --- | --- |
| `name` | `string` | Yes, max 255 |
| `logo` | `IFormFile?` | No |

Response:

- `200 OK` with `BrandDto`.

#### `DELETE /api/brands/{id}`

Deletes brand.

Auth: Required, `BrandsDelete`, minimum `Admin`.

Response:

- `204 No Content`

### Attribute Endpoints

All endpoints in this controller have `[Authorize]`.

#### `GET /api/attributes`

Returns attributes.

Auth: Required

Response item:

```json
{
  "id": 1,
  "code": "color",
  "name": "Color",
  "nameEn": "Color",
  "nameRu": "Color",
  "attributeType": 3,
  "unitId": null
}
```

#### `GET /api/attributes/{id}`

Returns attribute by id.

Auth: Required

Response:

- `200 OK` with `AttributeDto`.
- `404 Not Found`.

#### `GET /api/attributes/{id}/options`

Returns options for an attribute.

Auth: Required

Response item:

```json
{
  "id": 1,
  "order": 1,
  "name": "Red",
  "nameEn": "Red",
  "nameRu": "Red"
}
```

#### `POST /api/attributes`

Creates attribute.

Auth: Required, `AttributeAdd`, minimum `Admin`.

Request body:

```json
{
  "name": "Color",
  "nameEn": "Color",
  "nameRu": "Color",
  "code": "color",
  "unitId": null,
  "attributeType": 3
}
```

Response:

- `200 OK` with `AttributeDto`.

#### `PUT /api/attributes/{id}`

Updates attribute.

Auth: Required, `AttributeEdit`, minimum `Admin`.

Request body:

```json
{
  "name": "Color",
  "nameEn": "Color",
  "nameRu": "Color",
  "code": "color",
  "unitId": null,
  "attributeType": 3
}
```

Response:

- `204 No Content`

#### `DELETE /api/attributes/{id}`

Deletes attribute.

Auth: Required, `AttributeDelete`, minimum `Admin`.

Response:

- `204 No Content`

Implementation note: the current controller sends `DeleteUnitCommand` from this endpoint. The intended command appears to be `DeleteAttributeCommand`.

### Attribute Option Endpoints

#### `POST /api/attribute-options`

Creates an attribute option.

Auth: Required, `AttributeOptionsAdd`, minimum `Admin`.

Request body:

```json
{
  "attributeId": 1,
  "order": 1,
  "name": "Red",
  "nameEn": "Red",
  "nameRu": "Red"
}
```

Response:

- `200 OK` with `AttributeOptionDto`.

#### `PUT /api/attribute-options/{id}`

Updates an attribute option.

Auth: Required, currently annotated with `AttributeOptionsDelete`.

Request body:

```json
{
  "order": 1,
  "name": "Red",
  "nameEn": "Red",
  "nameRu": "Red"
}
```

Response:

- `204 No Content`

Implementation note: the edit endpoint currently uses delete permission. It likely should use `AttributeOptionsEdit`.

#### `DELETE /api/attribute-options/{id}`

Deletes an attribute option.

Auth: Required, currently annotated with `AttributeOptionsEdit`.

Response:

- `204 No Content`

Implementation note: the delete endpoint currently uses edit permission. It likely should use `AttributeOptionsDelete`.

### Unit Endpoints

All endpoints in this controller have `[Authorize]`.

#### `GET /api/units`

Returns attribute units.

Auth: Required

Response item:

```json
{
  "id": 1,
  "name": "Kilometer",
  "nameEn": "Kilometer",
  "nameRu": "Kilometer"
}
```

#### `GET /api/units/{id}`

Returns unit by id.

Auth: Required

Response:

- `200 OK` with `UnitDto`.
- `404 Not Found`.

#### `POST /api/units`

Creates unit.

Auth: Required, `UnitsAdd`, minimum `Admin`.

Request body:

```json
{
  "name": "Kilometer",
  "nameEn": "Kilometer",
  "nameRu": "Kilometer"
}
```

Response:

- `204 No Content`

Implementation note: the command returns `UnitDto`, but the controller responds with no content.

#### `PUT /api/units/{id}`

Updates unit.

Auth: Required, `UnitsEdit`, minimum `Admin`.

Request body:

```json
{
  "name": "Kilometer",
  "nameEn": "Kilometer",
  "nameRu": "Kilometer"
}
```

Response:

- `204 No Content`

#### `DELETE /api/units/{id}`

Deletes unit.

Auth: Required, `UnitsDelete`, minimum `Admin`.

Response:

- `204 No Content`

### Category Attribute Endpoints

#### `GET /api/category-attributes`

Returns category attributes using `GetAttributesQuery`.

Auth: Public

Request binding is currently an unannotated `GetAttributesQuery` parameter.

Response:

- `200 OK`

#### `POST /api/category-attributes`

Links an attribute to a category.

Auth: Public in current controller.

Request body:

```json
{
  "categoryId": 5,
  "attributeId": 10,
  "isRequired": true,
  "order": 1
}
```

Response:

- `200 OK` with `CategoryAttributeDto`.

#### `PUT /api/category-attributes/{id}`

Updates a category-attribute link.

Auth: Public in current controller.

Request body:

```json
{
  "attributeId": 10,
  "isRequired": true,
  "order": 1
}
```

Response:

- `204 No Content`

#### `DELETE /api/category-attributes/{id}`

Deletes a category-attribute link.

Auth: Public in current controller.

Response:

- `204 No Content`

### Category Brand Endpoints

#### `GET /api/category-brands`

Returns all category-brand links.

Auth: Public

Response item:

```json
{
  "id": 1,
  "categoryId": 5,
  "brandId": 3,
  "name": "Apple"
}
```

#### `GET /api/category-brands/{id}`

Returns category-brand links filtered by id parameter passed to query.

Auth: Public

Response:

- `200 OK`

#### `POST /api/category-brands/link`

Links a brand to a category.

Auth: Public in current controller.

Request body:

```json
{
  "categoryId": 5,
  "brandId": 3
}
```

Response:

- `204 No Content`

#### `POST /api/category-brands/unlink`

Unlinks a brand from a category.

Auth: Public in current controller.

Request body:

```json
{
  "categoryId": 5,
  "brandId": 3
}
```

Response:

- `204 No Content`

Business rule: unlinking fails if existing posts use the category-brand pair.

#### `POST /api/category-brands/add-multiple`

Links multiple brands to a category.

Auth: Public in current controller.

Request body:

```json
{
  "categoryId": 5,
  "brandIds": [3, 4, 5]
}
```

Response:

- `200 OK` with list of `CategoryBrandDto`.

#### `PUT /api/category-brands/{id}`

Updates a category-brand link.

Auth: Public in current controller.

Request body:

```json
{
  "categoryId": 5,
  "brandId": 3
}
```

Response:

- `200 OK`

#### `DELETE /api/category-brands/{id}`

Deletes a category-brand link.

Auth: Public in current controller.

Response:

- `200 OK`

### Home Category Endpoints

#### `GET /api/home-categories`

Returns home page categories.

Auth: Public

Response item:

```json
{
  "id": 1,
  "categoryId": 5,
  "order": 1,
  "logoUrl": "https://...",
  "name": "Cars"
}
```

#### `POST /api/home-categories`

Creates home category.

Auth: Required, `HomeCategoriesAdd`, minimum `Admin`.

Request body:

```json
{
  "categoryId": 5,
  "order": 1
}
```

Response:

- `200 OK` with `HomeCategoriesEntity`.

#### `PUT /api/home-categories/reorder`

Reorders home categories.

Auth: Required, `HomeCategoriesReorder`, minimum `Admin`.

Request body:

```json
{
  "items": [
    {
      "id": 1,
      "categoryId": 5,
      "order": 1
    },
    {
      "id": 2,
      "categoryId": 8,
      "order": 2
    }
  ]
}
```

Response:

- `204 No Content`

#### `PUT /api/home-categories/{id}`

Updates home category.

Auth: Required, `HomeCategoriesEdit`, minimum `Admin`.

Request body:

```json
{
  "categoryId": 5,
  "order": 1
}
```

Response:

- `200 OK` with `HomeCategoryDto`.

#### `DELETE /api/home-categories/{id}`

Deletes home category.

Auth: Required, `HomeCategoriesDelete`, minimum `Admin`.

Response:

- `204 No Content`

### City Endpoints

#### `GET /api/cities`

Returns cities.

Auth: Public

Response item:

```json
{
  "id": 1,
  "name": "Tbilisi"
}
```

### Role Endpoints

Role endpoints exist in the current code, even though broader role/permission management is not complete.

All role endpoints require `SuperAdmin` access level. They use `[HasPermission(default, AccessLevelType.SuperAdmin)]`, so they check access level only, not a specific permission id.

#### `GET /api/roles`

Returns paginated roles.

Auth: Required, minimum `SuperAdmin`.

Query parameters:

| Parameter | Type | Default |
| --- | --- | --- |
| `page` | `int` | 1 |
| `pageSize` | `int` | 20 |

Response item:

```json
{
  "id": 1,
  "name": "Content Manager",
  "permissionIds": [2, 3, 42]
}
```

#### `POST /api/roles`

Creates role and links permissions by id.

Auth: Required, minimum `SuperAdmin`.

Request body:

```json
{
  "name": "Content Manager",
  "permissionIds": [2, 3, 42]
}
```

Response:

- `200 OK` with `RoleDto`.

Validation:

- `Name`: required, max 100.
- `PermissionIds`: validator exists; exact rule depends on current validator body.

#### `PUT /api/roles/{id}`

Updates role name and replaces linked permissions.

Auth: Required, minimum `SuperAdmin`.

Request body:

```json
{
  "name": "Content Manager",
  "permissionIds": [2, 3, 42]
}
```

Response:

- `200 OK` with `RoleDto`.

#### `DELETE /api/roles/{id}`

Deletes role.

Auth: Required, minimum `SuperAdmin`.

Response:

- `204 No Content`

### Permission Management Status

Current implemented permission support:

- Permission enum and seeded `Permissions` table.
- Role entity with many-to-many `RolePermissions`.
- User entity with many-to-many `UserRoles`.
- JWT token includes permission claims from user roles.
- `HasPermission` attribute enforces permissions.

Not currently exposed as complete REST endpoints:

- Listing all permissions.
- Assigning roles to users.
- Removing roles from users.
- Direct link/unlink permission endpoint.

There is an application command named `LinkPermissionCommand`, but it currently throws `NotImplementedException` and has no controller endpoint.

## DTO Reference

### `AuthDto`

| Property | Type |
| --- | --- |
| `AccessToken` | `string` |
| `RefreshToken` | `string` |
| `ExpiresAt` | `DateTime` |
| `User` | `UserDto` |

### `UserDto`

| Property | Type |
| --- | --- |
| `Id` | `int` |
| `Name` | `string` |
| `Lastname` | `string` |
| `Email` | `string` |
| `EmailVerified` | `bool` |
| `FavoritesCount` | `int` |
| `Number` | `string` |
| `GenderType` | `GenderType` |
| `BirthYear` | `int` |

### `UserInfoDto`

| Property | Type |
| --- | --- |
| `Id` | `int` |
| `FirstName` | `string` |
| `Lastname` | `string` |
| `Email` | `string` |
| `GenderType` | `GenderType` |
| `BirthYear` | `int` |
| `PhoneNumber` | `string` |
| `EmailVerified` | `bool` |
| `PostsCount` | `int` |

### `CategoryDto`

| Property | Type |
| --- | --- |
| `Id` | `int` |
| `ParentId` | `int?` |
| `Name` | `string` |
| `NameEn` | `string?` |
| `NameRu` | `string?` |
| `HasChildren` | `bool` |
| `BrandRequired` | `bool` |
| `BrandVisible` | `bool` |
| `CategoryPostType` | `CategoryPostType` |
| `LogoUrl` | `string?` |

### `CategoryFlatDto`

| Property | Type |
| --- | --- |
| `Id` | `int` |
| `ParentId` | `int?` |
| `Name` | `string` |
| `CategoryPostType` | `CategoryPostType` |
| `BrandRequired` | `bool?` |
| `LogoUrl` | `string?` |

### `BrandDto`

| Property | Type |
| --- | --- |
| `Id` | `int` |
| `Name` | `string` |
| `LogoId` | `int` |
| `LogoUrl` | `string` |

### `AttributeDto`

| Property | Type |
| --- | --- |
| `Id` | `int` |
| `Code` | `string` |
| `Name` | `string` |
| `NameEn` | `string?` |
| `NameRu` | `string?` |
| `AttributeType` | `AttributeType` |
| `UnitId` | `int?` |

### `AttributeOptionDto`

| Property | Type |
| --- | --- |
| `Id` | `int` |
| `Order` | `int` |
| `Name` | `string` |
| `NameEn` | `string?` |
| `NameRu` | `string?` |

### `UnitDto`

| Property | Type |
| --- | --- |
| `Id` | `int` |
| `Name` | `string` |
| `NameEn` | `string?` |
| `NameRu` | `string?` |

### `CategoryAttributeDto`

| Property | Type |
| --- | --- |
| `Id` | `int` |
| `CategoryId` | `int` |
| `AttributeId` | `int` |
| `IsRequired` | `bool` |
| `Order` | `int` |

### `CategoryAttributeOptionsDto`

| Property | Type |
| --- | --- |
| `Id` | `int` |
| `CategoryId` | `int` |
| `AttributeId` | `int` |
| `AttributeName` | `string` |
| `AttributeType` | `AttributeType` |
| `UnitName` | `string?` |
| `IsRequired` | `bool` |
| `Order` | `int` |
| `Options` | `List<AttributeOptionDto>?` |

### `CategoryBrandDto`

| Property | Type |
| --- | --- |
| `Id` | `int` |
| `CategoryId` | `int` |
| `BrandId` | `int` |
| `Name` | `string` |

### `HomeCategoryDto`

| Property | Type |
| --- | --- |
| `Id` | `int` |
| `CategoryId` | `int` |
| `Order` | `int` |
| `LogoUrl` | `string?` |
| `Name` | `string?` |

### `RoleDto`

| Property | Type |
| --- | --- |
| `Id` | `int` |
| `Name` | `string` |
| `PermissionIds` | `List<int>` |

### `PostDto`

| Property | Type |
| --- | --- |
| `Id` | `int` |
| `AutoRenewal` | `bool` |
| `CanOfferPrice` | `bool` |
| `CategoryId` | `int` |
| `ConditionType` | `ConditionType` |
| `CurrencyType` | `CurrencyType` |
| `Description` | `string?` |
| `ForDisabledPerson` | `bool` |
| `IsColored` | `bool` |
| `IsNegotiable` | `bool` |
| `Name` | `string` |
| `PhoneNumber` | `string` |
| `PostType` | `PostType` |
| `Price` | `double?` |
| `PromoType` | `PromoType?` |
| `SalePercentage` | `byte` |
| `PriceAfterDiscount` | `double?` |
| `Title` | `string` |
| `BrandId` | `int?` |
| `Images` | `List<string>` |
| `IsFavorite` | `bool` |

### `PostDetailsDto`

Includes all `PostDto`-style detail fields plus:

| Property | Type |
| --- | --- |
| `City` | `string?` |
| `ViewsCount` | `int` |
| `CreatedAt` | `DateTime` |
| `ChatExists` | `bool` |
| `Breadcrumb` | `List<CategoryBreadcrumbDto>` |
| `Attributes` | `List<PostAttributeDto>` |
| `User` | `UserInfoDto` |

### `PostMyDto`

| Property | Type |
| --- | --- |
| `Result` | `PaginatedResult<PostMyItemDto>` |
| `ActiveCount` | `int` |
| `InactiveCount` | `int` |
| `BlockedCount` | `int` |
| `TotalViews` | `int` |

### `PostLiteItemListDto`

| Property | Type |
| --- | --- |
| `SuperVip` | `List<PostLiteItemDto>?` |
| `VipPlus` | `List<PostLiteItemDto>?` |
| `Vip` | `List<PostLiteItemDto>?` |

## Validation Summary

Validation is handled by FluentValidation pipeline behavior. Validation failures become HTTP 400 problem details.

Important validation rules:

- User names: required, max 72, letters only.
- Email: required, valid, max 256, unique for registration/update.
- Phone: required, unique for registration/update.
- Password: 8-64 chars, no spaces, uppercase, number, special character.
- Categories: names max 255; `Name`, `NameEn`, `NameRu`, `BrandRequired`, and `CategoryPostType` are validated.
- Brands: `Name` required max 255; create requires logo.
- Units: `Name`, `NameEn`, `NameRu` required max 255.
- Attributes: `Name`, `Code`, and `AttributeType` required; max 255; `Code` unique in DB.
- Attribute options: `AttributeId`, `Order`, `Name` required; names max 255.
- Category attributes: `CategoryId`, `AttributeId`, `Order` required.
- Category brands: `CategoryId`, `BrandId` required; DB enforces unique pair.
- Posts: `CategoryId`, `Title`, `Name`, `PhoneNumber`, `CityId`, `Price`, `PostType`, `CurrencyType` and conditional promo/brand/renewal fields are validated.
- Roles: `Name` required max 100.

## Database Notes

The EF Core context exposes these sets:

- `Users`
- `VerificationCode`
- `Categories`
- `CategoryAttributes`
- `CategoryBrands`
- `Brands`
- `Cities`
- `Posts`
- `PostAttributes`
- `PostsImages`
- `PostViews`
- `Images`
- `Attributes`
- `AttributeUnits`
- `AttributeOptions`
- `HomeCategories`
- `Favorites`
- `Roles`
- `Permissions`

Chat tables are present in the codebase but intentionally omitted from this documentation.

Auditing:

- Entities derived from `AuditableEntity` get `CreatedAt` on insert.
- `UpdatedAt` is set on modification.

Caching:

- EF second-level cache uses memory cache with key prefix `EF_`.

## Endpoint Authorization Matrix

| Method | Route | Auth | Permission / Access |
| --- | --- | --- | --- |
| POST | `/api/auth/register-user` | Public | None |
| POST | `/api/auth/send-email-verification-code` | Public | None |
| POST | `/api/auth/verify-email-code` | Public | None |
| POST | `/api/auth/login-user` | Public | None |
| POST | `/api/auth/send-password-recovery` | Public | None |
| POST | `/api/auth/verify-password-code` | Public | None |
| POST | `/api/auth/password-recovery` | Public | None |
| POST | `/api/auth/refresh-user` | Public | None |
| GET | `/api/auth/user-exists` | Public | None |
| GET | `/api/users/{id}` | Public | None |
| GET | `/api/users/{id}/phone-number` | Public | None |
| GET | `/api/users/current` | Required | Authenticated user |
| PUT | `/api/users` | Required | Authenticated user |
| POST | `/api/posts` | Required | Authenticated user |
| GET | `/api/posts` | Public | None |
| GET | `/api/posts/{id}` | Public | None |
| GET | `/api/posts/{id}/view` | Public | None |
| GET | `/api/posts/lite` | Public | None |
| GET | `/api/posts/my` | Required | Authenticated user |
| GET | `/api/posts/favorite` | Required | Authenticated user |
| POST | `/api/posts/{id}/favorite` | Required | Authenticated user |
| DELETE | `/api/posts/{id}/favorite` | Required | Authenticated user |
| DELETE | `/api/posts/{id}` | Required | Authenticated user |
| POST | `/api/posts/{id}/disable` | Required | Authenticated user |
| POST | `/api/posts/{id}/enable` | Required | Authenticated user |
| GET | `/api/categories` | Public | None |
| GET | `/api/categories/get-localized` | Public | None |
| GET | `/api/categories/{id}` | Public | None |
| GET | `/api/categories/{id}/attributes` | Public | None |
| GET | `/api/categories/{id}/brands` | Public | None |
| POST | `/api/categories` | Required | `CategoriesAdd`, Admin |
| PUT | `/api/categories/{id}` | Required | `CategoriesEdit`, Admin |
| DELETE | `/api/categories/{id}` | Required | `CategoriesDelete`, Admin |
| GET | `/api/brands` | Public | None |
| GET | `/api/brands/{id}` | Public | None |
| POST | `/api/brands` | Required | `BrandsAdd`, Admin |
| PUT | `/api/brands/{id}` | Required | `BrandsEdit`, Admin |
| DELETE | `/api/brands/{id}` | Required | `BrandsDelete`, Admin |
| GET | `/api/attributes` | Required | Authenticated user |
| GET | `/api/attributes/{id}` | Required | Authenticated user |
| GET | `/api/attributes/{id}/options` | Required | Authenticated user |
| POST | `/api/attributes` | Required | `AttributeAdd`, Admin |
| PUT | `/api/attributes/{id}` | Required | `AttributeEdit`, Admin |
| DELETE | `/api/attributes/{id}` | Required | `AttributeDelete`, Admin |
| POST | `/api/attribute-options` | Required | `AttributeOptionsAdd`, Admin |
| PUT | `/api/attribute-options/{id}` | Required | Currently `AttributeOptionsDelete` |
| DELETE | `/api/attribute-options/{id}` | Required | Currently `AttributeOptionsEdit` |
| GET | `/api/units` | Required | Authenticated user |
| GET | `/api/units/{id}` | Required | Authenticated user |
| POST | `/api/units` | Required | `UnitsAdd`, Admin |
| PUT | `/api/units/{id}` | Required | `UnitsEdit`, Admin |
| DELETE | `/api/units/{id}` | Required | `UnitsDelete`, Admin |
| GET | `/api/category-attributes` | Public | None |
| POST | `/api/category-attributes` | Public | None |
| PUT | `/api/category-attributes/{id}` | Public | None |
| DELETE | `/api/category-attributes/{id}` | Public | None |
| GET | `/api/category-brands` | Public | None |
| GET | `/api/category-brands/{id}` | Public | None |
| POST | `/api/category-brands/link` | Public | None |
| POST | `/api/category-brands/unlink` | Public | None |
| POST | `/api/category-brands/add-multiple` | Public | None |
| PUT | `/api/category-brands/{id}` | Public | None |
| DELETE | `/api/category-brands/{id}` | Public | None |
| GET | `/api/home-categories` | Public | None |
| POST | `/api/home-categories` | Required | `HomeCategoriesAdd`, Admin |
| PUT | `/api/home-categories/reorder` | Required | `HomeCategoriesReorder`, Admin |
| PUT | `/api/home-categories/{id}` | Required | `HomeCategoriesEdit`, Admin |
| DELETE | `/api/home-categories/{id}` | Required | `HomeCategoriesDelete`, Admin |
| GET | `/api/cities` | Public | None |
| GET | `/api/roles` | Required | SuperAdmin |
| POST | `/api/roles` | Required | SuperAdmin |
| PUT | `/api/roles/{id}` | Required | SuperAdmin |
| DELETE | `/api/roles/{id}` | Required | SuperAdmin |

## Known Implementation Notes

- Chat and SignalR routes exist but are intentionally not documented here.
- Role and permission support is partially implemented. Roles can be created/edited/deleted; permissions are seeded and checked; user-role assignment endpoints are not present.
- `LinkPermissionCommand` exists but is not implemented and has no controller route.
- `AttributesController.DELETE /api/attributes/{id}` currently sends `DeleteUnitCommand`; this appears inconsistent with the endpoint name.
- `AttributesOptionsController` has edit/delete permission attributes swapped in the current code.
- Some management routes, such as category-brand and category-attribute mutations, are public in the current controller code.
- `POST /api/units` returns `204 No Content` even though its command returns `UnitDto`.
- `UserExistsQuery` fields should be checked in the source before client integration because only the controller binding was inspected here.

