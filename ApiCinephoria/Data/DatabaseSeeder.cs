using MySqlConnector;

namespace ApiCinephoria.Data
{
    public class DatabaseSeeder
    {
        private readonly string _connectionString;

        public DatabaseSeeder(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ImportSqlDump(string filePath, bool skipCheck = false)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Fichier introuvable: {filePath}");
                return;
            }

            string sql = File.ReadAllText(filePath);

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            if (!skipCheck)
            {
                using (var checkCmd = new MySqlCommand("SHOW TABLES;", connection))
                using (var reader = checkCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine(" La base contient déjà des tables, import ignoré.");
                        return;
                    }
                }
            }

            using var cmd = new MySqlCommand();
            cmd.Connection = connection;

            var commands = sql.Split(";", StringSplitOptions.RemoveEmptyEntries);

            foreach (var command in commands)
            {
                string trimmed = command.Trim();
                if (string.IsNullOrWhiteSpace(trimmed)) continue;

                cmd.CommandText = trimmed + ";";
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Erreur sur commande: {trimmed}\n{ex.Message}");
                }
            }

            Console.WriteLine("Import terminé avec succès !");
        }


        public void ImportSqlDumpIfEmpty(string folderPath)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var checkCmd = new MySqlCommand("SHOW TABLES;", connection);
            using var reader = checkCmd.ExecuteReader();           

            // Parcours tous les fichiers SQL du dossier
            foreach (var file in Directory.GetFiles(folderPath, "*.sql"))
            {
                Console.WriteLine($"Import {file}...");
                ImportSqlDump(file);
            }
        }
        public void ImportSqlDumpForce(string folderPath)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            // Supprime toutes les tables existantes
            using (var cmd = new MySqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = "SET FOREIGN_KEY_CHECKS = 0;";
                cmd.ExecuteNonQuery();

                using var showCmd = new MySqlCommand("SHOW TABLES;", connection);
                using var reader = showCmd.ExecuteReader();
                var tables = new List<string>();
                while (reader.Read())
                {
                    tables.Add(reader.GetString(0));
                }
                reader.Close();

                foreach (var table in tables)
                {
                    cmd.CommandText = $"DROP TABLE IF EXISTS `{table}`;";
                    cmd.ExecuteNonQuery();
                    Console.WriteLine($"Table supprimée : {table}");
                }

                cmd.CommandText = "SET FOREIGN_KEY_CHECKS = 1;";
                cmd.ExecuteNonQuery();
            }

            // Parcours tous les fichiers SQL du dossier
            foreach (var file in Directory.GetFiles(folderPath, "*.sql"))
            {
                Console.WriteLine($"Import {file}...");
                ImportSqlDump(file, skipCheck: true); // on passe un flag pour ignorer la vérification
            }
        }



    }
}
