-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 12, 2025 at 11:42 AM
-- Server version: 10.4.28-MariaDB
-- PHP Version: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `bms`
--

-- --------------------------------------------------------

--
-- Table structure for table `booking`
--

CREATE TABLE `booking` (
  `BookingID` int(11) NOT NULL,
  `CustomerID` int(11) NOT NULL,
  `EventID` int(11) NOT NULL,
  `ServiceID` int(11) DEFAULT NULL,
  `BookingDate` date NOT NULL,
  `BookedBy` varchar(100) DEFAULT NULL,
  `BookingStatus` enum('Confirmed','Pending','Cancelled') DEFAULT 'Pending',
  `BookingTime` time DEFAULT NULL,
  `EventDate` date DEFAULT NULL,
  `TotalAmount` decimal(10,2) DEFAULT 0.00,
  `PaymentStatus` enum('Paid','Unpaid','Partially Paid') DEFAULT 'Unpaid',
  `Remarks` text DEFAULT NULL,
  `DiscountApplied` decimal(10,2) DEFAULT 0.00,
  `RefundStatus` enum('Refunded','Not Refunded') DEFAULT 'Not Refunded',
  `CreatedAt` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `booking`
--

INSERT INTO `booking` (`BookingID`, `CustomerID`, `EventID`, `ServiceID`, `BookingDate`, `BookedBy`, `BookingStatus`, `BookingTime`, `EventDate`, `TotalAmount`, `PaymentStatus`, `Remarks`, `DiscountApplied`, `RefundStatus`, `CreatedAt`) VALUES
(1, 101, 201, 301, '2025-04-01', 'admin', 'Confirmed', '14:00:00', '2025-04-15', 1500.00, 'Paid', 'All set', 100.00, 'Not Refunded', '2025-04-01 10:30:00'),
(2, 102, 202, NULL, '2025-04-03', 'staff1', 'Pending', '10:30:00', '2025-04-20', 800.00, 'Unpaid', NULL, 0.00, 'Not Refunded', '2025-04-03 09:45:00'),
(3, 103, 203, 302, '2025-04-05', 'staff2', 'Cancelled', '16:00:00', '2025-04-25', 1000.00, 'Partially Paid', 'Client canceled', 50.00, 'Refunded', '2025-04-05 11:00:00'),
(4, 104, 204, NULL, '2025-04-07', 'admin', 'Confirmed', '12:00:00', '2025-04-30', 2000.00, 'Paid', NULL, 200.00, 'Not Refunded', '2025-04-07 12:30:00'),
(5, 105, 205, 303, '2025-04-09', 'staff3', 'Pending', '09:00:00', '2025-05-01', 1200.00, 'Unpaid', 'Awaiting confirmation', 0.00, 'Not Refunded', '2025-04-09 08:15:00');

-- --------------------------------------------------------

--
-- Table structure for table `customer`
--

CREATE TABLE `customer` (
  `CustomerID` int(11) NOT NULL,
  `FirstName` varchar(50) NOT NULL,
  `LastName` varchar(50) NOT NULL,
  `MiddleName` varchar(50) DEFAULT NULL,
  `Gender` enum('Male','Female','Other') NOT NULL,
  `BirthDate` date DEFAULT NULL,
  `ContactNumber` varchar(20) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `AddressLine` varchar(100) DEFAULT NULL,
  `City` varchar(50) DEFAULT NULL,
  `Province` varchar(50) DEFAULT NULL,
  `ZipCode` varchar(10) DEFAULT NULL,
  `CustomerType` enum('Walk-in','Regular','Corporate') NOT NULL,
  `RegistrationDate` datetime DEFAULT current_timestamp(),
  `Status` enum('Active','Inactive') DEFAULT 'Active'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `customer`
--

INSERT INTO `customer` (`CustomerID`, `FirstName`, `LastName`, `MiddleName`, `Gender`, `BirthDate`, `ContactNumber`, `Email`, `AddressLine`, `City`, `Province`, `ZipCode`, `CustomerType`, `RegistrationDate`, `Status`) VALUES
(101, 'John', 'Doe', 'A', 'Male', '1990-01-01', '09171234567', 'john@example.com', '123 Main St', 'Manila', 'NCR', '1000', 'Regular', '2025-03-01 10:00:00', 'Active'),
(102, 'Jane', 'Smith', 'T', 'Female', '1985-05-10', '09281234567', 'jane@example.com', '456 Elm St', 'Quezon City', 'NCR', '1100', 'Walk-in', '2025-03-03 14:00:00', 'Active'),
(103, 'Carlos', 'Reyes', NULL, 'Male', '1992-08-15', '09181234567', 'carlos@example.com', '789 Pine St', 'Makati', 'NCR', '1200', 'Corporate', '2025-03-05 09:30:00', 'Active'),
(104, 'Anna', 'Cruz', 'B', 'Female', '1995-02-20', '09301234567', 'anna@example.com', '321 Oak St', 'Pasig', 'NCR', '1600', 'Regular', '2025-03-06 11:45:00', 'Active'),
(105, 'Leo', 'Garcia', 'T', 'Other', '1988-11-30', '09051234567', 'leo@example.com', '654 Cedar St', 'Taguig', 'NCR', '1630', 'Walk-in', '2025-03-08 13:20:00', 'Inactive');

-- --------------------------------------------------------

--
-- Table structure for table `event`
--

CREATE TABLE `event` (
  `EventID` int(11) NOT NULL,
  `EventName` varchar(100) NOT NULL,
  `CustomerID` int(11) NOT NULL,
  `EventType` enum('Private','Corporate') NOT NULL,
  `EventDate` date NOT NULL,
  `StartTime` time NOT NULL,
  `EndTime` time NOT NULL,
  `VenueLocation` varchar(100) DEFAULT NULL,
  `GuestCount` int(11) DEFAULT NULL,
  `Theme` varchar(100) DEFAULT NULL,
  `SpecialRequests` text DEFAULT NULL,
  `SetupTime` time DEFAULT NULL,
  `CleanupTime` time DEFAULT NULL,
  `Status` enum('Upcoming','Completed','Cancelled') DEFAULT 'Upcoming',
  `CreatedDate` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `event`
--

INSERT INTO `event` (`EventID`, `EventName`, `CustomerID`, `EventType`, `EventDate`, `StartTime`, `EndTime`, `VenueLocation`, `GuestCount`, `Theme`, `SpecialRequests`, `SetupTime`, `CleanupTime`, `Status`, `CreatedDate`) VALUES
(201, 'Wedding of John & Jane', 101, 'Private', '2025-04-15', '14:00:00', '20:00:00', 'Villa Escudero', 150, 'Rustic', 'Vegan menu', '12:00:00', '22:00:00', 'Upcoming', '2025-03-01 10:00:00'),
(202, 'Corporate Year-End Party', 102, 'Corporate', '2025-04-20', '18:00:00', '23:00:00', 'Hotel ABC', 200, 'Gatsby', NULL, '16:00:00', '01:00:00', 'Upcoming', '2025-03-03 14:00:00'),
(203, 'Birthday Bash', 103, 'Private', '2025-04-25', '17:00:00', '22:00:00', 'Beach Resort', 100, 'Tropical', 'Live band', '15:00:00', '23:00:00', 'Cancelled', '2025-03-05 09:30:00'),
(204, 'Product Launch', 104, 'Corporate', '2025-04-30', '10:00:00', '14:00:00', 'Mall of Asia', 300, 'Modern', NULL, '08:00:00', '16:00:00', 'Upcoming', '2025-03-06 11:45:00'),
(205, 'Anniversary Celebration', 105, 'Private', '2025-05-01', '12:00:00', '18:00:00', 'Tagaytay Highlands', 80, 'Garden', 'Fireworks', '10:00:00', '20:00:00', 'Upcoming', '2025-03-08 13:20:00'),
(206, 'test', 101, '', '2025-04-12', '10:25:00', '11:20:00', 'sa tabi', 1, 'test', 'Wala naman', '12:00:00', '12:00:00', 'Upcoming', '2025-04-12 16:50:54'),
(207, 'test', 101, 'Private', '2025-04-12', '16:58:56', '04:58:56', 'test', 1, 'test', 'wala naman', '16:58:56', '04:58:56', 'Upcoming', '2025-04-12 16:59:21');

-- --------------------------------------------------------

--
-- Table structure for table `payment`
--

CREATE TABLE `payment` (
  `PaymentID` int(11) NOT NULL,
  `BookingID` int(11) NOT NULL,
  `PaymentDate` date NOT NULL,
  `AmountPaid` decimal(10,2) NOT NULL,
  `PaymentMethod` enum('Cash','Card','Bank Transfer') NOT NULL,
  `ReferenceNumber` varchar(100) DEFAULT NULL,
  `PaymentStatus` enum('Full','Partial','Overpaid') NOT NULL,
  `ProcessedBy` varchar(100) DEFAULT NULL,
  `PaymentTime` time DEFAULT NULL,
  `Balance` decimal(10,2) DEFAULT 0.00,
  `DiscountAmount` decimal(10,2) DEFAULT 0.00,
  `RefundedAmount` decimal(10,2) DEFAULT 0.00,
  `Remarks` text DEFAULT NULL,
  `ORNumber` varchar(50) DEFAULT NULL,
  `CreatedAt` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `payment`
--

INSERT INTO `payment` (`PaymentID`, `BookingID`, `PaymentDate`, `AmountPaid`, `PaymentMethod`, `ReferenceNumber`, `PaymentStatus`, `ProcessedBy`, `PaymentTime`, `Balance`, `DiscountAmount`, `RefundedAmount`, `Remarks`, `ORNumber`, `CreatedAt`) VALUES
(401, 1, '2025-04-01', 1500.00, 'Cash', 'REF123456', 'Full', 'admin', '14:30:00', 0.00, 100.00, 0.00, 'Paid in full', 'OR001', '2025-04-01 15:00:00'),
(402, 2, '2025-04-03', 400.00, 'Card', 'REF234567', 'Partial', 'staff1', '10:45:00', 400.00, 0.00, 0.00, NULL, 'OR002', '2025-04-03 11:00:00'),
(403, 3, '2025-04-05', 1000.00, 'Bank Transfer', 'REF345678', 'Full', 'staff2', '16:15:00', 0.00, 50.00, 200.00, 'Refunded due to cancellation', 'OR003', '2025-04-05 17:00:00'),
(404, 4, '2025-04-07', 2000.00, 'Card', 'REF456789', 'Full', 'admin', '12:15:00', 0.00, 200.00, 0.00, NULL, 'OR004', '2025-04-07 13:00:00'),
(405, 5, '2025-04-09', 0.00, 'Cash', NULL, 'Partial', 'staff3', '09:30:00', 1200.00, 0.00, 0.00, 'Payment pending', 'OR005', '2025-04-09 10:00:00');

-- --------------------------------------------------------

--
-- Table structure for table `service_availed`
--

CREATE TABLE `service_availed` (
  `ServiceID` int(11) NOT NULL,
  `ServiceName` varchar(100) NOT NULL,
  `Description` text DEFAULT NULL,
  `Category` enum('Decoration','Food','Audio-Visual') NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `Unit` enum('Per hour','Per event','Per guest') NOT NULL,
  `Availability` enum('Available','Unavailable') DEFAULT 'Available',
  `SetupRequired` tinyint(1) DEFAULT 0,
  `DurationEstimate` time DEFAULT NULL,
  `MinGuest` int(11) DEFAULT 0,
  `MaxGuest` int(11) DEFAULT NULL,
  `CreatedBy` varchar(100) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT current_timestamp(),
  `UpdatedDate` datetime DEFAULT NULL,
  `Status` enum('Active','Archived') DEFAULT 'Active'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `service_availed`
--

INSERT INTO `service_availed` (`ServiceID`, `ServiceName`, `Description`, `Category`, `Price`, `Unit`, `Availability`, `SetupRequired`, `DurationEstimate`, `MinGuest`, `MaxGuest`, `CreatedBy`, `CreatedDate`, `UpdatedDate`, `Status`) VALUES
(301, 'Floral Decoration', 'Full floral venue decor', 'Decoration', 5000.00, 'Per event', 'Available', 1, '02:00:00', 50, 300, 'admin', '2025-03-01 10:00:00', NULL, 'Active'),
(302, 'Buffet Catering', 'Buffet for guests', 'Food', 400.00, 'Per guest', 'Available', 1, '04:00:00', 30, 500, 'staff1', '2025-03-03 14:00:00', NULL, 'Active'),
(303, 'Sound System', 'DJ + speakers', 'Audio-Visual', 3000.00, 'Per event', 'Available', 1, '01:00:00', 0, 200, 'staff2', '2025-03-05 09:30:00', NULL, 'Active'),
(304, 'LED Wall', 'High-res LED video wall', 'Audio-Visual', 1500.00, 'Per hour', 'Unavailable', 1, '01:30:00', 0, 0, 'admin', '2025-03-06 11:45:00', NULL, 'Archived'),
(305, 'Dessert Bar', 'Premium dessert station', 'Food', 250.00, 'Per guest', 'Available', 0, '00:45:00', 30, 150, 'staff3', '2025-03-08 13:20:00', NULL, 'Active'),
(306, 'service', 'description', 'Decoration', 100.00, 'Per hour', 'Available', 1, '17:11:27', 1, 1, 'Admingogo', '2025-04-12 17:11:47', '2025-04-12 17:11:47', 'Active');

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

CREATE TABLE `user` (
  `UserID` int(11) NOT NULL,
  `Username` varchar(100) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `UserLevelName` enum('Administrator','Accountant/Clerk','Staff') NOT NULL,
  `FullName` varchar(100) NOT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `PhoneNumber` varchar(20) DEFAULT NULL,
  `Status` enum('Active','Inactive') DEFAULT 'Active',
  `CreatedAt` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`UserID`, `Username`, `Password`, `UserLevelName`, `FullName`, `Email`, `PhoneNumber`, `Status`, `CreatedAt`) VALUES
(1, 'admin', '1', 'Administrator', 'Admingogo', 'admin@bms.com', '09171234567', 'Active', '2025-01-01 09:00:00'),
(2, 'acc', '2', 'Accountant/Clerk', 'Mia Reyes', 'mia@bms.com', '09181234567', 'Active', '2025-01-05 10:00:00'),
(3, 'staff', '3', 'Staff', 'Leo Santos', 'leo@bms.com', '09201234567', 'Active', '2025-01-10 11:00:00'),
(4, 'staff2', 'hashedpassword4', 'Staff', 'Anna Cruz', 'anna@bms.com', '09301234567', 'Inactive', '2025-01-15 12:00:00'),
(5, 'manager1', 'hashedpassword5', 'Administrator', 'Carlos Reyes', 'carlos@bms.com', '09401234567', 'Active', '2025-01-20 13:00:00');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `booking`
--
ALTER TABLE `booking`
  ADD PRIMARY KEY (`BookingID`),
  ADD KEY `CustomerID` (`CustomerID`),
  ADD KEY `EventID` (`EventID`),
  ADD KEY `ServiceID` (`ServiceID`);

--
-- Indexes for table `customer`
--
ALTER TABLE `customer`
  ADD PRIMARY KEY (`CustomerID`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- Indexes for table `event`
--
ALTER TABLE `event`
  ADD PRIMARY KEY (`EventID`),
  ADD KEY `CustomerID` (`CustomerID`);

--
-- Indexes for table `payment`
--
ALTER TABLE `payment`
  ADD PRIMARY KEY (`PaymentID`),
  ADD UNIQUE KEY `ORNumber` (`ORNumber`),
  ADD KEY `BookingID` (`BookingID`);

--
-- Indexes for table `service_availed`
--
ALTER TABLE `service_availed`
  ADD PRIMARY KEY (`ServiceID`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`UserID`),
  ADD UNIQUE KEY `Username` (`Username`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `booking`
--
ALTER TABLE `booking`
  MODIFY `BookingID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `customer`
--
ALTER TABLE `customer`
  MODIFY `CustomerID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=106;

--
-- AUTO_INCREMENT for table `event`
--
ALTER TABLE `event`
  MODIFY `EventID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=208;

--
-- AUTO_INCREMENT for table `payment`
--
ALTER TABLE `payment`
  MODIFY `PaymentID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=406;

--
-- AUTO_INCREMENT for table `service_availed`
--
ALTER TABLE `service_availed`
  MODIFY `ServiceID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=307;

--
-- AUTO_INCREMENT for table `user`
--
ALTER TABLE `user`
  MODIFY `UserID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `booking`
--
ALTER TABLE `booking`
  ADD CONSTRAINT `booking_ibfk_1` FOREIGN KEY (`CustomerID`) REFERENCES `customer` (`CustomerID`),
  ADD CONSTRAINT `booking_ibfk_2` FOREIGN KEY (`EventID`) REFERENCES `event` (`EventID`),
  ADD CONSTRAINT `booking_ibfk_3` FOREIGN KEY (`ServiceID`) REFERENCES `service_availed` (`ServiceID`);

--
-- Constraints for table `event`
--
ALTER TABLE `event`
  ADD CONSTRAINT `event_ibfk_1` FOREIGN KEY (`CustomerID`) REFERENCES `customer` (`CustomerID`);

--
-- Constraints for table `payment`
--
ALTER TABLE `payment`
  ADD CONSTRAINT `payment_ibfk_1` FOREIGN KEY (`BookingID`) REFERENCES `booking` (`BookingID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
