-- phpMyAdmin SQL Dump
-- version 4.6.6
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Mar 19, 2017 at 01:32 PM
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
BEGIN
    INSERT INTO `_check`
    VALUES(
        NULL, 1, DEFAULT, products, store
    );
END$$

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

CREATE DEFINER=`anton`@`localhost` PROCEDURE `producer.update` (IN `_name` VARCHAR(32), IN `_guarantee` INT(7), IN `_id` INT(10) UNSIGNED)  MODIFIES SQL DATA
UPDATE `producer` 
SET
    name = _name,
    guarantee = _guarantee
WHERE
	id = _id$$

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

CREATE DEFINER=`anton`@`localhost` PROCEDURE `product.update_counts` (IN `_stores` JSON, IN `_id` INT(10) UNSIGNED)  MODIFIES SQL DATA
UPDATE `product` 
SET
    stores = _stores
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

CREATE DEFINER=`anton`@`localhost` PROCEDURE `store.update` (IN `_name` VARCHAR(32) CHARSET utf8, IN `_administrator` VARCHAR(64) CHARSET utf8, IN `_address` TEXT CHARSET utf8, IN `_phone` VARCHAR(15) CHARSET utf8, IN `_start_time` TIME, IN `_stop_time` TIME, IN `_id` INT(10) UNSIGNED)  MODIFIES SQL DATA
UPDATE `store` 
SET
    name = _name,
    administrator = _administrator,
    address = _address,
    phone = _phone, 
    start_time = _start_time,
    stop_time = _stop_time
WHERE
	id = _id$$

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
(4, 1, 'Боровичи мебель(180 дней)', 180),
(5, 0, 'для удаления', 133),
(6, 0, 'для удаления2', NULL),
(7, 1, 'для удаления56565', NULL);

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
(22, 1, '58c956cfb4179', 'Стул из красного дуба', 4500, 300, 400, 1500, 7999, 30, '[{\"1\": \"6\", \"2\": \"5\"}]', 4),
(23, 0, '58c956cfb4180', 'Фиолетовый стул', 3000, 400, 400, 2000, 450, 3, '[{\"1\": \"4\", \"2\": \"3\"}]', 4),
(24, 1, '58cc6d6f6b803', 'Синий стул', 3000, 450, 300, 600, 2250, 7, '[{\"22\": \"5\"}]', 4),
(25, 1, '58cc6ddb183b1', 'Оранжевый стул', 3000, 450, 357, 677, 2500, 2, '[{\"1\": \"6\"}]', 4),
(26, 1, '58cc6e3dda5e2', 'Зеленый стул', 2500, 500, 357, 677, 3000, 14, '[{\"1\": \"5\"}, {\"1\": \"5\"}]', 4),
(27, 1, '58cc71b311ebb', 'Желтый стул', 2500, 500, 357, 300, 1700, 14, '[{\"1\": \"0\"}]', 3),
(28, 1, '58cc71dfb1d5c', 'Ярко эелтый стул', 2500, 300, 357, 300, 1800, 5, '[{\"1\": 13}]', 1),
(29, 1, '58cc735d3c95c', 'Журнальный столик, белый', 7500, 2000, 1000, 3000, 4599, 5, '[{\"1\": \"9\", \"2\": 10}]', 4),
(30, 1, '58cc73b23c4ca', 'Журнальный столик, стекло, круглый', 8500, 2000, 1000, 3000, 5999, 14, '[{\"1\": \"3\", \"2\": 6}]', 3),
(31, 1, '58cc741277a1f', 'Стол, красный дуб, форма ромба', 9500, 4000, 2000, 12000, 15999, 14, '[{\"1\": \"3\"}]', 4),
(32, 0, '58cd4e82ec953', '35345453', 5353, 5353, 535, 353, 535353, 535, '[{\"1\": \"0\"}]', 4),
(33, 1, '58ce4c0b0fda5', 'Диван из медвежей шкуры', 40000, 4000, 2000, 16000, 23450, 30, '[{\"1\": \"1\", \"3\": \"1\"}]', 4),
(34, 1, '58ce4d016ca61', 'Шкаф из слоновой кости', 75400, 5000, 3000, 45000, 135800, 60, '[{\"1\": \"1\"}]', 4);

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
(2, 1, 'Зеленый Теремок', 'Максимова Клементина Филипповна ', 'ул. Сталина, дом 79, корпус 2', '8(954)184-50-71', '09:00:00', '21:00:00'),
(3, 1, 'Желтый Теремок', 'Шершов Родион Эльдарович', 'ул. Бажова, дом 62, строение 150-2', '8(980)644-29-71', '10:00:00', '21:00:00'),
(4, 1, 'Красный Теремок', 'Русов Афиноген Русланович', 'ул. Авиационная, дом 66, строение 238', '8(972)535-16-68', '08:00:00', '19:00:00');

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
(8, 1, '2017-03-19 02:13:24', '[{\"22\": \"3\", \"23\": \"4\"}]', 1),
(9, 1, '2017-03-19 02:51:47', '[{\"22\": \"1\"}]', 1),
(10, 1, '2017-03-19 02:53:16', '[{\"22\": \"1\"}]', 1),
(11, 1, '2017-03-19 02:53:45', '[{\"22\": \"1\"}]', 1),
(12, 1, '2017-03-19 02:54:45', '[{\"22\": \"1\", \"23\": \"1\"}]', 1),
(13, 1, '2017-03-19 02:55:16', '[{\"22\": \"1\", \"23\": \"1\"}]', 1),
(14, 1, '2017-03-19 02:56:16', '[{\"22\": \"1\", \"23\": \"1\"}]', 1),
(15, 1, '2017-03-19 02:58:21', '[{\"22\": \"1\", \"23\": \"1\"}]', 1),
(16, 1, '2017-03-19 03:13:35', '[{\"22\": \"1\", \"23\": \"1\"}]', 1),
(17, 1, '2017-03-19 03:14:23', '[{\"22\": \"1\", \"23\": \"1\"}]', 1),
(18, 1, '2017-03-19 03:15:34', '[{\"22\": \"1\", \"23\": \"1\"}]', 1),
(19, 1, '2017-03-19 03:17:40', '[{\"22\": \"1\", \"23\": \"1\"}]', 1),
(20, 1, '2017-03-19 03:18:48', '[{\"22\": \"1\", \"23\": \"1\"}]', 1),
(21, 1, '2017-03-19 03:21:10', '[{\"22\": \"1\", \"23\": \"1\"}]', 1),
(22, 1, '2017-03-19 03:24:20', '[{\"22\": \"1\", \"23\": \"1\"}]', 1),
(23, 1, '2017-03-19 03:25:44', '[{\"22\": \"1\", \"23\": \"1\"}]', 1),
(24, 1, '2017-03-19 03:26:25', '[{\"22\": \"1\", \"23\": \"1\"}]', 1),
(25, 1, '2017-03-19 03:27:36', '[{\"22\": \"1\", \"23\": \"1\"}]', 1),
(26, 1, '2017-03-19 03:28:01', '[{\"22\": \"1\", \"23\": \"1\"}]', 1),
(27, 1, '2017-03-19 03:28:23', '[{\"22\": \"3\", \"23\": \"2\"}]', 1),
(28, 1, '2017-03-19 03:36:32', '[{\"22\": \"1\", \"23\": \"1\"}]', 1),
(29, 1, '2017-03-19 03:39:07', '[{\"22\": \"1\", \"25\": \"1\"}]', 1),
(30, 1, '2017-03-19 03:40:32', '[{\"22\": \"1\", \"25\": \"1\"}]', 1),
(31, 1, '2017-03-19 03:41:51', '[{\"22\": \"1\", \"25\": \"1\"}]', 1),
(32, 1, '2017-03-19 03:42:17', '[{\"22\": \"1\", \"25\": \"1\"}]', 1),
(33, 1, '2017-03-19 03:45:45', '[{\"22\": \"1\", \"25\": \"1\"}]', 1),
(34, 1, '2017-03-19 03:47:47', '[{\"22\": \"1\", \"25\": \"1\"}]', 1),
(35, 1, '2017-03-19 03:48:19', '[{\"22\": \"1\", \"25\": \"1\"}]', 1),
(36, 1, '2017-03-19 03:48:37', '[{\"22\": \"1\", \"25\": \"1\"}]', 1),
(37, 1, '2017-03-19 03:50:03', '[{\"22\": \"5\"}]', 2),
(38, 1, '2017-03-19 04:09:19', '[{\"22\": \"1\"}]', 1),
(39, 1, '2017-03-19 04:10:19', '[{\"22\": \"1\"}]', 1),
(40, 1, '2017-03-19 04:11:33', '[{\"22\": \"1\", \"25\": \"1\"}]', 1),
(41, 1, '2017-03-19 04:13:04', '[{\"22\": \"1\", \"25\": \"1\"}]', 1),
(42, 1, '2017-03-19 04:17:01', '[{\"22\": \"1\", \"25\": \"1\"}]', 1),
(43, 1, '2017-03-19 04:17:25', '[{\"22\": \"1\", \"25\": \"1\"}]', 1),
(44, 1, '2017-03-19 04:21:37', '[{\"22\": \"1\"}]', 1),
(45, 1, '2017-03-19 04:24:39', '[{\"22\": \"1\"}]', 1),
(46, 1, '2017-03-19 04:25:06', '[{\"22\": \"1\"}]', 1),
(47, 1, '2017-03-19 04:26:59', '[{\"22\": \"1\"}]', 1),
(48, 1, '2017-03-19 04:27:40', '[{\"22\": \"1\"}]', 1),
(49, 1, '2017-03-19 04:28:24', '[{\"22\": \"1\"}]', 1),
(50, 1, '2017-03-19 04:28:54', '[{\"22\": \"1\"}]', 1),
(51, 1, '2017-03-19 04:40:56', '[{\"22\": \"1\"}]', 1),
(52, 1, '2017-03-19 04:41:14', '[{\"22\": \"1\"}]', 1),
(53, 1, '2017-03-19 04:41:28', '[{\"22\": \"1\"}]', 1),
(54, 1, '2017-03-19 04:41:36', '[{\"22\": \"1\"}]', 1),
(55, 1, '2017-03-19 04:42:26', '[{\"22\": \"1\"}]', 1),
(56, 1, '2017-03-19 04:45:20', '[{\"22\": \"1\"}]', 1),
(57, 1, '2017-03-19 04:45:56', '[{\"22\": \"1\"}]', 1),
(58, 1, '2017-03-19 04:46:32', '[{\"22\": \"1\"}]', 1),
(59, 1, '2017-03-19 04:58:40', '[{\"22\": \"1\"}]', 1),
(60, 1, '2017-03-19 04:58:59', '[{\"22\": \"1\"}]', 1),
(61, 1, '2017-03-19 04:59:59', '[{\"22\": \"1\"}]', 1),
(62, 1, '2017-03-19 05:00:37', '[{\"22\": \"1\"}]', 1),
(63, 1, '2017-03-19 05:01:50', '[{\"22\": \"1\"}]', 1),
(64, 1, '2017-03-19 05:05:05', '[{\"22\": \"1\"}]', 1),
(65, 1, '2017-03-19 05:07:57', '[{\"22\": \"1\"}]', 1),
(66, 1, '2017-03-19 05:12:24', '[{\"22\": \"1\"}]', 1);

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
ALTER TABLE `producer` ADD FULLTEXT KEY `producer.search` (`name`);

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
ALTER TABLE `store` ADD FULLTEXT KEY `store.search` (`name`,`administrator`,`address`,`phone`);

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
  MODIFY `id` int(11) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;
--
-- AUTO_INCREMENT for table `product`
--
ALTER TABLE `product`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=35;
--
-- AUTO_INCREMENT for table `store`
--
ALTER TABLE `store`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;
--
-- AUTO_INCREMENT for table `_check`
--
ALTER TABLE `_check`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=67;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
