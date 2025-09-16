-- transaction_example.sql
-- But : Ajouter un nouvel employé avec son rôle, tout ou rien si erreur

BEGIN;

-- Ajouter un rôle 
INSERT INTO roles (roleName)
VALUES ('Technicien') 
ON DUPLICATE KEY UPDATE roleId=roleId; 
-- si le rôle existe déjà, on ne fait rien

-- 2?? Récupérer l'ID du rôle ajouté ou existant
SET @roleId = (SELECT roleId FROM roles WHERE roleName = 'Technicien');

-- 3?? Ajouter un utilisateur avec ce rôle
INSERT INTO users (firstName, lastName, email, password, roleId, userName, MustChangePassword)
VALUES ('Jean-Loup', 'Kazu', 'jl.kazu@example.com', 'MotDePasse123', @roleId, 'jlKazu', b'1');

-- Valider la transaction si tout s'est bien passé
COMMIT;

-- En cas d'erreur, tout sera annulé
-- ROLLBACK; -- à décommenter si nécessaire pour tester un échec
