Feature: Product
  As a user
  I want to retrieve products that are in Stock
  So that I can view available products for purchase
	
Scenario: Get Products
	Given Database contains products:
    | Id                                   | Name              | Description                         | Price | Stock |
    | b06f886f-d6d6-4d6b-ae4c-66879e298673 | CoolProduct       | Coolest product ever created        | 9.99  | 1     |
    | f0d8cb44-17fa-410d-9eb9-c092ae8ecf32 | SecondCoolProduct | Second Coolest product ever created | 30    | 0     |
    | fad592d0-e1c4-4a87-81e0-e60839ca2665 | ThirdCoolProduct  | Third Coolest product ever created  | 2.3   | 4     |
    | c2ab5934-4e6a-453d-845b-2c3af481d7cd | ForthCoolProduct  | Forth Coolest product ever created  | 3.2   | 0     |
	When A GET request is made to '/Product'
    Then The products should be retrieved successfully with status code 200
	And The result should be:
    | Id                                   | Name              | Price | Stock |
    | b06f886f-d6d6-4d6b-ae4c-66879e298673 | CoolProduct       | 9.99  | 1     |
    | f0d8cb44-17fa-410d-9eb9-c092ae8ecf32 | SecondCoolProduct | 30    | 0     |
    | fad592d0-e1c4-4a87-81e0-e60839ca2665 | ThirdCoolProduct  | 2.3   | 4     |
    | c2ab5934-4e6a-453d-845b-2c3af481d7cd | ForthCoolProduct  | 3.2   | 0     |


Scenario: Add a new product
    Given The database is empty
    When A POST request is made to '/Product' with the following data:
    | Id                                   | Name            | Description             | Price | Stock |
    | 123e4567-e89b-12d3-a456-426614174000 | NewProduct      | The newest product      | 15.99 | 10    |
    Then The product should be added successfully with status code 201
    And The product in the database should be:
    | Id                                   | Name            | Description             | Price | Stock |
    | 123e4567-e89b-12d3-a456-426614174000 | NewProduct      | The newest product      | 15.99 | 10    |


Scenario: Update an existing product
    Given Database contains products:
    | Id                                   | Name            | Description             | Price | Stock |
    | 123e4567-e89b-12d3-a456-426614174000 | OldProduct      | Some old product        | 10.00 | 5     |
    When A PUT request is made to '/Product/123e4567-e89b-12d3-a456-426614174000' with the following updated data:
    | Id                                   | Name            | Description             | Price | Stock |
    | 123e4567-e89b-12d3-a456-426614174000 | UpdatedProduct  | The product is updated  | 20.00 | 15    |
    Then The product should be updated successfully with status code 200
    And The product in the database should be:
    | Id                                   | Name            | Description             | Price | Stock |
    | 123e4567-e89b-12d3-a456-426614174000 | UpdatedProduct  | The product is updated  | 20.00 | 15    |


Scenario: Delete an existing product
    Given Database contains products:
    | Id                                   | Name            | Description             | Price | Stock |
    | 123e4567-e89b-12d3-a456-426614174000 | ProductToDelete | Some product to delete  | 5.00  | 2     |
    When A DELETE request is made to '/Product/123e4567-e89b-12d3-a456-426614174000'
    Then The product should be deleted successfully with status code 200
    And The product should no longer exist in the database
