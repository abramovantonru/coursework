CREATE TABLE `furnitureStore`.`product` ( 
	`id` INT NOT NULL AUTO_INCREMENT , 
	`article` VARCHAR(32) NOT NULL , 
	`name` VARCHAR(64) NOT NULL , 
	`weight` REAL NOT NULL DEFAULT '0' , 
	`length` REAL NOT NULL DEFAULT '0' , 
	`width` REAL NOT NULL DEFAULT '0' , 
	`volume` REAL NOT NULL DEFAULT '0' , 
	`cost` REAL NOT NULL DEFAULT '0' , 
	`days_wait` INT(2) , 
	`stores` JSON NOT NULL , 
	`producer` INT UNSIGNED NOT NULL , 
	PRIMARY KEY (`id`)
) ENGINE = InnoDB;