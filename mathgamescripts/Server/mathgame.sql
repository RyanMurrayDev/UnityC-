-- phpMyAdmin SQL Dump
-- version 4.9.5deb2
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Apr 21, 2021 at 05:48 PM
-- Server version: 8.0.23-0ubuntu0.20.04.1
-- PHP Version: 7.4.3

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `mathgame`
--
CREATE DATABASE IF NOT EXISTS `mathgame` DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;
USE `mathgame`;

-- --------------------------------------------------------

--
-- Table structure for table `userdata`
--

CREATE TABLE `userdata` (
  `username` varchar(45) NOT NULL,
  `wins_easy` int NOT NULL DEFAULT '0',
  `wins_medium` int NOT NULL DEFAULT '0',
  `wins_hard` int NOT NULL DEFAULT '0',
  `wins_multiplayer` int NOT NULL DEFAULT '0',
  `coins` int NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `userdata`
--

INSERT INTO `userdata` (`username`, `wins_easy`, `wins_medium`, `wins_hard`, `wins_multiplayer`, `coins`) VALUES
('d', 0, 0, 0, 0, 0),
('pat', 0, 0, 0, 0, 0),
('ry', 0, 0, 0, 0, 0),
('ryan', 1, 2, 3, 4, 1000);

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `user_id` int NOT NULL,
  `username` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  `hash` varchar(100) NOT NULL,
  `salt` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`user_id`, `username`, `email`, `hash`, `salt`) VALUES
(3, 'ryan', 'ryan@gmail.com', '$5$rounds=5000$steamedhamsryan$P/N/ippXbmG83IO9doYJEa74CK..CqdaeASbBQhsxp4', '$5$rounds=5000$steamedhamsryan$'),
(4, 'ry', 'ry@gmail.com', '$5$rounds=5000$steamedhamsry$6SLLOuQOlecwdG3cKG7Vo/.4Z3r2HTiZKtr2vzGuI..', '$5$rounds=5000$steamedhamsry$'),
(5, 'pat', 'pat@gmail.com', '$5$rounds=5000$steamedhamspat$NKMbM6lFRL.aiKAIaCBjgnf4L3Ph.srz5Wy2P3SxMxB', '$5$rounds=5000$steamedhamspat$'),
(8, 'd', 'd@gmail.com', '$5$rounds=5000$steamedhamsd$0fVxPqy05TDKJ8.VI9Q1ZgpcmpNTazLqHnmkpZdc0p.', '$5$rounds=5000$steamedhamsd$');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `userdata`
--
ALTER TABLE `userdata`
  ADD PRIMARY KEY (`username`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`user_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `user_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
