# Postman Test Script Guide - Lab 2

## 1. Giới Thiệu (Introduction)

Tài liệu này hướng dẫn cách viết test scripts trong Postman để:
1. **Kiểm tra HTTP Status Code**
2. **Validate JSON Schema** của response

---

## 2. Cách Thêm Test Script

Trong Postman, mở tab **Tests** của mỗi request và thêm JavaScript code.

![Postman Tests Tab](https://learning.postman.com/docs/writing-scripts/images/test-scripts-tab.png)

---

## 3. Test HTTP Status Code

### 3.1 Kiểm tra Status Code Cụ Thể

```javascript
// Test 200 OK
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

// Test 201 Created
pm.test("Status code is 201", function () {
    pm.response.to.have.status(201);
});

// Test 400 Bad Request
pm.test("Status code is 400", function () {
    pm.response.to.have.status(400);
});

// Test 401 Unauthorized
pm.test("Status code is 401", function () {
    pm.response.to.have.status(401);
});

// Test 404 Not Found
pm.test("Status code is 404", function () {
    pm.response.to.have.status(404);
});
```

### 3.2 Kiểm tra Một Trong Nhiều Status Codes

```javascript
// Check if status is one of: 200, 201, 204
pm.test("Status code is success code", function () {
    pm.expect([200, 201, 204]).to.include(pm.response.code);
});
```

### 3.3 Kiểm tra Status Code Family

```javascript
// Check if status is 2xx (success)
pm.test("Status code is 2xx", function () {
    pm.expect(pm.response.code).to.be.within(200, 299);
});

// Check if status is 4xx (client error)
pm.test("Status code is 4xx", function () {
    pm.expect(pm.response.code).to.be.within(400, 499);
});
```

---

## 4. JSON Schema Validation

### 4.1 Cơ Bản về JSON Schema

JSON Schema định nghĩa cấu trúc của JSON object:

| Keyword | Ý nghĩa |
|---------|---------|
| `type` | Kiểu dữ liệu: `object`, `array`, `string`, `integer`, `number`, `boolean`, `null` |
| `required` | Danh sách các field bắt buộc |
| `properties` | Định nghĩa các thuộc tính của object |
| `items` | Định nghĩa phần tử của array |
| `minimum` | Giá trị tối thiểu (số) |
| `maximum` | Giá trị tối đa (số) |
| `minLength` | Độ dài tối thiểu (string) |
| `maxLength` | Độ dài tối đa (string) |
| `enum` | Danh sách giá trị hợp lệ |

### 4.2 Ví Dụ Schema Đơn Giản

```javascript
// Schema cho Login Response
const loginResponseSchema = {
    type: "object",
    required: ["success", "message", "data"],
    properties: {
        success: { type: "boolean" },
        message: { type: "string" },
        data: {
            type: "object",
            required: ["token", "email", "role"],
            properties: {
                token: { type: "string" },
                accountId: { type: ["integer", "null"] },
                email: { type: "string" },
                name: { type: ["string", "null"] },
                role: { type: "integer" },
                roleName: { type: "string" }
            }
        },
        errors: { type: ["array", "null"] }
    }
};

// Validate response với schema
pm.test("Response matches JSON schema", function () {
    pm.response.to.have.jsonSchema(loginResponseSchema);
});
```

### 4.3 Schema Cho Paginated Response

```javascript
const paginatedSchema = {
    type: "object",
    required: ["success", "message", "data"],
    properties: {
        success: { type: "boolean" },
        message: { type: "string" },
        data: {
            type: "object",
            required: ["items", "page", "pageSize", "totalItems", "totalPages"],
            properties: {
                items: {
                    type: "array",
                    items: {
                        type: "object",
                        required: ["categoryID", "categoryName"],
                        properties: {
                            categoryID: { type: "integer" },
                            categoryName: { type: "string" },
                            categoryDesciption: { type: "string" },
                            isActive: { type: "boolean" }
                        }
                    }
                },
                page: { type: "integer", minimum: 1 },
                pageSize: { type: "integer", minimum: 1, maximum: 50 },
                totalItems: { type: "integer", minimum: 0 },
                totalPages: { type: "integer", minimum: 0 }
            }
        }
    }
};

pm.test("Response matches paginated schema", function () {
    pm.response.to.have.jsonSchema(paginatedSchema);
});
```

### 4.4 Schema Cho Error Response

```javascript
const errorSchema = {
    type: "object",
    required: ["success", "message"],
    properties: {
        success: { 
            type: "boolean", 
            enum: [false]  // success PHẢI là false
        },
        message: { type: "string" },
        data: { type: "null" },
        errors: {
            type: ["array", "null"],
            items: { type: "string" }
        }
    }
};

pm.test("Error response matches schema", function () {
    pm.response.to.have.jsonSchema(errorSchema);
});
```

---

## 5. Complete Test Examples

### 5.1 Login Request Test

```javascript
// ==== TESTS FOR LOGIN ====

// 1. Test HTTP Status Code
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

// 2. Test Response Time
pm.test("Response time is less than 2000ms", function () {
    pm.expect(pm.response.responseTime).to.be.below(2000);
});

// 3. Define Schema
const loginSchema = {
    type: "object",
    required: ["success", "message", "data"],
    properties: {
        success: { type: "boolean" },
        message: { type: "string" },
        data: {
            type: "object",
            required: ["token", "email", "role", "roleName"],
            properties: {
                token: { type: "string" },
                email: { type: "string" },
                role: { type: "integer" },
                roleName: { type: "string" }
            }
        }
    }
};

// 4. Validate Schema
pm.test("Response matches login schema", function () {
    pm.response.to.have.jsonSchema(loginSchema);
});

// 5. Test Specific Values
pm.test("Success is true", function () {
    const response = pm.response.json();
    pm.expect(response.success).to.be.true;
});

pm.test("Token is not empty", function () {
    const response = pm.response.json();
    pm.expect(response.data.token).to.be.a("string").and.not.empty;
});

// 6. Save Token (for subsequent requests)
const response = pm.response.json();
if (response.success && response.data.token) {
    pm.collectionVariables.set("adminToken", response.data.token);
}
```

### 5.2 Get Collection Test

```javascript
// ==== TESTS FOR GET CATEGORIES ====

// 1. Status Code
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

// 2. Schema
const categoryListSchema = {
    type: "object",
    required: ["success", "data"],
    properties: {
        success: { type: "boolean" },
        data: {
            type: "object",
            required: ["items", "page", "pageSize", "totalItems", "totalPages"],
            properties: {
                items: { type: "array" },
                page: { type: "integer" },
                pageSize: { type: "integer" },
                totalItems: { type: "integer" },
                totalPages: { type: "integer" }
            }
        }
    }
};

pm.test("Response matches schema", function () {
    pm.response.to.have.jsonSchema(categoryListSchema);
});

// 3. Test Pagination Logic
pm.test("Pagination is correct", function () {
    const response = pm.response.json();
    const data = response.data;
    
    pm.expect(data.page).to.be.at.least(1);
    pm.expect(data.pageSize).to.be.at.most(50);
    pm.expect(data.totalPages).to.equal(Math.ceil(data.totalItems / data.pageSize));
    pm.expect(data.items.length).to.be.at.most(data.pageSize);
});
```

### 5.3 Validation Error Test

```javascript
// ==== TESTS FOR VALIDATION ERROR ====

// 1. Status Code 400
pm.test("Status code is 400 Bad Request", function () {
    pm.response.to.have.status(400);
});

// 2. Schema
const validationErrorSchema = {
    type: "object",
    required: ["success", "message", "errors"],
    properties: {
        success: { type: "boolean", enum: [false] },
        message: { type: "string" },
        data: { type: "null" },
        errors: {
            type: "array",
            items: { type: "string" },
            minItems: 1
        }
    }
};

pm.test("Response matches validation error schema", function () {
    pm.response.to.have.jsonSchema(validationErrorSchema);
});

// 3. Test Error Messages
pm.test("Errors array contains messages", function () {
    const response = pm.response.json();
    pm.expect(response.errors).to.be.an("array").and.not.empty;
    response.errors.forEach(error => {
        pm.expect(error).to.be.a("string").and.not.empty;
    });
});
```

### 5.4 Not Found Test

```javascript
// ==== TESTS FOR 404 NOT FOUND ====

// 1. Status Code
pm.test("Status code is 404 Not Found", function () {
    pm.response.to.have.status(404);
});

// 2. Schema
const notFoundSchema = {
    type: "object",
    required: ["success", "message"],
    properties: {
        success: { type: "boolean", enum: [false] },
        message: { type: "string" },
        data: { type: "null" }
    }
};

pm.test("Response matches not found schema", function () {
    pm.response.to.have.jsonSchema(notFoundSchema);
});

// 3. Message Contains "not found"
pm.test("Message indicates resource not found", function () {
    const response = pm.response.json();
    pm.expect(response.message.toLowerCase()).to.include("not found");
});
```

---

## 6. Tips & Best Practices

### 6.1 Sử Dụng Variables

```javascript
// Save value to collection variable
pm.collectionVariables.set("token", response.data.token);

// Use variable in request header
// Authorization: Bearer {{token}}
```

### 6.2 Conditional Testing

```javascript
// Test based on status code
const status = pm.response.code;

if (status === 200) {
    pm.test("Success response has data", function () {
        const response = pm.response.json();
        pm.expect(response.data).to.not.be.null;
    });
} else if (status === 404) {
    pm.test("Not found response", function () {
        const response = pm.response.json();
        pm.expect(response.success).to.be.false;
    });
}
```

### 6.3 Multiple Type Support

```javascript
// Field có thể là integer hoặc null
properties: {
    parentCategoryID: { type: ["integer", "null"] }
}
```

### 6.4 Enum Validation

```javascript
// Status phải là 0, 1, hoặc 2
properties: {
    role: { 
        type: "integer", 
        enum: [0, 1, 2] 
    }
}
```

---

## 7. Import Collection

1. Mở Postman
2. Click **Import** 
3. Chọn file `FUNewsManagement.postman_collection.json`
4. Update `baseUrl` variable nếu cần
5. Run collection hoặc từng request

---

## 8. References

- [Postman Test Scripts](https://learning.postman.com/docs/writing-scripts/test-scripts/)
- [JSON Schema Validation in Postman](https://www.postman.com/postman/postman-api-monitoring-examples/documentation/pn2r7fb/example-02-json-schema-validation)
- [Chai Assertion Library](https://www.chaijs.com/api/bdd/)
