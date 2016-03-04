# _Shoe Store_
#### _A website that demonstrates many to many relationships in databases, 3/4_

#### _**By Simon Temple**_

## Description

_This is an application that someone create a list of Shoe Stores and a list of Shoe Brands. Once created, a user can then click on a store to view all available brands for sale at the store and add more shoes based on what is already available. The user can also click on brands and see what stores carry they are carried at, and can add more to the list._

## Setup/Installation Requirements

* Sql commands to create database are:
* sqlcmd -S "(localdb)\mssqllocaldb"
* CREATE DATABASE shoe_stores
* GO
* USE shoe_stores
* GO
* CREATE TABLE brands (id int identity(1,1), name VARCHAR(255))
* GO
* CREATE TABLE stores (id int identity(1,1), name VARCHAR(255))
* GO
* CREATE TABLE store_brand (id int identity(1,1), store_id INT, brand_id INT)
* GO
* quit
* Open Microsoft SQL Server Management studio
* find database shoe_stores, right click, select Tasks, click backup.
* right click shoe_stores, select Tasks, restore, database.
* name database shoe_stores_test



* Open windows power shell. Type 'cd Desktop' then 'git clone https://github.com/stemple87/shoe_store'
* Open Microsoft SQL Server Management studio
* Click on File then Open then File. locate shoe_stores.sql open the file and then click execute.
* Repeat with shoe_stores_test file
* Move into the shoe_store folder in your windows power shell.
* Run dnu restore, then dnx kestrel.
* In your browser type: localhost:5004


## Known Bugs

_None yet._

## Support and contact details

_You can contact me at simonctemple@live.com or at my github page https://github.com/stemple87/_
