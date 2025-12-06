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
  - [Kafka](#kafka)
    - [Producer](#producer)
    - [Consumer](#consumer)
    - [Data Generator](#data-generator)
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
- Kafka интеграция: автоматическая генерация и обработка заявок  

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

- **Kafka**
  - `KafkaProducer` – background сервис, который периодически генерирует заявки и отправляет их в Kafka  
  - `KafkaConsumer` – background сервис, который потребляет сообщения из Kafka, десериализует их и сохраняет заявки в базу данных  
  - `Generator` – утилита для генерации фейковых данных `ApplicationCreateEditDto`  

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

## Kafka

### Producer
- Периодически генерирует фейковые заявки через `Generator`  
- Отправляет их в Kafka топик в JSON-формате

### Consumer
- Подписан на Kafka топик заявок  
- Десериализует JSON в `ApplicationCreateEditDto`  
- Сохраняет заявки в базу данных через сервис `ApplicationService`  
- Поддерживает повторные попытки десериализации и коммит сообщений  

### Data Generator
- `Generator` создает фейковые заявки (`ApplicationCreateEditDto`)  
- Случайно выбирает тип заявки (`Sale` или `Purchase`)  
- Генерирует случайные `CounterpartyId`, `PropertyId` и `TotalCost`  

---

## Технологии

- .NET 8 Web API  
- Entity Framework Core  
- AutoMapper  
- Confluent.Kafka для producer/consumer  
- Bogus для генерации данных  
- xUnit для тестирования  
- Swagger / OpenAPI  
