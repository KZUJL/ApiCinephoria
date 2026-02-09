namespace ApiCinephoria.Data
{
    public interface IDatabaseSeeder
    {
        void ImportSqlDump(string filePath);
        void ImportSqlDumpForce(string folderPath);
        void ImportSqlDumpIfEmpty(string folderPath);
    }
}