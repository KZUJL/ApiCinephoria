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
-- Table structure for table `movies`
--

DROP TABLE IF EXISTS `movies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `movies` (
  `movieId` int NOT NULL AUTO_INCREMENT,
  `releaseDate` date NOT NULL,
  `title` varchar(250) NOT NULL,
  `genre` varchar(250) NOT NULL,
  `description` text NOT NULL,
  `duration` time NOT NULL,
  `poster` varchar(255) DEFAULT NULL,
  `trailer` varchar(255) DEFAULT NULL,
  `director` varchar(250) NOT NULL,
  `producer` varchar(250) NOT NULL,
  `cast` text NOT NULL,
  `sourcePoster` varchar(255) DEFAULT NULL,
  `sourceTrailer` varchar(255) DEFAULT NULL,
  `availableDate` date DEFAULT NULL,
  `minimumAge` varchar(45) DEFAULT NULL,
  `isfavorite` tinyint DEFAULT NULL,
  PRIMARY KEY (`movieId`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `movies`
--

LOCK TABLES `movies` WRITE;
/*!40000 ALTER TABLE `movies` DISABLE KEYS */;
INSERT INTO `movies` VALUES (1,'1922-01-01','Nosferatu','Horror','Une adaptation non autorisée du roman Dracula de Bram Stoker, qui est devenu un classique du cinéma d\'horreur.','01:34:00','https://upload.wikimedia.org/wikipedia/commons/a/a7/Wismar_Markt_Nosferatu_01_%28cropped%29.jpg','https://archive.org/download/CEP177/CEP177_512kb.mp4','Friedrich Wilhelm Murnau','Prana Film','Max Schreck, Gustav von Wangenheim, Greta Schroeder','https://commons.wikimedia.org','Archive.org','2025-09-03','Interdit - 12 ans',0),(2,'1931-02-14','Frankenstein','Horror','L\'histoire du docteur Frankenstein qui crée un monstre à partir de morceaux de cadavres, un autre grand classique du cinéma d\'horreur.','01:10:00','https://ia800804.us.archive.org/10/items/img-0466_202412/IMG_0466.jpeg','https://www.youtube.com/watch?v=BN8K-4osNb0','James Whale','Universal Pictures','Boris Karloff, Colin Clive, Mae Clarke','Archive.org','Archive.org','2025-07-16','Interdit - 12 ans',1),(3,'1927-01-01','Metropolis','Science Fiction','Une œuvre visionnaire de Fritz Lang sur un futur dystopique et l\'oppression sociale, un pilier du genre science-fiction.','02:28:00','https://ia601607.us.archive.org/12/items/lang-metropolis-poster/Lang_Metropolis_poster.jpg','https://archive.org/download/403596/lang_metropolis_2010_extrait.mp4','Fritz Lang','Universum Film AG','Brigitte Helm, Alfred Abel, Gustav Fröhlich','Archive.org','Archive.org','2025-07-24','Tout public',1),(4,'1940-01-01','The Phantom of the Opera','Horror','Un drame mystérieux et effrayant où un génie défiguré vit sous l\'Opéra de Paris, obsédé par une jeune chanteuse.','01:33:00','https://dn721600.ca.archive.org/0/items/1945-advertisement-for-phantom-of-the-opera/1945%20advertisement%20for%20Phantom%20of%20the%20Opera.png','https://archive.org/download/phantom-of-the-opera_202412/Phantom%20of%20the%20Opera%20Trailer.mp4','Arthur Lubin','Universal Pictures','Claude Rains, Nelson Eddy, Susanna Foster','Archive.org','Archive.org','2025-06-16','Tout public',0),(5,'1936-01-01','Modern Times','Comedy','Un film comique de Charlie Chaplin qui critique l\'industrialisation et les conditions de travail de l\'époque.','01:27:00','https://upload.wikimedia.org/wikipedia/commons/3/36/Modern_Times_poster.jpg','https://archive.org/download/ModernTimesTrailers/ModernTimesTrailer1.mp4','Charlie Chaplin','United Artists','Charlie Chaplin, Paulette Goddard','https://commons.wikimedia.org','Archive.org','2025-06-16','Tout public',0),(7,'1922-01-01','The Adventures of Robin Hood','Adventure, Drama','A classic silent film about the legendary English outlaw Robin Hood, starring Douglas Fairbanks.','01:45:00','https://upload.wikimedia.org/wikipedia/commons/f/f7/The_Adventures_of_Robin_Hood_%281938_poster%29.jpg','https://www.youtube.com/watch?v=HiPhvLKCwIY','Alan Dwan','Douglas Fairbanks','Douglas Fairbanks, Wallace Beery','https://commons.wikimedia.org','https://www.youtube.com/','2025-06-19','Tout public',0),(8,'1968-10-01','Night of the Living Dead','Horror, Zombies','A group of people trapped in a house must survive a zombie apocalypse. One of the first films to popularize modern zombie horror.','01:36:00','https://upload.wikimedia.org/wikipedia/commons/4/48/Night_of_the_living_Dead_Logo.png','https://archive.org/download/NightOfTheLivingDeadTrailer/NightLivingDeadTrailer.mp4','George A. Romero','Russell W. Streiner, John A. Russo','Duane Jones, Judith O\'Dea, Karl Hardman','https://commons.wikimedia.org','archive.org','2025-06-19','Tout public',0),(9,'1929-05-23','The Karnival Kid','comedy','The Karnival Kid is a short animated film featuring Mickey Mouse, produced by Walt Disney and Ub Iwerks. It is notable for being the first cartoon where Mickey Mouse speaks.','00:08:00','https://upload.wikimedia.org/wikipedia/commons/9/98/9_-_The_Karnival_Kid.jpg','https://ia600809.us.archive.org/30/items/the-karnival-kid_1929/the-karnival-kid_1929.ia.mp4','Walt Disney, Ub Iwerks','John Sutherland','Walt Disney, Marcellite Garner','https://commons.wikimedia.org','https://archive.org/','2025-06-11','Tout public',1),(10,'2001-01-01','Le Seigneur des anneaux : La Communauté de l\'anneau','Fantasy','Dans ce chapitre de la trilogie, le jeune et timide Hobbit, Frodon Sacquet, hérite d un anneau. Bien loin d être une simple babiole, il s agit de l Anneau Unique, un instrument de pouvoir absolu qui permettrait à Sauron, le Seigneur des ténèbres, de régner sur la Terre du Milieu et de réduire en esclavage ses peuples. À moins que Frodon, aidé d une Compagnie constituée de Hobbits, d Hommes, d un Magicien, d un Nain, et d un Elfe, ne parvienne à emporter l Anneau à travers la Terre du Milieu jusqu à la Crevasse du Destin, lieu où il a été forgé, et à le détruire pour toujours. Un tel périple signifie s aventurer très loin en Mordor, les terres du Seigneur des ténèbres, où est rassemblée son armée d Orques maléfiques... La Compagnie doit non seulement combattre les forces extérieures du mal mais aussi les dissensions internes et l influence corruptrice qu exerce l Anneau lui-même.','02:58:00','https://preview.redd.it/d93hacxq8khb1.jpg?auto=webp&s=591e2b66d3899134640726653a26b161c8c04e1c','https://www.youtube.com/watch?v=nalLU8i4zgs','Peter Jackson','Peter Jackson','Elijah Wood, Sean Astin, Ian McKellen','https://commons.wikimedia.org','https://commons.wikimedia.org','2025-06-25','Tout public',1),(13,'2025-06-11','Le Seigneur des anneaux : Les Deux Tours','Fantasy','Après la mort de Boromir et la disparition de Gandalf, la Communauté s\'est scindée en trois. Perdus dans les collines d\'Emyn Muil, Frodon et Sam découvrent qu\'ils sont suivis par Gollum, une créature versatile corrompue par l\'Anneau. Celui-ci promet de conduire les Hobbits jusqu\'à la Porte Noire du Mordor. A travers la Terre du Milieu, Aragorn, Legolas et Gimli font route vers le Rohan, le royaume assiégé de Theoden. Cet ancien grand roi, manipulé par l\'espion de Saroumane, le sinistre Langue de Serpent, est désormais tombé sous la coupe du malfaisant Magicien. Eowyn, la nièce du Roi, reconnaît en Aragorn un meneur d\'hommes. Entretemps, les Hobbits Merry et Pippin, prisonniers des Uruk-hai, se sont échappés et ont découvert dans la mystérieuse Forêt de Fangorn un allié inattendu : Sylvebarbe, gardien des arbres, représentant d\'un ancien peuple végétal dont Saroumane a décimé la forêt...','02:59:00','https://cdn11.bigcommerce.com/s-ydriczk/images/stencil/1500x1500/products/84264/83831/the_lord_of_the_rings_the_two_towers_gandalf_reprint_movie_poster_buy_now_at_starstills_678__11697__25009.1394513532.jpg?c=2&imbypass=on','https://www.youtube.com/watch?v=5e6CxvZo_6E',' Peter Jackson',' Peter Jackson',' Elijah Wood, Sean Astin, Viggo Mortensen','https://antreducinema.fr/','youtube','2025-06-19','Tous public',0);
/*!40000 ALTER TABLE `movies` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-09-08 13:30:38
