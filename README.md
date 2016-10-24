# GroceryCo.Kiosk
A kiosk checkout system for GroceryCo allowing their customers to perform a "checkout" based on prices and promotions defined by the company.

### Design
1. The system has three category of domain models: product, promotion, and sale transaction.
	- A product can have multiple promotions.
	- Each promotion type derives from the abstract promotion class, and has it's own implementation of how an effective price is determined.
	- Sale transaction is used to represent input from user, as well as post-checkout data.
2. Repositories folder contains unit of work for data access. It uses a fake data context with sample product and promotion data.
3. Sales service has the checkout process implementation.
	- Uses the latest product prices and promotions
	- Sets each sale item's regular price (price at the time of sale)
	- If the product has promotion(s), uses the promotion which gives customer the best deal. Sets transaction item's promotion applied, if any.
	- Sale transaction class has method for calculating total, using either the effective or regular product prices.
4. Console client is used to demonstrate how the sale service can be used. 
	- KioskUtilities.CreateSaleTransaction() can be used to simulate sale sale transaction initiated by user.
5. Unit tests are in GroceryCo.Kiosk.Test project, covering the important calculations and functionality of the project.

## Limitations
1. System doesn't support products sold by weight/volume.
2. Combination of promotions per sale transaction item is not supported.
