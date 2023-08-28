-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jul 16, 2023 at 02:30 PM
-- Server version: 10.4.27-MariaDB
-- PHP Version: 8.1.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `reservation`
--

-- --------------------------------------------------------

--
-- Table structure for table `produit`
--

CREATE TABLE `produit` (
  `CODE` varchar(11) NOT NULL,
  `NAME` varchar(50) DEFAULT NULL,
  `DESCRIPTION` varchar(230) NOT NULL,
  `PRICE` decimal(7,2) NOT NULL,
  `PICTURE` varchar(50) DEFAULT NULL,
  `FOURNISSEUR` varchar(50) NOT NULL,
  `ADDRESS` varchar(150) NOT NULL,
  `PHONE` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `produit`
--

INSERT INTO `produit` (`CODE`, `NAME`, `DESCRIPTION`, `PRICE`, `PICTURE`, `FOURNISSEUR`, `ADDRESS`, `PHONE`) VALUES
('ABC123', 'Parfum Sauvage', 'Parfume from france de paris', '2599.99', '3348901250146_4.jpg', 'Sauvage', 'France, Paris.', '0661559797'),
('PRFM45', 'Parfum L\'Interdit', 'Imported from Italy', '2499.99', 'interdit.jpg', 'L\'Interdit Inc.', 'Italy, Milano', '0711223344'),
('XYZ999', 'You', 'Imported from UK', '1499.99', 'you.jpg', 'You factory', 'UK, London.', '0537778899');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `produit`
--
ALTER TABLE `produit`
  ADD PRIMARY KEY (`CODE`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
