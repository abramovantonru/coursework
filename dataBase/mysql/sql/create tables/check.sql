CREATE TABLE `furnitureStore`.`check` ( 
	`id` INT UNSIGNED NOT NULL AUTO_INCREMENT , 
	`date` DATE NOT NULL , 
	`products` JSON NOT NULL , 
	`store` INT UNSIGNED NOT NULL , 

	PRIMARY KEY (`id`)
) ENGINE = InnoDB;

ALTER TABLE `check` 
	ADD FOREIGN KEY (`store`) 
	REFERENCES `store`(`id`) ON DELETE RESTRICT ON UPDATE RESTRICT;