using MySqlConnector;
using System.Text;

namespace ApiCinephoria.Data
{
    public class DatabaseSeeder
    {
        private readonly string _connectionString;

        public DatabaseSeeder(string connectionString)
        {
            _connectionString = connectionString;
        }

        // --- Fonction utilitaire pour découper proprement les commandes SQL ---
        private IEnumerable<string> SplitSqlStatements(string sql)
        {
            var sb = new StringBuilder();
            foreach (var line in sql.Split('\n'))
            {
                var trimmed = line.Trim();

                // Ignore commentaires MySQL
                if (trimmed.StartsWith("--") || trimmed.StartsWith("/*") || trimmed.StartsWith("/*!"))
                    continue;

                sb.AppendLine(line);

                // On exécute seulement quand une ligne se termine par ";"
                if (trimmed.EndsWith(";"))
                {
                    yield return sb.ToString();
                    sb.Clear();
                }
            }

            if (sb.Length > 0)
                yield return sb.ToString();
        }

        // --- Import d’un fichier SQL complet ---
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

            foreach (var statement in SplitSqlStatements(sql))
            {
                using var cmd = new MySqlCommand(statement, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Erreur SQL dans {Path.GetFileName(filePath)} : {ex.Message}");
                }
            }

            Console.WriteLine($"Import terminé pour {Path.GetFileName(filePath)}");
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
            using (var dropCmd = new MySqlCommand("SET FOREIGN_KEY_CHECKS = 0;", connection))
                dropCmd.ExecuteNonQuery();

            using (var cmd = new MySqlCommand("SHOW TABLES;", connection))
            using (var reader = cmd.ExecuteReader())
            {
                var tables = new List<string>();
                while (reader.Read())
                    tables.Add(reader.GetString(0));

                reader.Close();

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
                        Console.WriteLine($"⚠️ Erreur suppression {table}: {ex.Message}");
                    }
                }
            }

            using (var dropCmd = new MySqlCommand("SET FOREIGN_KEY_CHECKS = 1;", connection))
                dropCmd.ExecuteNonQuery();

            Console.WriteLine("📂 Réimport des fichiers SQL...");
            foreach (var file in Directory.GetFiles(folderPath, "*.sql"))
            {
                ImportSqlDump(file);
            }
        }
    }
}
