using MySql.Data.MySqlClient;

namespace ApiCinephoria.Data
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly string _connectionString;

        public DatabaseSeeder(string connectionString)
        {
            _connectionString = connectionString;
        }

        // --- Import d’un fichier SQL complet ---
        public void ImportSqlDump(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($" Fichier introuvable: {filePath}");
                return;
            }

            string sql = File.ReadAllText(filePath);

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            try
            {
                var script = new MySqlScript(connection, sql);
                int statementsExecuted = script.Execute();
                Console.WriteLine($" {Path.GetFileName(filePath)} importé ({statementsExecuted} commandes exécutées)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Erreur import {Path.GetFileName(filePath)} : {ex.Message}");
            }
        }

        // --- Import seulement si aucune table ---
        public void ImportSqlDumpIfEmpty(string folderPath)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var checkCmd = new MySqlCommand("SHOW TABLES;", connection);
            using var reader = checkCmd.ExecuteReader();

            if (reader.HasRows)
            {
                Console.WriteLine("La base contient déjà des tables, import ignoré.");
                return;
            }

            reader.Close();

            foreach (var file in Directory.GetFiles(folderPath, "*.sql"))
            {
                Console.WriteLine($"Import {file}...");
                ImportSqlDump(file);
            }
        }

        // --- Import forcé : supprime toutes les tables puis recharge ---
        public void ImportSqlDumpForce(string folderPath)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            Console.WriteLine("🗑 Suppression des tables existantes...");
            using (var disableKeys = new MySqlCommand("SET FOREIGN_KEY_CHECKS = 0;", connection))
                disableKeys.ExecuteNonQuery();

            var tables = new List<string>();
            using (var cmd = new MySqlCommand("SHOW TABLES;", connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    tables.Add(reader.GetString(0));
            }

            foreach (var table in tables)
            {
                try
                {
                    using var drop = new MySqlCommand($"DROP TABLE IF EXISTS `{table}`;", connection);
                    drop.ExecuteNonQuery();
                    Console.WriteLine($"   - Table {table} supprimée");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Erreur suppression {table}: {ex.Message}");
                }
            }

            using (var enableKeys = new MySqlCommand("SET FOREIGN_KEY_CHECKS = 1;", connection))
                enableKeys.ExecuteNonQuery();

            Console.WriteLine("Réimport des fichiers SQL...");
            foreach (var file in Directory.GetFiles(folderPath, "*.sql"))
            {
                ImportSqlDump(file);
            }
        }
    }
}
