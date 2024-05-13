# Capstone-Backend
Backendend Portion of the Capstone Project at UMKC

## Requirements
- Project uses .NET SDK 8.0
- Postgres Database

## Steps
- Setup Database Tables
- Add Data for Dynamic Transaction Types
- Setup Connection to Database
- Configuration for Authentication
- Start Backend

## Database Setup
```
CREATE SCHEMA public AUTHORIZATION pg_database_owner;

CREATE SEQUENCE public.accounts_accountid_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;-- public.accounts definition

CREATE TABLE public.accounts (
	accountid int4 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
	accountname varchar(25) NULL,
	accountowner varchar(64) NULL,
	balance numeric(10, 2) NULL,
	balmodificationdate timestamp NULL,
	modificationdate timestamp NULL,
	creationdate timestamp NULL,
	CONSTRAINT accounts_pkey PRIMARY KEY (accountid)
);

CREATE TABLE public.budgets (
	uid varchar(64) NOT NULL,
	budget json NULL,
	CONSTRAINT budgets_pkey PRIMARY KEY (uid)
);

CREATE TABLE public.transactions (
	transactionid int4 NOT NULL,
	accountid int4 NOT NULL,
	amount numeric(8, 2) NULL,
	transactiontype varchar(10) NULL,
	description varchar(80) NULL,
	modificationdate timestamp NULL,
	creationdate timestamp NULL,
	transactiondate date NULL,
	CONSTRAINT transactions_pkey PRIMARY KEY (transactionid, accountid)
);

CREATE TABLE public.transactiontypes (
	id int2 NOT NULL,
	code varchar(10) NULL,
	description varchar(25) NULL,
	CONSTRAINT transactiontypes_pkey PRIMARY KEY (id)
);

CREATE TABLE public.userprofiles (
	uid varchar(64) NOT NULL,
	defaultaccount int4 NULL,
	CONSTRAINT userprofiles_pkey PRIMARY KEY (uid)
);
```

## Dynamic Transaction Type Data
```
INSERT INTO public.transactiontypes (id,code,description) VALUES
	 (19003,'TRVLRENTL','Rentals'),
	 (19004,'TRVLVACA','Vacation'),
	 (1000,'AUTOALL','Auto & Transport'),
	 (1001,'AUTOINSURE','Auto Insurance'),
	 (1002,'AUTOLOAN','Auto Payment'),
	 (1003,'AUTOGAS','Gas & Fuel'),
	 (1004,'AUTOPARK','Parking'),
	 (1005,'AUTOPPLTRN','Public Transportation'),
	 (1006,'AUTORYDSHR','Ride Share'),
	 (1007,'AUTOMAINT','Service & Parts');
INSERT INTO public.transactiontypes (id,code,description) VALUES
	 (2000,'BILLALL','Bills & Utilities'),
	 (2001,'BILLHFONE','Home Phone'),
	 (2002,'BILLINTRNT','Internet'),
	 (2003,'BILLMFONE','Mobile Phone'),
	 (2004,'BILLTELE','Television'),
	 (2005,'BILLELCTRC','Electricity'),
	 (2006,'BILLWATER','Water & Sewer'),
	 (2007,'BILLTRASH','Trash Removal'),
	 (2008,'BILLUTIL','Utilities'),
	 (3000,'BUSYALL','Business');
INSERT INTO public.transactiontypes (id,code,description) VALUES
	 (3001,'BUSYADS','Advertising'),
	 (3002,'BUSYLGL','Legal'),
	 (3003,'BUSYOFCSPL','Office Supplies'),
	 (3004,'BUSYPRINT','Printing'),
	 (3005,'BUSYSHIP','Shipping'),
	 (4000,'EDUCALL','Education'),
	 (4001,'EDUCBOOK','Books'),
	 (4002,'EDUCLOAN','Student Loan'),
	 (4003,'EDUCCOST','Tuition'),
	 (5000,'NTANALL','Entertainment');
INSERT INTO public.transactiontypes (id,code,description) VALUES
	 (5001,'NTANART','Art'),
	 (5002,'NTANMOVIE','Movies'),
	 (5003,'NTANMUSIC','Music'),
	 (5004,'NTANNEWS','News'),
	 (5005,'NTANMAG','Magazines'),
	 (6000,'FEEALL','Fees & Charges'),
	 (6001,'FEEATM','ATM Fee'),
	 (6002,'FEEBANK','Bank Fee'),
	 (6003,'FEEPYMNT','Finance Charge'),
	 (6004,'FEELATE','Late Fee');
INSERT INTO public.transactiontypes (id,code,description) VALUES
	 (6005,'FEESERV','Service Fee'),
	 (6006,'FEETRADCOM','Trade Commissions'),
	 (7000,'FYNCLALL','Financial'),
	 (7001,'FYNCLADVZR','Financial Advisor'),
	 (7002,'FYNCLLFISR','Life Insurance'),
	 (8000,'FOODALL','Food & Dining'),
	 (8001,'FOODBAR','Bars & Alcohol'),
	 (8002,'FOODCFY','Coffee'),
	 (8003,'FOODFSTFUD','Fast Food'),
	 (8004,'FOODDLVRY','Food Delivery');
INSERT INTO public.transactiontypes (id,code,description) VALUES
	 (8005,'FOODGROCRY','Groceries'),
	 (8006,'FOODRSTRNT','Restaurants'),
	 (9000,'GIFTALL','Gifts'),
	 (9001,'GIFTCHRTY','Charity'),
	 (9002,'GIFTGIFT','Gift'),
	 (9003,'GIFTDONO','Donations'),
	 (10000,'HLTHALL','Health & Fitness'),
	 (10001,'HLTHTOOTH','Dentist'),
	 (10002,'HLTHDCTR','Doctor'),
	 (10003,'HLTHEYE','Eyecare');
INSERT INTO public.transactiontypes (id,code,description) VALUES
	 (10004,'HLTHGYM','Gym'),
	 (10005,'HLTHINSUR','Health Insurance'),
	 (10006,'HLTHPILL','Pharmacy'),
	 (10007,'HLTHSPORT','Sports'),
	 (11000,'HOMEALL','Home'),
	 (11001,'HOMENFM','Furnishings'),
	 (11002,'HOMERENO','Home Inprovement'),
	 (11003,'HOMEINSUR','Home Insurance'),
	 (11004,'HOMEMAINT','Home Maintance'),
	 (11005,'HOMESPLY','Home Supplies');
INSERT INTO public.transactiontypes (id,code,description) VALUES
	 (11006,'HOMEGRDN','Lawn & Garden'),
	 (11007,'HOMEMRGE','Mortgage'),
	 (11008,'HOMERENT','Rent'),
	 (12000,'NCOMALL','Income'),
	 (12001,'NCOMBONUS','Bonus'),
	 (12002,'NCOMINTRST','Interest'),
	 (12003,'NCOMPAY','Pay'),
	 (12004,'NCOMRFUND','Refund'),
	 (12005,'NCOMRENT','Rental'),
	 (13000,'KIDALL','Kids');
INSERT INTO public.transactiontypes (id,code,description) VALUES
	 (13001,'KIDALOW','Allowence'),
	 (13002,'KIDBABYSPY','Baby Supplies'),
	 (13003,'KIDWATCH','Babysitter & Daycare'),
	 (13004,'KIDSUPRT','Child Support'),
	 (13005,'KIDACTIVE','Activities'),
	 (13006,'KIDTOY','Toys'),
	 (14000,'HYGINALL','Hygiene'),
	 (14001,'HYGINHAIR','Hair'),
	 (14002,'HYGINCLOTH','Laundry'),
	 (14003,'HYGINSPA','Spa & Message');
INSERT INTO public.transactiontypes (id,code,description) VALUES
	 (15000,'PETALL','Pets'),
	 (15001,'PETFOOD','Pet Food & Supplies'),
	 (15002,'PETHAIR','Grooming'),
	 (15003,'PETVET','Vet'),
	 (16000,'SHOPALL','Shopping'),
	 (16001,'SHOPBOOK','Books'),
	 (16002,'SHOPCLOTH','Clothing'),
	 (16003,'SHOPDVICE','Electronics'),
	 (16004,'SHOPSFTWRE','Software'),
	 (16005,'SHOPGAME','Games');
INSERT INTO public.transactiontypes (id,code,description) VALUES
	 (16006,'SHOPHOBY','Hobbies'),
	 (16007,'SHOPSPORT','Sports'),
	 (17000,'TAXALL','Taxes'),
	 (17001,'TAXFEDS','Federal'),
	 (17002,'TAXSTATE','State'),
	 (17003,'TAXLCL','Local'),
	 (17004,'TAXPROP','Property'),
	 (17005,'TAXSALE','Sales'),
	 (18000,'TRNSFRALL','Transfer'),
	 (19000,'TRVLALL','Travel');
INSERT INTO public.transactiontypes (id,code,description) VALUES
	 (19001,'TRVLAIR','Airplane'),
	 (19002,'TRVLHTL','Hotel & Lodging');
```

## Database Configuration
- `dotnet user-secrets set "db-connection:Host" "<Host>"`
- `dotnet user-secrets set "db-connection:Port" "<Port>"`
- `dotnet user-secrets set "db-connection:Username" "<Username>"`
- `dotnet user-secrets set "db-connection:Password" "<Password>"`
- `dotnet user-secrets set "db-connection:Database" "<Database>"`

### Docker Setup
If using Docker to test application add the following to ./appsetting.json
rather than using user-secrets
```
"db-connection": {
  "Host": "localhost",
  "Port": "5440",
  "Username": "test",
  "Password": "test",
  "Database": "capstone"
}
```

## Authentication Configuration
To configure for authentication adjust the following in ./appsettings.json
for verifing JWT claims
```
  "JWT": {
    "issuer": "https://sso.ynlueke.com/application/o/capstone/",
    "audience": "{}",
    "authorization_endpoint": "https://sso.ynlueke.com/application/o/authorize/",
    "token_endpoint": "https://sso.ynlueke.com/application/o/token/"
  }
```

You must also add the following to .\appsettings.json
for JWT signature verification
```
"certificates": {
  "certificate": "{PEM Certificate}"
}
```

To disable Authentication comment out the following 2 lines in ./Startup.cs
```
if(_env.IsProduction())
    options.Filters.Add(new AuthorizeFilter());
```

## Starting the Backend
You can use the dotnet SDK to start the backend or docker
- To start with the sdk simply run the command `dotnet run`
- To use docker you must first build the image using `docker build -t backend-image -f Dockerfile.txt .` then start the container using `docker run -p 5180:5180 --name backend backend-image`