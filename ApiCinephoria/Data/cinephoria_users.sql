-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: cinephoria
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `userId` int NOT NULL AUTO_INCREMENT,
  `firstName` varchar(250) NOT NULL,
  `lastName` varchar(250) NOT NULL,
  `email` varchar(250) NOT NULL,
  `password` varchar(255) NOT NULL,
  `roleId` int DEFAULT NULL,
  `userName` varchar(250) NOT NULL,
  `MustChangePassword` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`userId`),
  KEY `fk_roleId` (`roleId`),
  CONSTRAINT `fk_roleId` FOREIGN KEY (`roleId`) REFERENCES `roles` (`roleId`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'JL','KZU','jean-loup.quazuguel@zf.com','$2a$11$AD57Q9ZEDI/rV0VN1cHahuFzZpsbbX4wEmBUngHwFxa1YLJso9nZy',1,'Jean-Loup',_binary '\0'),(2,'User1','User1','Test@test.com','$2a$11$HdjxrmaunFmk7uJdErVrZOjDR83/7ULzTTqH/n/eyb5RzjTspbvbS',3,'User_1',_binary '\0'),(22,'Arnaud','Gac','arnaud.gac@zf.com','$2a$11$ySIEwuuwXwDtCNWon49m1ON/xdlhc55ALnivaJaXbDKkwXDCz1fHa',3,'Arnaud',_binary '\0'),(23,'Camille','Lelimousin','lelimousin.camille@gmail.com','$2a$11$LcRg10HDgjy4ZuxFFEH5QeTjE3ErJ2KTiN5SoPIjDvEPvahNudoZ.',3,'Kam',_binary '\0'),(29,'Employé','Employé','jean-loup.quazuguel@test.com','$2a$11$0pne8j8ShfGDoYFUp9kOhO.i6Fsi3XQ5uLHVQxBVeJ2D1vvQYy1wO',2,'Employé_1',_binary '\0'),(30,'Arthur','Pont','arthur.pont@zf.com','$2a$11$yBAJengLazuSu81DjXq..OLDEZiYcFj5AXxv2OeYkTQxRJ4Do1P1S',3,'Arthur_Pont',_binary '\0');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-09-08 13:30:31
