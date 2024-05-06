Paging
Get  https://localhost:5001/api/companies/{companyId}/employees?pageNumber=2&pageSize=2

Filters
Get https://localhost:5001/api/companies/{companyId}/employees?minAge=26&maxAge=29

Example:
https://localhost:5001/api/companies/C9D4C053-49B6-410C-BC78-2D54A9991870/employees?pageNumber=1&pageSize=4&minAge=32&maxAge=35


Searching
Get https://localhost:5001/api/companies/{companyId}/employees?searchTerm={name}

Example:
https://localhost:5001/api/companies/C9D4C053-49B6-410C-BC78-2D54A9991870/employees?pageNumber=1&pageSize=4&minAge=32&maxAge=35&searchTerm=MA


Sorting
Get https://localhost:5001/api/companies/{companyId}/employees?orderBy=name,age desc

Example:
https://localhost:5001/api/companies/C9D4C053-49B6-410C-BC78-2D54A9991870/employees?orderBy=name,age desc


Data Shaping
Get https://localhost:5001/api/companies/{companyId}/employees?fields=name,age

Example:
https://localhost:5001/api/companies/C9D4C053-49B6-410C-BC78-2D54A9991870/employees?fields=name,age

Accept:text/xml

HATEOAS
Get https://localhost:5001/api/companies/C9D4C053-49B6-410C-BC78-2D54A9991870/employees

Accept:application/CompanyEmployeesApi.hateoas+json
Accept:application/CompanyEmployeesApi.hateoas+xml

Root
Get https://localhost:5001/api

Accept:application/CompanyEmployeesApi.apiroot+json
Accept:application/CompanyEmployeesApi.apiroot+xml

Authentication
POST https://localhost:5001/api/authentication

  "firstname": "Jonh",
  "lastname": "Doe",
  "username": "JDoe",
  "password": "Password1000",
  "email": "johndoe@mail.com",
  "phonenumber": "589-654",
  "roles": [
    "Manager"
  ]

  "firstname": "Jane",
  "lastname": "Doe",
  "username": "JaneDoe",
  "password": "Password2000",
  "email": "janedoe@mail.com",
  "phonenumber": "583-653",
  "roles": [
    "Administrator"
  ]

Authentication/login
POST https://localhost:5001/api/authentication/login

"username": "JDoe",
"password": "Password1000"

Swagger
https://localhost:5001/swagger/index.html
https://localhost:5001/swagger/v1/swagger.json
