# 🏢 LUỒNG ADMIN TẠO DEALER MỚI

> **Hướng dẫn chi tiết từng bước để Admin onboard một Dealer mới vào hệ thống**

---

## 📋 **TỔNG QUAN LUỒNG**

```
Step 1: Tạo Dealer (Thông tin công ty)
   ↓
Step 2: Tạo User Account cho Dealer Manager (Login account)
   ↓
Step 3: Link User với Dealer (Dealer Staff)
   ↓
Step 4: Tạo Contract (Optional - Hợp đồng hợp tác)
   ↓
Step 5: Setup Discount Policies (Optional - Chính sách chiết khấu)
   ↓
✅ HOÀN TẤT - Dealer có thể login và bắt đầu làm việc
```

---

## 📍 **STEP 1: TẠO DEALER (Thông tin công ty)**

### API Endpoint
```http
POST /api/cms/dealers
Authorization: Bearer {admin_token}
```

### Authorization
- **Required Roles:** `Admin` | `EVMManager`
- ❌ `EVMStaff` không có quyền tạo dealer

### Request Body
```json
{
  "dealerCode": "DL-HN-001",
  "dealerName": "Tesla Hanoi Center",
  "address": "123 Láng Hạ, Đống Đa",
  "city": "Hanoi",
  "district": "Đống Đa",
  "phoneNumber": "0901234567",
  "email": "hanoi@tesla.vn",
  "debtLimit": 10000000000
}
```

### Field Descriptions
| Field | Type | Required | Validation | Description |
|-------|------|----------|------------|-------------|
| `dealerCode` | string | ✅ | Max 20 chars, unique | Mã đại lý (VD: DL-HN-001) |
| `dealerName` | string | ✅ | Max 200 chars | Tên công ty đại lý |
| `address` | string | ✅ | - | Địa chỉ showroom |
| `city` | string | ✅ | - | Thành phố |
| `district` | string | ✅ | - | Quận/Huyện |
| `phoneNumber` | string | ✅ | 10-11 digits | Số điện thoại |
| `email` | string | ✅ | Valid email | Email công ty |
| `debtLimit` | decimal | ✅ | >= 0 | Hạn mức công nợ (VNĐ) |

### Response (Success)
```json
{
  "success": true,
  "message": "Dealer created successfully",
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "dealerCode": "DL-HN-001",
    "dealerName": "Tesla Hanoi Center",
    "address": "123 Láng Hạ, Đống Đa",
    "city": "Hanoi",
    "district": "Đống Đa",
    "phoneNumber": "0901234567",
    "email": "hanoi@tesla.vn",
    "status": "Active",
    "debtLimit": 10000000000,
    "currentDebt": 0,
    "createdAt": "2025-11-27T10:00:00Z"
  },
  "statusCode": 201
}
```

### Business Rules
- ✅ **DealerCode** phải unique trong hệ thống
- ✅ **Status** mặc định là `Active` khi tạo
- ✅ **CurrentDebt** mặc định là `0` khi tạo
- ✅ **DebtLimit** có thể điều chỉnh sau

### Error Cases
**400 - Validation Error:**
```json
{
  "success": false,
  "message": "Validation failed",
  "errors": {
    "dealerCode": ["Dealer with this code already exists"],
    "phoneNumber": ["Phone number must be 10-11 digits"]
  },
  "statusCode": 400
}
```

**401 - Unauthorized:**
```json
{
  "success": false,
  "message": "You do not have permission to create dealers",
  "statusCode": 403
}
```

---

## 👤 **STEP 2: TẠO USER ACCOUNT CHO DEALER MANAGER**

> **Tạo tài khoản đăng nhập cho người quản lý đại lý**

### API Endpoint
```http
POST /api/cms/users
Authorization: Bearer {admin_token}
```

### Authorization
- **Required Roles:** `Admin` only
- ⚠️ Chỉ Admin mới có quyền tạo user

### Request Body
```json
{
  "username": "dealer.hanoi",
  "email": "manager@tesla-hanoi.vn",
  "password": "SecurePassword@123",
  "firstName": "Nguyễn",
  "lastName": "Văn A",
  "phoneNumber": "0912345678",
  "role": "DealerManager"
}
```

### Field Descriptions
| Field | Type | Required | Validation | Description |
|-------|------|----------|------------|-------------|
| `username` | string | ✅ | Unique | Tên đăng nhập |
| `email` | string | ✅ | Valid email, unique | Email cá nhân |
| `password` | string | ✅ | Min 8 chars, strong | Mật khẩu |
| `firstName` | string | ✅ | - | Tên |
| `lastName` | string | ✅ | - | Họ |
| `phoneNumber` | string | ❌ | - | Số điện thoại cá nhân |
| `role` | string | ✅ | - | Vai trò: **DealerManager** |

### Response (Success)
```json
{
  "success": true,
  "message": "User created successfully",
  "data": {
    "id": "7fa85f64-5717-4562-b3fc-2c963f66afa7",
    "username": "dealer.hanoi",
    "email": "manager@tesla-hanoi.vn",
    "firstName": "Nguyễn",
    "lastName": "Văn A",
    "phoneNumber": "0912345678",
    "role": "DealerManager",
    "isActive": true,
    "createdAt": "2025-11-27T10:05:00Z"
  },
  "statusCode": 201
}
```

### Business Rules
- ✅ **Role** phải set là `DealerManager` (hoặc `DealerStaff` nếu là nhân viên)
- ✅ **Username** và **Email** phải unique
- ✅ **Password** phải đủ mạnh (min 8 ký tự, có chữ hoa, chữ thường, số, ký tự đặc biệt)
- ✅ User có thể login ngay sau khi tạo

### Roles Available
- `Admin` - Quản trị hệ thống
- `EVMManager` - Quản lý EVM
- `EVMStaff` - Nhân viên EVM
- `DealerManager` - **Quản lý đại lý** ⭐
- `DealerStaff` - Nhân viên đại lý

---

## 🔗 **STEP 3: LINK USER VỚI DEALER (Dealer Staff)**

> **Gán User Account vào Dealer để họ có quyền truy cập**

### API Endpoint
```http
POST /api/cms/dealer-staff
Authorization: Bearer {admin_token}
```

### Authorization
- **Required Roles:** `Admin` | `EVMManager` | `EVMStaff`

### Request Body
```json
{
  "userId": "7fa85f64-5717-4562-b3fc-2c963f66afa7",
  "dealerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "position": "General Manager",
  "salesTarget": 5000000000,
  "commissionRate": 3
}
```

### Field Descriptions
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `userId` | Guid | ✅ | ID của User (từ Step 2) |
| `dealerId` | Guid | ✅ | ID của Dealer (từ Step 1) |
| `position` | string | ✅ | Chức vụ (VD: General Manager, Sales Manager) |
| `salesTarget` | decimal | ❌ | Chỉ tiêu doanh số cá nhân (VNĐ) |
| `commissionRate` | decimal | ❌ | % hoa hồng (0-100) |

### Response (Success)
```json
{
  "success": true,
  "message": "Dealer staff added successfully",
  "data": {
    "id": "8fa85f64-5717-4562-b3fc-2c963f66afa8",
    "userId": "7fa85f64-5717-4562-b3fc-2c963f66afa7",
    "userName": "Nguyễn Văn A",
    "email": "manager@tesla-hanoi.vn",
    "dealerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "dealerName": "Tesla Hanoi Center",
    "position": "General Manager",
    "salesTarget": 5000000000,
    "commissionRate": 3,
    "isActive": true,
    "hireDate": "2025-11-27T10:10:00Z"
  },
  "statusCode": 201
}
```

### Business Rules
- ✅ **User phải có role** `DealerManager` hoặc `DealerStaff`
- ✅ **1 User có thể làm việc cho nhiều Dealer** (chuyển việc, part-time)
- ✅ **CommissionRate** từ 0-100%
- ✅ Sau khi link, user có thể login vào Dealer Portal

### ⚠️ Important Notes
- Nếu User có role `DealerManager`, họ sẽ có **full quyền** quản lý dealer đó
- Nếu User có role `DealerStaff`, họ chỉ có quyền **hạn chế** (không tạo contract, vehicle request)

---

## 📄 **STEP 4: TẠO CONTRACT (Optional)**

> **Tạo hợp đồng hợp tác giữa EVM và Dealer**

### API Endpoint
```http
POST /api/cms/dealer-contracts
Authorization: Bearer {admin_token}
```

### Authorization
- **Required Roles:** `Admin` | `EVMManager`

### Request Body
```json
{
  "dealerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "startDate": "2025-01-01",
  "endDate": "2025-12-31",
  "terms": "Cam kết bán tối thiểu 100 xe/năm. Hoa hồng 3% trên mỗi đơn hàng. Thanh toán trong vòng 30 ngày.",
  "commissionRate": 3
}
```

### Field Descriptions
| Field | Type | Required | Validation | Description |
|-------|------|----------|------------|-------------|
| `dealerId` | Guid | ✅ | - | ID của Dealer |
| `startDate` | Date | ✅ | - | Ngày bắt đầu hợp đồng |
| `endDate` | Date | ✅ | > startDate | Ngày kết thúc hợp đồng |
| `terms` | string | ✅ | Max 2000 chars | Điều khoản hợp đồng |
| `commissionRate` | decimal | ✅ | 0-100 | % hoa hồng |

### Response (Success)
```json
{
  "success": true,
  "message": "Dealer contract created successfully",
  "data": {
    "id": "9fa85f64-5717-4562-b3fc-2c963f66afa9",
    "contractNumber": "CT-2025-001",
    "dealerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "dealerName": "Tesla Hanoi Center",
    "startDate": "2025-01-01T00:00:00Z",
    "endDate": "2025-12-31T00:00:00Z",
    "terms": "Cam kết bán tối thiểu 100 xe/năm...",
    "commissionRate": 3,
    "status": "Draft",
    "createdAt": "2025-11-27T10:15:00Z"
  },
  "statusCode": 201
}
```

### Contract Status Flow
```
Draft → Active → Expired/Terminated
```

### Activate Contract
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

### Business Rules
- ✅ **Contract Number** tự động generate (CT-YYYY-XXX)
- ✅ **Status** mặc định là `Draft` khi tạo
- ✅ Phải **Active** contract trước khi dealer có thể đặt hàng
- ✅ **EndDate** phải sau **StartDate**
- ✅ 1 Dealer có thể có nhiều contracts (theo thời gian)

---

## 💰 **STEP 5: SETUP DISCOUNT POLICIES (Optional)**

> **Thiết lập chính sách chiết khấu cho Dealer theo từng dòng xe**

### API Endpoint
```http
POST /api/cms/dealer-discount-policies
Authorization: Bearer {admin_token}
```

### Authorization
- **Required Roles:** `Admin` | `EVMManager`

### Request Body
```json
{
  "dealerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "vehicleVariantId": "4fa85f64-5717-4562-b3fc-2c963f66afb1",
  "discountRate": 3,
  "minOrderQuantity": 5,
  "maxDiscountAmount": 50000000,
  "effectiveDate": "2025-01-01",
  "expiryDate": "2025-12-31"
}
```

### Field Descriptions
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `dealerId` | Guid | ✅ | ID của Dealer |
| `vehicleVariantId` | Guid | ✅ | ID của Vehicle Variant (Model Y Long Range, etc.) |
| `discountRate` | decimal | ✅ | % chiết khấu (0-100) |
| `minOrderQuantity` | int | ❌ | Số lượng tối thiểu để được chiết khấu |
| `maxDiscountAmount` | decimal | ❌ | Số tiền chiết khấu tối đa (VNĐ) |
| `effectiveDate` | Date | ✅ | Ngày bắt đầu áp dụng |
| `expiryDate` | Date | ✅ | Ngày hết hạn |

### Response (Success)
```json
{
  "success": true,
  "message": "Discount policy created successfully",
  "data": {
    "id": "1fa85f64-5717-4562-b3fc-2c963f66afb2",
    "dealerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "dealerName": "Tesla Hanoi Center",
    "vehicleVariantId": "4fa85f64-5717-4562-b3fc-2c963f66afb1",
    "vehicleVariantName": "Model Y Long Range",
    "discountRate": 3,
    "minOrderQuantity": 5,
    "maxDiscountAmount": 50000000,
    "effectiveDate": "2025-01-01T00:00:00Z",
    "expiryDate": "2025-12-31T00:00:00Z",
    "isActive": true,
    "createdAt": "2025-11-27T10:20:00Z"
  },
  "statusCode": 201
}
```

### Business Rules
- ✅ **1 Dealer - 1 Variant** chỉ có 1 policy active tại 1 thời điểm
- ✅ **DiscountRate** tự động áp dụng khi dealer tạo order
- ✅ **MinOrderQuantity**: Nếu set, dealer phải đặt tối thiểu số lượng này
- ✅ **MaxDiscountAmount**: Giới hạn số tiền chiết khấu (tránh lỗ)
- ✅ **Expiry Date** phải sau **Effective Date**

### Example Use Cases
**Case 1: Volume Discount**
```json
{
  "discountRate": 5,
  "minOrderQuantity": 10,
  "maxDiscountAmount": 100000000
}
```
➡️ Dealer đặt ≥ 10 xe → chiết khấu 5%, tối đa 100 triệu/xe

**Case 2: Seasonal Promotion**
```json
{
  "discountRate": 3,
  "minOrderQuantity": 1,
  "effectiveDate": "2025-12-01",
  "expiryDate": "2025-12-31"
}
```
➡️ Tháng 12 → chiết khấu 3% cho mọi đơn hàng

---

## ✅ **VERIFICATION & TESTING**

### 1. Verify Dealer Created
```http
GET /api/cms/dealers/{dealerId}
Authorization: Bearer {admin_token}
```

### 2. Test Dealer Manager Login
```http
POST /api/auth/dealer/login
```
**Request:**
```json
{
  "email": "manager@tesla-hanoi.vn",
  "password": "SecurePassword@123"
}
```

**Expected Response:**
```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": "7fa85f64-5717-4562-b3fc-2c963f66afa7",
      "email": "manager@tesla-hanoi.vn",
      "firstName": "Nguyễn",
      "lastName": "Văn A",
      "role": "DealerManager"
    },
    "expiresAt": "2025-11-28T10:00:00Z"
  }
}
```

### 3. Test Dealer Access to Portal
```http
GET /api/dealer/vehicles/models
Authorization: Bearer {dealer_token}
```

---

## 🔄 **TYPICAL SCENARIOS**

### Scenario 1: New Dealer (Full Setup)
```
1. Tạo Dealer
2. Tạo DealerManager User
3. Link User → Dealer
4. Tạo Contract (Active)
5. Setup Discount Policies (cho 3-5 dòng xe hot)
✅ Dealer ready to sell
```

### Scenario 2: Quick Onboarding (Minimal)
```
1. Tạo Dealer
2. Tạo DealerManager User
3. Link User → Dealer
✅ Dealer có thể login và explore
(Contract & Policies có thể setup sau)
```

### Scenario 3: Add Staff to Existing Dealer
```
1. Tạo DealerStaff User (role: DealerStaff)
2. Link User → Dealer (với Position, SalesTarget)
✅ Staff có thể login và hỗ trợ dealer
```

---

## 📊 **POST-ONBOARDING ACTIVITIES**

### What Dealer Can Do After Onboarding:

#### 1. Vehicle Management
- ✅ Browse vehicle catalog
- ✅ Compare vehicles
- ✅ View active promotions
- ✅ Request vehicles from EVM

#### 2. Customer Management
- ✅ Create customers
- ✅ View customer history
- ✅ Schedule test drives
- ✅ Create quotations

#### 3. Sales Operations
- ✅ Create orders
- ✅ Record payments
- ✅ Create installment plans
- ✅ Generate sales contracts

#### 4. Delivery & Service
- ✅ Manage deliveries
- ✅ Upload delivery photos
- ✅ Capture customer signatures
- ✅ Handle customer feedback

#### 5. Reporting
- ✅ View sales reports
- ✅ Track staff performance
- ✅ Monitor inventory

---

## ⚠️ **COMMON ERRORS & SOLUTIONS**

### Error 1: "Dealer with this code already exists"
**Cause:** DealerCode đã tồn tại  
**Solution:** Đổi DealerCode khác (VD: DL-HN-001 → DL-HN-002)

### Error 2: "Email already exists"
**Cause:** Email user đã được dùng  
**Solution:** Dùng email khác hoặc kiểm tra user đã tồn tại chưa

### Error 3: "User is not a dealer user"
**Cause:** User có role `Admin` hoặc `EVMStaff` (không phải dealer role)  
**Solution:** Tạo user mới với role `DealerManager` hoặc `DealerStaff`

### Error 4: "Dealer not found"
**Cause:** DealerId sai hoặc dealer đã bị xóa  
**Solution:** Kiểm tra lại DealerId từ response Step 1

---

## 📞 **SUPPORT**

- **Swagger Documentation:** http://localhost:5001/swagger
- **Backend Team:** backend@evdealer.com
- **Testing:** Use Postman Collection (provided separately)

---

## 🎯 **QUICK REFERENCE**

| Step | Endpoint | Role Required | Time Estimate |
|------|----------|---------------|---------------|
| 1. Create Dealer | `POST /api/cms/dealers` | Admin, EVMManager | 2 min |
| 2. Create User | `POST /api/cms/users` | Admin | 2 min |
| 3. Link User-Dealer | `POST /api/cms/dealer-staff` | Admin, EVMManager, EVMStaff | 1 min |
| 4. Create Contract | `POST /api/cms/dealer-contracts` | Admin, EVMManager | 3 min |
| 5. Setup Policies | `POST /api/cms/dealer-discount-policies` | Admin, EVMManager | 2 min/policy |
| **Total** | | | **~10-15 mins** |

---

**Last Updated:** November 27, 2025  
**Version:** 1.0  
**Status:** ✅ Production Ready

