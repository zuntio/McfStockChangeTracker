# McfStockChangeTracker

MyCashFlow does not provide UI for tracking changes in stock item quantities. So here you go, derpish CLI-tool for tracking those.

## Requirements

All items (products and variations) must have unique product code. Also, you need API credentials (refer to MCF's documentation).

You must also be able to use API. API is available if your plan is either Advanced, Pro or Enterprise.

## Restrictions

You can only search stock changes on one product or one variation at a time. Endpoint provides possibility to track stock changes of all products. For now I have decided to leave this feature out of this application since it would produce very high amount of requests. Your store might have hundreds of thousands or maybe millions of changes. We might not want to accidentaly fetch so much and put MCF API at high load.

## Disclaimer

This application is provided "as is". If you download binaries or compile this by yourself, you verify you have necessary knowledge to use this application responsibly. Bear in mind that API credentials are very powerful and in wrong hands, they can ruin your whole store faster than you can imagine. Protect your computer with high security measures and do not leave it unsupervised if this application is in use and/or API credentials are saved.

## Future of McfStockChangeTracker

Tracker will be updated to use Dependecy Injection. Also soon you will be able to provide list of trackable items in input-folder. Graphical User Interface is also under consideration.

## Getting started

After downloading/compiling, run the Executable. In the first run it will create you following folder and files:
* C:\Users\\\<youruser>\My Documents\StockChangeTracker\
* C:\Users\\\<youruser>\My Documents\StockChangeTracker\config\
* C:\Users\\\<youruser>\My Documents\StockChangeTracker\config\credentials.ini
* C:\Users\\\<youruser>\My Documents\StockChangeTracker\config\users.ini
* C:\Users\\\<youruser>\My Documents\StockChangeTracker\input\
* C:\Users\\\<youruser>\My Documents\StockChangeTracker\output\

Close windows by pressing any key.

Then navigate to My Documents -> StockChangeTracker -> config and open credentials.ini. Add following line:
`storename:apiuser:apikey`

`storename` is your `storename.mycashflow.fi` subdomain.

`apiuser` is email address which was defined when creating API user.

`apikey` is key given to you when created API user. Frequent regeneration of apikey is encouraged for security reasons.

Save the file. Application is ready to use.

Optionally you can also define users. Find out user id from MCF User Administration page and on your computer navigate to My Documents -> StockChangeTracker -> config and open users.ini. Add lines in following format:
`userid:user_in_human_readable_format`

In example:

`1:Boss`

`2:The Warehouse Guy`

`3:Mike`

## Usage

Application will ask you unique product code of the stock item you are about to track. Provide either code of variation, product or product which has variations.

After that you will be prompted to give start and end date for the search. They are optional. If you want to provide them, give them in format `YYYY-MM-DD`. If you do not want to provide dates just hit enter.

Fetch will start and result will be saved as semicolon separated csv in following format:

`C:\Users\<youruser>\My Documents\StockChangeTracker\output\timestamp-productcode.csv`

