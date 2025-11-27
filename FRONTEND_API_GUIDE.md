# 📘 Frontend API Integration Guide - Implementation Order

> **Hướng dẫn này được sắp xếp theo thứ tự implement để team frontend dễ follow**

---

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

## 📋 **IMPLEMENTATION ROADMAP**

```
Phase 0: Setup (Week 1)
Phase 1: Authentication (Week 1) 🔴
Phase 2: CMS - Vehicle Management (Week 2) 🔴
Phase 3: CMS - Dealer Management (Week 3) 🔴
Phase 4: CMS - Vehicle Requests (Week 3) 🟡
Phase 5: CMS - Promotions (Week 4) 🟡
Phase 6: Dealer - Basic Portal (Week 4) 🔴
Phase 7: Dealer - Customer Management (Week 5) 🔴
Phase 8: Dealer - Test Drives & Quotations (Week 5) 🟡
Phase 9: Dealer - Orders & Payments (Week 6-7) 🔴
Phase 10: Dealer - Deliveries (Week 7) 🔴
Phase 11: Dealer - Feedback (Week 8) 🟡
Phase 12: Reports (Week 8) 🟢
```

---

# 🔐 **PHASE 1: AUTHENTICATION** (Week 1)

> **Implement đầu tiên - Critical**

## 1. CMS Login (Admin/EVM Staff)
```http
POST /api/cms/auth/login
```

**Request:**
```json
{
  "email": "admin@evdealer.com",
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
      "email": "admin@evdealer.com",
      "firstName": "Admin",
      "lastName": "User",
      "role": "Admin"
    },
    "expiresAt": "2025-11-23T10:00:00Z"
  }
}
```

## 2. Dealer Login
```http
POST /api/dealer/auth/login
```

**Request:**
```json
{
  "email": "dealer@example.com",
  "password": "Password@123"
}
```

**Response:** Same structure as CMS login

## 3. Customer Login
```http
POST /api/customer/auth/login
```

**Frontend Tasks:**
- [ ] Setup axios with interceptors
- [ ] Create login pages (CMS, Dealer, Customer)
- [ ] Implement token storage (localStorage)
- [ ] Create route guards/protected routes
- [ ] Implement auto-redirect on 401

---

# 🚗 **PHASE 2: CMS - VEHICLE MANAGEMENT** (Week 2)

> **Implement sau Authentication**

## 1. Vehicle Models

### Get All Models
```http
GET /api/cms/vehicles/models?pageNumber={int}&pageSize={int}
```

### Create Model
```http
POST /api/cms/vehicles/models
```
**Request:**
```json
{
  "modelCode": "MODEL-Y",
  "modelName": "Tesla Model Y",
  "brand": "Tesla",
  "type": "SUV",
  "description": "Electric SUV"
}
```

### Update Model
```http
PUT /api/cms/vehicles/models/{id}
```

### Delete Model
```http
DELETE /api/cms/vehicles/models/{id}
```

## 2. Vehicle Variants

### Get All Variants
```http
GET /api/cms/vehicles/variants?pageNumber={int}&pageSize={int}&modelId={guid}
```

### Create Variant
```http
POST /api/cms/vehicles/variants
```
**Request:**
```json
{
  "modelId": "guid",
  "variantName": "Long Range",
  "variantCode": "MY-LR",
  "price": 1200000000,
  "specifications": "{\"battery\":\"75kWh\",\"range\":\"450km\"}"
}
```

## 3. Vehicle Colors

### Get All Colors
```http
GET /api/cms/vehicles/colors?pageNumber={int}&pageSize={int}
```

### Create Color
```http
POST /api/cms/vehicles/colors
```
**Request:**
```json
{
  "variantId": "guid",
  "colorName": "Pearl White",
  "colorCode": "PW",
  "hexCode": "#FFFFFF",
  "additionalPrice": 5000000
}
```

## 4. Vehicle Inventory

### Get Central Inventory
```http
GET /api/cms/vehicles/inventories/central?vehicleVariantId={guid}&status={status}
```

**Authorization:** `Admin | EVMManager | EVMStaff`

### Create Inventory
```http
POST /api/cms/vehicles/inventories
```
**Request:**
```json
{
  "vinNumber": "5YJ3E1EA9KF123456",
  "variantId": "guid",
  "colorId": "guid",
  "location": "Central Warehouse",
  "status": "Available"
}
```

### Allocate Vehicle to Dealer
```http
POST /api/cms/vehicles/inventories/allocate
```
**Request:**
```json
{
  "inventoryId": "guid",
  "dealerId": "guid",
  "allocationDate": "2025-11-27"
}
```

## 5. Compare Vehicles (Public)
```http
GET /api/cms/vehicles/variants/compare?variantIds={guid}&variantIds={guid}
```
**Authorization:** Public (No auth required)

**Frontend Tasks:**
- [ ] Create vehicle models CRUD pages
- [ ] Create variants CRUD pages
- [ ] Create colors CRUD pages
- [ ] Create inventory management page
- [ ] Implement vehicle allocation flow
- [ ] Create vehicle comparison page

---

# 🏢 **PHASE 3: CMS - DEALER MANAGEMENT** (Week 3)

> **Implement sau Vehicle Management**

## 1. Dealers

### Get All Dealers
```http
GET /api/cms/dealers?pageNumber={int}&pageSize={int}
```

### Create Dealer
```http
POST /api/cms/dealers
```
**Request:**
```json
{
  "dealerCode": "DL001",
  "name": "Tesla Hanoi Center",
  "address": "123 Láng Hạ",
  "city": "Hanoi",
  "phoneNumber": "0901234567",
  "email": "hanoi@tesla.vn",
  "contactPerson": "Nguyễn Văn A",
  "salesTarget": 50000000000,
  "debtLimit": 10000000000
}
```

### Get Dealer by ID
```http
GET /api/cms/dealers/{id}
```

## 2. Dealer Contracts ⭐ NEW

### Get Dealer Contracts
```http
GET /api/cms/dealer-contracts?dealerId={guid}&status={string}
```

**Status:** Draft, Active, Expired, Terminated

### Create Contract
```http
POST /api/cms/dealer-contracts
```
**Request:**
```json
{
  "dealerId": "guid",
  "startDate": "2025-01-01",
  "endDate": "2025-12-31",
  "terms": "Cam kết bán tối thiểu 100 xe/năm",
  "commissionRate": 5
}
```

### Update Contract Status
```http
PUT /api/cms/dealer-contracts/{id}/status
```
**Request:**
```json
{
  "status": "Active",
  "signedBy": "CEO Tesla Vietnam"
}
```

## 3. Dealer Staff ⭐ NEW

### Get Dealer Staff
```http
GET /api/cms/dealer-staff?dealerId={guid}
```

### Add Staff to Dealer
```http
POST /api/cms/dealer-staff
```
**Request:**
```json
{
  "userId": "guid",
  "dealerId": "guid",
  "position": "Sales Consultant",
  "salesTarget": 500000000,
  "commissionRate": 2
}
```

### Update Staff
```http
PUT /api/cms/dealer-staff/{id}
```
**Request:**
```json
{
  "position": "Senior Sales Consultant",
  "salesTarget": 700000000,
  "commissionRate": 3,
  "isActive": true
}
```

## 4. Dealer Debts ⭐ NEW

### Get Dealer Debts
```http
GET /api/cms/dealer-debts?dealerId={guid}
```

### Create Debt
```http
POST /api/cms/dealer-debts
```
**Request:**
```json
{
  "dealerId": "guid",
  "totalDebt": 500000000,
  "dueDate": "2025-12-31",
  "notes": "Thanh toán cho lô xe tháng 11"
}
```

### Record Payment
```http
PUT /api/cms/dealer-debts/{id}/pay
```
**Request:**
```json
{
  "paymentAmount": 100000000,
  "notes": "Thanh toán đợt 1"
}
```

## 5. Dealer Discount Policies ⭐ NEW

### Get Discount Policies
```http
GET /api/cms/dealer-discount-policies?dealerId={guid}&activeOnly={bool}
```

### Create Policy
```http
POST /api/cms/dealer-discount-policies
```
**Request:**
```json
{
  "dealerId": "guid",
  "vehicleVariantId": "guid",
  "discountRate": 3,
  "minOrderQuantity": 5,
  "maxDiscountAmount": 50000000,
  "effectiveDate": "2025-01-01",
  "expiryDate": "2025-12-31"
}
```

### Update Policy
```http
PUT /api/cms/dealer-discount-policies/{id}
```

**Frontend Tasks:**
- [ ] Create dealers CRUD pages
- [ ] Create dealer detail page with tabs (Info, Contracts, Staff, Debts, Policies)
- [ ] Implement contracts management
- [ ] Implement staff management
- [ ] Implement debt tracking & payment
- [ ] Implement discount policies management

---

# 📦 **PHASE 4: CMS - VEHICLE REQUESTS** (Week 3)

> **Implement sau Dealer Management**

## 1. Get Vehicle Requests
```http
GET /api/cms/vehicle-requests?pendingOnly={bool}
```

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "guid",
      "requestNumber": "VR-20251122-00001",
      "dealerId": "guid",
      "dealerName": "Dealer ABC",
      "vehicleVariantName": "City RS",
      "quantity": 5,
      "status": "Pending",
      "requestDate": "2025-11-22T00:00:00Z"
    }
  ]
}
```

## 2. Approve Request
```http
PUT /api/cms/vehicle-requests/{id}/approve
```
**Request:**
```json
{
  "vehicleInventoryIds": ["guid1", "guid2", "guid3"],
  "notes": "Đã phân xe VIN: xxx, yyy, zzz"
}
```

**Frontend Tasks:**
- [ ] Create vehicle requests list page
- [ ] Implement filter by status
- [ ] Create approval modal with vehicle selector
- [ ] Show request details

---

# 🎁 **PHASE 5: CMS - PROMOTIONS** (Week 4)

> **Implement sau Vehicle Requests**

## 1. Get Promotions
```http
GET /api/cms/promotions?status={string}&fromDate={date}&toDate={date}
```

**Status:** Active, Inactive, Expired

## 2. Create Promotion
```http
POST /api/cms/promotions
```
**Request:**
```json
{
  "promotionCode": "DEC2025",
  "name": "Ưu đãi tháng 12",
  "description": "Giảm 10% cho Model Y",
  "startDate": "2025-12-01",
  "endDate": "2025-12-31",
  "discountType": "Percentage",
  "discountPercentage": 10,
  "status": "Active",
  "applicableVehicleVariantIds": ["guid1", "guid2"],
  "applicableDealerIds": ["guid1", "guid2"],
  "maxUsageCount": 100
}
```

## 3. Update Promotion
```http
PUT /api/cms/promotions/{id}
```

## 4. Update Status
```http
PUT /api/cms/promotions/{id}/status
```
**Request:**
```json
{
  "status": "Active"
}
```

## 5. Delete Promotion
```http
DELETE /api/cms/promotions/{id}
```

**Frontend Tasks:**
- [ ] Create promotions list page
- [ ] Create promotion form (create/edit)
- [ ] Implement date range picker
- [ ] Implement multi-select for variants & dealers
- [ ] Show usage statistics
- [ ] Implement status toggle

---

# 🏪 **PHASE 6: DEALER PORTAL - BASIC** (Week 4)

> **Implement sau CMS phases**

## 1. Dealer Dashboard APIs

### Sales Report for Dealer
```http
GET /api/reports/sales-by-staff?dealerId={guid}&periodStart={date}&periodEnd={date}
```

### Dealer's Inventory
```http
GET /api/cms/vehicles/inventories?dealerId={guid}
```

## 2. View Vehicle Catalog (Dealer)

### Get Models
```http
GET /api/dealer/vehicles/models
```

### Get Variants
```http
GET /api/dealer/vehicles/variants
```

### Compare Vehicles
```http
GET /api/dealer/vehicles/variants/compare?variantIds={guid}&variantIds={guid}
```

### Get Active Promotions
```http
GET /api/dealer/promotions/active
```

## 3. Vehicle Requests (Dealer)

### Get Dealer's Requests
```http
GET /api/dealer/vehicle-requests
```

### Create Request
```http
POST /api/dealer/vehicle-requests
```
**Request:**
```json
{
  "vehicleVariantId": "guid",
  "vehicleColorId": "guid",
  "quantity": 3,
  "expectedDeliveryDate": "2025-12-15",
  "notes": "Cần gấp cho đơn hàng tháng 12"
}
```

**Frontend Tasks:**
- [ ] Create dealer dashboard with widgets
- [ ] Create vehicle catalog view
- [ ] Create vehicle comparison page
- [ ] Show active promotions
- [ ] Create vehicle request form

---

# 👥 **PHASE 7: DEALER - CUSTOMER MANAGEMENT** (Week 5)

> **Implement sau Dealer Basic**

## 1. Get Customers
```http
GET /api/dealer/customers?pageNumber={int}&pageSize={int}&searchTerm={string}
```

## 2. Get Customer by ID
```http
GET /api/dealer/customers/{id}
```

## 3. Get Customer History
```http
GET /api/dealer/customers/{id}/history
```

**Response includes:**
- Orders history
- Quotations
- Test drives
- Total spent
- Completed orders count

## 4. Create Customer
```http
POST /api/dealer/customers
```
**Request:**
```json
{
  "fullName": "Nguyễn Văn A",
  "email": "nguyenvana@example.com",
  "phoneNumber": "0901234567",
  "address": "123 ABC Street",
  "dateOfBirth": "1990-01-01",
  "identityCard": "001234567890"
}
```

## 5. Update Customer
```http
PUT /api/dealer/customers/{id}
```

**Frontend Tasks:**
- [ ] Create customers list with search
- [ ] Create customer form (create/edit)
- [ ] Create customer detail page
- [ ] Show customer history timeline
- [ ] Implement pagination

---

# 🚙 **PHASE 8: DEALER - TEST DRIVES & QUOTATIONS** (Week 5)

> **Implement sau Customer Management**

## 1. Test Drives

### Get Test Drives
```http
GET /api/dealer/test-drives?pageNumber={int}&pageSize={int}&status={string}
```

**Status:** Scheduled, Completed, Cancelled, NoShow

### Create Test Drive
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

### Update Status
```http
PUT /api/dealer/test-drives/{id}/status
```
**Request:**
```json
{
  "status": "Completed",
  "feedback": "Customer loved the car!",
  "rating": 5
}
```

## 2. Quotations

### Get Quotations
```http
GET /api/dealer/quotations?pageNumber={int}&pageSize={int}&status={string}&customerId={guid}
```

### Create Quotation
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
  "notes": "Special promotion"
}
```

**Frontend Tasks:**
- [ ] Create test drives list (calendar + list view)
- [ ] Create test drive scheduling form
- [ ] Implement status update with feedback
- [ ] Create quotations list
- [ ] Create quotation form with price calculator
- [ ] Auto-check and apply promotions
- [ ] Export quotation to PDF

---

# 🛒 **PHASE 9: DEALER - ORDERS & PAYMENTS** (Week 6-7)

> **Implement sau Test Drives - Critical Phase**

## 1. Orders

### Get Orders
```http
GET /api/dealer/orders?pageNumber={int}&pageSize={int}&status={string}&customerId={guid}
```

**Status:** Pending, Confirmed, Completed, Cancelled

### Create Order (Multi-step Wizard)
```http
POST /api/dealer/orders
```
**Request:**
```json
{
  "customerId": "guid",
  "vehicleVariantId": "guid",
  "vehicleColorId": "guid",
  "basePrice": 559000000,
  "discount": 10000000,
  "tax": 55900000,
  "totalAmount": 604900000,
  "paymentMethod": "Installment",
  "notes": "Delivery next month"
}
```

### Update Order Status
```http
PUT /api/dealer/orders/{id}/status
```
**Request:**
```json
{
  "status": "Confirmed",
  "notes": "All documents verified"
}
```

## 2. Payments

### Get Payments by Order
```http
GET /api/dealer/payments/by-order/{orderId}
```

**Response includes:**
- Payment history
- Installment plan details
- Remaining amount

### Create Payment
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

### Create Installment Plan
```http
POST /api/dealer/payments/installment-plans
```
**Request:**
```json
{
  "orderId": "guid",
  "totalAmount": 1200000000,
  "downPayment": 400000000,
  "numberOfInstallments": 24,
  "interestRate": 8,
  "startDate": "2025-12-27"
}
```

## 3. Sales Contracts

### Create Contract
```http
POST /api/dealer/sales-contracts
```
**Request:**
```json
{
  "orderId": "guid",
  "contractDate": "2025-11-27",
  "deliveryDate": "2025-12-15",
  "warrantyPeriod": 36,
  "specialTerms": "Bảo hành pin 8 năm"
}
```

**Frontend Tasks:**
- [ ] Create orders list with filters
- [ ] Create order wizard (5 steps):
  - Step 1: Select customer
  - Step 2: Configure vehicle
  - Step 3: Pricing (auto-apply promotions)
  - Step 4: Payment method
  - Step 5: Review & confirm
- [ ] Create order detail page
- [ ] Implement payment recording
- [ ] Create installment calculator
- [ ] Show payment schedule table
- [ ] Create sales contract form

---

# 🚚 **PHASE 10: DEALER - DELIVERIES** (Week 7) ⭐ NEW

> **Implement sau Orders - New Feature**

## 1. Get Deliveries
```http
GET /api/dealer/deliveries?status={string}&fromDate={date}&toDate={date}
```

**Status:** Pending, InTransit, Delivered

## 2. Create Delivery
```http
POST /api/dealer/deliveries
```
**Request:**
```json
{
  "orderId": "guid",
  "scheduledDate": "2025-12-15",
  "deliveryAddress": "456 Trần Duy Hưng, Hà Nội",
  "receiverName": "Lê Thị C",
  "receiverPhone": "0923456789",
  "notes": "Giao vào buổi sáng"
}
```

## 3. Update Delivery Status
```http
PUT /api/dealer/deliveries/{id}/status
```
**Request:**
```json
{
  "status": "InTransit",
  "actualDeliveryDate": "2025-12-15",
  "notes": "Xe đang trên đường giao"
}
```

## 4. Upload Photos
```http
POST /api/dealer/deliveries/{id}/photos
```
**Request:**
```json
{
  "photoUrls": [
    "https://storage.com/photo1.jpg",
    "https://storage.com/photo2.jpg"
  ]
}
```

## 5. Capture Signature
```http
POST /api/dealer/deliveries/{id}/signature
```
**Request:**
```json
{
  "signatureBase64": "data:image/png;base64,iVBORw0KG..."
}
```

## 6. Get Delivery by ID
```http
GET /api/dealer/deliveries/{id}
```

**Frontend Tasks:**
- [ ] Create deliveries list
- [ ] Create delivery form
- [ ] Create delivery detail page with status stepper
- [ ] Implement photo uploader (multiple files)
- [ ] Implement signature pad (canvas-based)
- [ ] Show delivery timeline

---

# 💬 **PHASE 11: DEALER - FEEDBACK** (Week 8) ⭐ NEW

> **Implement sau Deliveries - New Feature**

## 1. Get Feedbacks
```http
GET /api/dealer/feedbacks?status={string}&rating={int}
```

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "guid",
      "customerId": "guid",
      "customerName": "Lê Thị C",
      "orderId": "guid",
      "orderNumber": "ORD-001",
      "rating": 5,
      "feedbackType": "General",
      "comment": "Xe rất tốt!",
      "feedbackStatus": "Pending",
      "createdAt": "2025-11-27T00:00:00Z"
    }
  ]
}
```

## 2. Get Feedback by ID
```http
GET /api/dealer/feedbacks/{id}
```

## 3. Submit Feedback
```http
POST /api/dealer/feedbacks
```
**Request:**
```json
{
  "customerId": "guid",
  "orderId": "guid",
  "rating": 5,
  "feedbackType": "General",
  "comment": "Dịch vụ chu đáo!",
  "feedbackStatus": "Resolved"
}
```

## 4. Respond to Feedback
```http
PUT /api/dealer/feedbacks/{id}/respond
```
**Request:**
```json
{
  "response": "Cảm ơn quý khách đã tin tưởng!"
}
```

## 5. Update Status
```http
PUT /api/dealer/feedbacks/{id}/status
```
**Request:**
```json
{
  "feedbackStatus": "Resolved",
  "complaintStatus": "Resolved"
}
```

**Frontend Tasks:**
- [ ] Create feedbacks list with filters
- [ ] Color coding by rating (green 4-5, yellow 3, red 1-2)
- [ ] Create feedback detail page
- [ ] Implement response form
- [ ] Show feedback timeline
- [ ] Implement status update

---

# 📊 **PHASE 12: REPORTS** (Week 8)

> **Implement cuối cùng**

**Authorization:** Varies by report type

## 1. Sales by Dealer Report
```http
GET /api/reports/sales-by-dealer?periodStart={date}&periodEnd={date}&dealerId={guid}
```

**Authorization:** `Admin | EVMManager | EVMStaff | DealerManager`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "dealerId": "guid",
      "dealerName": "Dealer ABC",
      "totalOrders": 25,
      "completedOrders": 20,
      "totalRevenue": 5000000000,
      "averageOrderValue": 250000000
    }
  ]
}
```

## 2. Sales by Staff Report
```http
GET /api/reports/sales-by-staff?periodStart={date}&periodEnd={date}&dealerId={guid}&staffId={guid}
```

## 3. Inventory Turnover Report
```http
GET /api/reports/inventory-turnover?vehicleVariantId={guid}
```

**Authorization:** `Admin | EVMManager | EVMStaff`

## 4. Dealer Debt Report
```http
GET /api/reports/dealer-debt?dealerId={guid}
```

**Authorization:** `Admin | EVMManager`

## 5. Customer Debt Report
```http
GET /api/reports/customer-debt?customerId={guid}&dealerId={guid}
```

**Frontend Tasks:**
- [ ] Create reports dashboard
- [ ] Implement date range picker
- [ ] Create charts (Bar, Line, Pie) - use Chart.js/Recharts
- [ ] Create data tables with export (Excel/PDF)
- [ ] Show summary cards
- [ ] Implement filters

---

## 💡 **FRONTEND SETUP - Code Examples**

### 1. API Client Setup (Axios)
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
    if (error.response?.status === 401) {
      localStorage.removeItem('access_token');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default apiClient;
```

### 2. Token Management
```javascript
// Store token
localStorage.setItem('access_token', response.data.token);

// Get token
const token = localStorage.getItem('access_token');

// Remove token (logout)
localStorage.removeItem('access_token');
```

### 3. Example API Calls
```javascript
// Login
const login = async (email, password) => {
  const response = await apiClient.post('/api/cms/auth/login', {
    email,
    password
  });
  localStorage.setItem('access_token', response.data.token);
  return response;
};

// Get customers
const getCustomers = async (pageNumber = 1, pageSize = 10) => {
  const response = await apiClient.get('/api/dealer/customers', {
    params: { pageNumber, pageSize }
  });
  return response.data;
};

// Create order
const createOrder = async (orderData) => {
  const response = await apiClient.post('/api/dealer/orders', orderData);
  return response.data;
};
```

---

## ⚠️ **ERROR HANDLING**

### Standard Error Format
```json
{
  "success": false,
  "message": "Error message",
  "errors": {
    "fieldName": ["Validation error"]
  },
  "statusCode": 400
}
```

### HTTP Status Codes
- `200` OK - Success
- `201` Created - Resource created
- `400` Bad Request - Validation error
- `401` Unauthorized - Invalid token
- `403` Forbidden - Insufficient permissions
- `404` Not Found - Resource not found
- `500` Internal Server Error

---

## 🔒 **ROLE-BASED ACCESS**

| Feature | Admin | EVMManager | EVMStaff | DealerManager | DealerStaff |
|---------|-------|------------|----------|---------------|-------------|
| Vehicle Management | ✅ | ✅ | ✅ | ❌ | ❌ |
| Dealer Management | ✅ | ✅ | ❌ | ❌ | ❌ |
| Dealer Contracts | ✅ | ✅ | ❌ | ❌ | ❌ |
| Vehicle Requests (Approve) | ✅ | ✅ | ✅ | ❌ | ❌ |
| Promotions | ✅ | ✅ | ❌ | ❌ | ❌ |
| Customer Management | ✅ | ✅ | ✅ | ✅ | ✅ |
| Orders | ✅ | ✅ | ✅ | ✅ | ✅ |
| Deliveries | ✅ | ✅ | ✅ | ✅ | ✅ |
| Feedback | ✅ | ✅ | ✅ | ✅ | ✅ |
| Sales Contracts | ✅ | ✅ | ✅ | ✅ | ❌ |
| Reports (All) | ✅ | ✅ | ✅ | ✅ | ❌ |

---

## 📞 **SUPPORT**

- **Swagger UI:** http://localhost:5001/swagger
- **Backend Team:** backend@evdealer.com

---

**Last Updated:** November 27, 2025  
**API Version:** 1.0  
**Status:** ✅ Production Ready
