# Greggs.Products
## Introduction
This is the solution for reading the products from data access layer.

## User Story 1 approach
**As a** Greggs Fanatic<br/>
**I want to** be able to get the latest menu of products rather than the random static products it returns now<br/>
**So that** I get the most recently available products.

**Acceptance Criteria**<br/>
**Given** a previously implemented data access layer<br/>
**When** I hit a specified endpoint to get a list of products<br/>
**Then** a list or products is returned that uses the data access implementation rather than the static list it current utilises

### Solution
- GET /product will return products from the data access layer.
- It accepts optional parameters pageStart and pageSize which are defaulted to 0 and 5 respectively.
- Added validations in the controller for any negative pageStart and pageSize
- Violation of the parametere will return Status 400
- Added Service layer That fetches the data from access layer.
- Originally data is returned as an array rather than Object it is not often a good practice.
- So the the list is wrapped within object with otehr metadata values like what is the pagesize and pageStart (because the params are optional ) and how many items are returned.
- If no values are fetched then Staus 204 (no content found) is return (For Eg, pageStart is 10 and pageSize is 10) 



### User Story 2
**As a** Greggs Entrepreneur<br/>
**I want to** get the price of the products returned to me in Euros<br/>
**So that** I can set up a shop in Europe as part of our expansion

**Acceptance Criteria**<br/>
**Given** an exchange rate of 1GBP to 1.11EUR<br/>
**When** I hit a specified endpoint to get a list of products<br/>
**Then** I will get the products and their price(s) returned

### Solution
- Same GET request is modified to handle user story 2
- Configuration is added for currencies and its conversion rate in appsettings.json.
- This configuration can be scaled to any currencies and currency has to be in ISO 4217 format
- This configuration is abstracted by an interface as ideally this configuration should be pulled from database.
- configuration is read only once and Stored in Memory cache.
- Because currency conversion is dynamic and changes regularly, A background service is created that refreshes the cache every 3 hours
- GET /product will accept another optional parameter called currency which is defaulted to GBP
- Validation is added in controller to check the currency, failure of which returns Status 400 (bad request)
- Service layers checks fetches the product and converts the amount to equivated currency.
- Response metadata also returns the currency listed for the products amount. 
