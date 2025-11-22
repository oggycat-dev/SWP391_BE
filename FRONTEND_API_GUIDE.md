# 📘 Frontend API Integration Guide

## 🌐 Base Configuration

### API Base URL
```
Development: http://localhost:5001
Production: https://your-domain.com
```

### Headers Required
```javascript
{
  "Content-Type": "application/json",
  "Authorization": "Bearer {your_jwt_token}"
}
```

---

## 🔐 Authentication

### 1. CMS Login (Admin/EVM Staff)
```http
POST /api/auth/cms/login
```

**Request:**
```json
{
  "email": "admin@evm.com",
  "password": "Admin@123"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": "guid",
      "email": "admin@evm.com",
      "firstName": "Admin",
      "lastName": "User",
      "role": "Admin"
    },
    "expiresAt": "2025-11-23T10:00:00Z"
  }
}
```

### 2. Dealer Login
```http
POST /api/auth/dealer/login
```

**Request:**
```json
{
  "email": "dealer@example.com",
  "password": "Password@123"
}
```

**Response:** Same structure as CMS login

### 3. Customer Login
```http
POST /api/auth/customer/login
```

**Request:**
```json
{
  "email": "customer@example.com",
  "password": "Password@123"
}
```

**Response:** Same structure as CMS login

---

## 📊 Reports APIs (CMS Portal)

**Authorization Required:** `Admin | EVMManager | EVMStaff | DealerManager`

### 1. Sales by Dealer Report
```http
GET /api/reports/sales-by-dealer?periodStart={date}&periodEnd={date}&dealerId={guid}
```

**Query Parameters:**
- `periodStart` (optional): ISO date format (default: 1 month ago)
- `periodEnd` (optional): ISO date format (default: now)
- `dealerId` (optional): Filter by specific dealer

**Response:**
```json
{
  "success": true,
  "message": "Sales by dealer report retrieved successfully",
  "data": [
    {
      "dealerId": "guid",
      "dealerName": "Dealer ABC",
      "totalOrders": 25,
      "completedOrders": 20,
      "totalRevenue": 5000000000,
      "averageOrderValue": 250000000,
      "periodStart": "2025-01-01T00:00:00Z",
      "periodEnd": "2025-11-22T00:00:00Z"
    }
  ]
}
```

### 2. Sales by Staff Report
```http
GET /api/reports/sales-by-staff?periodStart={date}&periodEnd={date}&dealerId={guid}&staffId={guid}
```

**Query Parameters:**
- `periodStart` (optional)
- `periodEnd` (optional)
- `dealerId` (optional): Filter by dealer
- `staffId` (optional): Filter by specific staff

**Response:**
```json
{
  "success": true,
  "message": "Sales by staff report retrieved successfully",
  "data": [
    {
      "staffId": "guid",
      "staffName": "John Doe",
      "dealerId": "guid",
      "dealerName": "Dealer ABC",
      "totalOrders": 10,
      "completedOrders": 8,
      "totalRevenue": 2000000000,
      "commission": 20000000,
      "periodStart": "2025-01-01T00:00:00Z",
      "periodEnd": "2025-11-22T00:00:00Z"
    }
  ]
}
```

### 3. Inventory Turnover Report
```http
GET /api/reports/inventory-turnover?vehicleVariantId={guid}
```

**Authorization:** `Admin | EVMManager | EVMStaff`

**Query Parameters:**
- `vehicleVariantId` (optional): Filter by vehicle variant

**Response:**
```json
{
  "success": true,
  "message": "Inventory turnover report retrieved successfully",
  "data": [
    {
      "vehicleVariantId": "guid",
      "vehicleVariantName": "City RS",
      "vehicleModelName": "Honda City",
      "totalAvailable": 15,
      "totalReserved": 5,
      "totalSold": 30,
      "totalInTransit": 10,
      "daysInInventory": 45,
      "turnoverRate": 25.5
    }
  ]
}
```

### 4. Dealer Debt Report
```http
GET /api/reports/dealer-debt?dealerId={guid}
```

**Authorization:** `Admin | EVMManager`

**Query Parameters:**
- `dealerId` (optional): Filter by specific dealer

**Response:**
```json
{
  "success": true,
  "message": "Dealer debt report retrieved successfully",
  "data": [
    {
      "dealerId": "guid",
      "dealerName": "Dealer ABC",
      "totalDebt": 500000000,
      "creditLimit": 1000000000,
      "availableCredit": 500000000,
      "overdueDebts": 2,
      "overdueAmount": 50000000,
      "nextPaymentDate": "2025-12-01T00:00:00Z"
    }
  ]
}
```

### 5. Customer Debt Report
```http
GET /api/reports/customer-debt?customerId={guid}&dealerId={guid}
```

**Query Parameters:**
- `customerId` (optional): Filter by customer
- `dealerId` (optional): Filter by dealer

**Response:**
```json
{
  "success": true,
  "message": "Customer debt report retrieved successfully",
  "data": [
    {
      "customerId": "guid",
      "customerName": "Nguyen Van A",
      "orderId": "guid",
      "orderNumber": "ORD-20251122-00001",
      "totalAmount": 500000000,
      "paidAmount": 100000000,
      "remainingAmount": 400000000,
      "nextPaymentDate": "2025-12-15T00:00:00Z",
      "isOverdue": false
    }
  ]
}
```

---

## 🚗 Vehicle Management APIs

### 1. Compare Vehicles
```http
GET /api/cms/vehicles/variants/compare?variantIds={guid}&variantIds={guid}&variantIds={guid}
```

**Authorization:** Public (No auth required)

**Query Parameters:**
- `variantIds`: Array of variant IDs to compare (can pass multiple)

**Response:**
```json
{
  "success": true,
  "message": "Vehicle comparison retrieved successfully",
  "data": [
    {
      "id": "guid",
      "variantName": "City RS",
      "modelName": "Honda City",
      "price": 559000000,
      "specifications": {
        "engine": "1.5L VTEC Turbo",
        "power": "122 HP",
        "fuelConsumption": "5.5L/100km"
      },
      "description": "Sedan cao cấp"
    },
    {
      "id": "guid",
      "variantName": "Civic RS",
      "modelName": "Honda Civic",
      "price": 789000000,
      "specifications": {
        "engine": "1.5L VTEC Turbo",
        "power": "178 HP",
        "fuelConsumption": "6.0L/100km"
      },
      "description": "Sedan thể thao"
    }
  ]
}
```

### 2. Get Central Inventory (EVM Warehouse)
```http
GET /api/cms/vehicles/inventories/central?vehicleVariantId={guid}&status={status}
```

**Authorization:** `Admin | EVMManager | EVMStaff`

**Query Parameters:**
- `vehicleVariantId` (optional): Filter by variant
- `status` (optional): Available, Reserved, Sold, InTransit

**Response:**
```json
{
  "success": true,
  "message": "Central inventory retrieved successfully",
  "data": [
    {
      "id": "guid",
      "vin": "VIN123456789",
      "variantId": "guid",
      "variantName": "City RS",
      "modelName": "Honda City",
      "colorId": "guid",
      "colorName": "White Pearl",
      "status": "Available",
      "warehouseLocation": "Central",
      "dealerName": null
    }
  ]
}
```

---

## 👤 Customer Management APIs (Dealer Portal)

**Authorization Required:** `DealerManager | DealerStaff`

### 1. Get Customers
```http
GET /api/dealer/customers?pageNumber={int}&pageSize={int}&searchTerm={string}
```

**Query Parameters:**
- `pageNumber` (default: 1)
- `pageSize` (default: 10)
- `searchTerm` (optional): Search by name, email, phone

**Response:**
```json
{
  "success": true,
  "message": "Customers retrieved successfully",
  "data": {
    "items": [
      {
        "id": "guid",
        "fullName": "Nguyen Van A",
        "email": "nguyenvana@example.com",
        "phoneNumber": "0901234567",
        "address": "123 ABC Street",
        "dateOfBirth": "1990-01-01",
        "identityCard": "001234567890",
        "createdAt": "2025-01-01T00:00:00Z"
      }
    ],
    "pageNumber": 1,
    "pageSize": 10,
    "totalCount": 50,
    "totalPages": 5,
    "hasPreviousPage": false,
    "hasNextPage": true
  }
}
```

### 2. Get Customer by ID
```http
GET /api/dealer/customers/{id}
```

**Response:**
```json
{
  "success": true,
  "message": "Customer retrieved successfully",
  "data": {
    "id": "guid",
    "fullName": "Nguyen Van A",
    "email": "nguyenvana@example.com",
    "phoneNumber": "0901234567",
    "address": "123 ABC Street",
    "dateOfBirth": "1990-01-01",
    "identityCard": "001234567890",
    "createdAt": "2025-01-01T00:00:00Z"
  }
}
```

### 3. Get Customer History
```http
GET /api/dealer/customers/{id}/history
```

**Response:**
```json
{
  "success": true,
  "message": "Customer history retrieved successfully",
  "data": {
    "customerId": "guid",
    "customerName": "Nguyen Van A",
    "email": "nguyenvana@example.com",
    "phoneNumber": "0901234567",
    "createdAt": "2025-01-01T00:00:00Z",
    "totalSpent": 500000000,
    "totalOrders": 2,
    "completedOrders": 1,
    "orders": [
      {
        "id": "guid",
        "orderNumber": "ORD-20251122-00001",
        "orderDate": "2025-11-22T00:00:00Z",
        "vehicleName": "Honda City RS",
        "status": "Completed",
        "totalAmount": 500000000,
        "paidAmount": 500000000
      }
    ],
    "quotations": [
      {
        "id": "guid",
        "quotationNumber": "QUO-20251120-00001",
        "quotationDate": "2025-11-20T00:00:00Z",
        "vehicleName": "Honda Civic RS",
        "status": "Pending",
        "totalAmount": 789000000
      }
    ],
    "testDrives": [
      {
        "id": "guid",
        "scheduledDate": "2025-11-25T10:00:00Z",
        "vehicleName": "Honda CR-V",
        "status": "Scheduled",
        "feedback": null
      }
    ]
  }
}
```

### 4. Create Customer
```http
POST /api/dealer/customers
```

**Request:**
```json
{
  "fullName": "Nguyen Van A",
  "email": "nguyenvana@example.com",
  "phoneNumber": "0901234567",
  "address": "123 ABC Street",
  "dateOfBirth": "1990-01-01",
  "identityCard": "001234567890"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Customer created successfully",
  "data": {
    "id": "guid",
    "fullName": "Nguyen Van A",
    "email": "nguyenvana@example.com",
    "phoneNumber": "0901234567",
    "address": "123 ABC Street",
    "dateOfBirth": "1990-01-01",
    "identityCard": "001234567890",
    "createdAt": "2025-11-22T10:00:00Z"
  }
}
```

### 5. Update Customer
```http
PUT /api/dealer/customers/{id}
```

**Request:** Same as Create Customer

**Response:** Same as Create Customer

---

## 📋 Quotation Management APIs (Dealer Portal)

**Authorization Required:** `DealerManager | DealerStaff`

### 1. Get Quotations
```http
GET /api/dealer/quotations?pageNumber={int}&pageSize={int}&status={string}&customerId={guid}
```

**Query Parameters:**
- `pageNumber` (default: 1)
- `pageSize` (default: 10)
- `status` (optional): Pending, Approved, Rejected, Expired
- `customerId` (optional): Filter by customer

**Response:**
```json
{
  "success": true,
  "message": "Quotations retrieved successfully",
  "data": {
    "items": [
      {
        "id": "guid",
        "quotationNumber": "QUO-20251122-00001",
        "customerId": "guid",
        "customerName": "Nguyen Van A",
        "dealerId": "guid",
        "dealerName": "Dealer ABC",
        "vehicleVariantId": "guid",
        "vehicleVariantName": "City RS",
        "vehicleColorId": "guid",
        "vehicleColorName": "White Pearl",
        "basePrice": 559000000,
        "discount": 10000000,
        "tax": 55900000,
        "totalAmount": 604900000,
        "validUntil": "2025-12-22T00:00:00Z",
        "status": "Pending",
        "quotationDate": "2025-11-22T00:00:00Z"
      }
    ],
    "pageNumber": 1,
    "pageSize": 10,
    "totalCount": 25,
    "totalPages": 3,
    "hasPreviousPage": false,
    "hasNextPage": true
  }
}
```

### 2. Create Quotation
```http
POST /api/dealer/quotations
```

**Request:**
```json
{
  "customerId": "guid",
  "vehicleVariantId": "guid",
  "vehicleColorId": "guid",
  "basePrice": 559000000,
  "discount": 10000000,
  "additionalFees": 5000000,
  "validityDays": 30,
  "notes": "Special promotion for loyal customer"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Quotation created successfully",
  "data": {
    "id": "guid",
    "quotationNumber": "QUO-20251122-00001",
    "customerId": "guid",
    "customerName": "Nguyen Van A",
    "dealerId": "guid",
    "dealerName": "Dealer ABC",
    "vehicleVariantId": "guid",
    "vehicleVariantName": "City RS",
    "vehicleColorId": "guid",
    "vehicleColorName": "White Pearl",
    "basePrice": 559000000,
    "discount": 10000000,
    "additionalFees": 5000000,
    "tax": 55400000,
    "totalAmount": 609400000,
    "validUntil": "2025-12-22T00:00:00Z",
    "status": "Pending",
    "quotationDate": "2025-11-22T00:00:00Z",
    "notes": "Special promotion for loyal customer"
  }
}
```

---

## 🛒 Order Management APIs (Dealer Portal)

**Authorization Required:** `DealerManager | DealerStaff`

### 1. Get Orders
```http
GET /api/dealer/orders?pageNumber={int}&pageSize={int}&status={string}&customerId={guid}
```

**Query Parameters:**
- `pageNumber` (default: 1)
- `pageSize` (default: 10)
- `status` (optional): Pending, Approved, Confirmed, Completed, Cancelled
- `customerId` (optional): Filter by customer

**Response:**
```json
{
  "success": true,
  "message": "Orders retrieved successfully",
  "data": {
    "items": [
      {
        "id": "guid",
        "orderNumber": "ORD-20251122-00001",
        "customerId": "guid",
        "customerName": "Nguyen Van A",
        "dealerId": "guid",
        "dealerName": "Dealer ABC",
        "vehicleVariantId": "guid",
        "vehicleVariantName": "City RS",
        "vehicleModelName": "Honda City",
        "vehicleColorId": "guid",
        "vehicleColorName": "White Pearl",
        "basePrice": 559000000,
        "discount": 10000000,
        "tax": 55900000,
        "totalAmount": 604900000,
        "paidAmount": 100000000,
        "remainingAmount": 504900000,
        "paymentStatus": "Partial",
        "isInstallment": true,
        "status": "Approved",
        "orderDate": "2025-11-22T00:00:00Z"
      }
    ],
    "pageNumber": 1,
    "pageSize": 10,
    "totalCount": 50,
    "totalPages": 5
  }
}
```

### 2. Create Order
```http
POST /api/dealer/orders
```

**Request:**
```json
{
  "customerId": "guid",
  "vehicleVariantId": "guid",
  "vehicleColorId": "guid",
  "quotationId": "guid",
  "basePrice": 559000000,
  "discount": 10000000,
  "additionalFees": 5000000,
  "isInstallment": true,
  "downPayment": 100000000,
  "notes": "Customer prefers delivery next month"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Order created successfully",
  "data": {
    "id": "guid",
    "orderNumber": "ORD-20251122-00001",
    "customerId": "guid",
    "customerName": "Nguyen Van A",
    "totalAmount": 609400000,
    "paidAmount": 100000000,
    "remainingAmount": 509400000,
    "status": "Pending",
    "orderDate": "2025-11-22T00:00:00Z"
  }
}
```

### 3. Update Order Status
```http
PUT /api/dealer/orders/{id}/status
```

**Request:**
```json
{
  "status": "Approved",
  "notes": "All documents verified"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Order status updated successfully",
  "data": {
    "id": "guid",
    "orderNumber": "ORD-20251122-00001",
    "status": "Approved",
    "orderDate": "2025-11-22T00:00:00Z"
  }
}
```

---

## 💳 Payment Management APIs (Dealer Portal)

**Authorization Required:** `DealerManager | DealerStaff`

### 1. Get Payments by Order
```http
GET /api/dealer/payments/order/{orderId}
```

**Response:**
```json
{
  "success": true,
  "message": "Payments retrieved successfully",
  "data": {
    "orderId": "guid",
    "orderNumber": "ORD-20251122-00001",
    "totalAmount": 604900000,
    "paidAmount": 100000000,
    "remainingAmount": 504900000,
    "paymentStatus": "Partial",
    "isInstallment": true,
    "payments": [
      {
        "id": "guid",
        "orderId": "guid",
        "orderNumber": "ORD-20251122-00001",
        "amount": 100000000,
        "paymentMethod": "BankTransfer",
        "transactionId": "TXN123456",
        "paymentDate": "2025-11-22T10:00:00Z",
        "notes": "Down payment"
      }
    ],
    "installmentPlan": {
      "id": "guid",
      "orderId": "guid",
      "totalAmount": 504900000,
      "numberOfInstallments": 12,
      "monthlyAmount": 42075000,
      "interestRate": 0.0,
      "startDate": "2025-12-01T00:00:00Z",
      "endDate": "2026-11-01T00:00:00Z",
      "nextPaymentDate": "2025-12-01T00:00:00Z",
      "paidInstallments": 0,
      "remainingInstallments": 12,
      "status": "Active"
    }
  }
}
```

### 2. Create Payment
```http
POST /api/dealer/payments
```

**Request:**
```json
{
  "orderId": "guid",
  "amount": 100000000,
  "paymentMethod": "BankTransfer",
  "transactionId": "TXN123456",
  "notes": "Down payment"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Payment created successfully",
  "data": {
    "id": "guid",
    "orderId": "guid",
    "orderNumber": "ORD-20251122-00001",
    "amount": 100000000,
    "paymentMethod": "BankTransfer",
    "transactionId": "TXN123456",
    "paymentDate": "2025-11-22T10:00:00Z",
    "notes": "Down payment"
  }
}
```

### 3. Create Installment Plan
```http
POST /api/dealer/payments/installment
```

**Request:**
```json
{
  "orderId": "guid",
  "numberOfInstallments": 12,
  "interestRate": 0.0,
  "startDate": "2025-12-01"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Installment plan created successfully",
  "data": {
    "id": "guid",
    "orderId": "guid",
    "totalAmount": 504900000,
    "numberOfInstallments": 12,
    "monthlyAmount": 42075000,
    "interestRate": 0.0,
    "startDate": "2025-12-01T00:00:00Z",
    "endDate": "2026-11-01T00:00:00Z",
    "status": "Active"
  }
}
```

---

## 🚙 Test Drive Management APIs (Dealer Portal)

**Authorization Required:** `DealerManager | DealerStaff`

### 1. Get Test Drives
```http
GET /api/dealer/test-drives?pageNumber={int}&pageSize={int}&customerId={guid}&dealerId={guid}&status={string}
```

**Query Parameters:**
- `pageNumber` (default: 1)
- `pageSize` (default: 10)
- `customerId` (optional): Filter by customer
- `dealerId` (optional): Filter by dealer
- `status` (optional): Scheduled, Completed, Cancelled, NoShow

**Response:**
```json
{
  "success": true,
  "message": "Test drives retrieved successfully",
  "data": {
    "items": [
      {
        "id": "guid",
        "customerId": "guid",
        "customerName": "Nguyen Van A",
        "dealerId": "guid",
        "dealerName": "Dealer ABC",
        "vehicleVariantId": "guid",
        "vehicleVariantName": "City RS",
        "vehicleModelName": "Honda City",
        "scheduledDate": "2025-11-25T10:00:00Z",
        "completedDate": null,
        "status": "Scheduled",
        "assignedStaffId": "guid",
        "assignedStaffName": "John Doe",
        "notes": "Customer interested in sport features",
        "feedback": null,
        "rating": null,
        "createdAt": "2025-11-22T10:00:00Z"
      }
    ],
    "pageNumber": 1,
    "pageSize": 10,
    "totalCount": 15,
    "totalPages": 2
  }
}
```

### 2. Create Test Drive
```http
POST /api/dealer/test-drives
```

**Request:**
```json
{
  "customerId": "guid",
  "vehicleVariantId": "guid",
  "dealerId": "guid",
  "scheduledDate": "2025-11-25T10:00:00Z",
  "notes": "Customer wants to test highway performance"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Test drive scheduled successfully",
  "data": {
    "id": "guid",
    "customerId": "guid",
    "customerName": "Nguyen Van A",
    "vehicleVariantId": "guid",
    "vehicleVariantName": "City RS",
    "dealerId": "guid",
    "dealerName": "Dealer ABC",
    "scheduledDate": "2025-11-25T10:00:00Z",
    "status": "Scheduled",
    "notes": "Customer wants to test highway performance"
  }
}
```

### 3. Update Test Drive Status
```http
PUT /api/dealer/test-drives/{id}/status
```

**Request:**
```json
{
  "status": "Completed",
  "feedback": "Customer loved the car! Very interested in purchasing.",
  "rating": 5
}
```

**Response:**
```json
{
  "success": true,
  "message": "Test drive status updated successfully",
  "data": {
    "id": "guid",
    "customerId": "guid",
    "customerName": "Nguyen Van A",
    "status": "Completed",
    "scheduledDate": "2025-11-25T10:00:00Z",
    "completedDate": "2025-11-25T11:30:00Z",
    "feedback": "Customer loved the car! Very interested in purchasing.",
    "rating": 5
  }
}
```

---

## 📄 Sales Contract APIs (Dealer Portal)

**Authorization Required:** `DealerManager`

### 1. Create Sales Contract
```http
POST /api/dealer/contracts
```

**Request:**
```json
{
  "orderId": "guid",
  "contractDate": "2025-11-22T00:00:00Z",
  "specialTerms": "Free maintenance for 1 year"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Sales contract created successfully",
  "data": {
    "id": "guid",
    "contractNumber": "SC-20251122-00001",
    "orderId": "guid",
    "orderNumber": "ORD-20251122-00001",
    "customerId": "guid",
    "customerName": "Nguyen Van A",
    "dealerId": "guid",
    "dealerName": "Dealer ABC",
    "contractDate": "2025-11-22T00:00:00Z",
    "status": "Active",
    "totalAmount": 604900000,
    "specialTerms": "Free maintenance for 1 year",
    "createdAt": "2025-11-22T10:00:00Z"
  }
}
```

**Business Rules:**
- ✅ Order must be in `Approved` status
- ✅ Each order can only have ONE contract
- ✅ Contract number is auto-generated

---

## 🚀 Vehicle Request APIs (Dealer Portal)

**Authorization Required:** `DealerManager`

### 1. Get Vehicle Requests
```http
GET /api/dealer/vehicle-requests?pageNumber={int}&pageSize={int}&status={string}
```

**Query Parameters:**
- `pageNumber` (default: 1)
- `pageSize` (default: 10)
- `status` (optional): Pending, Approved, Rejected, Fulfilled

**Response:**
```json
{
  "success": true,
  "message": "Vehicle requests retrieved successfully",
  "data": {
    "items": [
      {
        "id": "guid",
        "requestNumber": "VR-20251122-00001",
        "dealerId": "guid",
        "dealerName": "Dealer ABC",
        "vehicleVariantId": "guid",
        "vehicleVariantName": "City RS",
        "vehicleModelName": "Honda City",
        "vehicleColorId": "guid",
        "vehicleColorName": "White Pearl",
        "quantity": 5,
        "unitPrice": 559000000,
        "totalAmount": 2795000000,
        "expectedDeliveryDate": "2025-12-15T00:00:00Z",
        "status": "Pending",
        "requestDate": "2025-11-22T00:00:00Z"
      }
    ],
    "pageNumber": 1,
    "pageSize": 10,
    "totalCount": 20,
    "totalPages": 2
  }
}
```

### 2. Create Vehicle Request
```http
POST /api/dealer/vehicle-requests
```

**Request:**
```json
{
  "vehicleVariantId": "guid",
  "vehicleColorId": "guid",
  "quantity": 5,
  "expectedDeliveryDate": "2025-12-15"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Vehicle request created successfully",
  "data": {
    "id": "guid",
    "requestNumber": "VR-20251122-00001",
    "dealerId": "guid",
    "dealerName": "Dealer ABC",
    "vehicleVariantId": "guid",
    "vehicleVariantName": "City RS",
    "quantity": 5,
    "status": "Pending",
    "requestDate": "2025-11-22T00:00:00Z"
  }
}
```

---

## ⚠️ Error Responses

### Standard Error Format
```json
{
  "success": false,
  "message": "Error message description",
  "errors": {
    "fieldName": ["Validation error message"]
  },
  "statusCode": 400
}
```

### Common HTTP Status Codes
- `200 OK` - Success
- `201 Created` - Resource created successfully
- `400 Bad Request` - Validation error
- `401 Unauthorized` - Missing or invalid token
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error

### Example Error Responses

**400 Validation Error:**
```json
{
  "success": false,
  "message": "Validation failed",
  "errors": {
    "email": ["Email is required"],
    "password": ["Password must be at least 8 characters"]
  },
  "statusCode": 400
}
```

**401 Unauthorized:**
```json
{
  "success": false,
  "message": "Authentication failed. Invalid credentials.",
  "statusCode": 401
}
```

**403 Forbidden:**
```json
{
  "success": false,
  "message": "You do not have permission to access this resource",
  "statusCode": 403
}
```

**404 Not Found:**
```json
{
  "success": false,
  "message": "Entity \"Customer\" (guid) was not found.",
  "statusCode": 404
}
```

---

## 💡 Best Practices

### 1. Token Management
```javascript
// Store token securely
localStorage.setItem('access_token', response.data.token);

// Include in all requests
const token = localStorage.getItem('access_token');
axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;

// Handle token expiration
axios.interceptors.response.use(
  response => response,
  error => {
    if (error.response.status === 401) {
      // Redirect to login
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);
```

### 2. API Client Setup (Axios Example)
```javascript
import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'http://localhost:5001',
  headers: {
    'Content-Type': 'application/json'
  }
});

// Request interceptor
apiClient.interceptors.request.use(config => {
  const token = localStorage.getItem('access_token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Response interceptor
apiClient.interceptors.response.use(
  response => response.data,
  error => {
    if (error.response.status === 401) {
      localStorage.removeItem('access_token');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default apiClient;
```

### 3. Example Usage
```javascript
// Login
const login = async (email, password) => {
  try {
    const response = await apiClient.post('/api/auth/cms/login', {
      email,
      password
    });
    
    localStorage.setItem('access_token', response.data.token);
    return response;
  } catch (error) {
    console.error('Login failed:', error);
    throw error;
  }
};

// Get customers
const getCustomers = async (pageNumber = 1, pageSize = 10) => {
  try {
    const response = await apiClient.get('/api/dealer/customers', {
      params: { pageNumber, pageSize }
    });
    return response.data;
  } catch (error) {
    console.error('Failed to fetch customers:', error);
    throw error;
  }
};

// Create order
const createOrder = async (orderData) => {
  try {
    const response = await apiClient.post('/api/dealer/orders', orderData);
    return response.data;
  } catch (error) {
    console.error('Failed to create order:', error);
    throw error;
  }
};
```

### 4. Pagination Helper
```javascript
const usePagination = (fetchFunction) => {
  const [data, setData] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [loading, setLoading] = useState(false);

  const loadData = async (page = 1) => {
    setLoading(true);
    try {
      const response = await fetchFunction(page, 10);
      setData(response.items);
      setPageNumber(response.pageNumber);
      setTotalPages(response.totalPages);
    } catch (error) {
      console.error('Failed to load data:', error);
    } finally {
      setLoading(false);
    }
  };

  return { data, pageNumber, totalPages, loading, loadData };
};
```

---

## 🔒 Role-Based Access Control

### Role Permissions Summary

| Feature | Admin | EVMManager | EVMStaff | DealerManager | DealerStaff |
|---------|-------|------------|----------|---------------|-------------|
| Reports (All) | ✅ | ✅ | ✅ | ✅ | ❌ |
| Dealer Debt Report | ✅ | ✅ | ❌ | ❌ | ❌ |
| Central Inventory | ✅ | ✅ | ✅ | ❌ | ❌ |
| Customer Management | ✅ | ✅ | ✅ | ✅ | ✅ |
| Order Management | ✅ | ✅ | ✅ | ✅ | ✅ |
| Payment Management | ✅ | ✅ | ✅ | ✅ | ✅ |
| Test Drive Management | ✅ | ✅ | ✅ | ✅ | ✅ |
| Sales Contract | ✅ | ✅ | ✅ | ✅ | ❌ |
| Vehicle Request | ✅ | ✅ | ✅ | ✅ | ❌ |

---

## 📞 Support

For any API integration issues, please contact:
- **Backend Team:** backend@evm.com
- **Documentation:** Check `COMPLETE_FEATURES_SUMMARY.md`
- **Swagger UI:** http://localhost:5001/swagger/index.html

---

**Last Updated:** November 22, 2025  
**API Version:** 1.0  
**Status:** ✅ Production Ready

