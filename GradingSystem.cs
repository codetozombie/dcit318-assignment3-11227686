using System;
using System.Collections.Generic;
using System.IO;

// Question 4: Grading System with File I/O and Exceptions

// Custom exceptions
public class InvalidScoreFormatException : Exception
{
    public InvalidScoreFormatException(string message) : base(message) { }
}

public class MissingFieldException : Exception
{
    public MissingFieldException(string message) : base(message) { }
}

// Student class
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Score { get; set; }

    public Student(int id, string name, double score)
    {
        Id = id;
        Name = name;
        Score = score;
    }

    public string GetGrade()
    {
        if (Score >= 90) return "A";
        if (Score >= 80) return "B";
        if (Score >= 70) return "C";
        if (Score >= 60) return "D";
        return "F";
    }

    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}, Score: {Score:F1}, Grade: {GetGrade()}";
    }
}

// Student Result Processor
public class StudentResultProcessor
{
    public List<Student> ReadStudentsFromFile(string filePath)
    {
        var students = new List<Student>();
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var lines = File.ReadAllLines(filePath);
        
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            
            var parts = line.Split(',');
            
            if (parts.Length < 3)
            {
                throw new MissingFieldException($"Missing fields in line: {line}");
            }
            
            if (!int.TryParse(parts[0].Trim(), out int id))
            {
                throw new InvalidScoreFormatException($"Invalid ID format: {parts[0]}");
            }
            
            string name = parts[1].Trim();
            
            if (!double.TryParse(parts[2].Trim(), out double score))
            {
                throw new InvalidScoreFormatException($"Invalid score format: {parts[2]}");
            }
            
            if (score < 0 || score > 100)
            {
                throw new InvalidScoreFormatException($"Score must be between 0 and 100: {score}");
            }
            
            students.Add(new Student(id, name, score));
        }
        
        return students;
    }

    public void WriteReportToFile(List<Student> students, string filePath)
    {
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Student Grade Report");
            writer.WriteLine("===================");
            writer.WriteLine();
            
            foreach (var student in students)
            {
                writer.WriteLine(student.ToString());
            }
            
            writer.WriteLine();
            writer.WriteLine($"Total Students: {students.Count}");
            
            // Calculate statistics
            if (students.Count > 0)
            {
                double average = 0;
                foreach (var student in students)
                {
                    average += student.Score;
                }
                average /= students.Count;
                
                writer.WriteLine($"Average Score: {average:F2}");
            }
        }
    }

    public static void Main()
    {
        Console.WriteLine("=== Grading System ===\n");
        
        var processor = new StudentResultProcessor();
        
        // Create sample data file
        string inputFile = "students.txt";
        string outputFile = "grade_report.txt";
        
        try
        {
            // Create sample input file
            File.WriteAllText(inputFile, @"1, John Doe, 85.5
2, Jane Smith, 92.0
3, Bob Johnson, 78.5
4, Alice Brown, 96.0
5, Charlie Wilson, 67.5");
            
            Console.WriteLine("Created sample student data file");
            
            // Read students from file
            var students = processor.ReadStudentsFromFile(inputFile);
            Console.WriteLine($"Successfully read {students.Count} students from file");
            
            // Display students
            foreach (var student in students)
            {
                Console.WriteLine(student);
            }
            
            // Write report to file
            processor.WriteReportToFile(students, outputFile);
            Console.WriteLine($"\nGrade report written to {outputFile}");
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"File Error: {ex.Message}");
        }
        catch (InvalidScoreFormatException ex)
        {
            Console.WriteLine($"Score Format Error: {ex.Message}");
        }
        catch (MissingFieldException ex)
        {
            Console.WriteLine($"Missing Field Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected Error: {ex.Message}");
        }
    }
}