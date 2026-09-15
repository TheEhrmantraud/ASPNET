CREATE DATABASE shop_system;
GO

USE shop_system;
GO

-- Роли пользователей
CREATE TABLE Roles (
    ID_role INT PRIMARY KEY IDENTITY(1,1),
    role_name VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(200)
);
GO

-- Пользователи с хешированными паролями
CREATE TABLE Users (
    ID_user INT PRIMARY KEY IDENTITY(1,1),
    role_ID INT NOT NULL,
    FOREIGN KEY (role_ID) REFERENCES Roles(ID_role),
    login VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL, -- Хеш пароля (например, SHA-256, bcrypt)
    surname VARCHAR(40) NOT NULL,
    name_user VARCHAR(30) NOT NULL,
    middle_name VARCHAR(30),
    phone_number VARCHAR(11) NOT NULL UNIQUE,
    email VARCHAR(100) NOT NULL UNIQUE,
    registration_date DATETIME NOT NULL DEFAULT GETDATE(),
    is_active BIT NOT NULL DEFAULT 1
);
GO

-- Категории товаров
CREATE TABLE Categories (
    ID_category INT PRIMARY KEY IDENTITY(1,1),
    category_name VARCHAR(100) NOT NULL UNIQUE,
    description VARCHAR(500),
    parent_category_ID INT NULL, -- Для вложенных категорий
    FOREIGN KEY (parent_category_ID) REFERENCES Categories(ID_category)
);
GO

-- Товары/услуги
CREATE TABLE Products (
    ID_product INT PRIMARY KEY IDENTITY(1,1),
    category_ID INT NOT NULL,
    FOREIGN KEY (category_ID) REFERENCES Categories(ID_category),
    product_name VARCHAR(200) NOT NULL,
    description VARCHAR(1000),
    price DECIMAL(10, 2) NOT NULL CHECK(price >= 0),
    stock_quantity INT NOT NULL DEFAULT 0 CHECK(stock_quantity >= 0),
    is_available BIT NOT NULL DEFAULT 1,
    image_url VARCHAR(500),
    created_date DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Корзина пользователя
CREATE TABLE Carts (
    ID_cart INT PRIMARY KEY IDENTITY(1,1),
    user_ID INT NOT NULL UNIQUE, -- У каждого пользователя одна корзина
    FOREIGN KEY (user_ID) REFERENCES Users(ID_user),
    created_date DATETIME NOT NULL DEFAULT GETDATE(),
    updated_date DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Позиции в корзине
CREATE TABLE CartItems (
    ID_cart_item INT PRIMARY KEY IDENTITY(1,1),
    cart_ID INT NOT NULL,
    FOREIGN KEY (cart_ID) REFERENCES Carts(ID_cart),
    product_ID INT NOT NULL,
    FOREIGN KEY (product_ID) REFERENCES Products(ID_product),
    quantity INT NOT NULL CHECK(quantity > 0),
    added_date DATETIME NOT NULL DEFAULT GETDATE(),
    UNIQUE(cart_ID, product_ID) -- Один товар один раз в корзине
);
GO

-- Статусы заказов
CREATE TABLE OrderStatuses (
    ID_order_status INT PRIMARY KEY IDENTITY(1,1),
    status_name VARCHAR(50) NOT NULL UNIQUE
);
GO

-- Заказы/брони
CREATE TABLE Orders (
    ID_order INT PRIMARY KEY IDENTITY(1,1),
    user_ID INT NOT NULL,
    FOREIGN KEY (user_ID) REFERENCES Users(ID_user),
    order_status_ID INT NOT NULL,
    FOREIGN KEY (order_status_ID) REFERENCES OrderStatuses(ID_order_status),
    order_date DATETIME NOT NULL DEFAULT GETDATE(),
    total_amount DECIMAL(10, 2) NOT NULL CHECK(total_amount >= 0),
    delivery_address VARCHAR(500),
    payment_method VARCHAR(50),
    notes VARCHAR(1000)
);
GO

-- Позиции заказа
CREATE TABLE OrderItems (
    ID_order_item INT PRIMARY KEY IDENTITY(1,1),
    order_ID INT NOT NULL,
    FOREIGN KEY (order_ID) REFERENCES Orders(ID_order),
    product_ID INT NOT NULL,
    FOREIGN KEY (product_ID) REFERENCES Products(ID_product),
    quantity INT NOT NULL CHECK(quantity > 0),
    price_at_order DECIMAL(10, 2) NOT NULL CHECK(price_at_order >= 0), -- Цена на момент заказа
    subtotal AS (quantity * price_at_order) PERSISTED
);
GO














-- ============================================
-- ЗАПОЛНЕНИЕ ТЕСТОВЫМИ ДАННЫМИ
-- ============================================

-- Роли
INSERT INTO Roles(role_name, description)
VALUES
    ('Администратор', 'Полный доступ к системе'),
    ('Менеджер', 'Управление заказами и товарами'),
    ('Клиент', 'Обычный покупатель'),
    ('VIP-клиент', 'Привилегированный покупатель');
GO

-- Пользователи (пароль = хеш от 'password123')
INSERT INTO Users(role_ID, login, password_hash, surname, name_user, middle_name, phone_number, email)
VALUES
    (1, 'admin', '0x45FC200C0E5B0F0F0BB12CBAAFEABD1F', 'Админов', 'Админ', 'Adminovich', '89001111111', 'admin@shop.ru'),
    (2, 'manager1', '0x45FC200C0E5B0F0F0BB12CBAAFEABD1F', 'Петров', 'Петр', 'Петрович', '89002222222', 'manager@shop.ru'),
    (3, 'ivan_petrov', '0x45FC200C0E5B0F0F0BB12CBAAFEABD1F', 'Петров', 'Иван', 'Сергеевич', '89125550101', 'ivan@mail.ru'),
    (3, 'anna_s', '0x45FC200C0E5B0F0F0BB12CBAAFEABD1F', 'Сидорова', 'Анна', 'Владимировна', '89125550102', 'anna@mail.ru'),
    (4, 'vip_client', '0x45FC200C0E5B0F0F0BB12CBAAFEABD1F', 'Богатов', 'Владимир', 'Александрович', '89125550103', 'vip@mail.ru');
GO

-- Категории
INSERT INTO Categories(category_name, description, parent_category_ID)
VALUES
    ('Спортивное оборудование', 'Товары для спорта и фитнеса', NULL),
    ('Тренажеры', 'Силовые и кардио тренажеры', 1),
    ('Аксессуары', 'Спортивные аксессуары', 1),
    ('Абонементы', 'Абонементы в спортзал', NULL),
    ('Услуги тренеров', 'Персональные тренировки', NULL);
GO

-- Товары
INSERT INTO Products(category_ID, product_name, description, price, stock_quantity, is_available)
VALUES
    (2, 'Беговая дорожка Premium', 'Профессиональная беговая дорожка', 85000.00, 5, 1),
    (2, 'Велотренажер', 'Велотренажер с программами', 35000.00, 10, 1),
    (3, 'Гантели 5 кг (пара)', 'Неопреновое покрытие', 1500.00, 50, 1),
    (3, 'Коврик для йоги', 'Противоскользящий', 1200.00, 100, 1),
    (4, 'Месячный абонемент', 'Безлимитное посещение', 3000.00, 999, 1),
    (4, 'Годовой абонемент VIP', 'VIP зал + бассейн', 50000.00, 999, 1),
    (5, 'Персональная тренировка', 'Занятие с тренером 60 мин', 2000.00, 999, 1);
GO

-- Корзины для пользователей
INSERT INTO Carts(user_ID)
VALUES
    (3), -- ivan_petrov
    (4), -- anna_s
    (5); -- vip_client
GO

-- Позиции в корзинах
INSERT INTO CartItems(cart_ID, product_ID, quantity)
VALUES
    (1, 3, 2), -- ivan_petrov: 2 пары гантелей
    (1, 4, 1), -- ivan_petrov: 1 коврик
    (2, 5, 1), -- anna_s: месячный абонемент
    (3, 6, 1), -- vip_client: годовой VIP
    (3, 7, 10); -- vip_client: 10 персональных тренировок
GO

-- Статусы заказов
INSERT INTO OrderStatuses(status_name)
VALUES
    ('Новый'),
    ('В обработке'),
    ('Оплачен'),
    ('Доставляется'),
    ('Выполнен'),
    ('Отменен');
GO

-- Заказы
INSERT INTO Orders(user_ID, order_status_ID, total_amount, delivery_address, payment_method, notes)
VALUES
    (3, 3, 4200.00, 'Москва, ул. Ленина, д.10, кв.5', 'Карта', 'Доставка после 18:00'),
    (4, 5, 3000.00, NULL, 'Наличные', 'Абонемент получен'),
    (5, 2, 70000.00, NULL, 'Карта', 'VIP клиент');
GO

-- Позиции заказов
INSERT INTO OrderItems(order_ID, product_ID, quantity, price_at_order)
VALUES
    -- Заказ 1 (ivan_petrov)
    (1, 3, 2, 1500.00),
    (1, 4, 1, 1200.00),
    -- Заказ 2 (anna_s)
    (2, 5, 1, 3000.00),
    -- Заказ 3 (vip_client)
    (3, 6, 1, 50000.00),
    (3, 7, 10, 2000.00);
GO







-- Проверочные запросы
SELECT * FROM Roles;
SELECT * FROM Users;
SELECT * FROM Categories;
SELECT * FROM Products;
SELECT * FROM vw_CartDetails;
SELECT * FROM vw_OrderDetails;
GO