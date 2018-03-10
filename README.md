Database MS SQL 
DB Purse. 
1. Create empty file Purse.sql;
2. Create DB PurseDB;
3. Add tables: CLIENT, USER, CURRENCY, PLAN, CATEGORY_SERVICE, SERVICE, FLIGHT, TEMPLATE_PLAN, TOTALLOG, SENTENCE


Server .NET
1. Create empty application PurseApi;
2. Create interface IEntity;
3. Create class: CategoryService, Client, Currenty, Flight, Plan, Sentence, ServiceData, TemplatePlan, TotalLog and UserData and implement IEntity;
4. Create GenericRepository and expand for each model Entities;
5. Create empty controllers: Client, Currency, Plan, TotalLog, Flight and Sentence;


Client Android
1. Create empty application PurseClient;