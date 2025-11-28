# API Агентства Недвижимости

Web API на .NET для управления агентством недвижимости: объекты недвижимости, клиенты (контрагенты), заявки на покупку/продажу и аналитика.

---

## Содержание

- [API Агентства Недвижимости](#api-агентства-недвижимости)
  - [Содержание](#содержание)
  - [Обзор](#обзор)
  - [Архитектура](#архитектура)
  - [База данных](#база-данных)
    - [Сущности](#сущности)
    - [**Counterparty**](#counterparty)
    - [**Property**](#property)
    - [**Application**](#application)
  - [Связи](#связи)
  - [Технологии](#технологии)

---

## Обзор

API позволяет управлять:

- **Контрагентами** (клиентами)  
- **Объектами недвижимости**  
- **Заявками** на покупку/продажу  
- **Аналитикой** (топ клиенты, минимальные заявки, заявки по типу недвижимости)  

Реализованы:

- Generic CRUD через контроллеры и сервисы  
- DTO маппинг с AutoMapper  
- Репозиторий с использованием EF Core  

---

## Архитектура

- **Контроллеры**
  - `CrudControllerBase<TEntityGetDto, TEntityCreateEditDto>` – базовый контроллер для CRUD  
  - `CounterpartiesController` – управление клиентами  
  - `PropertiesController` – управление объектами недвижимости, проверка типа и назначения  
  - `ApplicationsController` – управление заявками с проверкой типа и связанных сущностей  
  - `AnalyticsController` – аналитические эндпоинты  

- **Сервисы**
  - `CrudService<TEntity, TEntityGetDto, TEntityCreateEditDto>` – базовый сервис CRUD  
  - `CounterpartyService`, `PropertyService`, `ApplicationService` – сервисы доменной логики с дополнительной проверкой  
  - `AnalyticService` – аналитические запросы  

- **Репозитории**
  - `IRepository<T>` – интерфейс репозитория  
  - `Repository<T>` – реализация через EF Core  

- **Контекст базы данных**
  - `EstateAgencyDbContext` – DbSet для `Counterparty`, `Property`, `Application`  
  - Enum-to-string для типов, точность для decimal, каскадное удаление  

- **DTO**
  - `GetDto` и `CreateEditDto` для каждой сущности  
  - Специализированные DTO для аналитики: `TopCounterpartiesDto`, `PropertyTypeCountDto`, `ClientWithMinRequestDto`  

- **Mapping**
  - AutoMapper профиль `EstateAgencyMappingProfile` для конвертации сущностей и DTO  

---

## База данных

### Сущности

---

### **Counterparty**
Поля:

- `Id` – идентификатор  
- `FullName` – ФИО контрагента  
- `PassportNumber` – номер паспорта  
- `PhoneNumber` – номер телефона  

---

### **Property**
Поля:

- `Id`  
- `CadastralNumber` – кадастровый номер  
- `Type` – тип недвижимости (enum)  
- `Purpose` – назначение (enum)  
- `Address` – адрес  
- `TotalFloors` – количество этажей  
- `TotalArea` – общая площадь  
- `RoomCount` – количество комнат  
- `CeilingHeight` – высота потолков  
- `Floor` – этаж  
- `HasEncumbrances` – наличие обременений  

---

### **Application**
Поля:

- `Id`  
- `CounterpartyId` – ссылка на контрагента  
- `PropertyId` – ссылка на объект недвижимости  
- `Type` – тип заявки: Purchase / Sale  
- `TotalCost` – цена заявки  
- `CreatedAt` – дата создания  

---

## Связи

- Один **Counterparty** может иметь **много Application**  
- Один **Property** может иметь **много Application**  
- Каскадное удаление включено для обеих связей  

---

## Технологии

- .NET 8 Web API  
- Entity Framework Core  
- AutoMapper  
- xUnit для тестирования  
- Swagger / OpenAPI  

---
