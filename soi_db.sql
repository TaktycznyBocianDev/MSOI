CREATE DATABASE soi_manager_db;
USE soi_manager_db;

CREATE TABLE workers(
id INT AUTO_INCREMENT PRIMARY KEY,
worker_name VARCHAR(50) NOT NULL,
worker_surname VARCHAR(50) NOT NULL,
position VARCHAR(50) NOT NULL,
employment_date DATE NOT NULL,
pesel CHAR(11) NOT NULL UNIQUE) ENGINE=InnoDB;

CREATE TABLE item_type(
id INT AUTO_INCREMENT PRIMARY KEY,
item_name VARCHAR(100) NOT NULL,
default_replacement_period INT NOT NULL,
item_description TEXT )engine = InnoDB;

CREATE TABLE item_release (
id INT AUTO_INCREMENT PRIMARY KEY,
worker_id INT NOT NULL,
item_type_id INT NOT NULL,
size VARCHAR(10),
color VARCHAR(50),
release_date DATE NOT NULL,
exchange_adate DATE NOT NULL,
FOREIGN KEY (worker_id) REFERENCES workers(id)
	ON DELETE RESTRICT ON UPDATE CASCADE,
FOREIGN KEY (item_type_id) REFERENCES item_type(id)
	ON DELETE RESTRICT ON UPDATE CASCADE) engine=InnoDB;
 

position VARCHAR(50) NOT NULL,
employment_date DATE NOT NULL,
 

INSERT INTO workers (worker_name, worker_surname, position, employment_date, pesel) VALUES
('Jan', 'Kowalski', 'Magazynier', '2020-01-02', '90010112345'),
('Anna', 'Nowak', 'Magazynier', '2020-01-02', '92050554321'),
('Piotr', 'Wiśniewski', 'Magazynier', '2020-01-02', '85031298765'),
('Katarzyna', 'Dąbrowska', 'Magazynier', '2020-01-02', '95071845678'),
('Michał', 'Wójcik', 'Magazynier', '2020-01-02', '88091078901');

INSERT INTO item_type (item_name, default_replacement_period, item_description) VALUES
('Kask', 365, 'Ochronny kask budowlany'),
('Buty robocze', 730, 'Buty z metalowym noskiem'),
('Kombinezon ochronny', 180, 'Odzież zabezpieczająca przed chemikaliami'),
('Rękawice ochronne', 90, 'Rękawice antypoślizgowe'),
('Okulary ochronne', 365, 'Okulary z filtrem UV');

INSERT INTO item_release (worker_id, item_type_id, size, color, release_date, exchange_adate) VALUES
(1, 1, 'L', 'Żółty', '2024-01-10', '2025-01-10'),
(2, 2, '42', 'Czarny', '2024-02-15', '2026-02-15'),
(3, 3, 'M', 'Niebieski', '2024-03-20', '2024-09-20'),
(4, 4, 'XL', 'Czerwony', '2024-04-25', '2024-07-25'),
(5, 5, '-', 'Przezroczysty', '2024-05-30', '2025-05-30');

INSERT INTO item_release (worker_id, item_type_id, size, color, release_date, exchange_adate) VALUES
(1, 4, '100', 'Czarne', '2024-06-10', '2025-06-10');


SELECT * FROM item_release;
SELECT * FROM item_type;
SELECT * FROM workers;
 
SELECT * FROM workers WHERE worker_surname = 'Nowak';

SELECT w.worker_name, w.worker_surname, it.*
FROM workers w
JOIN item_release ir ON w.id = ir.worker_id
JOIN item_type it ON ir.item_type_id = it.id;

SELECT w.worker_name, w.worker_surname, COUNT(ir.id) AS total_items
FROM workers w
JOIN item_release ir ON w.id = ir.worker_id
GROUP BY w.worker_name, w.worker_surname HAVING COUNT(ir.id) >= 2;

SELECT w.id, w.worker_name, w.worker_surname, it.item_name, COUNT(ir.id) OVER (PARTITION BY w.id) AS item_amount
FROM workers w
JOIN item_release ir ON w.id = ir.worker_id
JOIN item_type it ON ir.item_type_id = it.id
WHERE w.id IN (
    SELECT worker_id
    FROM item_release
    GROUP BY worker_id
    HAVING COUNT(*) >= 2
);

UPDATE workers 
SET worker_surname = 'Wojtas'
WHERE id = 1;

  
