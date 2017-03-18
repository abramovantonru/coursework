-- phpMyAdmin SQL Dump
-- version 4.6.6
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Mar 18, 2017 at 03:50 AM
-- Server version: 5.7.17
-- PHP Version: 7.0.15

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `anton`
--

DELIMITER $$
--
-- Procedures
--
CREATE DEFINER=`anton`@`localhost` PROCEDURE `check.create` (IN `products` JSON, IN `store` INT(10) UNSIGNED)  MODIFIES SQL DATA
INSERT INTO `_check`
VALUES(
	NULL, 1, DEFAULT, products, store
)$$

CREATE DEFINER=`anton`@`localhost` PROCEDURE `check.delete` (IN `_id` INT(10) UNSIGNED)  MODIFIES SQL DATA
UPDATE `_check` SET active = 0 WHERE id = _id$$

CREATE DEFINER=`anton`@`localhost` PROCEDURE `producer.create` (IN `name` VARCHAR(32), IN `guarantee` INT(7))  MODIFIES SQL DATA
INSERT INTO `producer` 
VALUES
(
	NULL, 1, name, guarantee
)$$

CREATE DEFINER=`anton`@`localhost` PROCEDURE `producer.delete` (IN `_id` INT(10) UNSIGNED)  MODIFIES SQL DATA
UPDATE `producer` SET active = 0 WHERE id = _id$$

CREATE DEFINER=`anton`@`localhost` PROCEDURE `product.create` (IN `article` VARCHAR(32) CHARSET utf8, IN `name` VARCHAR(64) CHARSET utf8, IN `weight` DOUBLE, IN `length` DOUBLE, IN `width` DOUBLE, IN `volume` DOUBLE, IN `cost` DOUBLE, IN `days_wait` INT(2), IN `stores` JSON, IN `producer` INT UNSIGNED)  MODIFIES SQL DATA
INSERT INTO `product` 
VALUES
(
    NULL, 1, article, name, weight, length, width, 
    volume, cost, days_wait, stores, producer
)$$

CREATE DEFINER=`anton`@`localhost` PROCEDURE `product.delete` (IN `_id` INT(10) UNSIGNED)  MODIFIES SQL DATA
UPDATE `product` SET active = 0 WHERE id = _id$$

CREATE DEFINER=`anton`@`localhost` PROCEDURE `product.update` (IN `_article` VARCHAR(32) CHARSET utf8, IN `_name` VARCHAR(64) CHARSET utf8, IN `_weight` DOUBLE, IN `_length` DOUBLE, IN `_width` DOUBLE, IN `_volume` DOUBLE, IN `_cost` DOUBLE, IN `_days_wait` INT(2), IN `_stores` JSON, IN `_producer` INT(10) UNSIGNED, IN `_id` INT(10) UNSIGNED)  MODIFIES SQL DATA
UPDATE `product` 
SET
    article = _article,
    name = _name,
    weight = _weight,
    length = _length,
    width = _width, 
    volume = _volume,
    cost = _cost,
    days_wait = _days_wait,
    stores = _stores,
    producer = _producer
WHERE
	id = _id$$

CREATE DEFINER=`anton`@`localhost` PROCEDURE `store.create` (IN `name` VARCHAR(32) CHARSET utf8, IN `administrator` VARCHAR(64) CHARSET utf8, IN `address` TEXT CHARSET utf8, IN `phone` VARCHAR(15) CHARSET utf8, IN `start_time` TIME, IN `stop_time` TIME)  MODIFIES SQL DATA
INSERT INTO `store`
VALUES(
    NULL, 1, name, administrator, address,
    phone, start_time, stop_time
)$$

CREATE DEFINER=`anton`@`localhost` PROCEDURE `store.delete` (IN `_id` INT(10) UNSIGNED)  MODIFIES SQL DATA
UPDATE `store` SET active = 0 WHERE id = _id$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `producer`
--

CREATE TABLE `producer` (
  `id` int(11) UNSIGNED NOT NULL,
  `active` tinyint(1) NOT NULL DEFAULT '1',
  `name` varchar(32) NOT NULL,
  `guarantee` int(7) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `producer`
--

INSERT INTO `producer` (`id`, `active`, `name`, `guarantee`) VALUES
(1, 1, 'IKEA', NULL),
(3, 1, 'IKEA (2 года)', NULL),
(4, 1, 'Боровичи мебель', 180);

--
-- Triggers `producer`
--
DELIMITER $$
CREATE TRIGGER `before.producer.delete` BEFORE DELETE ON `producer` FOR EACH ROW BEGIN

SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Производителей нельзя удалять, только деактивировать. Используйте процедуру producer.delete.';

END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `product`
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
-- Dumping data for table `product`
--

INSERT INTO `product` (`id`, `active`, `article`, `name`, `weight`, `length`, `width`, `volume`, `cost`, `days_wait`, `stores`, `producer`) VALUES
(22, 1, '58c956cfb4179', 'Название name', 1, 1, 1, 1, 1235.5, 3, '[{\"1\": \"3\"}]', 1),
(23, 0, '58c956cfb4180', 'Фиолетовый стул', 3000, 400, 400, 2000, 450, 3, '2', 4),
(24, 1, '58cc6d6f6b803', 'Синий стул', 3000, 450, 300, 600, 2250, 7, '{\"1\": \"5\"}', 4),
(25, 1, '58cc6ddb183b1', 'Оранжевый стул', 3000, 450, 357, 677, 2500, 2, '{\"1\": 10}', 4),
(26, 1, '58cc6e3dda5e2', 'Зеленый стул', 2500, 500, 357, 677, 3000, 14, '[{\"1\": \"5\"}, {\"1\": \"5\"}]', 4),
(27, 1, '58cc71b311ebb', 'Желтый стул стул', 2500, 500, 357, 300, 1700, 14, '{\"1\": 10}', 3),
(28, 1, '58cc71dfb1d5c', 'Ярко эелтый стул', 2500, 300, 357, 300, 1800, 5, '{\"1\": 13}', 1),
(29, 1, '58cc735d3c95c', 'Журнальный столик, белый', 7500, 2000, 1000, 3000, 4599, 5, '{\"1\": \"9\", \"2\": 10}', 4),
(30, 1, '58cc73b23c4ca', 'Журнальный столик, стекло, круглый', 8500, 2000, 1000, 3000, 5999, 14, '[{\"1\": \"3\", \"2\": 6}]', 3),
(31, 1, '58cc741277a1f', 'Стол, красный дуб, форма ромба', 9500, 4000, 2000, 12000, 15999, 14, '[{\"1\": \"3\"}]', 4);

--
-- Triggers `product`
--
DELIMITER $$
CREATE TRIGGER `before.product.delete` BEFORE DELETE ON `product` FOR EACH ROW BEGIN

SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Товары нельзя удалять, только деактивировать. Используйте процедуру product.delete.';

END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `store`
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
-- Dumping data for table `store`
--

INSERT INTO `store` (`id`, `active`, `name`, `administrator`, `address`, `phone`, `start_time`, `stop_time`) VALUES
(1, 1, 'Синий Теремок', 'Иванов Эммануил Ильич', 'ул. Сержанта Елизарова, дом 79', '8(955)998-14-87', '10:00:00', '22:00:00'),
(2, 1, 'Зеленый Теремок', 'Максимова Клементина Филипповна ', 'г. Горячий Ключ, ул. Сталина, дом 79, корпус 2', '8(954)184-50-71', '09:00:00', '21:00:00');

--
-- Triggers `store`
--
DELIMITER $$
CREATE TRIGGER `before.store.delete` BEFORE DELETE ON `store` FOR EACH ROW BEGIN

SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Магазины нельзя удалять, только деактивировать. Используйте процедуру store.delete.';

END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `_check`
--

CREATE TABLE `_check` (
  `id` int(10) UNSIGNED NOT NULL,
  `active` tinyint(1) NOT NULL DEFAULT '1',
  `date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `products` json NOT NULL,
  `store` int(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `_check`
--

INSERT INTO `_check` (`id`, `active`, `date`, `products`, `store`) VALUES
(6, 1, '2017-03-17 01:25:05', '[{\"1\": 12}, {\"2\": 2}]', 1),
(7, 1, '2017-03-17 01:26:12', '[{\"count\": 3, \"product_id\": 1}, {\"count\": 4, \"product_id\": 2}]', 1);

--
-- Triggers `_check`
--
DELIMITER $$
CREATE TRIGGER `before.check.delete` BEFORE DELETE ON `_check` FOR EACH ROW BEGIN

SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Чеки нельзя удалять, только деактивировать. Используйте процедуру check.delete.';

END
$$
DELIMITER ;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `producer`
--
ALTER TABLE `producer`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `producer_name` (`name`);

--
-- Indexes for table `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`id`);
ALTER TABLE `product` ADD FULLTEXT KEY `products.search` (`article`,`name`);

--
-- Indexes for table `store`
--
ALTER TABLE `store`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `_check`
--
ALTER TABLE `_check`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `producer`
--
ALTER TABLE `producer`
  MODIFY `id` int(11) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
--
-- AUTO_INCREMENT for table `product`
--
ALTER TABLE `product`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=32;
--
-- AUTO_INCREMENT for table `store`
--
ALTER TABLE `store`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT for table `_check`
--
ALTER TABLE `_check`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
