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
-- Table structure for table `cinema_schedule`
--

DROP TABLE IF EXISTS `cinema_schedule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cinema_schedule` (
  `scheduleId` int NOT NULL AUTO_INCREMENT,
  `cinemaId` int DEFAULT NULL,
  `jour` enum('Lundi','Mardi','Mercredi','Jeudi','Vendredi','Samedi','Dimanche') DEFAULT NULL,
  `heure_ouverture` time DEFAULT NULL,
  `heure_fermeture` time DEFAULT NULL,
  PRIMARY KEY (`scheduleId`),
  KEY `cinemaId` (`cinemaId`),
  CONSTRAINT `cinema_schedule_ibfk_1` FOREIGN KEY (`cinemaId`) REFERENCES `cinemas` (`cinemaId`)
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cinema_schedule`
--

LOCK TABLES `cinema_schedule` WRITE;
/*!40000 ALTER TABLE `cinema_schedule` DISABLE KEYS */;
INSERT INTO `cinema_schedule` VALUES (1,2,'Lundi','10:00:00','23:30:00'),(2,2,'Mardi','10:00:00','23:30:00'),(3,2,'Mercredi','10:00:00','23:30:00'),(4,2,'Jeudi','10:00:00','23:30:00'),(5,2,'Vendredi','10:00:00','01:00:00'),(6,2,'Samedi','10:00:00','01:00:00'),(7,2,'Dimanche','10:00:00','23:00:00'),(8,3,'Lundi',NULL,NULL),(9,3,'Mardi','09:30:00','22:30:00'),(10,3,'Mercredi','09:30:00','22:30:00'),(11,3,'Jeudi','09:30:00','23:00:00'),(12,3,'Vendredi','10:00:00','23:30:00'),(13,3,'Samedi','10:00:00','23:30:00'),(14,3,'Dimanche','10:00:00','22:00:00'),(15,4,'Lundi',NULL,NULL),(16,4,'Mardi','10:00:00','23:00:00'),(17,4,'Mercredi','10:00:00','23:00:00'),(18,4,'Jeudi','10:00:00','23:30:00'),(19,4,'Vendredi','10:00:00','00:00:00'),(20,4,'Samedi','11:00:00','00:30:00'),(21,4,'Dimanche','10:00:00','22:30:00'),(22,5,'Lundi',NULL,NULL),(23,5,'Mardi','09:00:00','22:00:00'),(24,5,'Mercredi','09:00:00','22:00:00'),(25,5,'Jeudi','09:00:00','23:00:00'),(26,5,'Vendredi','09:30:00','23:30:00'),(27,5,'Samedi','10:00:00','23:30:00'),(28,5,'Dimanche','10:00:00','22:30:00'),(29,6,'Lundi',NULL,NULL),(30,6,'Mardi','10:00:00','23:00:00'),(31,6,'Mercredi','10:30:00','23:30:00'),(32,6,'Jeudi','10:30:00','23:30:00'),(33,6,'Vendredi','10:00:00','23:45:00'),(34,6,'Samedi','11:00:00','00:00:00'),(35,6,'Dimanche','10:30:00','22:30:00'),(36,7,'Lundi',NULL,NULL),(37,7,'Mardi','09:30:00','22:30:00'),(38,7,'Mercredi','09:30:00','22:30:00'),(39,7,'Jeudi','09:30:00','23:00:00'),(40,7,'Vendredi','10:00:00','23:30:00'),(41,7,'Samedi','10:30:00','23:30:00'),(42,7,'Dimanche','10:00:00','22:00:00'),(43,8,'Lundi',NULL,NULL),(44,8,'Mardi','10:00:00','22:30:00'),(45,8,'Mercredi','10:00:00','22:30:00'),(46,8,'Jeudi','10:00:00','23:00:00'),(47,8,'Vendredi','10:30:00','23:45:00'),(48,8,'Samedi','11:00:00','00:00:00'),(49,8,'Dimanche','10:00:00','22:00:00');
/*!40000 ALTER TABLE `cinema_schedule` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-09-08 13:30:43
