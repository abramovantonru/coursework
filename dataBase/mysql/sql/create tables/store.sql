CREATE TABLE `furnitureStore`.`store` ( 
	`id` INT UNSIGNED NOT NULL AUTO_INCREMENT , 
	`name` VARCHAR(32) NOT NULL , 
	`administrator` VARCHAR(64) NOT NULL , 
	`address` TEXT NULL DEFAULT NULL , 
	`phone` VARCHAR(15) NULL DEFAULT NULL , 
	`start_time` TIME(6) NULL DEFAULT NULL , 
	`stop_time` TIME(6) NULL DEFAULT NULL , 

	PRIMARY KEY (`id`)
) ENGINE = InnoDB;