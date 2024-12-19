using System;
using System.Data.SqlClient;

namespace StudentGradesApp
{
    class Program
    {
        static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StudentGrades;Integrated Security=True;";
        static SqlConnection connection;

        static void Main(string[] args)
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("1.Connect to the database");
                Console.WriteLine("2.Disconnect from the database");
                Console.WriteLine("3.Display all information from the table");
                Console.WriteLine("4.Display the full name of all students");
                Console.WriteLine("5.Display all average grades");
                Console.WriteLine("6.Show students with a minimum grade greater than specified");
                Console.WriteLine("7.Show unique subjects with minimum average scores");
                Console.WriteLine("8.Show minimum grade point average");
                Console.WriteLine("9.Show maximum average score");
                Console.WriteLine("10.The number of students with a minimum average grade in mathematics");
                Console.WriteLine("11.The number of students with the highest average grade in mathematics");
                Console.WriteLine("12.Number of students in each group");
                Console.WriteLine("13.The average score of each group");
                Console.WriteLine("14.Exit");

                switch (Console.ReadLine())
                {
                    case "1":
                        ConnectToDatabase();
                        break;
                    case "2":
                        DisconnectFromDatabase();
                        break;
                    case "3":
                        ExecuteQuery("SELECT * FROM Grades");
                        break;
                    case "4":
                        ExecuteQuery("SELECT FullName FROM Grades");
                        break;
                    case "5":
                        ExecuteQuery("SELECT AvgGrade FROM Grades");
                        break;
                    case "6":
                        ShowStudentsWithMinGradeHigherThan();
                        break;
                    case "7":
                        ExecuteQuery("SELECT DISTINCT MinGradeSubject FROM Grades");
                        break;
                    case "8":
                        ExecuteQuery("SELECT MIN(AvgGrade) AS MinAvgGrade FROM Grades");
                        break;
                    case "9":
                        ExecuteQuery("SELECT MAX(AvgGrade) AS MaxAvgGrade FROM Grades");
                        break;
                    case "10":
                        ExecuteQuery("SELECT COUNT(*) AS Count FROM Grades WHERE MinGradeSubject = 'Математика'");
                        break;
                    case "11":
                        ExecuteQuery("SELECT COUNT(*) AS Count FROM Grades WHERE MaxGradeSubject = 'Математика'");
                        break;
                    case "12":
                        ExecuteQuery("SELECT GroupName, COUNT(*) AS StudentCount FROM Grades GROUP BY GroupName");
                        break;
                    case "13":
                        ExecuteQuery("SELECT GroupName, AVG(AvgGrade) AS GroupAvg FROM Grades GROUP BY GroupName");
                        break;
                    case "14":
                        isRunning = false;
                        DisconnectFromDatabase();
                        break;
                    default:
                        Console.WriteLine("Error!");
                        break;
                }
                Console.ReadKey();
            }
        }
        static void ConnectToDatabase()
        {
            try
            {
                if (connection == null)
                {
                    connection = new SqlConnection(connectionString);
                }

                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                    Console.WriteLine("Database connection successful!");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void DisconnectFromDatabase()
        {
            try
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Database connection successful!");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void ExecuteQuery(string query)
        {
            try
            {
                if (connection == null || connection.State != System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Error");
                    return;
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write($"{reader.GetName(i)}: {reader[i]} ");
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void ShowStudentsWithMinGradeHigherThan()
        {
            Console.Write("Input min grade: ");
            if (double.TryParse(Console.ReadLine(), out double minGrade))
            {
                string query = $"SELECT FullName FROM Grades WHERE AvgGrade > {minGrade}";
                ExecuteQuery(query);
            }
            else
            {
                Console.WriteLine("Error");
            }
        }
    }
}