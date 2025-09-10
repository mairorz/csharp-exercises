drop database if exists api_db;
create database api_db;

use api_db;

SHOW TABLES;
-- Usuarios del sistema
create table users (
  id int auto_increment,
  name varchar(60) not null,
  surname varchar(60) not null,
  password_hash varchar(255) not null,
  phone varchar(20) not null,
  email varchar(120) not null unique,
  role enum('owner', 'support') default 'owner',
  status enum('active', 'inactive') not null default 'active',
  created_at datetime not null,
  updated_at datetime not null,
  primary key(id)
);

-- Clientes (empresas)
create table clients (
  id int auto_increment,
  user_id int not null,
  name varchar(100) not null unique,
  contact_phone varchar(20) not null,
  contact_email varchar(120) not null unique,
  status enum('active', 'inactive') not null default 'active',
  created_at datetime not null,
  updated_at datetime not null,
  primary key(id),
  constraint fk_clients_user foreign key (user_id) references users(id)
);

-- Servicios
create table services (
  id int auto_increment,
  name varchar(120) not null,
  description varchar(255) null,
  price decimal(12, 4) not null,
  status enum('active', 'inactive') not null default 'active',
  created_at datetime not null,
  updated_at datetime not null,
  primary key(id)
);

-- Facturas
create table invoices (
  id int auto_increment,
  client_id int not null,
  user_id int not null,
  issue_date date not null,
  paid_date datetime not null,
  created_at datetime not null,
  status enum('draft', 'issued', 'paid', 'overdue') not null default 'draft',
  primary key(id),
  constraint fk_invoices_user foreign key (user_id) references users(id),
  constraint fk_invoices_client foreign key (client_id) references clients(id)
);

-- Detalle de factura
create table invoice_detail (
  id int auto_increment,
  invoice_id int not null,
  service_id int not null,
  description varchar(255) null,
  quantity decimal(12, 3) not null,
  unit_price decimal(12, 4) not null,
  primary key(id),
  constraint fk_detail_invoice foreign key (invoice_id) references invoices(id),
  constraint fk_detail_service foreign key (service_id) references services(id)
);

-- Proveedores
create table suppliers (
  id int auto_increment,
  name varchar(120) not null,
  description varchar(255) null,
  phone varchar(30) not null,
  email varchar(120) not null,
  primary key(id)
);

-- Categorías de insumos
create table inputs_categories (
  id int auto_increment,
  name varchar(80) not null unique,
  primary key(id)
);

-- Insumos
create table inputs (
  id int auto_increment,
  category_id int not null,
  name varchar(120) not null,
  unit varchar(20) not null,
  primary key(id),
  constraint fk_inputs_categories foreign key (category_id) references inputs_categories(id)
);

-- Compras para el inventario de la empresa(Compras a proveedores)
create table purchases (
  id int auto_increment,
  supplier_id int not null,
  input_id int not null,
  name varchar(120) not null,
  description varchar(255) not null,
  quantity decimal(14, 4) not null,
  unit_cost decimal(12, 2) not null,
  purchased_date date not null,
  primary key(id),
  constraint fk_purchases_suppliers foreign key (supplier_id) references suppliers(id),
  constraint fk_purchases_inputs foreign key (input_id) references inputs(id)
);

-- Órdenes de trabajo
create table work_orders (
  id int auto_increment,
  client_id int not null,
  service_id int not null,
  notes varchar(255),
  scheduled_date datetime not null,
  status enum('scheduled','done','cancelled') default 'scheduled',
  primary key(id),
  constraint fk_work_orders_clients foreign key (client_id) references clients(id),
  constraint fk_work_orders_services foreign key (service_id) references services(id)
);

-- Consumos del inventario
create table consumptions (
  id int auto_increment primary key,
  work_order_id int not null,
  input_id int not null,
  quantity decimal(14, 4) not null,
  consumed_date datetime not null,
  constraint fk_consumptions_work_orders foreign key (work_order_id) references work_orders(id),
  constraint fk_consumptions_inputs foreign key (input_id) references inputs(id)
);

-- Inserts de las tablas
-- Usuarios
insert into users (name, surname, password_hash, phone, email, role, created_at, updated_at) values
('juan', 'perez', 'hash1', '5551-0001', 'juan@empresa.com', 'owner', '2025-09-04 10:00:00', '2025-09-04 10:00:00'),
('maria', 'lopez', 'hash2', '5552-0002', 'maria@empresa.com', 'support', '2025-09-04 10:05:00', '2025-09-04 10:05:00'),
('carlos', 'gomez', 'hash3', '5553-0003', 'carlos@empresa.com', 'owner', '2025-09-04 10:10:00', '2025-09-04 10:10:00'),
('ana', 'ruiz', 'hash4', '5554-0004', 'ana@empresa.com', 'owner', '2025-09-04 10:15:00', '2025-09-04 10:15:00'),
('luis', 'torres', 'hash5', '5555-0005', 'luis@empresa.com', 'support', '2025-09-04 10:20:00', '2025-09-04 10:20:00');

-- Clientes
insert into clients (user_id, name, contact_phone, contact_email, created_at, updated_at) values
(1, 'empresa alpha', '5021-1111', 'contacto@alpha.com', '2025-09-04 11:00:00','2025-09-04 11:00:00'),
(2, 'empresa beta', '5022-2222', 'contacto@beta.com', '2025-09-04 11:05:00','2025-09-04 11:05:00'),
(3, 'empresa gamma', '5023-3333', 'contacto@gamma.com', '2025-09-04 11:10:00','2025-09-04 11:10:00'),
(4, 'empresa delta', '5024-4444', 'contacto@delta.com', '2025-09-04 11:15:00','2025-09-04 11:15:00'),
(5, 'empresa epsilon', '5025-5555', 'contacto@epsilon.com', '2025-09-04 11:20:00','2025-09-04 11:20:00');

-- Servicios
insert into services (name, description, price, created_at, updated_at) values
('agua potable', 'suministro de agua a granel', 2000.00, '2025-09-04 12:00:00', '2025-09-04 12:00:00'),
('internet empresarial', 'conectividad dedicada', 5000.00, '2025-09-04 12:05:00', '2025-09-04 12:05:00'),
('mantenimiento electrico', 'revisión y reparación básica', 7500.00, '2025-09-04 12:10:00', '2025-09-04 12:10:00'),
('madera tratada', 'venta por m3', 12000.00, '2025-09-04 12:15:00', '2025-09-04 12:15:00'),
('limpieza industrial', 'jornada de limpieza profunda', 1000.00, '2025-09-04 12:20:00', '2025-09-04 12:20:00');

-- Facturas
insert into invoices (client_id, user_id, issue_date, paid_date, created_at, status) values
(1, 1, '2025-09-04', '2025-02-28 13:00:00', '2025-09-04 13:00:00', 'issued'),
(2, 2, '2025-09-03', '2025-09-03 18:30:00', '2025-09-03 12:30:00', 'paid'),
(3, 3, '2025-09-02', '2025-04-30 14:00:00', '2025-09-02 09:15:00', 'issued'),
(4, 4, '2025-09-01', '2025-09-01 15:00:00', '2025-09-01 10:00:00', 'draft'),
(5, 5, '2025-08-31', '2025-06-30 16:00:00', '2025-08-31 11:00:00', 'paid');


-- Detalle de factura (5 líneas en total, repartidas en varias facturas)
insert into invoice_detail (invoice_id, service_id, description, quantity, unit_price) values
(1, 1, 'agua potable 10,000 l', 10.00, 2000.00),
(1, 2, 'internet empresarial mensual', 2.00, 5000.00),
(2, 3, 'mantenimiento eléctrico básico', 4.00, 7500.00),
(3, 4, 'madera tratada m3', 3.00, 12000.00),
(4, 5, 'limpieza industrial', 3.00, 1000.00);

-- Proveedores
insert into suppliers (name, description, phone, email) values
('aguas sa', 'proveedor de agua', '5556-1001', 'ventas@aguassa.com'),
('combustibles norte', 'proveedor de diésel', '5557-1002', 'facturacion@cdn.com'),
('quimicos gt', 'proveedor de cloro', '5558-1003', 'ventas@quimicosgt.com'),
('ferreteria central', 'materiales y tornillería', '5559-1004', 'info@ferrecentral.com'),
('equipos aliados', 'equipos y epp', '5550-1005', 'contacto@equiposaliados.com');

-- Categorías de insumos
insert into inputs_categories (name) values
('agua'),
('combustible'),
('quimicos'),
('materiales'),
('equipos');

-- Insumos
insert into inputs (category_id, name, unit) values
(1, 'agua a granel', 'l'),
(2, 'diesel', 'l'),
(3, 'cloro', 'l'),
(4, 'tornillos', 'unidad'),
(5, 'guantes', 'par');

-- Compras (entradas a inventario) ligadas al proveedor
insert into purchases (supplier_id, input_id, name, description, quantity, unit_cost, purchased_date) values
(1, 1, 'compra agua inicial', '5000 l de agua a granel', 5000.00, 0.15, '2025-09-01'),
(2, 2, 'compra diésel', '300 l para transporte', 300.00, 8.00, '2025-09-02'),
(3, 3, 'compra cloro', '100 l para potabilizar', 100.00, 5.50, '2025-09-03'),
(4, 4, 'compra tornillos', '1000 unidades para mantenimiento', 1000.00, 0.25, '2025-09-03'),
(5, 5, 'compra guantes', '200 pares epp', 200.00, 3.00, '2025-09-03');


-- Órdenes de trabajo
insert into work_orders (client_id, service_id, notes, scheduled_date) values
(1, 1, 'entrega de agua', '2025-09-04 16:00:00'),
(2, 2, 'instalación de enlace', '2025-09-05 09:00:00'),
(3, 3, 'revisión de tableros', '2025-09-05 14:00:00'),
(4, 4, 'despacho de madera', '2025-09-06 10:00:00'),
(5, 5, 'jornada de limpieza', '2025-09-06 15:00:00');

-- Consumos (salidas de inventario) por orden de trabajo
insert into consumptions (work_order_id, input_id, quantity, consumed_date) values
(1, 1, 4500.00, '2025-09-04 17:30:00'),
(1, 2, 50.00, '2025-09-04 17:30:00'),
(1, 3, 10.00, '2025-09-04 17:30:00'),
(4, 4, 100.00, '2025-09-06 11:30:00'),
(5, 5, 20.00, '2025-09-06 17:00:00');

-- Selects sencillos para comprobar la funcionalidad de la base de datos
-- Usuarios
select id, name, surname, email, role from users;

-- Clientes con su encargado
select c.id, c.name as cliente, u.name as encargado
from clients c
join users u on u.id = c.user_id;

-- Facturas con cliente y usuario que la emitió
select i.id as factura, c.name as cliente, u.name as usuario, i.issue_date, i.status
from invoices i
join clients c on c.id = i.client_id
join users u on u.id = i.user_id
order by i.id;

-- Detalle de factura (qué servicios se cobraron)
select d.invoice_id, s.name as servicio, d.quantity, d.unit_price, (d.quantity * d.unit_price) as total
from invoice_detail d
join services s on s.id = d.service_id
order by d.invoice_id, d.id;

-- Compras con proveedor e insumo
select p.id, s.name as proveedor, i.name as insumo, p.quantity, p.unit_cost, p.purchased_date
from purchases p
join suppliers s on s.id = p.supplier_id
join inputs i on i.id = p.input_id
order by p.id;

-- Órdenes de trabajo con cliente y servicio
select w.id, c.name as cliente, s.name as servicio, w.scheduled_date, w.status
from work_orders w
join clients c on c.id = w.client_id
join services s on s.id = w.service_id
order by w.id;

-- Consumos por orden de trabajo (qué insumos se gastaron)
select c.work_order_id, i.name as insumo, c.quantity, c.consumed_date
from consumptions c
join inputs i on i.id = c.input_id
order by c.work_order_id, c.id;

-- Totales de entradas por insumo (stock de entrada)
select input_id, sum(quantity) as total_entradas
from purchases
group by input_id;

-- Totales de salidas por insumo (consumos)
select input_id, sum(quantity) as total_salidas
from consumptions
group by input_id;

-- Reportes acerca de la base de datos
select 'factura' as evento,
       i.created_at as fecha,
       i.id as ref_id,
       c.name as participante,
       'factura emitida' as detalle,
       null as cantidad,
       (select sum(d.quantity * d.unit_price)
          from invoice_detail d
         where d.invoice_id = i.id) as monto
from invoices i
join clients c on c.id = i.client_id

union all

select 'compra' as evento,
       p.purchased_date as fecha,
       p.id as ref_id,
       s.name as participante,
       p.name as detalle,
       p.quantity as cantidad,
       (p.quantity * p.unit_cost) as monto
from purchases p
join suppliers s on s.id = p.supplier_id

union all

select 'consumo' as evento,
       c.consumed_date as fecha,
       c.id as ref_id,
       cl.name as participante,
       i.name as detalle,
       c.quantity as cantidad,
       null as monto
from consumptions c
join work_orders w on w.id = c.work_order_id
join clients cl on cl.id = w.client_id
join inputs i on i.id = c.input_id

union all

select 'orden_trabajo' as evento,
       w.scheduled_date as fecha,
       w.id as ref_id,
       cl.name as participante,
       sv.name as detalle,
       null as cantidad,
       null as monto
from work_orders w
join clients cl on cl.id = w.client_id
join services sv on sv.id = w.service_id

order by fecha desc, evento;


