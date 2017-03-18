-- phpMyAdmin SQL Dump
-- version 4.6.6deb1+deb.cihar.com~xenial.2
-- https://www.phpmyadmin.net/
--
-- Хост: localhost
-- Время создания: Мар 17 2017 г., 12:25
-- Версия сервера: 5.7.17-0ubuntu0.16.04.1
-- Версия PHP: 7.0.15-0ubuntu0.16.04.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `furnitureStore`
--

DELIMITER $$
--
-- Процедуры
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `check.create` (IN `products` JSON, IN `store` INT(10) UNSIGNED)  MODIFIES SQL DATA
INSERT INTO `check`
VALUES(
	NULL, DEFAULT, products, store
)$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `check.delete` (IN `_id` INT(10) UNSIGNED)  MODIFIES SQL DATA
UPDATE `check` SET active = 0 WHERE id = _id$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `producer.create` (IN `name` VARCHAR(32), IN `guarantee` INT(7))  NO SQL
INSERT INTO `producer` 
VALUES
(
	NULL, name, guarantee
)$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `producer.delete` (IN `_id` INT(10) UNSIGNED)  MODIFIES SQL DATA
UPDATE `producer` SET active = 0 WHERE id = _id$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `product.create` (IN `article` VARCHAR(32) CHARSET utf8, IN `name` VARCHAR(64) CHARSET utf8, IN `weight` DOUBLE, IN `length` DOUBLE, IN `width` DOUBLE, IN `volume` DOUBLE, IN `cost` DOUBLE, IN `days_wait` INT(2), IN `stores` JSON, IN `producer` INT UNSIGNED)  MODIFIES SQL DATA
INSERT INTO `product` 
VALUES
(
	NULL, article, name, weight, length, width, 
     volume, cost, days_wait, stores, producer
)$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `product.delete` (IN `_id` INT(10) UNSIGNED)  MODIFIES SQL DATA
UPDATE `product` SET active = 0 WHERE id = _id$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `store.create` (IN `name` VARCHAR(32) CHARSET utf8, IN `administrator` VARCHAR(64) CHARSET utf8, IN `address` TEXT CHARSET utf8, IN `phone` VARCHAR(15) CHARSET utf8, IN `start_time` TIME, IN `stop_time` TIME)  MODIFIES SQL DATA
INSERT INTO `store`
VALUES(
	NULL, name, administrator, address,
    phone, start_time, stop_time
)$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `store.delete` (IN `_id` INT(10) UNSIGNED)  MODIFIES SQL DATA
UPDATE `store` SET active = 0 WHERE id = _id$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Структура таблицы `check`
--

CREATE TABLE `check` (
  `id` int(10) UNSIGNED NOT NULL,
  `active` tinyint(1) NOT NULL DEFAULT '1',
  `date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `products` json NOT NULL,
  `store` int(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `check`
--

INSERT INTO `check` (`id`, `active`, `date`, `products`, `store`) VALUES
(6, 1, '2017-03-17 01:25:05', '[{\"1\": 12}, {\"2\": 2}]', 1),
(7, 1, '2017-03-17 01:26:12', '[{\"count\": 3, \"product_id\": 1}, {\"count\": 4, \"product_id\": 2}]', 1);

--
-- Триггеры `check`
--
DELIMITER $$
CREATE TRIGGER `before.check.delete` BEFORE DELETE ON `check` FOR EACH ROW BEGIN

SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Чеки нельзя удалять, только деактивировать. Используйте процедуру check.delete.';

END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Структура таблицы `producer`
--

CREATE TABLE `producer` (
  `id` int(11) UNSIGNED NOT NULL,
  `active` tinyint(1) NOT NULL DEFAULT '1',
  `name` varchar(32) NOT NULL,
  `guarantee` int(7) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `producer`
--

INSERT INTO `producer` (`id`, `active`, `name`, `guarantee`) VALUES
(1, 1, 'IKEA', NULL),
(3, 1, 'IKEA (2 года)', NULL),
(4, 1, 'Боровичи мебель', 180);

--
-- Триггеры `producer`
--
DELIMITER $$
CREATE TRIGGER `before.producer.delete` BEFORE DELETE ON `producer` FOR EACH ROW BEGIN

SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Производителей нельзя удалять, только деактивировать. Используйте процедуру producer.delete.';

END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Структура таблицы `product`
--

CREATE TABLE `product` (
  `id` int(11) NOT NULL,
  `active` tinyint(1) NOT NULL DEFAULT '1',
  `article` varchar(32) DEFAULT NULL,
  `name` varchar(64) DEFAULT NULL,
  `weight` double NOT NULL DEFAULT '0',
  `length` double NOT NULL DEFAULT '0',
  `width` double NOT NULL DEFAULT '0',
  `volume` double NOT NULL DEFAULT '0',
  `cost` double NOT NULL DEFAULT '0',
  `days_wait` int(2) DEFAULT NULL,
  `stores` json NOT NULL,
  `producer` int(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `product`
--

INSERT INTO `product` (`id`, `active`, `article`, `name`, `weight`, `length`, `width`, `volume`, `cost`, `days_wait`, `stores`, `producer`) VALUES
(22, 0, '58c956cfb4179', 'Название name', 1, 1, 1, 1, 1235.5, 3, '[{\"1\": \"1\"}, {\"2\": \"2\"}]', 1);

--
-- Триггеры `product`
--
DELIMITER $$
CREATE TRIGGER `before.product.delete` BEFORE DELETE ON `product` FOR EACH ROW BEGIN

SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Товары нельзя удалять, только деактивировать. Используйте процедуру product.delete.';

END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Структура таблицы `store`
--

CREATE TABLE `store` (
  `id` int(10) UNSIGNED NOT NULL,
  `active` tinyint(1) NOT NULL DEFAULT '1',
  `name` varchar(32) NOT NULL,
  `administrator` varchar(64) NOT NULL,
  `address` text,
  `phone` varchar(15) DEFAULT NULL,
  `start_time` time DEFAULT NULL,
  `stop_time` time DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `store`
--

INSERT INTO `store` (`id`, `active`, `name`, `administrator`, `address`, `phone`, `start_time`, `stop_time`) VALUES
(1, 1, 'Синий Теремок', 'Абрамов Антон Андреевич', 'ул. Сержанта Елизарова, дом 79', '8(955)998-14-87', '10:00:00', '22:00:00');

--
-- Триггеры `store`
--
DELIMITER $$
CREATE TRIGGER `before.store.delete` BEFORE INSERT ON `store` FOR EACH ROW BEGIN

SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Магазины нельзя удалять, только деактивировать. Используйте процедуру store.delete.';

END
$$
DELIMITER ;

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `check`
--
ALTER TABLE `check`
  ADD PRIMARY KEY (`id`);

--
-- Индексы таблицы `producer`
--
ALTER TABLE `producer`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `producer_name` (`name`);

--
-- Индексы таблицы `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`id`);
ALTER TABLE `product` ADD FULLTEXT KEY `products.search` (`article`,`name`);

--
-- Индексы таблицы `store`
--
ALTER TABLE `store`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `check`
--
ALTER TABLE `check`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;
--
-- AUTO_INCREMENT для таблицы `producer`
--
ALTER TABLE `producer`
  MODIFY `id` int(11) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
--
-- AUTO_INCREMENT для таблицы `product`
--
ALTER TABLE `product`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;
--
-- AUTO_INCREMENT для таблицы `store`
--
ALTER TABLE `store`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
