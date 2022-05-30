# Greggs.Products
## Introduction
Hello and welcome to the Greggs Products repository, thanks for finding it!

## The Solution
So at the moment the api is currently returning a random selection from a fixed set of Greggs products directly 
from the controller itself. We currently have a data access class and it's interface but 
it's not plugged in (please ignore the class itself, we're pretending it hits a database),
we're also going to pretend that the data access functionality is fully tested so we don't need 
to worry about testing those lines of functionality.

We're mainly looking for the way you work, your code structure and how you would approach tackling the following 
scenarios.

## User Stories
Our product owners have asked us to implement the following stories, we'd like you to have 
a go at implementing them. You can use whatever patterns you're used to using or even better 
whatever patterns you would like to use to achieve the goal. Anyhow, back to the 
user stories:

### User Story 1
**As a** Greggs Fanatic<br/>
**I want to** be able to get the latest menu of products rather than the random static products it returns now<br/>
**So that** I get the most recently available products.

**Acceptance Criteria**<br/>
**Given** a previously implemented data access layer<br/>
**When** I hit a specified endpoint to get a list of products<br/>
**Then** a list or products is returned that uses the data access implementation rather than the static list it current utilises

### User Story 2
**As a** Greggs Entrepreneur<br/>
**I want to** get the price of the products returned to me in Euros<br/>
**So that** I can set up a shop in Europe as part of our expansion

**Acceptance Criteria**<br/>
**Given** an exchange rate of 1GBP to 1.11EUR<br/>
**When** I hit a specified endpoint to get a list of products<br/>
**Then** I will get the products and their price(s) returned
