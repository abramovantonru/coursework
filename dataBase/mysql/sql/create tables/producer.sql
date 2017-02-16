CREATE TABLE `furnitureStore`.`producer` ( 
	`id` INT UNSIGNED NOT NULL AUTO_INCREMENT , 
	`name` VARCHAR(32) NOT NULL , 
	`guarantee` TIME(6) NULL , 

	PRIMARY KEY (`id`), 
	UNIQUE `producer_name` (`name`(32))
) ENGINE = InnoDB;