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
-- Table structure for table `rooms`
--

DROP TABLE IF EXISTS `rooms`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rooms` (
  `roomId` int NOT NULL AUTO_INCREMENT,
  `cinemaId` int NOT NULL,
  `name` varchar(250) NOT NULL,
  `quality` varchar(50) NOT NULL,
  `seatsNumber` int DEFAULT NULL,
  PRIMARY KEY (`roomId`),
  KEY `cinemaId` (`cinemaId`),
  CONSTRAINT `rooms_ibfk_1` FOREIGN KEY (`cinemaId`) REFERENCES `cinemas` (`cinemaId`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rooms`
--

LOCK TABLES `rooms` WRITE;
/*!40000 ALTER TABLE `rooms` DISABLE KEYS */;
INSERT INTO `rooms` VALUES (1,2,'Salle 1','IMAX',150),(2,2,'Salle 2','Classique',150),(3,2,'Salle 3','Classique',150),(4,2,'Salle 4','Classique',150),(5,2,'Salle 5','Classique',120),(6,3,'Salle 1','IMAX',150),(7,3,'Salle 2','Classique',120),(8,3,'Salle 3','Classique',120),(9,4,'Salle 1','Classique',120),(10,4,'Salle 2','Classique',150),(11,5,'Salle 1','IMAX',150),(12,5,'Salle 2','Classique',150),(13,5,'Salle 3','Classique',150),(14,6,'Salle 1','Classique',120),(16,7,'Salle 1','Classique',150),(17,7,'Salle 2','IMAX',150),(18,8,'Salle 1','IMAX',150),(19,8,'Salle 2','Classique',150),(21,6,'Salle 2','Classique',150),(27,2,'Salle Test','Classique',30),(30,3,'Molene','Classique',150);
/*!40000 ALTER TABLE `rooms` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-09-08 13:30:34
