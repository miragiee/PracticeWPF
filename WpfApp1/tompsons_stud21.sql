-- phpMyAdmin SQL Dump
-- version 4.9.7
-- https://www.phpmyadmin.net/
--
-- Хост: localhost
-- Время создания: Ноя 21 2025 г., 13:54
-- Версия сервера: 5.7.21-20-beget-5.7.21-20-1-log
-- Версия PHP: 5.6.40

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `tompsons_stud21`
--

-- --------------------------------------------------------

--
-- Структура таблицы `category`
--
-- Создание: Ноя 20 2025 г., 14:04
-- Последнее обновление: Ноя 20 2025 г., 14:04
--

DROP TABLE IF EXISTS `category`;
CREATE TABLE `category` (
  `ID` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `category`
--

INSERT INTO `category` (`ID`, `Name`) VALUES
(1, 'Мучные Изделия'),
(2, 'Фрукты'),
(3, 'Овощи'),
(4, 'Мясо'),
(5, 'Безалкогольные напитки'),
(6, 'Алкогольные напитки'),
(7, 'Готовые блюда'),
(8, 'Конфеты'),
(9, 'Туалетная Бумага');

-- --------------------------------------------------------

--
-- Структура таблицы `delivery`
--
-- Создание: Ноя 21 2025 г., 09:39
-- Последнее обновление: Ноя 21 2025 г., 09:39
--

DROP TABLE IF EXISTS `delivery`;
CREATE TABLE `delivery` (
  `ID` int(11) NOT NULL,
  `DilveryAddress` varchar(255) NOT NULL,
  `PickupAddress` varchar(255) NOT NULL,
  `DeliveryTime` time NOT NULL,
  `EmployeeID` int(11) NOT NULL,
  `OrderID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `delivery`
--

INSERT INTO `delivery` (`ID`, `DilveryAddress`, `PickupAddress`, `DeliveryTime`, `EmployeeID`, `OrderID`) VALUES
(1, 'Улица Пушкина, дом 20, квартира 10', 'Рижский проспект, 48', '01:00:00', 5, 1),
(2, 'Улица Мира, дом 1337, квартира 228', 'Рижский проспект, 48', '01:00:00', 5, 2),
(3, 'Проспект Ветеранов, дом 123, квартира 987', 'Рижский проспект, 48', '01:00:00', 5, 3),
(4, 'г. Тарков, улица Кильмова, дом 1, квартира 110', 'Рижский проспект, 48', '01:00:00', 5, 4),
(5, 'Улица Экскалибурная, дом 3, квартира 1', 'Рижский проспект, 48', '01:00:00', 5, 5);

-- --------------------------------------------------------

--
-- Структура таблицы `goods`
--
-- Создание: Ноя 18 2025 г., 07:21
--

DROP TABLE IF EXISTS `goods`;
CREATE TABLE `goods` (
  `ID` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Price` decimal(10,0) NOT NULL,
  `CategoryID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `goods`
--

INSERT INTO `goods` (`ID`, `Name`, `Price`, `CategoryID`) VALUES
(1, 'Лаваш', '60', 1),
(2, 'Хлеб Ржаной', '30', 1),
(3, 'Яблоки Зелёные', '80', 2),
(4, 'Апельсины', '90', 2),
(5, 'Картофель Молодой', '20', 3),
(6, 'Огурцы Короткоплодные', '40', 3),
(7, 'Куриная Грудка', '250', 4),
(8, 'Свинина Лопатка', '200', 4),
(9, 'Зева 4 рулона', '240', 9),
(10, 'Морковь', '20', 3),
(11, 'Помидор', '30', 3),
(12, 'Сникерс', '89', 8),
(13, 'Салат Цезарь', '180', 7),
(14, 'Пиво Светлое Нефильтрованное', '120', 6),
(15, 'Добрый Кола', '90', 5),
(16, 'Пряники', '100', 1);

-- --------------------------------------------------------

--
-- Структура таблицы `goods_accounting`
--
-- Создание: Ноя 18 2025 г., 07:21
--

DROP TABLE IF EXISTS `goods_accounting`;
CREATE TABLE `goods_accounting` (
  `ID` int(11) NOT NULL,
  `GoodsID` int(11) NOT NULL,
  `GoodsAmount` int(11) NOT NULL,
  `WarehouseID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `goods_accounting`
--

INSERT INTO `goods_accounting` (`ID`, `GoodsID`, `GoodsAmount`, `WarehouseID`) VALUES
(1, 1, 15, 1),
(2, 2, 30, 1),
(3, 3, 100, 2),
(4, 4, 70, 2),
(5, 5, 90, 2),
(6, 6, 72, 2),
(7, 7, 15, 1),
(8, 8, 24, 1),
(9, 9, 25, 1),
(10, 10, 70, 2),
(11, 11, 12, 1),
(12, 12, 15, 2),
(13, 13, 7, 1),
(14, 14, 40, 2),
(15, 15, 30, 2);

-- --------------------------------------------------------

--
-- Структура таблицы `orders`
--
-- Создание: Ноя 18 2025 г., 07:21
--

DROP TABLE IF EXISTS `orders`;
CREATE TABLE `orders` (
  `ID` int(11) NOT NULL,
  `ClientID` int(11) NOT NULL,
  `TotalCost` decimal(10,0) NOT NULL,
  `Delivery` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `orders`
--

INSERT INTO `orders` (`ID`, `ClientID`, `TotalCost`, `Delivery`) VALUES
(1, 1, '390', 1),
(2, 2, '900', 1),
(3, 3, '2730', 0),
(4, 4, '550', 0),
(5, 5, '620', 1);

-- --------------------------------------------------------

--
-- Структура таблицы `orders_goods`
--
-- Создание: Ноя 18 2025 г., 07:21
--

DROP TABLE IF EXISTS `orders_goods`;
CREATE TABLE `orders_goods` (
  `OrdersGoodsID` int(11) NOT NULL,
  `OrderID` int(11) NOT NULL,
  `GoodsID` int(11) NOT NULL,
  `Amount` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `orders_goods`
--

INSERT INTO `orders_goods` (`OrdersGoodsID`, `OrderID`, `GoodsID`, `Amount`) VALUES
(1, 1, 7, 1),
(2, 1, 1, 1),
(3, 1, 11, 2),
(4, 1, 5, 1),
(5, 2, 8, 4),
(6, 2, 5, 5),
(7, 3, 8, 5),
(8, 3, 14, 4),
(9, 3, 6, 10),
(10, 3, 2, 2),
(11, 3, 7, 3),
(12, 3, 5, 2),
(13, 4, 7, 1),
(14, 4, 3, 2),
(15, 4, 15, 1),
(16, 4, 10, 1),
(17, 4, 11, 1),
(18, 5, 14, 1),
(19, 5, 7, 2);

-- --------------------------------------------------------

--
-- Структура таблицы `order_employee`
--
-- Создание: Ноя 21 2025 г., 09:40
-- Последнее обновление: Ноя 21 2025 г., 09:40
--

DROP TABLE IF EXISTS `order_employee`;
CREATE TABLE `order_employee` (
  `OrderEmployeeID` int(11) NOT NULL,
  `orderID` int(11) NOT NULL,
  `EmployeeID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `order_employee`
--

INSERT INTO `order_employee` (`OrderEmployeeID`, `orderID`, `EmployeeID`) VALUES
(1, 1, 1),
(2, 2, 2),
(3, 3, 3),
(4, 4, 4),
(5, 5, 2),
(6, 1, 3);

-- --------------------------------------------------------

--
-- Структура таблицы `postlist`
--
-- Создание: Ноя 21 2025 г., 09:50
-- Последнее обновление: Ноя 21 2025 г., 09:51
--

DROP TABLE IF EXISTS `postlist`;
CREATE TABLE `postlist` (
  `ID` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Salary` decimal(10,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `postlist`
--

INSERT INTO `postlist` (`ID`, `Name`, `Salary`) VALUES
(1, 'Администратор', '60000'),
(2, 'Кассир', '30000'),
(3, 'Доставщик', '20000'),
(4, 'Пекарь', '50000');

-- --------------------------------------------------------

--
-- Структура таблицы `Role`
--
-- Создание: Ноя 21 2025 г., 10:02
--

DROP TABLE IF EXISTS `Role`;
CREATE TABLE `Role` (
  `RoleID` int(11) NOT NULL,
  `RoleName` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `Role`
--

INSERT INTO `Role` (`RoleID`, `RoleName`) VALUES
(1, 'Администратор'),
(2, 'Сотрудник'),
(3, 'Пользователь');

-- --------------------------------------------------------

--
-- Структура таблицы `suppliers`
--
-- Создание: Ноя 18 2025 г., 07:21
--

DROP TABLE IF EXISTS `suppliers`;
CREATE TABLE `suppliers` (
  `ID` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `PhoneNumber` varchar(20) NOT NULL,
  `EMail` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `suppliers`
--

INSERT INTO `suppliers` (`ID`, `Name`, `PhoneNumber`, `EMail`) VALUES
(1, 'ИП Корась', '+79567136469', 'koras.igor@gmail.com'),
(2, 'ООО \"Хлебопекарный завод\"', '88005551632', 'BestBreadOnEarthHumanityEverSeen@yandex.ru'),
(3, 'ООО \"Рога и копыта\"', '88007351525', 'EatOurMeat@yandex.ru'),
(4, 'ООО \"Винно-водочный завод\"', '88001756590', 'notOnlyAlcohol@gmail.com');

-- --------------------------------------------------------

--
-- Структура таблицы `suppliers_goods`
--
-- Создание: Ноя 18 2025 г., 07:21
--

DROP TABLE IF EXISTS `suppliers_goods`;
CREATE TABLE `suppliers_goods` (
  `SupplierGoodsID` int(11) NOT NULL,
  `SupplierID` int(11) NOT NULL,
  `GoodsID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `suppliers_goods`
--

INSERT INTO `suppliers_goods` (`SupplierGoodsID`, `SupplierID`, `GoodsID`) VALUES
(1, 1, 4),
(2, 2, 1),
(3, 2, 2),
(4, 1, 3),
(5, 1, 5),
(6, 1, 6),
(7, 3, 7),
(8, 3, 8),
(9, 1, 9),
(10, 1, 10),
(11, 1, 11),
(12, 4, 12),
(13, 4, 13),
(14, 4, 14),
(15, 4, 15);

-- --------------------------------------------------------

--
-- Структура таблицы `users`
--
-- Создание: Ноя 21 2025 г., 10:04
-- Последнее обновление: Ноя 21 2025 г., 10:04
--

DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `ID` int(11) NOT NULL,
  `RoleID` int(255) NOT NULL,
  `Login` varchar(255) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `LastName` varchar(255) NOT NULL,
  `Patronymic` varchar(255) NOT NULL,
  `PhoneNumber` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `BirthDate` date NOT NULL,
  `Address` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `users`
--

INSERT INTO `users` (`ID`, `RoleID`, `Login`, `Password`, `Name`, `LastName`, `Patronymic`, `PhoneNumber`, `Email`, `BirthDate`, `Address`) VALUES
(1, 1, 'Admin', '123456', 'Максим', 'Пупа', 'Анатольевич', '+79053655455', 'lupaMaxim@gmail.com', '2004-10-31', 'Улица Мемная, дом 67, квартира 228'),
(2, 2, 'BestBaker', '123456', 'Евгения', 'Биба', 'Михайловна', '895173463411', 'BobaFett@gmail.com', '2007-07-20', 'Улица Тарковская, дом 28, квартира 101'),
(3, 2, 'Cashier2', '123456', 'Галина', 'Стекольникова', 'Петровна', '89003513417', 'StekolnikovaGalina@yandex.ru', '1952-03-17', 'Улица Антоновская, дом 129, квартира 812'),
(4, 2, 'Cashier1', '123456', 'Василиса', 'Дурапова', 'Антоновна', '83557614616', 'Durapova@yandex.ru', '1958-01-15', 'Улица Командарма Белова, дом 89, кварти 97'),
(5, 2, 'DeliveryGuy', '123456', 'Максим', 'Мазепа', 'Михайлович', '+73334445566', 'MMMaxim@mail.ru', '2007-05-14', 'Улица Мавродиевская, дом 333, квартира 555'),
(6, 3, 'user228', '123456', 'Михаил', 'Михайлов', 'Олегович', '+78005553535', 'badMovieMaker@gmail.com', '2000-01-31', 'Улица Мира, дом 1337, квартира 228'),
(7, 3, 'megaUser', '123456', 'Максим', 'Пронкин', 'Михайлович', '+79527136469', 'medved@yandex.ru', '2007-07-10', 'Улица Пушкина, дом 20, квартира 10'),
(8, 3, 'BestUser', '123456', 'Великий', 'Владимир', 'Павлович', '+79136663412', 'megamanvovan@gmail.com', '2006-09-13', 'Проспект Ветеранов, дом 123, квартира 987'),
(9, 3, 'miniPizza', '123456', 'Буянов', 'Никита', 'Михайлович', '+79097776915', 'buyko.n@gmail.com', '1999-01-01', 'г. Тарков, улица Кильмова, дом 1, квартира 110'),
(10, 3, 'SayHi', '123456', 'Стив', 'Синклер', 'Варфреймович', '+79005003040', 'greatGameDeveloper@gmail.com', '1980-03-12', 'Улица Экскалибурная, дом 3, квартира 1');

-- --------------------------------------------------------

--
-- Структура таблицы `user_post`
--
-- Создание: Ноя 21 2025 г., 09:47
-- Последнее обновление: Ноя 21 2025 г., 09:47
--

DROP TABLE IF EXISTS `user_post`;
CREATE TABLE `user_post` (
  `UP_ID` int(11) NOT NULL,
  `UserID` int(11) NOT NULL,
  `PostID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `user_post`
--

INSERT INTO `user_post` (`UP_ID`, `UserID`, `PostID`) VALUES
(1, 1, 1),
(2, 2, 4),
(3, 3, 2),
(4, 4, 2),
(5, 5, 3);

-- --------------------------------------------------------

--
-- Структура таблицы `warehouse`
--
-- Создание: Ноя 18 2025 г., 07:21
--

DROP TABLE IF EXISTS `warehouse`;
CREATE TABLE `warehouse` (
  `ID` int(11) NOT NULL,
  `Address` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `warehouse`
--

INSERT INTO `warehouse` (`ID`, `Address`) VALUES
(1, 'Улица Сталеваров, строение 15'),
(2, 'Улица Чугунная, строение 207');

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `category`
--
ALTER TABLE `category`
  ADD PRIMARY KEY (`ID`);

--
-- Индексы таблицы `delivery`
--
ALTER TABLE `delivery`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `OrderID` (`OrderID`),
  ADD KEY `delivery_ibfk_1` (`EmployeeID`);

--
-- Индексы таблицы `goods`
--
ALTER TABLE `goods`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `CategoryID` (`CategoryID`);

--
-- Индексы таблицы `goods_accounting`
--
ALTER TABLE `goods_accounting`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `GoodsID` (`GoodsID`),
  ADD KEY `WarehouseID` (`WarehouseID`);

--
-- Индексы таблицы `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `ClientID` (`ClientID`);

--
-- Индексы таблицы `orders_goods`
--
ALTER TABLE `orders_goods`
  ADD PRIMARY KEY (`OrdersGoodsID`),
  ADD KEY `orders_goods_ibfk_1` (`OrderID`),
  ADD KEY `orders_goods_ibfk_2` (`GoodsID`);

--
-- Индексы таблицы `order_employee`
--
ALTER TABLE `order_employee`
  ADD PRIMARY KEY (`OrderEmployeeID`),
  ADD KEY `EmployeeID` (`EmployeeID`),
  ADD KEY `orderID` (`orderID`);

--
-- Индексы таблицы `postlist`
--
ALTER TABLE `postlist`
  ADD PRIMARY KEY (`ID`);

--
-- Индексы таблицы `Role`
--
ALTER TABLE `Role`
  ADD PRIMARY KEY (`RoleID`);

--
-- Индексы таблицы `suppliers`
--
ALTER TABLE `suppliers`
  ADD PRIMARY KEY (`ID`);

--
-- Индексы таблицы `suppliers_goods`
--
ALTER TABLE `suppliers_goods`
  ADD PRIMARY KEY (`SupplierGoodsID`),
  ADD KEY `GoodsID` (`GoodsID`),
  ADD KEY `SupplierID` (`SupplierID`);

--
-- Индексы таблицы `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `RoleID` (`RoleID`);

--
-- Индексы таблицы `user_post`
--
ALTER TABLE `user_post`
  ADD PRIMARY KEY (`UP_ID`),
  ADD KEY `UserID` (`UserID`),
  ADD KEY `PostID` (`PostID`);

--
-- Индексы таблицы `warehouse`
--
ALTER TABLE `warehouse`
  ADD PRIMARY KEY (`ID`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `category`
--
ALTER TABLE `category`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT для таблицы `delivery`
--
ALTER TABLE `delivery`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT для таблицы `goods`
--
ALTER TABLE `goods`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT для таблицы `goods_accounting`
--
ALTER TABLE `goods_accounting`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT для таблицы `orders`
--
ALTER TABLE `orders`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT для таблицы `orders_goods`
--
ALTER TABLE `orders_goods`
  MODIFY `OrdersGoodsID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- AUTO_INCREMENT для таблицы `order_employee`
--
ALTER TABLE `order_employee`
  MODIFY `OrderEmployeeID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT для таблицы `postlist`
--
ALTER TABLE `postlist`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT для таблицы `Role`
--
ALTER TABLE `Role`
  MODIFY `RoleID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT для таблицы `suppliers`
--
ALTER TABLE `suppliers`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT для таблицы `suppliers_goods`
--
ALTER TABLE `suppliers_goods`
  MODIFY `SupplierGoodsID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT для таблицы `users`
--
ALTER TABLE `users`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT для таблицы `user_post`
--
ALTER TABLE `user_post`
  MODIFY `UP_ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT для таблицы `warehouse`
--
ALTER TABLE `warehouse`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `delivery`
--
ALTER TABLE `delivery`
  ADD CONSTRAINT `delivery_ibfk_1` FOREIGN KEY (`EmployeeID`) REFERENCES `users` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `delivery_ibfk_2` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `goods`
--
ALTER TABLE `goods`
  ADD CONSTRAINT `goods_ibfk_1` FOREIGN KEY (`CategoryID`) REFERENCES `category` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `goods_accounting`
--
ALTER TABLE `goods_accounting`
  ADD CONSTRAINT `goods_accounting_ibfk_1` FOREIGN KEY (`GoodsID`) REFERENCES `goods` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `goods_accounting_ibfk_2` FOREIGN KEY (`WarehouseID`) REFERENCES `warehouse` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`ClientID`) REFERENCES `clients` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `orders_goods`
--
ALTER TABLE `orders_goods`
  ADD CONSTRAINT `orders_goods_ibfk_1` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `orders_goods_ibfk_2` FOREIGN KEY (`GoodsID`) REFERENCES `goods` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `order_employee`
--
ALTER TABLE `order_employee`
  ADD CONSTRAINT `order_employee_ibfk_1` FOREIGN KEY (`EmployeeID`) REFERENCES `users` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `order_employee_ibfk_2` FOREIGN KEY (`orderID`) REFERENCES `orders` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `suppliers_goods`
--
ALTER TABLE `suppliers_goods`
  ADD CONSTRAINT `suppliers_goods_ibfk_1` FOREIGN KEY (`GoodsID`) REFERENCES `goods` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `suppliers_goods_ibfk_2` FOREIGN KEY (`SupplierID`) REFERENCES `suppliers` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `users`
--
ALTER TABLE `users`
  ADD CONSTRAINT `users_ibfk_1` FOREIGN KEY (`RoleID`) REFERENCES `Role` (`RoleID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `user_post`
--
ALTER TABLE `user_post`
  ADD CONSTRAINT `user_post_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `users` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `user_post_ibfk_2` FOREIGN KEY (`PostID`) REFERENCES `postlist` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
