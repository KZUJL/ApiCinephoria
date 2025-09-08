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

        public void ImportSqlDump(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Fichier introuvable: {filePath}");
                return;
            }

            string sql = File.ReadAllText(filePath);

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            // Vérifie si des tables existent déjà
            using (var checkCmd = new MySqlCommand("SHOW TABLES;", connection))
            using (var reader = checkCmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Console.WriteLine(" La base contient déjà des tables, import ignoré.");
                    return;
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
            if (reader.HasRows)
            {
                Console.WriteLine("Base déjà remplie, import SQL ignoré.");
                return;
            }

            // Parcours tous les fichiers SQL du dossier
            foreach (var file in Directory.GetFiles(folderPath, "*.sql"))
            {
                Console.WriteLine($"Import {file}...");
                ImportSqlDump(file);
            }
        }

    }
}
