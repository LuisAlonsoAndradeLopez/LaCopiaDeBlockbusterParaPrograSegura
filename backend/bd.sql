DROP DATABASE IF EXISTS LaCopiaDeBlockbusterParaPrograSegura;
CREATE DATABASE LaCopiaDeBlockbusterParaPrograSegura CHARACTER SET utf8mb4;
USE LaCopiaDeBlockbusterParaPrograSegura;

/*
CREATE USER usuarioparaprograsegura@localhost IDENTIFIED BY 'LaPrograEsSegura@23052024';
GRANT ALL ON LaCopiaDeBlockbusterParaPrograSegura.* TO usuarioparaprograsegura@localhost;
GRANT SELECT, INSERT, UPDATE, DELETE ON LaCopiaDeBlockbusterParaPrograSegura.* TO usuarioparaprograsegura@localhost;
REVOKE ALL ON LaCopiaDeBlockbusterParaPrograSegura.* FROM usuarioparaprograsegura@localhost;
SHOW GRANTS FOR usuarioparaprograsegura@localhost;
*/

/*
DELIMITER $$

CREATE TRIGGER adjust_auto_increment_after_delete
AFTER DELETE ON movie
FOR EACH ROW
BEGIN
    DECLARE max_id INT;

    -- Find the current maximum id in the table
    SELECT IFNULL(MAX(id), 0) INTO max_id FROM movie;

    -- If the table is empty, reset auto_increment to 1, else set it to max_id + 1
    IF max_id IS NULL THEN
        SET max_id = 0;
    END IF;

    SET @new_auto_increment = max_id + 1;

    -- Alter the table to reset the auto-increment value
    SET @sql = CONCAT('ALTER TABLE users AUTO_INCREMENT = ', @new_auto_increment);
    PREPARE stmt FROM @sql;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
END$$

DELIMITER ;
*/