create database usersdb;

use usersdb;

CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    names VARCHAR(50),
    surnames VARCHAR(50),
    password VARCHAR(125)
);

INSERT INTO users (names, surnames, password)
VALUES
  ('Ana', 'López', '12345'),
  ('Luis', 'Martínez', 'abcde'),
  ('Marta', 'Gómez', 'qwerty');

select * from users;

SHOW TABLES;
