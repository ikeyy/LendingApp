# LendingApp
**Prerequisites:**
1. Download Node.js thru this link https://nodejs.org/
2. Install Angular CLI thru Visual Studio Code Terminal then type the code below:
      * npm install -g @angular/cli
3. Import LenderAppData.bacpac file to the SQL database.
4. Import LendingApp.postman_collection.json to Postman.
        * Can be used for Adding Blacklist Domains or Mobile Number please see json samples below
        * Used for Adding Domain Blacklist
                * {
                        "Type":"Domain",
                        "Value": "gmail.com"
                   }
        * Used for Adding Mobile Blacklist
                {
                        "Type":"Mobile",
                        "Value": "09099099909"
                }
        * Also there is an API for adding Product Data like on the sample below.
            {
                "DisplayName": "Product A",
                "InterestRate": 0.0599,
                "InterestFreeMonths": "all",
                "MinTermMonths": 2,
                "MaxTermMonths": 60,
                "MinLoanAmount": 500.00,
                "MaxLoanAmount": 15000.00,
                "DefaultTermMonths": 2,
                "Description": "Interest-free for the entire loan period"
            }
        Note: For InterestFreeMonths you can use 'all' to set all interest free months
              or set a number instead if you only want specific months like for free first 2 months 
              set it to '2'. Both setup should be in string format.


5. Under the Angular app, change the apiURL value in the environment.development.ts based on localhost value on the .Net Core app. 
6. Under the .Net app, change the AllowedOrigins value in the appSettings.development.json based on localhost value on the Angular app. 
7. Align the server name under appSettings.development.json with the current machine SQL credentials.

**Instructions:**
1. To start the Angular App type ng-serve to the Visual Studio Terminal and follow the localhost link provided in the terminal.
2. Run the .Net Core App
3. Run the {baseUrl}/api/LoanRequest (Note: Please see the Postman collection/Swagger API for the API request) to get the redirection link for the loan application.
